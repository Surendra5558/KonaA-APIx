using KonaAI.Master.Model.Common;

namespace KonaAI.Master.Business.Master.MetaData.Logic.Interface;

/// <summary>
/// Defines the contract for business logic operations related to role types.
/// </summary>
public interface IRoleTypeBusiness
{
    /// <summary>
    /// Retrieves a collection of role types asynchronously.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation,
    /// containing an <see cref="IQueryable{T}"/> of <see cref="MetaDataViewModel"/> entities.
    /// </returns>
    Task<IQueryable<MetaDataViewModel>> GetAsync();
}