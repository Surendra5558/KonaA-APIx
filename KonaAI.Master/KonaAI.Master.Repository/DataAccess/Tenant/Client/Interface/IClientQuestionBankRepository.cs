using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Domain.Tenant.Client;

namespace KonaAI.Master.Repository.DataAccess.Tenant.Client.Interface;

/// <summary>
/// Provides a contract for performing data access operations on
/// <see cref="ClientQuestionBank"/> entities within the tenant context.
/// </summary>
/// <remarks>
/// This interface inherits standard CRUD operations from
/// <see cref="IRepository{TContext, TEntity}"/> for <see cref="ClientQuestionBank"/>.
/// Additional query methods specific to client question banks can be defined here.
/// </remarks>
public interface IClientQuestionBankRepository : IRepository<DefaultContext, ClientQuestionBank>
{
}