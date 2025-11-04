using KonaAI.Master.Model.Common;

namespace KonaAI.Master.Business.Master.MetaData.Logic.Interface;

/// <summary>
/// Defines the contract for business logic related to <see cref="MetaDataViewModel"/> entities.
/// </summary>
public interface IModuleSourceTypeBusiness
{
    /// <summary>
    /// Asynchronously retrieves a queryable collection of <see cref="MetaDataViewModel"/> entities.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// The task result contains an <see cref="IQueryable{MetaDataViewModel}"/> collection of modules.
    /// </returns>
    Task<IQueryable<MetaDataViewModel>> GetAsync();
}