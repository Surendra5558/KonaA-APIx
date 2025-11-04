using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Domain.Master.App;

namespace KonaAI.Master.Repository.DataAccess.Master.App.Interface;

/// <summary>
/// Defines the contract for data access operations related to <see cref="QuestionBank"/>.
/// </summary>
/// <remarks>
/// This interface inherits generic CRUD methods from
/// <see cref="IRepository{TContext, TEntity}"/> for the <see cref="QuestionBank"/> entity.
/// Implement this interface in a repository class to add any custom queries
/// or business logic specific to question bank management.
/// </remarks>
public interface IQuestionBankRepository : IRepository<DefaultContext, QuestionBank>
{
}