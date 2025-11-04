using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Domain.Tenant.Client;
using KonaAI.Master.Repository.Domain.Tenant.ClientUserMetaData;

namespace KonaAI.Master.Repository.DataAccess.Tenant.Client.Interface;

/// <summary>
/// Defines repository operations for <see cref="ClientProjectUser"/> entities within the <see cref="DefaultContext"/>.
/// Inherits standard CRUD and query methods from <see cref="IRepository{DefaultContext, ClientProjectUser}"/>,
/// including asynchronous retrieval, addition, update, and deletion of <see cref="ClientProjectUser"/> records.
/// </summary>
public interface IClientProjectUserRepository : IRepository<DefaultContext, ClientProjectUser>
{
}