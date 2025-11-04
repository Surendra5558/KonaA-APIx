using KonaAI.Master.Model.Common;

namespace KonaAI.Master.Business.Master.UserMetaData.Logic.Interface;

/// <summary>
/// Defines business operations for master-level Project Departments.
/// </summary>
public interface IProjectDepartmentBusiness
{
    /// <summary>
    /// Retrieves master Project Departments projected to MetaDataViewModel.
    /// </summary>
    Task<IQueryable<MetaDataViewModel>> GetAsync();
}