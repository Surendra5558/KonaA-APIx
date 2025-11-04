using KonaAI.Master.Model.Common;

namespace KonaAI.Master.Model.Master.MetaData;

/// <summary>
/// Represents the association between a navigation item and a user action in the system.
/// Inherits identity information from <see cref="BaseViewModel"/>.
/// </summary>
public class NavigationViewModel : BaseViewModel
{
    /// <summary>
    /// Gets or sets the display name of the navigation item.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the unique row identifier of the user action.
    /// </summary>
    public Guid? ParentRowId { get; set; }

    /// <summary>
    /// Gets or sets the display name of the user action.
    /// </summary>
    public string? ParentName { get; set; } = null!;
}