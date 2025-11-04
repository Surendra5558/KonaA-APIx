using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Domain.Master.MetaData;

namespace KonaAI.Master.Repository.DataAccess.Master.MetaData.Interface;

/// <summary>
/// Repository contract for managing <see cref="ProjectStatus"/> metadata entities.
/// </summary>
/// <remarks>
/// Inherits CRUD operations from <see cref="IRepository{DefaultContext, ProjectStatus}"/>.
/// </remarks>
/// <seealso cref="ProjectStatus"/>
/// <seealso cref="DefaultContext"/>
/// <seealso cref="IRepository{DefaultContext, ProjectStatus}"/>
public interface IProjectStatusRepository : IRepository<DefaultContext, ProjectStatus>
{
}