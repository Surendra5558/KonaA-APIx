using KonaAI.Master.Repository.Common;
using KonaAI.Master.Repository.DataAccess.Master.MetaData.Interface;
using KonaAI.Master.Repository.Domain.Master.MetaData;

namespace KonaAI.Master.Repository.DataAccess.Master.MetaData;

/// <summary>
/// Repository for performing CRUD operations on <see cref="ProjectStatus"/> entities within the <see cref="DefaultContext"/>.
/// Inherits generic data access methods from <see cref="GenericRepository{DefaultContext, ClientProjectStatus}"/>
/// and implements <see cref="IProjectStatusRepository"/>.
/// </summary>
///
public class ProjectStatusRepository(DefaultContext context)
    : GenericRepository<DefaultContext, ProjectStatus>(context), IProjectStatusRepository
{
}