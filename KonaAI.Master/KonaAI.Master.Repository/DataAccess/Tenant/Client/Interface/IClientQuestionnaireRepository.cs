using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Domain.Tenant.Client;

namespace KonaAI.Master.Repository.DataAccess.Tenant.Client.Interface;

/// <summary>
/// Defines the contract for data access operations related to
/// <see cref="ClientQuestionnaire"/> entities.
/// </summary>
/// <remarks>
/// This interface inherits generic CRUD operations from
/// <see cref="IRepository{TContext, TEntity}"/> for the
/// <see cref="ClientQuestionnaire"/> entity.
/// Implement this interface to add custom query methods
/// or business logic specific to client questionnaires.
/// </remarks>
public interface IClientQuestionnaireRepository : IRepository<DefaultContext, ClientQuestionnaire>
{
}