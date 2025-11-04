using KonaAI.Master.Repository.Common.Constants;
using KonaAI.Master.Repository.Common.Domain;
using KonaAI.Master.Repository.Common.Model;

namespace KonaAI.Master.Repository.Common.Interface;

/// <summary>
/// Provides methods to manage and apply user context information to domain entities.
/// </summary>
public interface IUserContextService
{
    /// <summary>
    /// Gets or sets the current user context containing user and client information.
    /// </summary>
    UserContext? UserContext { get; set; }

    /// <summary>
    /// Sets default values on the specified domain entity according to the current user context
    /// and the provided <paramref name="dataModes"/> operation.
    /// </summary>
    /// <typeparam name="T">The type of the domain entity, derived from <see cref="BaseDomain"/>.</typeparam>
    /// <param name="domain">The domain entity to update.</param>
    /// <param name="dataModes">The data operation mode (e.g., Add, Edit, Delete, DeActive).</param>
    void SetDomainDefaults<T>(T domain, DataModes dataModes) where T : BaseDomain;

    /// <summary>
    /// Sets default values on a list of domain entities according to the current user context
    /// and the provided <paramref name="dataModes"/> operation.
    /// </summary>
    /// <typeparam name="T">The type of the domain entities, derived from <see cref="BaseDomain"/>.</typeparam>
    /// <param name="domains">The list of domain entities to update.</param>
    /// <param name="dataModes">The data operation mode (e.g., Add, Edit, Delete, DeActive).</param>
    void SetDomainDefaults<T>(List<T> domains, DataModes dataModes) where T : BaseDomain;
}