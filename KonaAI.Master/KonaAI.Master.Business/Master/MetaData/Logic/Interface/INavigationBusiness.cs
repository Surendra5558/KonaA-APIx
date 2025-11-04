using KonaAI.Master.Model.Master.MetaData;

namespace KonaAI.Master.Business.Master.MetaData.Logic.Interface;

/// <summary>
/// Defines the business logic contract for managing application navigation.
/// </summary>
public interface INavigationBusiness
{
    /// <summary>
    /// Retrieves the application navigation items asynchronously.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// The task result contains an <see cref="IQueryable{AppNavigation}"/>
    /// sequence of application navigation entities.
    /// </returns>
    Task<IQueryable<NavigationViewModel>> GetAsync();
}