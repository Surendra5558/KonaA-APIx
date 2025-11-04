using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Domain.Tenant.ClientUserMetaData;

namespace KonaAI.Master.Repository.DataAccess.Tenant.ClientUserMetaData.Interface;

/// <summary>
/// Repository contract for managing <see cref="ClientProjectDepartment"/> metadata in a tenant/client context.
/// </summary>
/// <remarks>
/// Inherits asynchronous CRUD and query operations from <see cref="IRepository{TContext, TEntity}"/> using
/// <see cref="DefaultContext"/> as the EF Core context and <see cref="ClientProjectDepartment"/> as the entity.
/// </remarks>
/// <seealso cref="IRepository{DefaultContext, ClientProjectDepartment}"/>
/// <seealso cref="DefaultContext.ProjectDepartments"/>
public interface IClientProjectDepartmentRepository : IRepository<DefaultContext, ClientProjectDepartment>
{
}