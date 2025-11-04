using KonaAI.Master.Repository.Common;
using KonaAI.Master.Repository.DataAccess.Master.MetaData.Interface;
using KonaAI.Master.Repository.Domain.Master.MetaData;

namespace KonaAI.Master.Repository.DataAccess.Master.MetaData;

/// <summary>
/// Repository for performing CRUD operations on <see cref="Navigation"/> entities within the <see cref="DefaultContext"/>.
/// Inherits generic data access methods from <see cref="GenericRepository{DefaultContext, AppNavigation}"/>
/// and implements <see cref="INavigationRepository"/>.
/// </summary>
public class NavigationRepository(DefaultContext context)
    : GenericRepository<DefaultContext, Navigation>(context), INavigationRepository
{
}