#if WINDOWS
using Microsoft.Maui.Platform;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Platform;
using Windows.Foundation;

namespace TR.Maui.AnchorPopover.Platforms.Windows;

/// <summary>
/// Windows implementation of anchor popover using Flyout.
/// </summary>
internal class AnchorPopoverImplementation : IAnchorPopover
{
    private Microsoft.UI.Xaml.Controls.Flyout? _flyout;
    private TaskCompletionSource<bool>? _dismissTaskCompletionSource;

    public bool IsShowing => _flyout != null;

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
        var anchorNativeElement = anchor.ToPlatform(mauiContext) as Microsoft.UI.Xaml.FrameworkElement;
        if (anchorNativeElement == null)
            throw new InvalidOperationException("Unable to convert anchor to native FrameworkElement.");

        await ShowFlyoutAsync(content, mauiContext, anchorNativeElement, null, options);
    }

    public async Task ShowAsync(Microsoft.Maui.Controls.View content, Microsoft.Maui.Graphics.Rect anchorBounds, PopoverOptions? options = null)
    {
        if (content == null)
            throw new ArgumentNullException(nameof(content));

        var mauiContext = content.Handler?.MauiContext ?? Microsoft.Maui.Controls.Application.Current?.Windows[0]?.Page?.Handler?.MauiContext;
        if (mauiContext == null)
            throw new InvalidOperationException("Unable to get MauiContext from content view.");

        await ShowFlyoutAsync(content, mauiContext, null, anchorBounds, options);
    }

    public Task DismissAsync()
    {
        if (_flyout != null)
        {
            _flyout.Hide();
            _dismissTaskCompletionSource?.TrySetResult(true);
            _flyout = null;
        }
        return Task.CompletedTask;
    }

    private async Task ShowFlyoutAsync(
        Microsoft.Maui.Controls.View content,
        IMauiContext mauiContext,
        Microsoft.UI.Xaml.FrameworkElement? anchorElement,
        Microsoft.Maui.Graphics.Rect? anchorBounds,
        PopoverOptions? options)
    {
        options ??= new PopoverOptions();

        // Dismiss any existing flyout
        if (_flyout != null)
        {
            await DismissAsync();
        }

        _dismissTaskCompletionSource = new TaskCompletionSource<bool>();

        // Convert MAUI view to native view
        var nativeView = content.ToPlatform(mauiContext) as Microsoft.UI.Xaml.FrameworkElement;
        if (nativeView == null)
            throw new InvalidOperationException("Unable to convert content to native FrameworkElement.");

        // Create flyout
        _flyout = new Microsoft.UI.Xaml.Controls.Flyout();
        _flyout.Content = nativeView;

        // Configure size
        if (options.PreferredWidth.HasValue)
        {
            nativeView.Width = options.PreferredWidth.Value;
        }
        if (options.PreferredHeight.HasValue)
        {
            nativeView.Height = options.PreferredHeight.Value;
        }

        // Configure placement
        _flyout.Placement = ConvertArrowDirectionToPlacement(options.ArrowDirection);

        // Configure dismissal behavior
        _flyout.LightDismissOverlayMode = options.DismissOnTapOutside 
            ? Microsoft.UI.Xaml.Controls.LightDismissOverlayMode.On 
            : Microsoft.UI.Xaml.Controls.LightDismissOverlayMode.Off;

        // Configure styling
        var flyoutPresenterStyle = new Microsoft.UI.Xaml.Style(typeof(Microsoft.UI.Xaml.Controls.FlyoutPresenter));
        
        if (options.BackgroundColor != null)
        {
            var color = global::Windows.UI.Color.FromArgb(
                (byte)(options.BackgroundColor.Alpha * 255),
                (byte)(options.BackgroundColor.Red * 255),
                (byte)(options.BackgroundColor.Green * 255),
                (byte)(options.BackgroundColor.Blue * 255)
            );
            flyoutPresenterStyle.Setters.Add(new Microsoft.UI.Xaml.Setter(
                Microsoft.UI.Xaml.Controls.FlyoutPresenter.BackgroundProperty,
                new Microsoft.UI.Xaml.Media.SolidColorBrush(color)
            ));
        }

        flyoutPresenterStyle.Setters.Add(new Microsoft.UI.Xaml.Setter(Microsoft.UI.Xaml.Controls.FlyoutPresenter.PaddingProperty, new Microsoft.UI.Xaml.Thickness(0)));
        flyoutPresenterStyle.Setters.Add(new Microsoft.UI.Xaml.Setter(Microsoft.UI.Xaml.Controls.FlyoutPresenter.BorderThicknessProperty, new Microsoft.UI.Xaml.Thickness(1)));
        flyoutPresenterStyle.Setters.Add(new Microsoft.UI.Xaml.Setter(
            Microsoft.UI.Xaml.Controls.FlyoutPresenter.BorderBrushProperty,
            new Microsoft.UI.Xaml.Media.SolidColorBrush(global::Windows.UI.Colors.LightGray)
        ));
        flyoutPresenterStyle.Setters.Add(new Microsoft.UI.Xaml.Setter(Microsoft.UI.Xaml.Controls.FlyoutPresenter.CornerRadiusProperty, new Microsoft.UI.Xaml.CornerRadius(8)));
        
        _flyout.FlyoutPresenterStyle = flyoutPresenterStyle;

        // Handle dismissal
        _flyout.Closed += (s, e) =>
        {
            _dismissTaskCompletionSource?.TrySetResult(true);
            _flyout = null;
        };

        // Show the flyout
        if (anchorElement != null)
        {
            _flyout.ShowAt(anchorElement);
        }
        else if (anchorBounds.HasValue)
        {
            // Bounds-based positioning is not implemented for Windows yet
            throw new NotImplementedException("Bounds-based popover positioning is not supported on Windows.");
        }

        // Wait for dismissal
        await _dismissTaskCompletionSource.Task;
    }

    private static Microsoft.UI.Xaml.Controls.Primitives.FlyoutPlacementMode ConvertArrowDirectionToPlacement(PopoverArrowDirection direction)
    {
        return direction switch
        {
            PopoverArrowDirection.Up => Microsoft.UI.Xaml.Controls.Primitives.FlyoutPlacementMode.Top,
            PopoverArrowDirection.Down => Microsoft.UI.Xaml.Controls.Primitives.FlyoutPlacementMode.Bottom,
            PopoverArrowDirection.Left => Microsoft.UI.Xaml.Controls.Primitives.FlyoutPlacementMode.Left,
            PopoverArrowDirection.Right => Microsoft.UI.Xaml.Controls.Primitives.FlyoutPlacementMode.Right,
            _ => Microsoft.UI.Xaml.Controls.Primitives.FlyoutPlacementMode.Auto
        };
    }
}
#endif
