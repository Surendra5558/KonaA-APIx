using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Domain.Master.App;

namespace KonaAI.Master.Repository.DataAccess.Master.App.Interface;

/// <summary>
/// Defines repository operations for <see cref="QuestionnaireAssociation"/> entities within the <see cref="DefaultContext"/>.
/// Inherits standard CRUD and query methods from <see cref="IRepository{DefaultContext, QuestionnaireAssociation}"/>,
/// including asynchronous retrieval, addition, update, and deletion of <see cref="QuestionnaireAssociation"/> records.
/// </summary>
public interface IQuestionnaireAssociationRepository : IRepository<DefaultContext, QuestionnaireAssociation>
{
}