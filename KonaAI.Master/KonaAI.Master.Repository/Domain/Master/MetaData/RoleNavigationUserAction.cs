using KonaAI.Master.Repository.Common.Domain;

namespace KonaAI.Master.Repository.Domain.Master.MetaData;

/// <summary>
/// Represents role-based permissions for navigation actions.
/// Inherits common metadata properties (Name, Description, OrderBy, audit fields) from <see cref="BaseMetaDataDomain"/>.
/// </summary>
/// <remarks>
/// This entity maps a specific role to a navigation action, enabling role-based access control.
/// Additional relationships or constraints can be configured via EF Core navigation properties.
/// </remarks>
public class RoleNavigationUserAction : BaseMetaDataDomain
{
    /// <summary>
    /// Gets or sets the identifier of the associated navigation action.
    /// </summary>
    public long NavigationUserActionId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the associated role type.
    /// </summary>
    public long RoleTypeId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the navigation item is visible to the user.
    /// </summary>
    public bool IsVisible { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the user can access the navigation item.
    /// </summary>
    public bool IsAccessible { get; set; }
}