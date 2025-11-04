using KonaAI.Master.Repository.Common;
using KonaAI.Master.Repository.DataAccess.Tenant.Client.Interface;
using KonaAI.Master.Repository.Domain.Tenant.Client;

namespace KonaAI.Master.Repository.DataAccess.Tenant.Client;

/// <summary>
/// Repository implementation for performing data access operations
/// on <see cref="ClientQuestionnaireSection"/> entities within the tenant client context.
/// </summary>
/// <remarks>
/// Inherits basic CRUD operations from <see cref="GenericRepository{TContext, TEntity}"/>.
/// Use this class to add custom data access logic specific to the <see cref="ClientQuestionnaireSection"/> entity.
/// </remarks>
public class ClientQuestionnaireSectionRepository(DefaultContext context)
        : GenericRepository<DefaultContext, ClientQuestionnaireSection>(context), IClientQuestionnaireSectionRepository
{
}