using KonaAI.Master.Model.Master.App.SaveModel;
using KonaAI.Master.Model.Master.App.ViewModel;

namespace KonaAI.Master.Business.Master.App.Logic.Interface;

/// <summary>
/// Provides business operations for managing clients, including retrieval, creation, update, and deletion.
/// </summary>
public interface IClientBusiness
{
    /// <summary>
    /// Asynchronously retrieves a queryable collection of all clients.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains an <see cref="IQueryable{ClientViewModel}"/>
    /// representing the collection of clients.
    /// </returns>
    Task<IQueryable<ClientViewModel>> GetAsync();

    /// <summary>
    /// Asynchronously retrieves a client by its unique row identifier.
    /// </summary>
    /// <param name="rowId">The unique row identifier (GUID) of the client.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the <see cref="ClientViewModel"/>
    /// for the specified client.
    /// </returns>
    Task<ClientViewModel> GetByRowIdAsync(Guid rowId);

    /// <summary>
    /// Asynchronously creates a new client using the provided client creation model.
    /// </summary>
    /// <param name="client">The <see cref="ClientCreateModel"/> containing the data for the new client.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the number of records affected.
    /// </returns>
    Task<int> CreateAsync(ClientCreateModel client);

    /// <summary>
    /// Asynchronously updates an existing client identified by the specified row identifier using the provided update model.
    /// </summary>
    /// <param name="rowId">The unique row identifier (GUID) of the client to update.</param>
    /// <param name="client">The <see cref="ClientUpdateModel"/> containing the updated client data.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the number of records affected.
    /// </returns>
    Task<int> UpdateAsync(Guid rowId, ClientUpdateModel client);

    /// <summary>
    /// Asynchronously deletes a client identified by the specified row identifier.
    /// </summary>
    /// <param name="rowId">The unique row identifier (GUID) of the client to delete.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the number of records affected.
    /// </returns>
    Task<int> DeleteAsync(Guid rowId);
}