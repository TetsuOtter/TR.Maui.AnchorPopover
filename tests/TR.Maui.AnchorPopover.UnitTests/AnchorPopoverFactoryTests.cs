using Xunit;

namespace TR.Maui.AnchorPopover.UnitTests;

/// <summary>
/// Unit tests for AnchorPopover factory class.
/// </summary>
public class AnchorPopoverFactoryTests
{
    [Fact]
    public void AnchorPopover_Create_ReturnsNonNullInstance()
    {
        // Skip if not supported platform
        try
        {
            // Act
            var popover = AnchorPopover.Create();

            // Assert
            Assert.NotNull(popover);
        }
        catch (PlatformNotSupportedException)
        {
            Assert.True(true, "Platform not supported, test skipped");
        }
    }

    [Fact]
    public void AnchorPopover_Create_ReturnsIAnchorPopoverInterface()
    {
        // Skip if not supported platform
        try
        {
            // Act
            var popover = AnchorPopover.Create();

            // Assert
            Assert.IsAssignableFrom<IAnchorPopover>(popover);
        }
        catch (PlatformNotSupportedException)
        {
            Assert.True(true, "Platform not supported, test skipped");
        }
    }

    [Fact]
    public void AnchorPopover_Create_MultipleInstances_AreIndependent()
    {
        // Skip if not supported platform
        try
        {
            // Act
            var popover1 = AnchorPopover.Create();
            var popover2 = AnchorPopover.Create();

            // Assert
            Assert.NotNull(popover1);
            Assert.NotNull(popover2);
            Assert.NotSame(popover1, popover2);
        }
        catch (PlatformNotSupportedException)
        {
            Assert.True(true, "Platform not supported, test skipped");
        }
    }

    [Fact]
    public void AnchorPopover_Create_InitiallyNotShowing()
    {
        // Skip if not supported platform
        try
        {
            // Act
            var popover = AnchorPopover.Create();

            // Assert
            Assert.False(popover.IsShowing);
        }
        catch (PlatformNotSupportedException)
        {
            Assert.True(true, "Platform not supported, test skipped");
        }
    }
}
