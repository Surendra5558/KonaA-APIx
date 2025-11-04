using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Domain.Master.UserMetaData;

namespace KonaAI.Master.Repository.DataAccess.Master.UserMetaData.Interface;

/// <summary>
/// Defines repository operations for ProjectAuditResponsibility entities within the <see cref="DefaultContext"/>.
/// Inherits standard CRUD and query methods from <see cref="IRepository{DefaultContext, AuditResponsibility}"/>.
/// </summary>
public interface IProjectAuditResponsibilityRepository : IRepository<DefaultContext, ProjectAuditResponsibility>
{
}