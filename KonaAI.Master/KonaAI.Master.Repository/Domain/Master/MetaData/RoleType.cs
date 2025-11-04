using KonaAI.Master.Repository.Common.Domain;
using KonaAI.Master.Repository.Domain.Master.App;

namespace KonaAI.Master.Repository.Domain.Master.MetaData;

/// <summary>
/// Represents a role classification (e.g., Administrator, Manager, Viewer) with metadata such as
/// name, description, and ordering inherited from <see cref="BaseMetaDataDomain"/>.
/// </summary>
public class RoleType : BaseMetaDataDomain
{
    /// <summary>
    /// Gets or sets a value indicating whether this role type is a system-defined role.
    /// </summary>
    public bool IsSystemRole { get; set; } = true;

    /// <summary>
    /// Gets or sets the collection of users that are associated with this role type.
    /// </summary>
    /// <remarks>
    /// This is the inverse navigation of the foreign key held on the <c>User.RoleTypeId</c> property.
    /// </remarks>
    public ICollection<User> Users { get; set; } = new List<User>();
}