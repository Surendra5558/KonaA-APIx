using KonaAI.Master.Repository.Common;
using KonaAI.Master.Repository.DataAccess.Master.App.Interface;
using KonaAI.Master.Repository.DataAccess.Master.UserMetaData.Interface;
using KonaAI.Master.Repository.Domain.Master.App;
using KonaAI.Master.Repository.Domain.Master.UserMetaData;

namespace KonaAI.Master.Repository.DataAccess.Master.UserMetaData;

/// <summary>
/// Repository for performing CRUD operations on <see cref="Client"/> entities within the <see cref="DefaultContext"/>.
/// Inherits generic data access methods from <see cref="GenericRepository{DefaultContext, Client}"/>
/// and implements <see cref="IClientRepository"/>.
/// </summary>
public class ProjectDepartmentRepository(DefaultContext context)
    : GenericRepository<DefaultContext, ProjectDepartment>(context), IProjectDepartmentRepository
{
}