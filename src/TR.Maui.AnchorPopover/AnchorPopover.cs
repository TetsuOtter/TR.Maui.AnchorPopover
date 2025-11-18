namespace TR.Maui.AnchorPopover;

/// <summary>
/// Factory class for creating platform-specific anchor popover implementations.
/// </summary>
public static class AnchorPopover
{
        /// <summary>
        /// Creates a new instance of IAnchorPopover for the current platform.
        /// </summary>
        /// <returns>A platform-specific implementation of IAnchorPopover.</returns>
        public static IAnchorPopover Create()
        {
#if TEST
        return new TestAnchorPopoverImplementation();
#elif ANDROID
                return new Platforms.Android.AnchorPopoverImplementation();
#elif IOS || MACCATALYST
        return new Platforms.iOS.AnchorPopoverImplementation();
#elif WINDOWS
        return new Platforms.Windows.AnchorPopoverImplementation();
#else
        throw new PlatformNotSupportedException("AnchorPopover is not supported on this platform.");
#endif
        }
}

/// <summary>
/// Test implementation of IAnchorPopover for unit testing.
/// </summary>
internal class TestAnchorPopoverImplementation : IAnchorPopover
{
        public bool IsShowing { get; private set; }

        public Task ShowAsync(View content, View anchor, PopoverOptions? options = null)
        {
                IsShowing = true;
                return Task.CompletedTask;
        }

        public Task ShowAsync(View content, Rect anchorBounds, PopoverOptions? options = null)
        {
                IsShowing = true;
                return Task.CompletedTask;
        }

        public void Dismiss()
        {
                IsShowing = false;
        }
}
