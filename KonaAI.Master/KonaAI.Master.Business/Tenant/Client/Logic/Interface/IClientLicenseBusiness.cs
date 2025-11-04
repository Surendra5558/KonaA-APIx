using KonaAI.Master.Model.Tenant.Client.SaveModel;
using KonaAI.Master.Model.Tenant.Client.ViewModel;

namespace KonaAI.Master.Business.Tenant.Client.Logic.Interface;

/// <summary>
/// Defines the contract for business operations related to Client License Info.
/// Provides methods for retrieving, creating, updating, and deleting license info records.
/// </summary>
public interface IClientLicenseBusiness
{
    /// <summary>
    /// Retrieves all active client license info records.
    /// </summary>
    /// <returns>
    /// A queryable collection of <see cref="ClientLicenseViewModel"/>.
    /// </returns>
    Task<IQueryable<ClientLicenseViewModel>> GetAsync();

    /// <summary>
    /// Retrieves a client license info record by its unique identifier.
    /// </summary>
    /// <param name="rowId">The unique identifier of the license info record.</param>
    /// <returns>
    /// The <see cref="ClientLicenseViewModel"/> if found; otherwise, null.
    /// </returns>
    Task<ClientLicenseViewModel?> GetByRowIdAsync(Guid rowId);

    /// <summary>
    /// Updates the specified client license record in the database with the provided license information.
    /// </summary>
    /// <remarks>
    /// This method performs an asynchronous update operation.
    /// Ensure that the provided <paramref name="rowId"/> corresponds to an existing license record in the database.
    /// </remarks>
    /// <param name="rowId">The unique identifier of the client license record to update.</param>
    /// <param name="license">The license data used to update the existing record. Must contain valid values for all required fields.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the number of rows affected by the update.
    /// </returns>
    Task<int> UpdateAsync(Guid rowId, ClientLicenseUpdateModel license);

    /// <summary>
    /// Deletes a client license info record by its unique identifier.
    /// </summary>
    /// <param name="rowId">The unique identifier of the license info record to delete.</param>
    /// <returns>
    /// An integer indicating the number of records deleted.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown if no license info record exists for the provided <paramref name="rowId"/>.
    /// </exception>
    Task<int> DeleteAsync(Guid rowId);
}