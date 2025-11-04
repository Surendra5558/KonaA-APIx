using KonaAI.Master.Repository.Common;
using KonaAI.Master.Repository.DataAccess.Tenant.Client.Interface;
using KonaAI.Master.Repository.Domain.Tenant.Client;

namespace KonaAI.Master.Repository.DataAccess.Tenant.Client;

/// <summary>
/// Repository implementation for managing <see cref="ClientQuestionnaire"/> entities.
/// </summary>
/// <remarks>
/// Inherits standard CRUD operations from
/// <see cref="GenericRepository{TContext, TEntity}"/> and provides
/// data access for client-specific questionnaire records.
/// Extend this class with custom queries or business logic as needed
/// to handle client-questionnaire relationships efficiently.
/// </remarks>
public class ClientQuestionnaireRepository(DefaultContext context)
    : GenericRepository<DefaultContext, ClientQuestionnaire>(context), IClientQuestionnaireRepository
{
}