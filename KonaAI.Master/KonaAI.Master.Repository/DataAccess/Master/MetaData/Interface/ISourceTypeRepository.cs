using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Domain.Master.MetaData;

namespace KonaAI.Master.Repository.DataAccess.Master.MetaData.Interface;

/// <summary>
/// Repository contract for managing <see cref="SourceType"/> metadata entities.
/// </summary>
/// <remarks>
/// Inherits CRUD and query operations from <see cref="IRepository{DefaultContext, SourceType}"/>.
/// </remarks>
/// <seealso cref="SourceType"/>
/// <seealso cref="DefaultContext"/>
/// <seealso cref="IRepository{DefaultContext, SourceType}"/>
public interface ISourceTypeRepository : IRepository<DefaultContext, SourceType>
{
}