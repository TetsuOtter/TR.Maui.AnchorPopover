using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

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
#if ANDROID
        return new TR.Maui.AnchorPopover.Platforms.Android.AnchorPopoverImplementation();
#elif IOS
                return new TR.Maui.AnchorPopover.Platforms.iOS.AnchorPopoverImplementation();
#elif MACCATALYST
                return new TR.Maui.AnchorPopover.Platforms.MacCatalyst.AnchorPopoverImplementation();
#elif WINDOWS
        return new TR.Maui.AnchorPopover.Platforms.Windows.AnchorPopoverImplementation();
#else
        throw new PlatformNotSupportedException("AnchorPopover is not supported on this platform.");
#endif
        }
}
