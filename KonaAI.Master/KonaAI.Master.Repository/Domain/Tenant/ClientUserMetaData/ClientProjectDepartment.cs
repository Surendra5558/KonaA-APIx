using KonaAI.Master.Repository.Common.Domain;

namespace KonaAI.Master.Repository.Domain.Tenant.ClientUserMetaData;

/// <summary>
/// Represents a business department within the organization.
/// Inherits common metadata properties (Name, Description, OrderBy, audit fields) from <see cref="BaseMetaDataDomain"/>.
/// </summary>
/// <remarks>
/// Can be extended with relationships to employees, projects, or other organizational structures
/// via EF Core navigation properties.
/// </remarks>
public class ClientProjectDepartment : BaseClientMetaDataDomain
{
    /// <summary>
    /// Gets or sets the client project identifier.
    /// </summary>
    /// <value>
    /// The client project identifier.
    /// </value>
    public Guid ClientProjectId { get; set; }

    /// <summary>
    /// Gets or sets the project department identifier.
    /// </summary>
    /// <value>
    /// The project department identifier.
    /// </value>
    public long ProjectDepartmentId { get; set; }
}