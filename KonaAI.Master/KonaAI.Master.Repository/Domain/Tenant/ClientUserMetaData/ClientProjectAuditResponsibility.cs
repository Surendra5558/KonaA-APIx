using KonaAI.Master.Repository.Common.Domain;

namespace KonaAI.Master.Repository.Domain.Tenant.ClientUserMetaData;

/// <summary>
/// Represents a specific audit responsibility or role within the system.
/// Inherits common metadata properties (Name, Description, OrderBy, audit fields) from <see cref="BaseMetaDataDomain"/>.
/// </summary>
/// <remarks>
/// Can be extended in the future with relationships to users, departments, or modules
/// via EF Core navigation properties.
/// </remarks>
public class ClientProjectAuditResponsibility : BaseClientMetaDataDomain
{
    /// <summary>
    /// Gets or sets the client project identifier.
    /// </summary>
    /// <value>
    /// The client project identifier.
    /// </value>
    public Guid ClientProjectId { get; set; }

    /// <summary>
    /// Gets or sets the project audit responsibility identifier.
    /// </summary>
    /// <value>
    /// The project audit responsibility identifier.
    /// </value>
    public long ProjectAuditResponsibilityId { get; set; }
}