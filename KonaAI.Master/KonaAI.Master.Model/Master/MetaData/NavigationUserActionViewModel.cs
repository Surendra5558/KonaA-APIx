using KonaAI.Master.Model.Common;

namespace KonaAI.Master.Model.Master.MetaData;

/// <summary>
/// Represents the association between a navigation item and a user action in the system.
/// Inherits identity information from <see cref="BaseViewModel"/>.
/// </summary>
public class NavigationUserActionViewModel : BaseViewModel
{
    /// <summary>
    /// Gets or sets the unique row identifier of the navigation item.
    /// </summary>
    public Guid NavigationRowId { get; set; }

    /// <summary>
    /// Gets or sets the display name of the navigation item.
    /// </summary>
    public string NavigationName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique row identifier of the user action.
    /// </summary>
    public Guid UserActionRowId { get; set; }

    /// <summary>
    /// Gets or sets the display name of the user action.
    /// </summary>
    public string UserActionName { get; set; } = string.Empty;
}