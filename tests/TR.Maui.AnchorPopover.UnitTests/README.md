# TR.Maui.AnchorPopover Unit Tests

This project contains unit tests for the TR.Maui.AnchorPopover library.

## Running the Tests

### Prerequisites

Since this is a MAUI application test project, you need one of the following to run the tests:

- **Android**: Android device or emulator
- **iOS**: iOS device or simulator (macOS only)
- **Windows**: Windows machine
- **macOS**: Mac with macOS

### Running on Android

1. Start an Android emulator or connect an Android device
2. Run the tests:
   ```bash
   dotnet test tests/TR.Maui.AnchorPopover.UnitTests/TR.Maui.AnchorPopover.UnitTests.csproj -f net10.0-android
   ```

### Running on iOS (macOS only)

1. Open the solution in Visual Studio for Mac or Visual Studio 2022 on Mac
2. Select an iOS device or simulator
3. Run the unit tests from Test Explorer

### Running on Windows

1. Open the solution in Visual Studio 2022
2. Select the Windows target
3. Run the unit tests from Test Explorer

## Test Categories

### PopoverOptionsTests
Tests the `PopoverOptions` class configuration:
- Default values
- Property setters and getters
- Initialization with all properties

### PopoverArrowDirectionTests
Tests the `PopoverArrowDirection` enum:
- Enum values
- Flags attribute functionality
- Flag combinations

### AnchorPopoverFactoryTests
Tests the `AnchorPopover.Create()` factory method:
- Instance creation
- Interface compliance
- Instance independence
- Initial state

## Continuous Integration

These tests are designed to run in CI/CD environments with appropriate platform support:
- Android tests can run on Linux with Android emulator
- iOS tests require macOS with iOS simulator
- Windows tests require Windows OS

## Adding New Tests

When adding new tests:
1. Follow the existing naming convention: `[ClassName]Tests.cs`
2. Use xUnit attributes: `[Fact]`, `[Theory]`, `[InlineData]`
3. Follow the Arrange-Act-Assert pattern
4. Add descriptive test names that explain what is being tested
