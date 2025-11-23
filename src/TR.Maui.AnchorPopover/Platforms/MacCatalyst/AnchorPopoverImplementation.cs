#if IOS || MACCATALYST
using Microsoft.Maui.Platform;
using UIKit;

namespace TR.Maui.AnchorPopover.Platforms.MacCatalyst;

/// <summary>
/// iOS/MacCatalyst implementation of anchor popover using UIPopoverPresentationController.
/// </summary>
internal class AnchorPopoverImplementation : IAnchorPopover
{
    private UIViewController? _popoverViewController;
    private TaskCompletionSource<bool>? _dismissTaskCompletionSource;

    public bool IsShowing => _popoverViewController != null;

    public async Task ShowAsync(Microsoft.Maui.Controls.View content, Microsoft.Maui.Controls.View anchor, PopoverOptions? options = null)
    {
        if (content == null)
            throw new ArgumentNullException(nameof(content));
        if (anchor == null)
            throw new ArgumentNullException(nameof(anchor));

        var mauiContext = content.Handler?.MauiContext ?? anchor.Handler?.MauiContext;
        if (mauiContext == null)
            throw new InvalidOperationException("Unable to get MauiContext from content or anchor view.");

        // Get the native view for the anchor
        var anchorNativeView = anchor.ToPlatform(mauiContext);
        if (anchorNativeView == null)
            throw new InvalidOperationException("Unable to convert anchor to native view.");

        await ShowPopoverAsync(content, mauiContext, anchorNativeView, null, options);
    }

    public async Task ShowAsync(Microsoft.Maui.Controls.View content, Rect anchorBounds, PopoverOptions? options = null)
    {
        if (content == null)
            throw new ArgumentNullException(nameof(content));

        var mauiContext = content.Handler?.MauiContext ?? Application.Current?.Windows[0]?.Page?.Handler?.MauiContext;
        if (mauiContext == null)
            throw new InvalidOperationException("Unable to get MauiContext from content view.");

        var nativeBounds = new CoreGraphics.CGRect(
            anchorBounds.X,
            anchorBounds.Y,
            anchorBounds.Width,
            anchorBounds.Height
        );

        await ShowPopoverAsync(content, mauiContext, null, nativeBounds, options);
    }

    public async Task DismissAsync()
    {
        if (_popoverViewController == null)
        {
            return;
        }

        var tcs = new TaskCompletionSource<bool>();
        _popoverViewController.DismissViewController(true, () =>
        {
            _dismissTaskCompletionSource?.TrySetResult(true);
            _popoverViewController = null;
            tcs.TrySetResult(true);
        });

        // Wait for dismissal with timeout
        var timeoutTask = Task.Delay(1000);
        var completedTask = await Task.WhenAny(tcs.Task, timeoutTask);
        if (completedTask == timeoutTask)
        {
            // Timeout occurred, force completion
            _dismissTaskCompletionSource?.TrySetResult(true);
            _popoverViewController = null;
        }
    }

    private async Task ShowPopoverAsync(
        Microsoft.Maui.Controls.View content,
        IMauiContext mauiContext,
        UIView? anchorView,
        CoreGraphics.CGRect? anchorBounds,
        PopoverOptions? options)
    {
        options ??= new PopoverOptions();

        // Dismiss any existing popover
        if (_popoverViewController != null)
        {
            await DismissAsync();
        }

        _dismissTaskCompletionSource = new TaskCompletionSource<bool>();

        // Get the current view controller
        var currentViewController = GetCurrentViewController(mauiContext);
        if (currentViewController == null)
            throw new InvalidOperationException("Unable to get current view controller.");

        // Convert MAUI view to native view
        var nativeView = content.ToPlatform(mauiContext);
        if (nativeView == null)
            throw new InvalidOperationException("Unable to convert content to native view.");

        // Create a view controller for the content
        var contentViewController = new UIViewController();
        contentViewController.View = nativeView;

        // Configure preferred content size if specified
        if (options.PreferredWidth.HasValue || options.PreferredHeight.HasValue)
        {
            var width = options.PreferredWidth ?? 320;
            var height = options.PreferredHeight ?? 480;
            contentViewController.PreferredContentSize = new CoreGraphics.CGSize(width, height);
        }
        else
        {
            // Use content's measure to determine size
            var size = content.Measure(double.PositiveInfinity, double.PositiveInfinity);
            contentViewController.PreferredContentSize = new CoreGraphics.CGSize(
                size.Width > 0 ? size.Width : 320,
                size.Height > 0 ? size.Height : 480
            );
        }

        // Configure as popover
        contentViewController.ModalPresentationStyle = UIModalPresentationStyle.Popover;
        var popoverController = contentViewController.PopoverPresentationController;

        if (popoverController != null)
        {
            // Set anchor
            if (anchorView != null)
            {
                popoverController.SourceView = anchorView;
                popoverController.SourceRect = anchorView.Bounds;
            }
            else if (anchorBounds.HasValue)
            {
                popoverController.SourceView = currentViewController.View!;
                popoverController.SourceRect = anchorBounds.Value;
            }

            // Configure arrow direction
            popoverController.PermittedArrowDirections = ConvertArrowDirection(options.ArrowDirection);

            // Configure background color
            if (options.BackgroundColor != null)
            {
                popoverController.BackgroundColor = options.BackgroundColor.ToPlatform();
            }

            // Handle dismissal
            var delegateHandler = new PopoverDelegate(() =>
            {
                _dismissTaskCompletionSource?.TrySetResult(true);
                _popoverViewController = null;
            });
            popoverController.Delegate = delegateHandler;
        }

        // Present the popover
        _popoverViewController = contentViewController;
        await currentViewController.PresentViewControllerAsync(contentViewController, true);

        // Wait for dismissal
        await _dismissTaskCompletionSource.Task;
    }

    private static UIPopoverArrowDirection ConvertArrowDirection(PopoverArrowDirection direction)
    {
        var result = UIPopoverArrowDirection.Unknown;

        if (direction == PopoverArrowDirection.Any)
            return UIPopoverArrowDirection.Any;

        // Note: PopoverArrowDirection.Up means popover appears above the anchor (arrow points down)
        // PopoverArrowDirection.Down means popover appears below the anchor (arrow points up)
        if (direction.HasFlag(PopoverArrowDirection.Up))
            result |= UIPopoverArrowDirection.Down;  // Arrow points down, popover above
        if (direction.HasFlag(PopoverArrowDirection.Down))
            result |= UIPopoverArrowDirection.Up;    // Arrow points up, popover below
        if (direction.HasFlag(PopoverArrowDirection.Left))
            result |= UIPopoverArrowDirection.Right; // Arrow points right, popover left
        if (direction.HasFlag(PopoverArrowDirection.Right))
            result |= UIPopoverArrowDirection.Left;  // Arrow points left, popover right

        return result;
    }

    private static UIViewController? GetCurrentViewController(IMauiContext mauiContext)
    {
        return Platform.GetCurrentUIViewController();
    }

    private class PopoverDelegate : UIPopoverPresentationControllerDelegate
    {
        private readonly Action _onDismiss;

        public PopoverDelegate(Action onDismiss)
        {
            _onDismiss = onDismiss;
        }

        public override void DidDismissPopover(UIPopoverPresentationController popoverPresentationController)
        {
            _onDismiss?.Invoke();
        }
    }
}
#endif
