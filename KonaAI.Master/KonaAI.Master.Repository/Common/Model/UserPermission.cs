namespace KonaAI.Master.Repository.Common.Model;

/// <summary>
/// Represents a user's permission mapping for a specific navigation item and action.
/// </summary>
public class UserPermission
{
    /// <summary>
    /// Gets or sets the row identifier of the navigation entity associated with this permission.
    /// </summary>
    public Guid NavigationRowId { get; set; }

    /// <summary>
    /// Gets or sets the numeric identifier of the navigation entity.
    /// </summary>
    public long NavigationId { get; set; }

    /// <summary>
    /// Gets or sets the display name of the navigation entity.
    /// </summary>
    public string NavigationName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the row identifier of the user action associated with this permission.
    /// </summary>
    public Guid UserActionRowId { get; set; }

    /// <summary>
    /// Gets or sets the numeric identifier of the user action.
    /// </summary>
    public long UserActionId { get; set; }

    /// <summary>
    /// Gets or sets the display name of the user action.
    /// </summary>
    public string UserActionName { get; set; } = null!;
}