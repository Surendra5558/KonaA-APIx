using KonaAI.Master.Repository.Common.Domain;

namespace KonaAI.Master.Repository.Domain.Tenant.ClientUserMetaData;

/// <summary>
/// Represents a business unit within the organization.
/// Inherits common metadata properties (Name, Description, OrderBy, audit fields) from <see cref="BaseMetaDataDomain"/>.
/// </summary>
/// <remarks>
/// Can be extended with relationships to departments, projects, or employees
/// via EF Core navigation properties.
/// </remarks>
public class ClientProjectUnit : BaseClientMetaDataDomain
{
    /// <summary>
    /// Gets or sets the client project identifier.
    /// </summary>
    /// <value>
    /// The client project identifier.
    /// </value>
    public Guid ClientProjectId { get; set; }

    /// <summary>
    /// Gets or sets the project unit identifier.
    /// </summary>
    /// <value>
    /// The project unit identifier.
    /// </value>
    public long ProjectUnitId { get; set; }
}