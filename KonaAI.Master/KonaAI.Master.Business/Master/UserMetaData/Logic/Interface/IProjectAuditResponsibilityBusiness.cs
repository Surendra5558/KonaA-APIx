using KonaAI.Master.Model.Common;

namespace KonaAI.Master.Business.Master.UserMetaData.Logic.Interface;

/// <summary>
/// Defines business operations for master-level Project Audit Responsibilities.
/// </summary>
public interface IProjectAuditResponsibilityBusiness
{
    /// <summary>
    /// Retrieves all master Project Audit Responsibilities projected to MetaDataViewModel.
    /// </summary>
    /// <returns>IQueryable of MetaDataViewModel containing RowId, Name, Description, OrderBy.</returns>
    Task<IQueryable<MetaDataViewModel>> GetAsync();
}