using KonaAI.Master.Model.Master.App.SaveModel;
using KonaAI.Master.Model.Tenant.Client.SaveModel;
using KonaAI.Master.Model.Tenant.Client.ViewModel;

namespace KonaAI.Master.Business.Tenant.Client.Logic.Interface;

/// <summary>
/// Defines business operations for managing client questionnaires.
/// </summary>
public interface IClientQuestionnaireBusiness
{
    /// <summary>
    /// Retrieves all client questionnaires.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// The task result contains a queryable collection of <see cref="ClientQuestionnaireViewModel"/> objects.
    /// </returns>
    Task<IQueryable<ClientQuestionnaireViewModel>> GetAsync();

    /// <summary>
    /// Retrieves a specific client questionnaire by its unique row identifier.
    /// </summary>
    /// <param name="questionnaireRowId">The unique row identifier (GUID) of the client questionnaire.</param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// The task result contains the <see cref="ClientQuestionnaireViewModel"/> if found; otherwise, <c>null</c>.
    /// </returns>
    Task<ClientQuestionnaireDetailsViewModel?> GetQuestionnaireDetailsAsync(Guid questionnaireRowId);

    /// <summary>
    /// Creates a new client questionnaire based on the provided create model.
    /// </summary>
    /// <param name="clientQuestionnaire"></param>
    /// <param name="clientId"></param>
    /// <remarks>Creates a questionnaire with sections and question associations.</remarks>
    Task<int> CreateAsync(ClientQuestionnaireCreateModel clientQuestionnaire, long clientId);
}