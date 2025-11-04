using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Domain.Tenant.ClientMetaData;

namespace KonaAI.Master.Repository.DataAccess.Tenant.ClientMetaData.Interface;

/// <summary>
/// Repository contract for managing <see cref="ClientRoleType"/> metadata for a tenant/client.
/// </summary>
/// <remarks>
/// Inherits asynchronous CRUD and query operations from <see cref="IRepository{TContext, TEntity}"/> using
/// <see cref="DefaultContext"/> as the EF Core context and <see cref="ClientRoleType"/> as the entity.
/// </remarks>
/// <seealso cref="IRepository{DefaultContext, ClientRoleType}"/>
/// <seealso cref="DefaultContext"/>
public interface IClientRoleTypeRepository : IRepository<DefaultContext, ClientRoleType>
{
}