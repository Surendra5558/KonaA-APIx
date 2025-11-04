using KonaAI.Master.Model.Master.App.SaveModel;
using KonaAI.Master.Model.Tenant.Client.SaveModel;

namespace KonaAI.Master.Business.Tenant.Client.Logic.Interface;

/// <summary>
/// Defines business operations for managing client question banks.
/// </summary>
public interface IClientQuestionBankBusiness
{
    /// <summary>
    /// Retrieves all client question banks.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// The task result contains a queryable collection of <see cref="ClientQuestionBankViewModel"/> objects.
    /// </returns>
    Task<IQueryable<ClientQuestionBankViewModel>> GetAsync(long clientId);

    /// <summary>
    /// Retrieves a specific client question bank by its unique row identifier.
    /// </summary>
    /// <param name="rowId">The unique row identifier (GUID) of the client question bank.</param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// The task result contains the <see cref="ClientQuestionBankViewModel"/> if found; otherwise, <c>null</c>.
    /// </returns>
    Task<ClientQuestionBankViewModel?> GetByRowIdAsync(Guid rowId);

    /// <summary>
    /// Creates a new client question bank based on the provided create model.
    /// </summary>
    /// <param name="questionBank">
    /// The <see cref="QuestionBankCreateModel"/> instance containing the data for the new question bank.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains an <see cref="int"/> indicating the number of records created
    /// (typically 1 if the operation succeeds).
    /// </returns>
    Task<int> CreateAsync(ClientQuestionBankCreateModel questionBank);
}