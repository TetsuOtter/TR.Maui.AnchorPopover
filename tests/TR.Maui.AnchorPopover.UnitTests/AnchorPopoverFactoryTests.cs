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
        // Act
        var popover = AnchorPopover.Create();

        // Assert
        Assert.NotNull(popover);
    }

    [Fact]
    public void AnchorPopover_Create_ReturnsIAnchorPopoverInterface()
    {
        // Act
        var popover = AnchorPopover.Create();

        // Assert
        Assert.IsAssignableFrom<IAnchorPopover>(popover);
    }

    [Fact]
    public void AnchorPopover_Create_MultipleInstances_AreIndependent()
    {
        // Act
        var popover1 = AnchorPopover.Create();
        var popover2 = AnchorPopover.Create();

        // Assert
        Assert.NotNull(popover1);
        Assert.NotNull(popover2);
        Assert.NotSame(popover1, popover2);
    }

    [Fact]
    public void AnchorPopover_Create_InitiallyNotShowing()
    {
        // Act
        var popover = AnchorPopover.Create();

        // Assert
        Assert.False(popover.IsShowing);
    }
}
