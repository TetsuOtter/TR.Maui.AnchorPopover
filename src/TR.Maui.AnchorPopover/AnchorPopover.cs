namespace TR.Maui.AnchorPopover;

/// <summary>
/// Factory class for creating platform-specific anchor popover implementations.
/// </summary>
public static class AnchorPopover
{
        private static IAnchorPopover? _instance;

        /// <summary>
        /// Creates a new instance of IAnchorPopover for the current platform.
        /// </summary>
        /// <returns>A platform-specific implementation of IAnchorPopover.</returns>
        public static IAnchorPopover Create()
        {
                if (_instance != null)
                {
                        return _instance;
                }

#if TEST
        _instance = new TestAnchorPopoverImplementation();
#elif ANDROID
        _instance = new Platforms.Android.AnchorPopoverImplementation();
#elif IOS || MACCATALYST
                _instance = new Platforms.iOS.AnchorPopoverImplementation();
#elif WINDOWS
        _instance = new Platforms.Windows.AnchorPopoverImplementation();
#else
        throw new PlatformNotSupportedException("AnchorPopover is not supported on this platform.");
#endif

                return _instance;
        }
}/// <summary>
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

        public Task DismissAsync()
        {
                IsShowing = false;
                return Task.CompletedTask;
        }
}
