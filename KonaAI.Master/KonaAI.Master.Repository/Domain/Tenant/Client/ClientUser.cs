using KonaAI.Master.Repository.Common.Domain;

namespace KonaAI.Master.Repository.Domain.Tenant.Client;

/// <summary>
/// Represents the association between a client and a user.
/// Inherits common audit and identity properties from <see cref="BaseDomain"/>.
/// </summary>
public class ClientUser : BaseClientDomain
{
    /// <summary>
    /// Gets or sets the unique identifier of the associated user.
    /// </summary>
    public long UserId { get; set; }
}