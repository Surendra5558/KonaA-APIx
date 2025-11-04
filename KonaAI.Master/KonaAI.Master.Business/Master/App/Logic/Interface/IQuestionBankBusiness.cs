using KonaAI.Master.Model.Master.App.SaveModel;
using KonaAI.Master.Model.Master.App.ViewModel;
using KonaAI.Master.Model.Tenant.Client.SaveModel;

namespace KonaAI.Master.Business.Master.App.Logic.Interface;

public interface IQuestionBankBusiness
{
    /// <summary>
    /// Retrieves all question banks as a queryable sequence.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains an <see cref="IQueryable{QuestionBankViewModel}"/>
    /// sequence of question banks.
    /// </returns>
    Task<IQueryable<QuestionBankViewModel>> GetAsync();

    /// <summary>
    /// Retrieves a specific question bank by its unique row identifier.
    /// </summary>
    /// <param name="rowId">
    /// The <see cref="Guid"/> identifier of the question bank.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains a <see cref="QuestionBankViewModel"/> if found; otherwise, <c>null</c>.
    /// </returns>
    Task<QuestionBankViewModel> GetByRowIdAsync(Guid rowId);

    /// <summary>
    /// Creates a new question bank based on the provided create model.
    /// </summary>
    /// <param name="questionBank">
    /// The <see cref="QuestionBankCreateModel"/> instance containing the data for the new question bank.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the number of records affected.
    /// </returns>
    Task<int> CreateAsync(QuestionBankCreateModel questionBank);

    /// <summary>
    /// Asynchronously updates an existing question identified by the specified row identifier using the provided update model.
    /// </summary>
    /// <param name="rowId">The unique row identifier (GUID) of the question to update.</param>
    /// <param name="client">The <see cref="ClientUpdateModel"/> containing the updated client data.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the number of records affected.
    /// </returns>
    Task<int> UpdateAsync(Guid rowId, QuestionBankUpdateModel client);

    /// <summary>
    /// Asynchronously deletes a question identified by the specified row identifier.
    /// </summary>
    /// <param name="rowId">The unique row identifier (GUID) of the question to delete.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the number of records affected.
    /// </returns>
    Task<int> DeleteAsync(Guid rowId);
}