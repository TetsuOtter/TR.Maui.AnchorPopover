# TR.Maui.AnchorPopover

A .NET MAUI library for displaying native popovers with anchor support across iOS, macOS, Android, and Windows platforms.

## Features

- ✨ **Native Popover Implementation**: Uses platform-specific native controls
  - iOS/MacCatalyst: `UIPopoverPresentationController`
  - Android: `PopupWindow` with Material Design styling
  - Windows: `Flyout` controls
- 🎯 **Anchor Support**: Anchor popovers to specific MAUI views or screen positions
- 📱 **Cross-Platform**: Works on iOS 15+, Android 21+, Windows 10+, and macOS
- 🎨 **Customizable**: Configure arrow direction, size, background color, and dismissal behavior
- 🔧 **Easy to Use**: Simple, intuitive API

## Installation

Install via NuGet Package Manager:

```bash
dotnet add package TR.Maui.AnchorPopover
```

Or via Package Manager Console:

```powershell
Install-Package TR.Maui.AnchorPopover
```

## Quick Start

### Basic Usage

```csharp
using TR.Maui.AnchorPopover;

// Create a popover instance
var popover = AnchorPopover.Create();

// Create your content
var content = new Label
{
    Text = "Hello from Popover!",
    Padding = 20
};

// Show the popover anchored to a button
await popover.ShowAsync(content, myButton);
```

### Advanced Usage with Options

```csharp
var popover = AnchorPopover.Create();

var content = new VerticalStackLayout
{
    Padding = 20,
    Children =
    {
        new Label { Text = "Custom Popover", FontSize = 18 },
        new Entry { Placeholder = "Type here..." },
        new Button { Text = "Close", Command = new Command(() => popover.Dismiss()) }
    }
};

var options = new PopoverOptions
{
    ArrowDirection = PopoverArrowDirection.Up,
    PreferredWidth = 300,
    PreferredHeight = 200,
    BackgroundColor = Colors.LightBlue,
    DismissOnTapOutside = true
};

await popover.ShowAsync(content, anchorView, options);
```

### Position-Based Anchoring

```csharp
var popover = AnchorPopover.Create();

var content = new Label { Text = "Positioned popover", Padding = 20 };

// Show at a specific screen position
var bounds = new Rect(x: 100, y: 100, width: 50, height: 50);
await popover.ShowAsync(content, bounds);
```

## API Reference

### `AnchorPopover.Create()`

Factory method to create a platform-specific popover instance.

**Returns:** `IAnchorPopover`

### `IAnchorPopover` Interface

#### Methods

- **`ShowAsync(View content, View anchor, PopoverOptions? options = null)`**
  
  Shows a popover with the specified content, anchored to the given view.
  
  - `content`: The MAUI view to display in the popover
  - `anchor`: The view to anchor the popover to
  - `options`: Optional configuration for the popover
  - Returns: A task that completes when the popover is dismissed

- **`ShowAsync(View content, Rect anchorBounds, PopoverOptions? options = null)`**
  
  Shows a popover with the specified content, anchored to a specific location.
  
  - `content`: The MAUI view to display in the popover
  - `anchorBounds`: The rectangle (in screen coordinates) to anchor the popover to
  - `options`: Optional configuration for the popover
  - Returns: A task that completes when the popover is dismissed

- **`Dismiss()`**
  
  Dismisses the currently displayed popover.

#### Properties

- **`IsShowing`**: Gets a value indicating whether a popover is currently being displayed

### `PopoverOptions` Class

Configuration options for displaying a popover.

#### Properties

- **`ArrowDirection`**: Preferred arrow direction (`PopoverArrowDirection` enum)
  - `Any` (default): System chooses the best direction
  - `Up`: Arrow points upward
  - `Down`: Arrow points downward
  - `Left`: Arrow points to the left
  - `Right`: Arrow points to the right

- **`IsModal`**: Whether the popover should be modal (default: `false`)

- **`PreferredWidth`**: Preferred width of the popover content

- **`PreferredHeight`**: Preferred height of the popover content

- **`DismissOnTapOutside`**: Whether to dismiss when tapping outside (default: `true`)

- **`BackgroundColor`**: Background color of the popover

## Platform-Specific Notes

### iOS/MacCatalyst

- Uses native `UIPopoverPresentationController`
- Fully supports all arrow directions
- Automatically adapts to screen edges
- Best popover experience with native appearance

### Android

- Uses `PopupWindow` with Material Design styling
- Includes elevation and rounded corners for a modern look
- Arrow directions are approximated through positioning
- May have slight visual differences from iOS

### Windows

- Uses `Flyout` controls
- Supports all arrow directions via placement
- Modern Fluent Design appearance
- Integrates well with Windows 10/11 UI

## Sample Application

Check out the [sample application](./samples/TR.Maui.AnchorPopover.Sample) for complete examples of all features.

## Requirements

- .NET 10.0 or later
- .NET MAUI workload installed

## Building from Source

```bash
# Clone the repository
git clone https://github.com/TetsuOtter/TR.Maui.AnchorPopover.git

# Build the library
cd TR.Maui.AnchorPopover
dotnet build src/TR.Maui.AnchorPopover/TR.Maui.AnchorPopover.csproj

# Build the sample
dotnet build samples/TR.Maui.AnchorPopover.Sample/TR.Maui.AnchorPopover.Sample.csproj
```

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgments

Built with ❤️ using .NET MAUI