using KonaAI.Master.Model.Master.MetaData;
using KonaAI.Master.Repository.Domain.Master.MetaData;

namespace KonaAI.Master.Business.Master.MetaData.Logic.Interface;

/// <summary>
/// Defines business logic operations for managing role-based navigation actions.
/// </summary>
public interface IRoleNavigationUserActionBusiness
{
    /// <summary>
    /// Asynchronously retrieves a queryable collection of <see cref="RoleNavigationUserAction"/>
    /// objects representing the role-to-navigation-action mappings.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains an <see cref="IQueryable{T}"/> of <see cref="RoleNavigationUserAction"/>.
    /// </returns>
    Task<IQueryable<RoleNavigationUserActionViewModel>> GetAsync();
}