# TR.Maui.AnchorPopover Usage Guide

This guide provides detailed information on how to use the TR.Maui.AnchorPopover library in your .NET MAUI applications.

## Table of Contents

- [Installation](#installation)
- [Basic Concepts](#basic-concepts)
- [Simple Examples](#simple-examples)
- [Advanced Usage](#advanced-usage)
- [Platform-Specific Behavior](#platform-specific-behavior)
- [Best Practices](#best-practices)
- [Troubleshooting](#troubleshooting)

## Installation

### Via NuGet Package Manager Console

```powershell
Install-Package TR.Maui.AnchorPopover
```

### Via .NET CLI

```bash
dotnet add package TR.Maui.AnchorPopover
```

## Basic Concepts

### Popover vs Dialog

A popover is a UI element that appears anchored to a specific view or location on screen. Unlike dialogs:
- Popovers are positioned relative to an anchor point
- They typically have an arrow pointing to the anchor
- They're dismissed by tapping outside (configurable)
- They're great for contextual information and actions

### Anchor Points

An anchor point is the reference location for displaying the popover. You can anchor to:
- **MAUI Views**: Any visible MAUI control (Button, Label, Image, etc.)
- **Screen Coordinates**: A specific rectangle on the screen

## Simple Examples

### Example 1: Basic Text Popover

```csharp
using TR.Maui.AnchorPopover;

private async void ShowInfoButton_Clicked(object sender, EventArgs e)
{
    var popover = AnchorPopover.Create();
    
    var content = new Label
    {
        Text = "This is additional information about this feature.",
        Padding = 20,
        FontSize = 14
    };
    
    await popover.ShowAsync(content, InfoButton);
}
```

### Example 2: Interactive Popover

```csharp
private async void ShowInteractivePopover_Clicked(object sender, EventArgs e)
{
    var popover = AnchorPopover.Create();
    
    var content = new VerticalStackLayout
    {
        Padding = 20,
        Spacing = 10,
        Children =
        {
            new Label { Text = "Choose an option:", FontAttributes = FontAttributes.Bold },
            new Button 
            { 
                Text = "Option 1", 
                Command = new Command(() => 
                {
                    popover.Dismiss();
                    // Handle option 1
                })
            },
            new Button 
            { 
                Text = "Cancel", 
                Command = new Command(() => popover.Dismiss())
            }
        }
    };
    
    await popover.ShowAsync(content, sender as View);
}
```

## Advanced Usage

### Custom Content Views

```csharp
var content = new Grid
{
    RowDefinitions = 
    {
        new RowDefinition { Height = GridLength.Auto },
        new RowDefinition { Height = GridLength.Auto }
    },
    Padding = 15,
    RowSpacing = 10
};

content.Add(new Label { Text = "Name:" }, 0, 0);
content.Add(new Entry { Placeholder = "Enter name..." }, 0, 1);

await popover.ShowAsync(content, anchorButton);
```

### Position-Based Anchoring

```csharp
var popover = AnchorPopover.Create();
var content = new Label { Text = "Positioned popover", Padding = 20 };

var bounds = new Rect(x: 100, y: 200, width: 50, height: 50);
await popover.ShowAsync(content, bounds);
```

## Platform-Specific Behavior

### iOS and MacCatalyst
- Uses native `UIPopoverPresentationController`
- Full arrow direction support
- Automatically repositions if needed

### Android
- Uses `PopupWindow` with Material Design
- Arrow simulated through positioning
- Modern rounded corners and shadows

### Windows
- Uses `Flyout` controls
- Fluent Design principles
- Native Windows 10/11 appearance

## Best Practices

1. **Await ShowAsync**: Always use `await` to properly handle popover lifecycle
2. **Specify Size**: Use `PreferredWidth` and `PreferredHeight` for consistent sizing
3. **Test All Platforms**: Appearance varies by platform
4. **Handle Dismissal**: Store popover reference if you need to dismiss programmatically

## Troubleshooting

### Popover Not Appearing
- Ensure anchor view is visible and has valid size
- Check that MauiContext is available

### Content Not Sized Correctly
- Set explicit `WidthRequest` and `HeightRequest` on content
- Use `PopoverOptions` to specify preferred dimensions

For more examples, see the [Sample Application](https://github.com/TetsuOtter/TR.Maui.AnchorPopover/tree/main/samples).
