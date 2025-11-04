using KonaAI.Master.Repository.Common;
using KonaAI.Master.Repository.DataAccess.Tenant.Client.Interface;
using KonaAI.Master.Repository.Domain.Tenant.Client;

namespace KonaAI.Master.Repository.DataAccess.Tenant.Client;

/// <summary>
/// Repository implementation for performing data access operations
/// on <see cref="ClientQuestionBank"/> entities.
/// </summary>
/// <remarks>
/// Inherits from <see cref="GenericRepository{TContext, TEntity}"/> to
/// provide basic CRUD operations for <see cref="ClientQuestionBank"/>.
/// Implement additional custom query methods here if needed.
/// </remarks>
public class ClientQuestionBankRepository(DefaultContext context)
    : GenericRepository<DefaultContext, ClientQuestionBank>(context), IClientQuestionBankRepository
{
}