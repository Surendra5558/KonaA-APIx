using KonaAI.Master.Repository.Common.Domain;

namespace KonaAI.Master.Repository.Domain.Master.MetaData;

/// <summary>
/// Represents the association between a navigation menu item and a system action.
/// Inherits common metadata properties (Name, Description, OrderBy, audit fields) from <see cref="BaseMetaDataDomain"/>.
/// </summary>
/// <remarks>
/// This entity enables mapping of actions (e.g., Create, Read, Update, Delete) to specific navigation items.
/// Additional configuration or relationships can be added via EF Core navigation properties.
/// </remarks>
public class NavigationUserAction : BaseMetaDataDomain
{
    /// <summary>
    /// Gets or sets the identifier of the associated navigation menu item.
    /// </summary>
    public long NavigationId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the associated system action.
    /// </summary>
    public long UserActionId { get; set; }
}