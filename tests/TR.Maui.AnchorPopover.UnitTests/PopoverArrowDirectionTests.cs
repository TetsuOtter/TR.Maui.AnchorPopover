using Xunit;

namespace TR.Maui.AnchorPopover.UnitTests;

/// <summary>
/// Unit tests for PopoverArrowDirection enum.
/// </summary>
public class PopoverArrowDirectionTests
{
    [Fact]
    public void PopoverArrowDirection_EnumValues_AreCorrect()
    {
        // Assert
        Assert.Equal(0, (int)PopoverArrowDirection.Any);
        Assert.Equal(1, (int)PopoverArrowDirection.Up);
        Assert.Equal(2, (int)PopoverArrowDirection.Down);
        Assert.Equal(4, (int)PopoverArrowDirection.Left);
        Assert.Equal(8, (int)PopoverArrowDirection.Right);
    }

    [Fact]
    public void PopoverArrowDirection_FlagsAttribute_AllowsCombination()
    {
        // Act
        var combined = PopoverArrowDirection.Up | PopoverArrowDirection.Down;

        // Assert
        Assert.True(combined.HasFlag(PopoverArrowDirection.Up));
        Assert.True(combined.HasFlag(PopoverArrowDirection.Down));
        Assert.False(combined.HasFlag(PopoverArrowDirection.Left));
    }

    [Fact]
    public void PopoverArrowDirection_MultipleFlagsCombined_WorkCorrectly()
    {
        // Act
        var combined = PopoverArrowDirection.Up | PopoverArrowDirection.Down | PopoverArrowDirection.Left;

        // Assert
        Assert.True(combined.HasFlag(PopoverArrowDirection.Up));
        Assert.True(combined.HasFlag(PopoverArrowDirection.Down));
        Assert.True(combined.HasFlag(PopoverArrowDirection.Left));
        Assert.False(combined.HasFlag(PopoverArrowDirection.Right));
    }

    [Theory]
    [InlineData(PopoverArrowDirection.Any)]
    [InlineData(PopoverArrowDirection.Up)]
    [InlineData(PopoverArrowDirection.Down)]
    [InlineData(PopoverArrowDirection.Left)]
    [InlineData(PopoverArrowDirection.Right)]
    public void PopoverArrowDirection_AllValues_AreDefined(PopoverArrowDirection direction)
    {
        // Assert
        Assert.True(Enum.IsDefined(typeof(PopoverArrowDirection), direction));
    }
}
