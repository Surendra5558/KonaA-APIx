using KonaAI.Master.Repository.Common;
using KonaAI.Master.Repository.DataAccess.Master.App.Interface;
using KonaAI.Master.Repository.Domain.Master.App;

namespace KonaAI.Master.Repository.DataAccess.Master.App;

/// <summary>
/// Repository implementation for managing <see cref="QuestionnaireAssociation"/> entities.
/// </summary>
/// <remarks>
/// This repository inherits standard CRUD operations from
/// <see cref="GenericRepository{TContext, TEntity}"/> and is used to interact
/// with the <see cref="QuestionnaireAssociation"/> data in the database.
/// Add any <see cref="QuestionnaireAssociation"/>-specific queries or business logic here if needed.
/// </remarks>
public class QuestionnaireAssociationRepository(DefaultContext context)
    : GenericRepository<DefaultContext, QuestionnaireAssociation>(context), IQuestionnaireAssociationRepository
{
}