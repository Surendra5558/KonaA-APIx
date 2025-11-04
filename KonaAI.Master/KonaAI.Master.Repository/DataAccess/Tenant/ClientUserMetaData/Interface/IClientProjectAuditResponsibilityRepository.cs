using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Domain.Tenant.ClientUserMetaData;

namespace KonaAI.Master.Repository.DataAccess.Tenant.ClientUserMetaData.Interface;

/// <summary>
/// Repository contract for managing <see cref="ClientProjectAuditResponsibility"/> metadata for a tenant/client.
/// </summary>
/// <remarks>
/// Inherits asynchronous CRUD and query operations from <see cref="IRepository{TContext, TEntity}"/> using
/// <see cref="DefaultContext"/> as the EF Core context and <see cref="ClientProjectAuditResponsibility"/> as the entity.
/// </remarks>
/// <seealso cref="IRepository{DefaultContext, ClientProjectAuditResponsibility}"/>
/// <seealso cref="DefaultContext.ProjectAuditResponsibilities"/>
public interface IClientProjectAuditResponsibilityRepository : IRepository<DefaultContext, ClientProjectAuditResponsibility>
{
}