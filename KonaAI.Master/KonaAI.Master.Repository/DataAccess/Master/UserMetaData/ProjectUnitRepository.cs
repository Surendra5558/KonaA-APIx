using KonaAI.Master.Repository.Common;
using KonaAI.Master.Repository.DataAccess.Master.UserMetaData.Interface;
using KonaAI.Master.Repository.Domain.Master.UserMetaData;

namespace KonaAI.Master.Repository.DataAccess.Master.UserMetaData;

/// <summary>
/// Repository for performing CRUD operations on ProjectUnit entities within the <see cref="DefaultContext"/>.
/// Inherits generic data access methods from <see cref="GenericRepository{DefaultContext, BusinessUnit}"/>
/// and implements <see cref="IProjectUnitRepository"/>.
/// </summary>
public class ProjectUnitRepository(DefaultContext context)
    : GenericRepository<DefaultContext, ProjectUnit>(context), IProjectUnitRepository
{
}