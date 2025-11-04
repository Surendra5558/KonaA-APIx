using KonaAI.Master.Model.Master.MetaData;
using KonaAI.Master.Repository.Domain.Master.MetaData;

namespace KonaAI.Master.Business.Master.MetaData.Logic.Interface;

/// <summary>
/// Defines the business logic contract for managing navigation actions within the application.
/// </summary>
public interface INavigationUserActionBusiness
{
    /// <summary>
    /// Retrieves all available navigation actions.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains a queryable collection of NavigationUserActionViewModel.
    /// </returns>
    Task<IQueryable<NavigationUserActionViewModel>> GetAsync();
}