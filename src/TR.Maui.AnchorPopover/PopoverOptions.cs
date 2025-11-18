namespace TR.Maui.AnchorPopover;

/// <summary>
/// Configuration options for displaying a popover.
/// </summary>
public class PopoverOptions
{
    /// <summary>
    /// Gets or sets the preferred arrow direction for the popover.
    /// </summary>
    public PopoverArrowDirection ArrowDirection { get; set; } = PopoverArrowDirection.Any;

    /// <summary>
    /// Gets or sets a value indicating whether the popover should be modal.
    /// When true, the popover blocks interaction with the rest of the application.
    /// </summary>
    public bool IsModal { get; set; } = false;

    /// <summary>
    /// Gets or sets the preferred width of the popover content.
    /// If not set, the content will determine its own size.
    /// </summary>
    public double? PreferredWidth { get; set; }

    /// <summary>
    /// Gets or sets the preferred height of the popover content.
    /// If not set, the content will determine its own size.
    /// </summary>
    public double? PreferredHeight { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the popover should dismiss when tapping outside.
    /// Default is true.
    /// </summary>
    public bool DismissOnTapOutside { get; set; } = true;

    /// <summary>
    /// Gets or sets the background color of the popover.
    /// </summary>
    public Color? BackgroundColor { get; set; }
}
