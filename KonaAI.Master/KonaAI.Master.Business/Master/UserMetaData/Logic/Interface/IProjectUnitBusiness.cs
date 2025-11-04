using KonaAI.Master.Model.Common;

namespace KonaAI.Master.Business.Master.UserMetaData.Logic.Interface;

/// <summary>
/// Defines business operations for master-level Project Units.
/// </summary>
public interface IProjectUnitBusiness
{
    /// <summary>
    /// Retrieves master Project Units projected to MetaDataViewModel.
    /// </summary>
    Task<IQueryable<MetaDataViewModel>> GetAsync();
}