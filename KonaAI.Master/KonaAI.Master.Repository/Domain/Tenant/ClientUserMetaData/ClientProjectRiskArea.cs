using KonaAI.Master.Repository.Common.Domain;

namespace KonaAI.Master.Repository.Domain.Tenant.ClientUserMetaData;

/// <summary>
/// Represents a risk area within the organization or system.
/// Inherits common metadata properties (Name, Description, OrderBy, audit fields) from <see cref="BaseMetaDataDomain"/>.
/// </summary>
/// <remarks>
/// Can be extended with relationships to modules, departments, or processes
/// via EF Core navigation properties.
/// </remarks>
public class ClientProjectRiskArea : BaseClientMetaDataDomain
{
    /// <summary>
    /// Gets or sets the client project identifier.
    /// </summary>
    /// <value>
    /// The client project identifier.
    /// </value>
    public Guid ClientProjectId { get; set; }

    /// <summary>
    /// Gets or sets the project risk area identifier.
    /// </summary>
    /// <value>
    /// The project risk area identifier.
    /// </value>
    public long ProjectRiskAreaId { get; set; }
}