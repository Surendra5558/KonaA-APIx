using KonaAI.Master.Repository.Common.Domain;

namespace KonaAI.Master.Repository.Domain.Tenant.Client;

/// <summary>
/// Links a client project to a module-source type combination.
/// </summary>
public class ClientProjectModuleSourceType : BaseClientDomain
{
    /// <summary>
    /// Gets or sets the identifier of the associated client project.
    /// </summary>
    public long ClientProjectId { get; set; }

    /// <summary>
    /// Navigation to the associated client project.
    /// </summary>
    public ClientProject ClientProject { get; set; } = null!;

    /// <summary>
    /// Gets or sets the identifier of the associated module-source type.
    /// </summary>
    public long ModuleSourceTypeId { get; set; }
}