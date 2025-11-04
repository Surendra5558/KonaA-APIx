using KonaAI.Master.Repository.Common;
using KonaAI.Master.Repository.DataAccess.Master.UserMetaData.Interface;
using KonaAI.Master.Repository.Domain.Master.UserMetaData;

namespace KonaAI.Master.Repository.DataAccess.Master.UserMetaData;

/// <summary>
/// Repository for performing CRUD operations on ProjectAuditResponsibility entities within the <see cref="DefaultContext"/>.
/// Inherits generic data access methods from <see cref="GenericRepository{DefaultContext, AuditResponsibility}"/>
/// and implements <see cref="IProjectAuditResponsibilityRepository"/>.
/// </summary>
public class ProjectAuditResponsibilityRepository(DefaultContext context)
    : GenericRepository<DefaultContext, ProjectAuditResponsibility>(context), IProjectAuditResponsibilityRepository
{
}