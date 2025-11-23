# TR.Maui.AnchorPopover

A .NET MAUI library for displaying native popovers with anchor support across iOS, macOS, Android, and Windows platforms.

## Features

- ✨ Native popover implementation using platform-specific controls
- 🎯 Anchor support for attaching to MAUI views
- 📱 Cross-platform: iOS 15+, Android 21+, Windows 10+, macOS
- 🎨 Customizable appearance and behavior
- 🔧 Simple and intuitive API

## Quick Start

```csharp
using TR.Maui.AnchorPopover;

// Create a popover
var popover = AnchorPopover.Create();

// Create content
var content = new Label
{
    Text = "Hello from Popover!",
    Padding = 20
};

// Show anchored to a button
await popover.ShowAsync(content, myButton);
```

## Advanced Usage

```csharp
var options = new PopoverOptions
{
    ArrowDirection = PopoverArrowDirection.Up,
    PreferredWidth = 300,
    BackgroundColor = Colors.LightBlue,
    DismissOnTapOutside = true
};

await popover.ShowAsync(content, anchorView, options);
```

## Documentation

For complete documentation, examples, and API reference, visit:
https://github.com/TetsuOtter/TR.Maui.AnchorPopover

## Platform Support

| Platform | Minimum Version | Implementation |
|----------|----------------|----------------|
| iOS | 15.0 | UIPopoverPresentationController |
| macOS | 15.0 | UIPopoverPresentationController |
| Android | API 21 | PopupWindow |
| Windows | 10.0.17763.0 | Flyout |

## License

MIT License - see LICENSE file for details
