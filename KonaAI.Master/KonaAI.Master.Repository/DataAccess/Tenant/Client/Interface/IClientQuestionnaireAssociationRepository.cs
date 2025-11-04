using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Domain.Tenant.Client;

namespace KonaAI.Master.Repository.DataAccess.Tenant.Client.Interface;

/// <summary>
/// Defines the contract for data access operations related to
/// <see cref="ClientQuestionnaireAssociation"/>.
/// </summary>
/// <remarks>
/// This interface inherits standard CRUD operations from
/// <see cref="IRepository{TContext, TEntity}"/> for the
/// <see cref="ClientQuestionnaireAssociation"/> entity.
/// Implement this interface to add custom queries or data access logic
/// specifically for managing questionnaire associations.
/// </remarks>
public interface IClientQuestionnaireAssociationRepository : IRepository<DefaultContext, ClientQuestionnaireAssociation>
{
}