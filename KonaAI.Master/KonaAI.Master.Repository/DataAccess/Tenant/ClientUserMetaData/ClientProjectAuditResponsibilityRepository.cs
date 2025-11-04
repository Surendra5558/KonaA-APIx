using KonaAI.Master.Repository.Common;
using KonaAI.Master.Repository.DataAccess.Tenant.ClientUserMetaData.Interface;
using KonaAI.Master.Repository.Domain.Tenant.ClientUserMetaData;

namespace KonaAI.Master.Repository.DataAccess.Tenant.ClientUserMetaData;

/// <summary>
/// Repository implementation for managing <see cref="ClientProjectAuditResponsibility"/> metadata.
/// </summary>
/// <remarks>
/// Inherits common CRUD and query operations from <see cref="GenericRepository{TContext, TEntity}"/>,
/// using <see cref="DefaultContext"/> as the EF Core context.
/// </remarks>
/// <param name="context">The EF Core <see cref="DefaultContext"/> instance used for data access.</param>
/// <seealso cref="GenericRepository{DefaultContext, ClientProjectAuditResponsibility}"/>
/// <seealso cref="IClientProjectAuditResponsibilityRepository"/>
/// <seealso cref="DefaultContext.ProjectAuditResponsibilities"/>
public class ClientProjectAuditResponsibilityRepository(DefaultContext context)
    : GenericRepository<DefaultContext, ClientProjectAuditResponsibility>(context),
        IClientProjectAuditResponsibilityRepository
{
}