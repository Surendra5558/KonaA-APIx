using KonaAI.Master.Repository.Common;
using KonaAI.Master.Repository.DataAccess.Master.UserMetaData.Interface;
using KonaAI.Master.Repository.Domain.Master.UserMetaData;

namespace KonaAI.Master.Repository.DataAccess.Master.UserMetaData;

/// <summary>
/// Repository for performing CRUD operations on ProjectRiskArea entities within the <see cref="DefaultContext"/>.
/// Inherits generic data access methods from <see cref="GenericRepository{DefaultContext, RiskArea}"/>
/// and implements <see cref="IProjectRiskAreaRepository"/>.
/// </summary>
public class ProjectRiskAreaRepository(DefaultContext context)
    : GenericRepository<DefaultContext, ProjectRiskArea>(context), IProjectRiskAreaRepository
{
}