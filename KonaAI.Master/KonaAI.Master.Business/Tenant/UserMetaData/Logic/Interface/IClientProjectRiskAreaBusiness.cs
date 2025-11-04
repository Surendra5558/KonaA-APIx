using KonaAI.Master.Model.Common;

namespace KonaAI.Master.Business.Tenant.UserMetaData.Logic.Interface;

/// <summary>
/// Defines business logic operations related to <see cref="MetaDataViewModel"/>.
/// </summary>
public interface IClientProjectRiskAreaBusiness
{
    /// <summary>
    /// Retrieves all available risk areas asynchronously.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// The task result contains an <see cref="IQueryable{MetaDataViewModel}"/>
    /// representing the collection of risk areas.
    /// </returns>
    Task<IQueryable<MetaDataViewModel>> GetAsync();
}