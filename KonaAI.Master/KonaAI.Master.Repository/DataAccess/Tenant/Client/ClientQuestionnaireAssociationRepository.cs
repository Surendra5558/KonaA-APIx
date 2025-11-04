using KonaAI.Master.Repository.Common;
using KonaAI.Master.Repository.DataAccess.Tenant.Client.Interface;
using KonaAI.Master.Repository.Domain.Tenant.Client;

namespace KonaAI.Master.Repository.DataAccess.Tenant.Client;

/// <summary>
/// Repository implementation for managing <see cref="ClientQuestionnaireAssociation"/> entities.
/// </summary>
/// <remarks>
/// This repository inherits standard CRUD operations from
/// <see cref="GenericRepository{TContext, TEntity}"/> and is used to handle
/// data access logic related to linking questionnaires with other entities
/// (such as clients, users, or processes).
/// Add any <see cref="ClientQuestionnaireAssociation"/>-specific queries or
/// business logic here as needed.
/// </remarks>
public class ClientQuestionnaireAssociationRepository(DefaultContext context)
    : GenericRepository<DefaultContext, ClientQuestionnaireAssociation>(context), IClientQuestionnaireAssociationRepository
{
}