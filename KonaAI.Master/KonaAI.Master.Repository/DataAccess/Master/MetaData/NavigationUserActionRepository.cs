using KonaAI.Master.Repository.Common;
using KonaAI.Master.Repository.DataAccess.Master.MetaData.Interface;
using KonaAI.Master.Repository.Domain.Master.MetaData;

namespace KonaAI.Master.Repository.DataAccess.Master.MetaData;

/// <summary>
/// Repository for performing CRUD operations on <see cref="NavigationUserAction"/> entities within the <see cref="DefaultContext"/>.
/// Inherits generic data access methods from <see cref="GenericRepository{DefaultContext, AppNavigationAction}"/>
/// and implements <see cref="INavigationUserActionRepository"/>.
/// </summary>
public class NavigationUserActionRepository(DefaultContext context)
    : GenericRepository<DefaultContext, NavigationUserAction>(context), INavigationUserActionRepository
{
}