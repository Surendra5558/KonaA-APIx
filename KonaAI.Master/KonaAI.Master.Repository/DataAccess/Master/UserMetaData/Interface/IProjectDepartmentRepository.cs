using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Domain.Master.UserMetaData;

namespace KonaAI.Master.Repository.DataAccess.Master.UserMetaData.Interface;

/// <summary>
/// Defines repository operations for <see cref="ProjectDepartmentRepository"/> entities within the <see cref="DefaultContext"/>.
/// Inherits standard CRUD and query methods from <see cref="IRepository{DefaultContext, ProjectDepartment}"/>,
/// including asynchronous retrieval, addition, update, and deletion of <see cref="ProjectDepartmentRepository"/> records.
/// </summary>
public interface IProjectDepartmentRepository : IRepository<DefaultContext, ProjectDepartment>
{
}