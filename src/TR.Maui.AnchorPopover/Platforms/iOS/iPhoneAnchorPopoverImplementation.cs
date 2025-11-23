#if IOS
using Microsoft.Maui.Platform;
using UIKit;
using CoreGraphics;
using CoreAnimation;

namespace TR.Maui.AnchorPopover.Platforms.iOS;

/// <summary>
/// iPhone implementation of anchor popover using custom UIView.
/// </summary>
internal class iPhoneAnchorPopoverImplementation : IAnchorPopover
{
  private UIView? _customPopoverView;
  private UIView? _backgroundView;
  private UIView? _arrowView;
  private TaskCompletionSource<bool>? _dismissTaskCompletionSource;

  public bool IsShowing => _customPopoverView != null;

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

    await ShowCustomPopoverAsync(content, mauiContext, anchorNativeView, null, options);
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

    await ShowCustomPopoverAsync(content, mauiContext, null, nativeBounds, options);
  }

  public async Task DismissAsync()
  {
    if (_customPopoverView != null && _backgroundView != null && _arrowView != null)
    {
      // Animate out
      await UIView.AnimateAsync(0.2, () =>
      {
        _backgroundView.Alpha = 0;
        _customPopoverView.Alpha = 0;
        _customPopoverView.Transform = CoreGraphics.CGAffineTransform.MakeScale(0.8f, 0.8f);
        _arrowView.Alpha = 0;
      });

      _customPopoverView.RemoveFromSuperview();
      _backgroundView.RemoveFromSuperview();
      _arrowView.RemoveFromSuperview();
      _customPopoverView = null;
      _backgroundView = null;
      _arrowView = null;
      _dismissTaskCompletionSource?.TrySetResult(true);
    }
  }

  private async Task ShowCustomPopoverAsync(
      Microsoft.Maui.Controls.View content,
      IMauiContext mauiContext,
      UIView? anchorView,
      CoreGraphics.CGRect? anchorBounds,
      PopoverOptions? options)
  {
    options ??= new PopoverOptions();

    // Dismiss any existing popover
    if (_customPopoverView != null)
    {
      await DismissAsync();
    }

    _dismissTaskCompletionSource = new TaskCompletionSource<bool>();

    // Get the key window
    UIView? window = null;
    if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
    {
      var windowScene = UIApplication.SharedApplication.ConnectedScenes.OfType<UIWindowScene>().FirstOrDefault();
      window = windowScene?.Windows.FirstOrDefault();
    }
    else
    {
      window = UIApplication.SharedApplication.KeyWindow;
    }
    if (window == null)
      throw new InvalidOperationException("Unable to get key window.");

    // Get anchor rect in window coordinates
    CoreGraphics.CGRect anchorRect;
    if (anchorView != null)
    {
      anchorRect = anchorView.ConvertRectToView(anchorView.Bounds, window);
    }
    else if (anchorBounds.HasValue)
    {
      anchorRect = anchorBounds.Value;
    }
    else
    {
      throw new InvalidOperationException("Anchor not specified.");
    }

    // Convert MAUI view to native view
    var nativeView = content.ToPlatform(mauiContext);
    if (nativeView == null)
      throw new InvalidOperationException("Unable to convert content to native view.");

    // Configure size
    nfloat width = (nfloat)(options.PreferredWidth ?? 320);
    nfloat height = (nfloat)(options.PreferredHeight ?? 480);
    if (!options.PreferredWidth.HasValue || !options.PreferredHeight.HasValue)
    {
      var size = content.Measure(double.PositiveInfinity, double.PositiveInfinity);
      width = size.Width > 0 ? (nfloat)size.Width : width;
      height = size.Height > 0 ? (nfloat)size.Height : height;
    }

    // Calculate position based on arrow direction
    nfloat x, y;
    var screenBounds = UIScreen.MainScreen.Bounds;
    var arrowDirection = options.ArrowDirection;

    if (arrowDirection == PopoverArrowDirection.Any || arrowDirection.HasFlag(PopoverArrowDirection.Down))
    {
      // Prefer below
      x = (anchorRect.X + anchorRect.Width / 2) - width / 2;
      y = anchorRect.Y + anchorRect.Height + 10;
      if (y + height > screenBounds.Height)
      {
        // If not enough space below, try above
        y = anchorRect.Y - height - 10;
      }
    }
    else if (arrowDirection.HasFlag(PopoverArrowDirection.Up))
    {
      // Above
      x = (anchorRect.X + anchorRect.Width / 2) - width / 2;
      y = anchorRect.Y - height - 10;
      if (y < 0)
      {
        // If not enough space above, place below
        y = anchorRect.Y + anchorRect.Height + 10;
      }
    }
    else
    {
      // Default to below
      x = (anchorRect.X + anchorRect.Width / 2) - width / 2;
      y = anchorRect.Y + anchorRect.Height + 10;
    }

    // Ensure within screen bounds
    x = (nfloat)Math.Max(10, Math.Min(x, screenBounds.Width - width - 10));
    y = (nfloat)Math.Max(10, Math.Min(y, screenBounds.Height - height - 10));

    nativeView.Frame = new CoreGraphics.CGRect(x, y, width, height);

    // Configure background color
    UIColor defaultBackgroundColor;
    if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
    {
      defaultBackgroundColor = UIColor.SystemGray6;
    }
    else
    {
      defaultBackgroundColor = window.TraitCollection.UserInterfaceStyle == UIUserInterfaceStyle.Dark
          ? UIColor.Gray
          : UIColor.LightGray;
    }
    nativeView.BackgroundColor = options.BackgroundColor?.ToPlatform() ?? defaultBackgroundColor;

    // Rounded corners and shadow
    nativeView.Layer.CornerRadius = 10;
    nativeView.Layer.ShadowOpacity = 0.5f;
    nativeView.Layer.ShadowRadius = 5;
    nativeView.Layer.ShadowOffset = new CoreGraphics.CGSize(0, 2);
    nativeView.Layer.ShadowColor = UIColor.Black.CGColor;

    // Create background view for dismissal on tap
    _backgroundView = new UIView(window.Bounds);
    _backgroundView.BackgroundColor = UIColor.Black.ColorWithAlpha(0.5f);
    _backgroundView.Alpha = 0; // Start hidden for animation
    var tapGesture = new UITapGestureRecognizer(() =>
    {
      DismissAsync().ConfigureAwait(false);
    });
    _backgroundView.AddGestureRecognizer(tapGesture);

    // Create arrow view
    var arrowSize = 10f;
    var arrowX = (anchorRect.X + anchorRect.Width / 2) - arrowSize / 2;
    nfloat arrowY;
    UIBezierPath arrowPath;
    if (y > anchorRect.Y + anchorRect.Height) // Popover below anchor
    {
      arrowY = y - arrowSize;
      arrowPath = new UIBezierPath();
      arrowPath.MoveTo(new CGPoint(arrowSize / 2, 0));
      arrowPath.AddLineTo(new CGPoint(0, arrowSize));
      arrowPath.AddLineTo(new CGPoint(arrowSize, arrowSize));
      arrowPath.ClosePath();
    }
    else // Popover above anchor
    {
      arrowY = y + height;
      arrowPath = new UIBezierPath();
      arrowPath.MoveTo(new CGPoint(0, 0));
      arrowPath.AddLineTo(new CGPoint(arrowSize, 0));
      arrowPath.AddLineTo(new CGPoint(arrowSize / 2, arrowSize));
      arrowPath.ClosePath();
    }
    _arrowView = new UIView(new CGRect(arrowX, arrowY, arrowSize, arrowSize));
    var arrowLayer = new CAShapeLayer();
    arrowLayer.Path = arrowPath.CGPath;
    arrowLayer.FillColor = defaultBackgroundColor.CGColor;
    _arrowView.Layer.AddSublayer(arrowLayer);
    _arrowView.Alpha = 0; // Start hidden

    // Add to window
    window.AddSubview(_backgroundView);
    window.AddSubview(_arrowView);
    window.AddSubview(nativeView);

    _customPopoverView = nativeView;

    // Set initial state for animation
    nativeView.Alpha = 0;
    nativeView.Transform = CoreGraphics.CGAffineTransform.MakeScale(0.8f, 0.8f);

    // Animate in
    await UIView.AnimateAsync(0.3, () =>
    {
      _backgroundView.Alpha = 1;
      nativeView.Alpha = 1;
      nativeView.Transform = CoreGraphics.CGAffineTransform.MakeIdentity();
      _arrowView.Alpha = 1;
    });

    // Wait for dismissal
    await _dismissTaskCompletionSource.Task;
  }
}
#endif