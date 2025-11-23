#if WINDOWS
using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;
using Windows.Foundation;

namespace TR.Maui.AnchorPopover.Platforms.Windows;

/// <summary>
/// Windows implementation of anchor popover using Flyout.
/// </summary>
internal class AnchorPopoverImplementation : IAnchorPopover
{
    private Flyout? _flyout;
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
        var anchorNativeElement = anchor.ToPlatform(mauiContext) as FrameworkElement;
        if (anchorNativeElement == null)
            throw new InvalidOperationException("Unable to convert anchor to native FrameworkElement.");

        await ShowFlyoutAsync(content, mauiContext, anchorNativeElement, null, options);
    }

    public async Task ShowAsync(Microsoft.Maui.Controls.View content, Rect anchorBounds, PopoverOptions? options = null)
    {
        if (content == null)
            throw new ArgumentNullException(nameof(content));

        var mauiContext = content.Handler?.MauiContext ?? Application.Current?.Windows[0]?.Page?.Handler?.MauiContext;
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
        FrameworkElement? anchorElement,
        Rect? anchorBounds,
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
        var nativeView = content.ToPlatform(mauiContext) as FrameworkElement;
        if (nativeView == null)
            throw new InvalidOperationException("Unable to convert content to native FrameworkElement.");

        // Create flyout
        _flyout = new Flyout();
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
            ? LightDismissOverlayMode.On 
            : LightDismissOverlayMode.Off;

        // Configure styling
        var flyoutPresenterStyle = new Style(typeof(FlyoutPresenter));
        
        if (options.BackgroundColor != null)
        {
            flyoutPresenterStyle.Setters.Add(new Setter(
                FlyoutPresenter.BackgroundProperty,
                new SolidColorBrush(options.BackgroundColor.ToWindowsColor())
            ));
        }

        flyoutPresenterStyle.Setters.Add(new Setter(FlyoutPresenter.PaddingProperty, new Thickness(0)));
        flyoutPresenterStyle.Setters.Add(new Setter(FlyoutPresenter.BorderThicknessProperty, new Thickness(1)));
        flyoutPresenterStyle.Setters.Add(new Setter(
            FlyoutPresenter.BorderBrushProperty,
            new SolidColorBrush(Microsoft.UI.Colors.LightGray)
        ));
        flyoutPresenterStyle.Setters.Add(new Setter(FlyoutPresenter.CornerRadiusProperty, new CornerRadius(8)));
        
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
            // For bounds-based positioning, we need to create a temporary anchor element
            var window = mauiContext.Services.GetService<Microsoft.UI.Xaml.Window>();
            if (window?.Content is FrameworkElement rootElement)
            {
                var tempAnchor = new Border
                {
                    Width = anchorBounds.Value.Width,
                    Height = anchorBounds.Value.Height,
                    Margin = new Thickness(anchorBounds.Value.X, anchorBounds.Value.Y, 0, 0),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top
                };

                // Note: In a real scenario, you'd need to add this to a canvas or absolute positioned container
                // For simplicity, we'll just use the ShowAt method with the root element
                _flyout.ShowAt(rootElement, new FlyoutShowOptions
                {
                    Position = new Point(anchorBounds.Value.X, anchorBounds.Value.Y),
                    Placement = _flyout.Placement
                });
            }
        }

        // Wait for dismissal
        await _dismissTaskCompletionSource.Task;
    }

    private static FlyoutPlacementMode ConvertArrowDirectionToPlacement(PopoverArrowDirection direction)
    {
        return direction switch
        {
            PopoverArrowDirection.Up => FlyoutPlacementMode.Top,
            PopoverArrowDirection.Down => FlyoutPlacementMode.Bottom,
            PopoverArrowDirection.Left => FlyoutPlacementMode.Left,
            PopoverArrowDirection.Right => FlyoutPlacementMode.Right,
            _ => FlyoutPlacementMode.Auto
        };
    }
}
#endif
