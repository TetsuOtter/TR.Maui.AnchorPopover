# Testing Guide for TR.Maui.AnchorPopover

This document describes the testing strategy and available tests for the TR.Maui.AnchorPopover library.

## Test Structure

The project includes two types of tests:

1. **Unit Tests** - Automated tests for core functionality
2. **UI Tests** - Interactive manual tests for visual verification

## Unit Tests

### Location
`tests/TR.Maui.AnchorPopover.UnitTests/`

### What's Tested

#### PopoverOptionsTests
- Default property values
- Property setters and getters
- Object initialization
- Configuration combinations

**Test Cases:**
- `PopoverOptions_DefaultValues_AreCorrect()` - Verifies default values
- `PopoverOptions_ArrowDirection_CanBeSet()` - Tests arrow direction property
- `PopoverOptions_IsModal_CanBeSet()` - Tests modal property
- `PopoverOptions_PreferredDimensions_CanBeSet()` - Tests size configuration
- `PopoverOptions_DismissOnTapOutside_CanBeToggled()` - Tests dismissal behavior
- `PopoverOptions_BackgroundColor_CanBeSet()` - Tests color customization
- `PopoverOptions_AllProperties_CanBeInitialized()` - Tests complete initialization

#### PopoverArrowDirectionTests
- Enum value correctness
- Flags attribute functionality
- Flag combination behavior

**Test Cases:**
- `PopoverArrowDirection_EnumValues_AreCorrect()` - Verifies enum values
- `PopoverArrowDirection_FlagsAttribute_AllowsCombination()` - Tests flag combinations
- `PopoverArrowDirection_MultipleFlagsCombined_WorkCorrectly()` - Tests multiple flags
- `PopoverArrowDirection_AllValues_AreDefined()` - Verifies all enum values are defined

#### AnchorPopoverFactoryTests
- Factory method functionality
- Instance creation
- Interface compliance
- Instance independence

**Test Cases:**
- `AnchorPopover_Create_ReturnsNonNullInstance()` - Verifies instance creation
- `AnchorPopover_Create_ReturnsIAnchorPopoverInterface()` - Tests interface compliance
- `AnchorPopover_Create_MultipleInstances_AreIndependent()` - Tests instance independence
- `AnchorPopover_Create_InitiallyNotShowing()` - Verifies initial state

### Running Unit Tests

#### Prerequisites
- .NET 10.0 SDK
- MAUI workload installed
- Android emulator/device (for Android target)
- iOS simulator/device (for iOS target, macOS only)
- Windows machine (for Windows target)

#### Commands

**Build Tests:**
```bash
dotnet build tests/TR.Maui.AnchorPopover.UnitTests/TR.Maui.AnchorPopover.UnitTests.csproj
```

**Run Tests (Android):**
```bash
dotnet test tests/TR.Maui.AnchorPopover.UnitTests/TR.Maui.AnchorPopover.UnitTests.csproj -f net10.0-android
```

**Run Tests (iOS, macOS only):**
```bash
dotnet test tests/TR.Maui.AnchorPopover.UnitTests/TR.Maui.AnchorPopover.UnitTests.csproj -f net10.0-ios
```

**Note:** Unit tests require a device/emulator as they use MAUI types.

### Test Framework
- **xUnit** - Unit testing framework
- **Microsoft.NET.Test.Sdk** - Test SDK

## UI Tests

### Location
`tests/TR.Maui.AnchorPopover.UITests/`

### What's Tested

The UI test application provides 5 interactive test scenarios:

#### Test 1: Basic Popover
- **Purpose**: Verify basic popover functionality
- **Steps**:
  1. Tap "Test 1: Basic Popover" button
  2. Verify popover appears anchored to button
  3. Check text is displayed correctly
  4. Tap outside to dismiss
- **Expected Result**: Simple text popover appears and dismisses correctly

#### Test 2: Custom Content
- **Purpose**: Verify complex content display
- **Steps**:
  1. Tap "Test 2: Custom Content" button
  2. Verify all controls are visible (Label, Entry, Button)
  3. Test input field is interactive
  4. Click "Close" button
- **Expected Result**: Popover with multiple controls displays and functions correctly

#### Test 3: Arrow Direction
- **Purpose**: Verify arrow direction configuration
- **Steps**:
  1. Tap "Test 3: Arrow Directions" button
  2. Check arrow points upward
  3. Verify light blue background
  4. Tap outside to dismiss
- **Expected Result**: Popover appears with upward arrow and custom color

#### Test 4: Positioned Popover
- **Purpose**: Verify position-based anchoring
- **Steps**:
  1. Tap "Test 4: Positioned Popover" button
  2. Verify popover appears near screen center
  3. Check it's not anchored to a view
  4. Tap outside to dismiss
- **Expected Result**: Popover appears at specified coordinates

#### Test 5: Interactive Content
- **Purpose**: Verify interactive elements work correctly
- **Steps**:
  1. Tap "Test 5: Interactive Content" button
  2. Click "Interact" button multiple times
  3. Verify counter increments
  4. Click "Close & Verify"
- **Expected Result**: Interactions are tracked and reported

### Running UI Tests

#### Build and Run

**Android:**
```bash
dotnet build -t:Run -f net10.0-android tests/TR.Maui.AnchorPopover.UITests/TR.Maui.AnchorPopover.UITests.csproj
```

**iOS (macOS only):**
Open in Visual Studio for Mac and run

**Windows:**
Open in Visual Studio 2022 and run

### Manual Testing Checklist

For each platform, verify:

- [ ] All test buttons are visible and functional
- [ ] Test 1: Basic popover displays correctly
- [ ] Test 2: Custom content is fully visible
- [ ] Test 3: Arrow direction is appropriate
- [ ] Test 4: Positioned popover appears at center
- [ ] Test 5: Interactive elements work
- [ ] All popovers dismiss on outside tap
- [ ] Status label updates correctly
- [ ] No visual artifacts or crashes

## Continuous Integration

### GitHub Actions

The project includes three automated workflows:

#### 1. Build and Test Workflow (`.github/workflows/build.yml`)
Runs on: Ubuntu (Linux)
- Builds library, sample, and test projects
- Validates compilation on Android target
- Uploads NuGet package artifacts

#### 2. Unit Tests Workflow (`.github/workflows/unit-tests.yml`)
Runs on: Ubuntu and macOS

**Ubuntu Job:**
- Installs Android emulator (API 33)
- Runs unit tests on Android target
- Uploads test results as artifacts

**macOS Job:**
- Installs iOS/macOS workloads
- Boots iOS simulator (iPhone 15)
- Runs unit tests on iOS target
- Uploads test results as artifacts

#### 3. UI Tests Workflow (`.github/workflows/ui-tests.yml`)
Runs on: Ubuntu and macOS

**Android Job:**
- Builds UI test app for Android
- Deploys to Android emulator
- Serves as smoke test for app deployment

**iOS Job:**
- Builds UI test app for iOS
- Deploys to iOS simulator
- Serves as smoke test for app deployment

### Workflow Triggers

All workflows trigger on:
- Push to `main` or `develop` branches
- Pull requests to `main` or `develop` branches
- Manual workflow dispatch

### Test Execution in CI

**Android Tests:**
- Executed in Android emulator (API 33, x86_64, Pixel 6 profile)
- Automated via `reactivecircus/android-emulator-runner` action
- Test results uploaded as TRX files

**iOS Tests:**
- Executed in iOS simulator (iPhone 15)
- Requires macOS runner
- Test results uploaded as TRX files
- Simulator lifecycle managed automatically

### Viewing Test Results

Test results are available as workflow artifacts:
- `test-results-ubuntu-android` - Android unit test results
- `test-results-macos-ios` - iOS unit test results
- `ui-tests-apk` - Built Android UI test APK

Download artifacts from the Actions tab in GitHub to view detailed test results.

## Test Coverage

### What's Covered
✅ Core configuration classes (PopoverOptions, PopoverArrowDirection)  
✅ Factory method functionality  
✅ Basic interface compliance  
✅ Visual rendering (manual)  
✅ Interactive behavior (manual)  
✅ Platform-specific implementations (manual)  

### What's Not Covered (Future Work)
❌ Platform-specific implementation logic (requires mocking)  
❌ Automated UI interaction tests  
❌ Performance tests  
❌ Accessibility tests  
❌ Memory leak detection  

## Adding New Tests

### Adding Unit Tests

1. Create a new test class in `tests/TR.Maui.AnchorPopover.UnitTests/`
2. Follow naming convention: `[ClassName]Tests.cs`
3. Use xUnit attributes: `[Fact]`, `[Theory]`, `[InlineData]`
4. Follow Arrange-Act-Assert pattern
5. Add descriptive test names

Example:
```csharp
[Fact]
public void MyClass_MyMethod_ReturnsExpectedValue()
{
    // Arrange
    var myClass = new MyClass();
    
    // Act
    var result = myClass.MyMethod();
    
    // Assert
    Assert.Equal(expectedValue, result);
}
```

### Adding UI Tests

1. Add a new button to `MainPage.xaml`
2. Create event handler in `MainPage.xaml.cs`
3. Implement test scenario
4. Update status label with results
5. Document test in UI Tests README

## Troubleshooting

### Unit Tests Won't Run
- **Issue**: Tests compile but don't execute
- **Solution**: Ensure Android emulator or device is connected

### UI Tests Crash on Launch
- **Issue**: App crashes immediately
- **Solution**: Check platform-specific requirements are met

### Tests Pass Locally but Fail in CI
- **Issue**: Environment differences
- **Solution**: Review CI configuration and platform requirements

## Best Practices

1. **Write descriptive test names** - Test names should explain what is being tested
2. **Keep tests focused** - Each test should verify one specific behavior
3. **Use Arrange-Act-Assert** - Structure tests clearly
4. **Test edge cases** - Don't just test the happy path
5. **Update tests with code changes** - Keep tests in sync with implementation
6. **Document manual test procedures** - Make UI tests repeatable

## Resources

- [xUnit Documentation](https://xunit.net/)
- [.NET MAUI Testing](https://learn.microsoft.com/en-us/dotnet/maui/deployment/testing)
- [GitHub Actions for .NET](https://docs.github.com/en/actions/automating-builds-and-tests/building-and-testing-net)
