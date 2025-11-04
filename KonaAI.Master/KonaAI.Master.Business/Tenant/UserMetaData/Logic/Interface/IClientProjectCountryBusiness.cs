using KonaAI.Master.Model.Common;

namespace KonaAI.Master.Business.Tenant.UserMetaData.Logic.Interface;

/// <summary>
/// Defines the contract for business logic operations related to countries.
/// </summary>
public interface IClientProjectCountryBusiness
{
    /// <summary>
    /// Retrieves a collection of countries asynchronously.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation,
    /// containing an <see cref="IQueryable{T}"/> of <see cref="MetaDataViewModel"/> entities.
    /// </returns>
    Task<IQueryable<MetaDataViewModel>> GetAsync();
}