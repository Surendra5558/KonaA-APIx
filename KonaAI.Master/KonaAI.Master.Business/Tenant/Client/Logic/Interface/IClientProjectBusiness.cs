using KonaAI.Master.Model.Tenant.Client.SaveModel;
using KonaAI.Master.Model.Tenant.Client.ViewModel;

namespace KonaAI.Master.Business.Tenant.Client.Logic.Interface;

/// <summary>
/// Provides business operations for managing client projects,
/// including creation, update, and retrieval.
/// </summary>
public interface IClientProjectBusiness
{
    /// <summary>
    /// Retrieves all client projects with their related metadata and modules.
    /// </summary>
    /// <returns>A queryable collection of all client project view models.</returns>
    Task<IQueryable<ClientProjectViewModel>> GetAsync();

    /// <summary>
    /// Retrieves a specific client project by its unique row identifier (GUID) with related metadata and modules.
    /// </summary>
    /// <param name="rowId">The unique row identifier (GUID) of the client project.</param>
    /// <returns>The client project view model if found; otherwise, null.</returns>
    Task<ClientProjectViewModel?> GetByRowIdAsync(Guid rowId);

    /// <summary>
    /// Creates a new client project using the provided model.
    /// </summary>
    /// <param name="project">The project data to create.</param>
    /// <returns>The number of records affected.</returns>
    Task<int> CreateAsync(ClientProjectCreateModel project);

    /// <summary>
    /// Deletes a client project by its unique row identifier.
    /// </summary>
    /// <param name="rowId">The unique row identifier (GUID) of the project to delete.</param>
    /// <returns>The number of records affected (0 if not found, 1 if deleted).</returns>
    Task<int> DeleteAsync(Guid rowId);

}