using KonaAI.Master.Repository.Common.Domain;

namespace KonaAI.Master.Repository.Domain.Tenant.ClientMetaData;

/// <summary>
/// Client-specific role type metadata entity.
/// </summary>
public class ClientRoleType : BaseClientMetaDataDomain
{
    /// <summary>
    /// Gets or sets the unique identifier for the role type.
    /// </summary>
    public long RoleTypeId { get; set; }
}