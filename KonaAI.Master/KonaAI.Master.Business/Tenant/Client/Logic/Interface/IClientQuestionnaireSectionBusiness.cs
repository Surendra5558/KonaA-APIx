using KonaAI.Master.Model.Tenant.Client.ViewModel;

namespace KonaAI.Master.Business.Tenant.Client.Logic.Interface;

/// <summary>
/// Defines operations for retrieving client questionnaire sections.
/// </summary>
/// <remarks>
/// Implementations should return a query that provides access to
/// <see cref="ClientQuestionnaireSectionViewModel"/> items appropriate to the current context.
/// </remarks>
public interface IClientQuestionnaireSectionBusiness
{
    /// <summary>
    /// Asynchronously gets a queryable sequence of questionnaire sections.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a deferred, filterable
    /// <see cref="IQueryable{T}"/> of <see cref="ClientQuestionnaireSectionViewModel"/> items.
    /// </returns>
    /// <remarks>
    /// The returned query is not executed until enumerated; callers may compose additional query operators before materialization.
    /// </remarks>
    public Task<IQueryable<ClientQuestionnaireSectionViewModel>> GetAsync();
}