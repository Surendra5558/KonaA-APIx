using KonaAI.Master.Model.Common;

namespace KonaAI.Master.Business.Master.UserMetaData.Logic.Interface;

/// <summary>
/// Defines business operations for master-level Project Risk Areas.
/// </summary>
public interface IProjectRiskAreaBusiness
{
    /// <summary>
    /// Retrieves master Project Risk Areas projected to MetaDataViewModel.
    /// </summary>
    Task<IQueryable<MetaDataViewModel>> GetAsync();
}