using KonaAI.Master.Business.Authentication.Logic.Interface;
using KonaAI.Master.Model.Common;
using KonaAI.Master.Repository.Common.Interface;
using Microsoft.Extensions.Logging;

namespace KonaAI.Master.Business.Authentication.Logic;

public class MenuBusiness(
    ILogger<MenuBusiness> logger,
    IUnitOfWork unitOfWork,
    IUserContextService userContextService) : IMenuBusiness
{
    private const string ClassName = nameof(MenuBusiness);

    public async Task<IQueryable<MetaDataViewModel>> GetAsync()
    {
        const string methodName = $"{ClassName}: {nameof(GetAsync)}";

        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);

            var userContext = userContextService.UserContext!;

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

            var result = from rnua in await unitOfWork.RoleNavigationUserActions.GetAsync()
                         join nua in await unitOfWork.NavigationUserActions.GetAsync()
                             on rnua.NavigationUserActionId equals nua.Id
                         join n in await unitOfWork.Navigations.GetAsync()
                             on nua.NavigationId equals n.Id
                         where nua.UserActionId == 1 && rnua.RoleTypeId == rolesInfo.Id
                         orderby n.OrderBy
                         select new MetaDataViewModel
                         {
                             RowId = n.RowId,
                             Name = n.Name,
                             Description = n.Description,
                             OrderBy = n.OrderBy
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