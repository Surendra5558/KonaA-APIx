using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Domain.Tenant.Client;

namespace KonaAI.Master.Repository.DataAccess.Tenant.Client.Interface;

/// <summary>
/// Provides a contract for data access operations on <see cref="ClientQuestionnaireSection"/> entities
/// within the tenant client context.
/// </summary>
/// <remarks>
/// This interface inherits generic CRUD operations from
/// <see cref="IRepository{TContext, TEntity}"/> for <see cref="ClientQuestionnaireSection"/>.
/// You can extend this interface with custom query methods specific to
/// sections if needed.
/// </remarks>
public interface IClientQuestionnaireSectionRepository : IRepository<DefaultContext, ClientQuestionnaireSection>
{
}