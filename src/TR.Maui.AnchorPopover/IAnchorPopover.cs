namespace TR.Maui.AnchorPopover;

/// <summary>
/// Interface for creating and managing popovers anchored to specific views.
/// </summary>
public interface IAnchorPopover
{
    /// <summary>
    /// Shows a popover with the specified content, anchored to the given view.
    /// </summary>
    /// <param name="content">The MAUI view to display in the popover.</param>
    /// <param name="anchor">The view to anchor the popover to.</param>
    /// <param name="options">Optional configuration for the popover.</param>
    /// <returns>A task that completes when the popover is dismissed.</returns>
    Task ShowAsync(View content, View anchor, PopoverOptions? options = null);

    /// <summary>
    /// Shows a popover with the specified content, anchored to a specific location.
    /// </summary>
    /// <param name="content">The MAUI view to display in the popover.</param>
    /// <param name="anchorBounds">The rectangle (in screen coordinates) to anchor the popover to.</param>
    /// <param name="options">Optional configuration for the popover.</param>
    /// <returns>A task that completes when the popover is dismissed.</returns>
    Task ShowAsync(View content, Rect anchorBounds, PopoverOptions? options = null);

    /// <summary>
    /// Dismisses the currently displayed popover.
    /// </summary>
    void Dismiss();

    /// <summary>
    /// Gets a value indicating whether a popover is currently being displayed.
    /// </summary>
    bool IsShowing { get; }
}
