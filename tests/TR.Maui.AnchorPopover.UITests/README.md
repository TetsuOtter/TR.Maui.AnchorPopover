# TR.Maui.AnchorPopover UI Tests

This is a visual/manual UI testing application for the TR.Maui.AnchorPopover library. It provides interactive tests to verify popover behavior across different platforms.

## Purpose

While automated UI testing for MAUI is complex, this application provides manual testing scenarios to validate:
- Popover rendering on each platform
- Anchor positioning accuracy
- Arrow direction behavior
- Content display capabilities
- Interactive element functionality
- Dismissal behavior

## Running the UI Tests

### Android
```bash
# Using Android emulator or device
dotnet build -t:Run -f net10.0-android tests/TR.Maui.AnchorPopover.UITests/TR.Maui.AnchorPopover.UITests.csproj
```

### iOS (macOS only)
1. Open the solution in Visual Studio for Mac or Visual Studio 2022
2. Select the UITests project
3. Choose an iOS simulator or device
4. Run the application

### Windows
1. Open the solution in Visual Studio 2022
2. Select the UITests project  
3. Set the target to Windows
4. Run the application

### macOS
1. Open the solution in Visual Studio for Mac
2. Select the UITests project
3. Run the application

## Test Scenarios

### Test 1: Basic Popover
- **What it tests**: Basic popover with simple text content
- **Expected**: Popover appears anchored to the button
- **Verify**: 
  - Popover appears
  - Anchored correctly
  - Dismisses when tapping outside

### Test 2: Custom Content
- **What it tests**: Popover with multiple MAUI controls
- **Expected**: Popover displays complex layout with input fields and buttons
- **Verify**:
  - All controls are visible
  - Input field is interactive
  - Close button works
  - Preferred width is respected

### Test 3: Arrow Direction
- **What it tests**: Arrow direction configuration
- **Expected**: Popover appears with upward arrow (prefers to show below button)
- **Verify**:
  - Arrow points upward
  - Custom background color (light blue) is applied
  - Platform respects arrow direction preference

### Test 4: Positioned Popover
- **What it tests**: Position-based anchoring
- **Expected**: Popover appears at screen center
- **Verify**:
  - Popover appears at specified coordinates
  - Not anchored to any view
  - Dismisses normally

### Test 5: Interactive Content
- **What it tests**: Interactive elements inside popover
- **Expected**: Button click updates counter
- **Verify**:
  - Button inside popover is clickable
  - Label updates on each click
  - Close button works
  - Final count is displayed in status

## Test Results

After running each test, check the status label at the bottom of the screen:
- ✓ **Passed**: Test executed successfully
- ✗ **Failed**: Test encountered an error (see message)

## Platform-Specific Notes

### iOS/MacCatalyst
- Uses native `UIPopoverPresentationController`
- Arrow direction is fully supported
- May adapt to full screen on iPhone in portrait mode

### Android
- Uses `PopupWindow` with Material Design
- Arrow is simulated through positioning
- Includes elevation shadow

### Windows
- Uses `Flyout` controls
- Follows Fluent Design
- Arrow via placement modes

## Manual Testing Checklist

For each platform, verify:

- [ ] All 5 tests execute without errors
- [ ] Popovers appear in correct positions
- [ ] Content is fully visible and not clipped
- [ ] Arrow directions are appropriate
- [ ] Dismissal works by tapping outside
- [ ] Interactive elements are functional
- [ ] Custom styling is applied
- [ ] No visual artifacts or glitches

## Adding New Tests

To add new UI tests:

1. Add a new button to `MainPage.xaml`
2. Create corresponding event handler in `MainPage.xaml.cs`
3. Implement test scenario with popover
4. Update status label with results
5. Document the test in this README

## Automated Testing

For automated UI testing, consider:
- Appium for cross-platform automation
- Platform-specific frameworks (Espresso for Android, XCTest for iOS)
- Visual regression testing tools
- Screenshot comparison tools

This manual testing app serves as a foundation that could be automated with such tools in the future.
