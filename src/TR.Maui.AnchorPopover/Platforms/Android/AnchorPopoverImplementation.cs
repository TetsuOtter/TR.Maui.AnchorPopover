#if ANDROID
using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;
using Microsoft.Maui.Platform;
using AView = Android.Views.View;

namespace TR.Maui.AnchorPopover.Platforms.Android;

/// <summary>
/// Android implementation of anchor popover using PopupWindow.
/// </summary>
internal class AnchorPopoverImplementation : IAnchorPopover
{
    private PopupWindow? _popupWindow;
    private TaskCompletionSource<bool>? _dismissTaskCompletionSource;

    public bool IsShowing => _popupWindow?.IsShowing ?? false;

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

        var mauiContext = content.Handler?.MauiContext;
        if (mauiContext == null)
            throw new InvalidOperationException("Unable to get MauiContext from content view.");

        await ShowPopoverAsync(content, mauiContext, null, anchorBounds, options);
    }

    public void Dismiss()
    {
        if (_popupWindow?.IsShowing == true)
        {
            _popupWindow.Dismiss();
            _dismissTaskCompletionSource?.TrySetResult(true);
            _popupWindow = null;
        }
    }

    private async Task ShowPopoverAsync(
        Microsoft.Maui.Controls.View content,
        IMauiContext mauiContext,
        AView? anchorView,
        Rect? anchorBounds,
        PopoverOptions? options)
    {
        options ??= new PopoverOptions();

        // Dismiss any existing popover
        if (_popupWindow?.IsShowing == true)
        {
            Dismiss();
        }

        _dismissTaskCompletionSource = new TaskCompletionSource<bool>();

        // Convert MAUI view to native view
        var nativeView = content.ToPlatform(mauiContext);
        if (nativeView == null)
            throw new InvalidOperationException("Unable to convert content to native view.");

        var context = mauiContext.Context;
        if (context == null)
            throw new InvalidOperationException("Context is null.");

        // Measure content
        var size = content.Measure(double.PositiveInfinity, double.PositiveInfinity);
        var width = options.PreferredWidth.HasValue 
            ? (int)(options.PreferredWidth.Value * context.Resources.DisplayMetrics.Density)
            : (size.Width > 0 ? (int)(size.Width * context.Resources.DisplayMetrics.Density) : ViewGroup.LayoutParams.WrapContent);
        
        var height = options.PreferredHeight.HasValue
            ? (int)(options.PreferredHeight.Value * context.Resources.DisplayMetrics.Density)
            : (size.Height > 0 ? (int)(size.Height * context.Resources.DisplayMetrics.Density) : ViewGroup.LayoutParams.WrapContent);

        // Create popup window
        _popupWindow = new PopupWindow(nativeView, width, height, true);
        
        // Configure popup appearance
        _popupWindow.OutsideTouchable = options.DismissOnTapOutside;
        _popupWindow.Focusable = true;

        // Set background with elevation for shadow effect (Material Design style)
        var drawable = new GradientDrawable();
        drawable.SetCornerRadius(8 * context.Resources.DisplayMetrics.Density);
        
        if (options.BackgroundColor != null)
        {
            drawable.SetColor(options.BackgroundColor.ToPlatform());
        }
        else
        {
            drawable.SetColor(global::Android.Graphics.Color.White);
        }
        
        _popupWindow.SetBackgroundDrawable(drawable);
        _popupWindow.Elevation = 8 * context.Resources.DisplayMetrics.Density;

        // Handle dismissal
        _popupWindow.DismissEvent += (s, e) =>
        {
            _dismissTaskCompletionSource?.TrySetResult(true);
            _popupWindow = null;
        };

        // Show the popup
        if (anchorView != null)
        {
            ShowAtAnchor(anchorView, options.ArrowDirection);
        }
        else if (anchorBounds.HasValue)
        {
            ShowAtLocation(context, anchorBounds.Value, options.ArrowDirection);
        }

        // Wait for dismissal
        await _dismissTaskCompletionSource.Task;
    }

    private void ShowAtAnchor(AView anchorView, PopoverArrowDirection direction)
    {
        if (_popupWindow == null)
            return;

        // Calculate position based on arrow direction
        var location = new int[2];
        anchorView.GetLocationOnScreen(location);

        var gravity = GetGravity(direction);
        var xOffset = 0;
        var yOffset = 0;

        // Position the popup relative to the anchor
        if (direction.HasFlag(PopoverArrowDirection.Down) || direction == PopoverArrowDirection.Any)
        {
            // Show below the anchor
            yOffset = anchorView.Height;
        }
        else if (direction.HasFlag(PopoverArrowDirection.Up))
        {
            // Show above the anchor
            yOffset = -_popupWindow.Height;
        }
        else if (direction.HasFlag(PopoverArrowDirection.Right))
        {
            // Show to the right of the anchor
            xOffset = anchorView.Width;
        }
        else if (direction.HasFlag(PopoverArrowDirection.Left))
        {
            // Show to the left of the anchor
            xOffset = -_popupWindow.Width;
        }

        _popupWindow.ShowAsDropDown(anchorView, xOffset, yOffset, gravity);
    }

    private void ShowAtLocation(Context context, Rect bounds, PopoverArrowDirection direction)
    {
        if (_popupWindow == null)
            return;

        var activity = Platform.CurrentActivity ?? (context as Activity);
        if (activity == null)
            return;

        var rootView = activity.Window?.DecorView?.RootView;
        if (rootView == null)
            return;

        var density = context.Resources.DisplayMetrics.Density;
        var x = (int)(bounds.X * density);
        var y = (int)(bounds.Y * density);

        // Adjust position based on arrow direction
        if (direction.HasFlag(PopoverArrowDirection.Down) || direction == PopoverArrowDirection.Any)
        {
            y += (int)(bounds.Height * density);
        }
        else if (direction.HasFlag(PopoverArrowDirection.Up))
        {
            y -= _popupWindow.Height;
        }
        else if (direction.HasFlag(PopoverArrowDirection.Right))
        {
            x += (int)(bounds.Width * density);
        }
        else if (direction.HasFlag(PopoverArrowDirection.Left))
        {
            x -= _popupWindow.Width;
        }

        _popupWindow.ShowAtLocation(rootView, GravityFlags.NoGravity, x, y);
    }

    private static GravityFlags GetGravity(PopoverArrowDirection direction)
    {
        return direction switch
        {
            PopoverArrowDirection.Up => GravityFlags.Bottom,
            PopoverArrowDirection.Down => GravityFlags.Top,
            PopoverArrowDirection.Left => GravityFlags.Right,
            PopoverArrowDirection.Right => GravityFlags.Left,
            _ => GravityFlags.NoGravity
        };
    }
}
#endif
