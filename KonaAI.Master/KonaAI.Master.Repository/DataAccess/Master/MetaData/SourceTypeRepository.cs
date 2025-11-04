using KonaAI.Master.Repository.Common;
using KonaAI.Master.Repository.DataAccess.Master.MetaData.Interface;
using KonaAI.Master.Repository.Domain.Master.MetaData;

namespace KonaAI.Master.Repository.DataAccess.Master.MetaData;

/// <summary>
/// Concrete repository for <see cref="SourceType"/> entities.
/// </summary>
/// <remarks>
/// Provides CRUD operations via <see cref="GenericRepository{DefaultContext, SourceType}"/> and
/// implements the contract defined by <see cref="ISourceTypeRepository"/>.
/// </remarks>
/// <param name="context">The EF Core <see cref="DefaultContext"/> used for data access.</param>
/// <seealso cref="ISourceTypeRepository"/>
/// <seealso cref="GenericRepository{DefaultContext, SourceType}"/>
/// <seealso cref="SourceType"/>
/// <seealso cref="DefaultContext"/>
public class SourceTypeRepository(DefaultContext context) : GenericRepository<DefaultContext, SourceType>(context), ISourceTypeRepository
{
}