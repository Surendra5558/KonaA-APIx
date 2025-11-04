using KonaAI.Master.Repository.Common;
using KonaAI.Master.Repository.DataAccess.Tenant.Client.Interface;
using KonaAI.Master.Repository.Domain.Tenant.Client;

namespace KonaAI.Master.Repository.DataAccess.Tenant.Client;

/// <summary>
/// Repository for performing CRUD operations on <see cref="ClientProject"/> entities within the <see cref="DefaultContext"/>.
/// Inherits generic data access methods from <see cref="GenericRepository{DefaultContext, ClientProject}"/>
/// and implements <see cref="IClientProjectRepository"/>.
/// </summary>
public class ClientProjectRepository(DefaultContext context)
    : GenericRepository<DefaultContext, ClientProject>(context), IClientProjectRepository
{
}