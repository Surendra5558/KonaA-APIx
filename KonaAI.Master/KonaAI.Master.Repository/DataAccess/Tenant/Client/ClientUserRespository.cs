using KonaAI.Master.Repository.Common;
using KonaAI.Master.Repository.DataAccess.Tenant.Client.Interface;
using KonaAI.Master.Repository.Domain.Tenant.Client;

namespace KonaAI.Master.Repository.DataAccess.Tenant.Client;

/// <summary>
/// Repository for performing CRUD operations on <see cref="ClientUser"/> entities within the <see cref="DefaultContext"/>.
/// Inherits generic data access methods from <see cref="GenericRepository{DefaultContext, ClientUser}"/>
/// and implements <see cref="IClientUserRepository"/>.
/// </summary>
public class ClientUserRepository(DefaultContext context)
    : GenericRepository<DefaultContext, ClientUser>(context), IClientUserRepository
{
}