using KonaAI.Master.Model.Common;

namespace KonaAI.Master.Business.Tenant.UserMetaData.Logic.Interface;

/// <summary>
/// Defines the contract for business logic operations related to <see cref="MetaDataViewModel"/>.
/// </summary>
public interface IClientProjectUnitBusiness
{
    /// <summary>
    /// Retrieves all available <see cref="MetaDataViewModel"/> records.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation,
    /// containing an <see cref="IQueryable{T}"/> of <see cref="MetaDataViewModel"/> objects.
    /// </returns>
    Task<IQueryable<MetaDataViewModel>> GetAsync();
}