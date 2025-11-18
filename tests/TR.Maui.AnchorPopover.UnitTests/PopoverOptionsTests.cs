using Xunit;

namespace TR.Maui.AnchorPopover.UnitTests;

/// <summary>
/// Unit tests for PopoverOptions configuration class.
/// </summary>
public class PopoverOptionsTests
{
    [Fact]
    public void PopoverOptions_DefaultValues_AreCorrect()
    {
        // Arrange & Act
        var options = new PopoverOptions();

        // Assert
        Assert.Equal(PopoverArrowDirection.Any, options.ArrowDirection);
        Assert.False(options.IsModal);
        Assert.Null(options.PreferredWidth);
        Assert.Null(options.PreferredHeight);
        Assert.True(options.DismissOnTapOutside);
        Assert.Null(options.BackgroundColor);
    }

    [Fact]
    public void PopoverOptions_ArrowDirection_CanBeSet()
    {
        // Arrange
        var options = new PopoverOptions();

        // Act
        options.ArrowDirection = PopoverArrowDirection.Up;

        // Assert
        Assert.Equal(PopoverArrowDirection.Up, options.ArrowDirection);
    }

    [Fact]
    public void PopoverOptions_IsModal_CanBeSet()
    {
        // Arrange
        var options = new PopoverOptions();

        // Act
        options.IsModal = true;

        // Assert
        Assert.True(options.IsModal);
    }

    [Fact]
    public void PopoverOptions_PreferredDimensions_CanBeSet()
    {
        // Arrange
        var options = new PopoverOptions
        {
            PreferredWidth = 300,
            PreferredHeight = 400
        };

        // Assert
        Assert.Equal(300, options.PreferredWidth);
        Assert.Equal(400, options.PreferredHeight);
    }

    [Fact]
    public void PopoverOptions_DismissOnTapOutside_CanBeToggled()
    {
        // Arrange
        var options = new PopoverOptions();

        // Act
        options.DismissOnTapOutside = false;

        // Assert
        Assert.False(options.DismissOnTapOutside);
    }

    [Fact]
    public void PopoverOptions_BackgroundColor_CanBeSet()
    {
        // Arrange
        var options = new PopoverOptions();
        var color = Colors.Red;

        // Act
        options.BackgroundColor = color;

        // Assert
        Assert.Equal(color, options.BackgroundColor);
    }

    [Fact]
    public void PopoverOptions_AllProperties_CanBeInitialized()
    {
        // Arrange & Act
        var options = new PopoverOptions
        {
            ArrowDirection = PopoverArrowDirection.Down,
            IsModal = true,
            PreferredWidth = 250,
            PreferredHeight = 350,
            DismissOnTapOutside = false,
            BackgroundColor = Colors.Blue
        };

        // Assert
        Assert.Equal(PopoverArrowDirection.Down, options.ArrowDirection);
        Assert.True(options.IsModal);
        Assert.Equal(250, options.PreferredWidth);
        Assert.Equal(350, options.PreferredHeight);
        Assert.False(options.DismissOnTapOutside);
        Assert.Equal(Colors.Blue, options.BackgroundColor);
    }
}
