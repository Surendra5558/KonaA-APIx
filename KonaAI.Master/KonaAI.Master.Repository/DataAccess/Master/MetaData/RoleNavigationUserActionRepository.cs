using KonaAI.Master.Repository.Common;
using KonaAI.Master.Repository.DataAccess.Master.MetaData.Interface;
using KonaAI.Master.Repository.Domain.Master.MetaData;

namespace KonaAI.Master.Repository.DataAccess.Master.MetaData;

/// <summary>
/// Repository for performing CRUD operations on <see cref="RoleNavigationUserAction"/> entities within the <see cref="DefaultContext"/>.
/// Inherits generic data access methods from <see cref="GenericRepository{DefaultContext, AppRoleNavigationUserAction}"/>
/// and implements <see cref="IRoleNavigationUserActionRepository"/>.
/// </summary>
public class RoleNavigationUserActionRepository(DefaultContext context)
    : GenericRepository<DefaultContext, RoleNavigationUserAction>(context), IRoleNavigationUserActionRepository
{
}