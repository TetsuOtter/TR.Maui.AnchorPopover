namespace TR.Maui.AnchorPopover;

/// <summary>
/// Specifies the direction of the arrow for the popover.
/// </summary>
[Flags]
public enum PopoverArrowDirection
{
    /// <summary>
    /// No arrow direction specified. The system will choose the best direction.
    /// </summary>
    Any = 0,

    /// <summary>
    /// Arrow points upward.
    /// </summary>
    Up = 1 << 0,

    /// <summary>
    /// Arrow points downward.
    /// </summary>
    Down = 1 << 1,

    /// <summary>
    /// Arrow points to the left.
    /// </summary>
    Left = 1 << 2,

    /// <summary>
    /// Arrow points to the right.
    /// </summary>
    Right = 1 << 3
}
