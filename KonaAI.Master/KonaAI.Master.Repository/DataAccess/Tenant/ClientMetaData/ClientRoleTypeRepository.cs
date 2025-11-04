using KonaAI.Master.Repository.Common;
using KonaAI.Master.Repository.DataAccess.Tenant.ClientMetaData.Interface;
using KonaAI.Master.Repository.Domain.Tenant.ClientMetaData;

namespace KonaAI.Master.Repository.DataAccess.Tenant.ClientMetaData;

/// <summary>
/// Repository implementation for managing <see cref="ClientRoleType"/> metadata.
/// </summary>
/// <remarks>
/// Inherits common CRUD and query operations from <see cref="GenericRepository{TContext, TEntity}"/>,
/// using <see cref="DefaultContext"/> as the EF Core context.
/// </remarks>
/// <param name="context">The EF Core <see cref="DefaultContext"/> instance used for data access.</param>
/// <seealso cref="GenericRepository{DefaultContext, ClientRoleType}"/>
/// <seealso cref="IClientRoleTypeRepository"/>
/// <seealso cref="DefaultContext.RoleTypes"/>
public class ClientRoleTypeRepository(DefaultContext context)
    : GenericRepository<DefaultContext, ClientRoleType>(context),
        IClientRoleTypeRepository
{
}