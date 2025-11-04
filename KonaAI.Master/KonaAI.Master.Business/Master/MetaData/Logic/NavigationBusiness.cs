using KonaAI.Master.Business.Master.MetaData.Logic.Interface;
using KonaAI.Master.Model.Master.MetaData;
using KonaAI.Master.Repository.Common.Interface;
using Microsoft.Extensions.Logging;

namespace KonaAI.Master.Business.Master.MetaData.Logic;

/// <summary>
/// </summary>
public class NavigationBusiness(
    ILogger<NavigationBusiness> logger,
    IUnitOfWork unitOfWork,
    IUserContextService userContextService) : INavigationBusiness
{
    private const string ClassName = nameof(NavigationBusiness);

    /// <summary>
    /// Retrieves the collection of <see cref="NavigationViewModel"/> items asynchronously
    /// filtered by the current user's role permissions.
    /// </summary>
    /// <returns>
    /// An <see cref="IQueryable{NavigationViewModel}"/> representing the navigation items
    /// accessible to the current user based on their role.
    /// </returns>
    /// <exception cref="UnauthorizedAccessException">
    /// Thrown when user role information is invalid or user is not authorized.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when an error occurs during data retrieval or mapping.
    /// </exception>
    public async Task<IQueryable<NavigationViewModel>> GetAsync()
    {
        const string methodName = $"{ClassName}: {nameof(GetAsync)}";

        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);

            var userContext = userContextService.UserContext!;

            // Get user's role information
            var rolesInfo = (from r in await unitOfWork.RoleTypes.GetAsync()
                             join u in await unitOfWork.Users.GetAsync() on r.Id equals u.RoleTypeId
                             join cr in await unitOfWork.ClientRoleTypes.GetAsync() on r.Id equals cr.RoleTypeId
                             where u.Id == userContext.UserLoginId
                             select new
                             {
                                 r.Id,
                                 r.Name,
                                 cr.ClientId
                             }).FirstOrDefault();

            if (rolesInfo == null)
            {
                logger.LogError("User role information is invalid");
                throw new UnauthorizedAccessException("User role information is invalid");
            }

            // Get navigation items with parent information based on role permissions
            var result = from rnua in await unitOfWork.RoleNavigationUserActions.GetAsync()
                         join nua in await unitOfWork.NavigationUserActions.GetAsync()
                             on rnua.NavigationUserActionId equals nua.Id
                         join n in await unitOfWork.Navigations.GetAsync()
                             on nua.NavigationId equals n.Id
                         join parent in await unitOfWork.Navigations.GetAsync()
                             on n.ParentId equals parent.Id into parentGroup
                         from p in parentGroup.DefaultIfEmpty()
                         where rnua.RoleTypeId == rolesInfo.Id && n.IsActive && !n.IsDeleted
                         orderby n.OrderBy
                         select new NavigationViewModel
                         {
                             RowId = n.RowId,
                             Name = n.Name,
                             ParentRowId = n.ParentId.HasValue && n.ParentId.Value > 0 && p != null
                                 ? p.RowId
                                 : (Guid?)null,
                             ParentName = n.ParentId.HasValue && n.ParentId.Value > 0 && p != null
                                 ? p.Name
                                 : null
                         };

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError("{MethodName} - Error in execution: {Message}", methodName, ex.Message);
            throw new InvalidOperationException("An error occurred during user authentication", ex);
        }
        finally
        {
            logger.LogInformation("{MethodName} - method execution completed", methodName);
        }
    }
}