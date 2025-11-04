using KonaAI.Master.Repository.Common;
using KonaAI.Master.Repository.DataAccess.Tenant.Client.Interface;
using KonaAI.Master.Repository.Domain.Tenant.Client;

namespace KonaAI.Master.Repository.DataAccess.Tenant.Client;

/// <summary>
/// Repository for performing CRUD operations on <see cref="ClientProjectModuleSourceType"/> entities within the <see cref="DefaultContext"/>.
/// Inherits generic data access methods from <see cref="GenericRepository{DefaultContext, ProjectModule}"/>
/// and implements <see cref="IClientProjectSourceModuleTypeRepository"/>.
/// </summary>
public class ClientProjectSourceModuleTypeRepository(DefaultContext context)
    : GenericRepository<DefaultContext, ClientProjectModuleSourceType>(context), IClientProjectSourceModuleTypeRepository
{
}