using KonaAI.Master.Model.Tenant.Client.SaveModel;
using KonaAI.Master.Model.Tenant.Client.ViewModel;

namespace KonaAI.Master.Business.Tenant.Client.Logic.Interface;

public interface IClientUserBusiness
{
    /// <summary>
    /// Retrieves all client users with their related metadata and modules.
    /// </summary>
    /// <returns>A queryable collection of all client project view models.</returns>
    Task<IQueryable<ClientUserViewModel>> GetAsync();

    /// <summary>
    /// Retrieves a specific client user by its unique identifier with related metadata and modules.
    /// </summary>
    /// <param name="rowId">The unique identifier of the client project.</param>
    /// <returns>The client user view model if found; otherwise, null.</returns>
    Task<ClientUserViewModel?> GetByRowIdAsync(Guid rowId);

    /// <summary>
    /// Creates a new client user using the provided model.
    /// </summary>
    /// <param name="user">The user data to create.</param>
    /// <returns>The number of records affected.</returns>
    Task<int> CreateAsync(ClientUserCreateModel user);

    /// <summary>
    /// Asynchronously deletes a client user identified by the specified row identifier.
    /// </summary>
    /// <param name="rowId">The unique row identifier (GUID) of the client to delete.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the number of records affected.
    /// </returns>
    Task<int> DeleteAsync(Guid rowId);

    /// <summary>
    /// Updates the specified row in the database with the provided user information.
    /// </summary>
    /// <remarks>The method performs an update operation asynchronously. Ensure that the provided <paramref
    /// name="rowId"/> corresponds to an existing row in the database.</remarks>
    /// <param name="rowId">The unique identifier of the row to update.</param>
    /// <param name="user">The user data to update the row with. Must contain valid values for all required fields.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the number of rows affected by the
    /// update.</returns>
    Task<int> UpdateAsync(Guid rowId, ClientUserUpdateModel user);
}