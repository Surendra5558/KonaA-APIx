using KonaAI.Master.Model.Common;
using KonaAI.Master.Model.Tenant.Client.SaveModel;

namespace KonaAI.Master.Business.Tenant.MetaData.Logic.Interface;

public interface IClientRoleTypeBusiness
{
    /// <summary>
    /// Asynchronously retrieves a queryable collection of all role types.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains an <see cref="IQueryable{ClientRoleTypeViewModel}"/>
    /// representing the collection of role types.
    /// </returns>
    Task<IQueryable<MetaDataViewModel>> GetAsync();

    /// <summary>
    /// Asynchronously retrieves a role type by its unique row identifier.
    /// </summary>
    /// <param name="rowId">The unique row identifier (GUID) of the role type.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the <see cref="MetaDataViewModel"/>
    /// for the specified role type.
    /// </returns>
    Task<MetaDataViewModel> GetByRowIdAsync(Guid rowId);

    /// <summary>
    /// Asynchronously creates a new role type using the provided creation model.
    /// </summary>
    /// <param name="roleType">The <see cref="ClientRoleTypeCreateModel"/> containing the data for the new role type.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the number of records affected.
    /// </returns>
    Task<int> CreateAsync(ClientRoleTypeCreateModel roleType);

    /// <summary>
    /// Asynchronously updates an existing role type identified by the specified row identifier using the provided update model.
    /// </summary>
    /// <param name="rowId">The unique row identifier (GUID) of the role type to update.</param>
    /// <param name="roleType">The <see cref="ClientRoleTypeUpdateModel"/> containing the updated role type data.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the number of records affected.
    /// </returns>
    Task<int> UpdateAsync(Guid rowId, ClientRoleTypeUpdateModel roleType);

    /// <summary>
    /// Asynchronously deletes a role type identified by the specified row identifier.
    /// </summary>
    /// <param name="rowId">The unique row identifier (GUID) of the role type to delete.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the number of records affected.
    /// </returns>
    Task<int> DeleteAsync(Guid rowId);
}