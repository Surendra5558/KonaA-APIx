using KonaAI.Master.Repository.Common;
using KonaAI.Master.Repository.DataAccess.Tenant.Client.Interface;
using KonaAI.Master.Repository.Domain.Tenant.Client;
using KonaAI.Master.Repository.Domain.Tenant.ClientUserMetaData;

namespace KonaAI.Master.Repository.DataAccess.Tenant.Client;

/// <summary>
/// Repository for performing CRUD operations on <see cref="ClientProjectUser"/> entities within the <see cref="DefaultContext"/>.
/// Inherits generic data access methods from <see cref="GenericRepository{DefaultContext, ClientProjectUser}"/>
/// and implements <see cref="IClientProjectUserRepository"/>.
/// </summary>
public class ClientProjectUserRepository(DefaultContext context)
    : GenericRepository<DefaultContext, ClientProjectUser>(context), IClientProjectUserRepository
{
}