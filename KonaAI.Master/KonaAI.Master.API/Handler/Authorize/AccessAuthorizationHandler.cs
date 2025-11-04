using KonaAI.Master.Repository.Common.Constants;
using KonaAI.Master.Repository.Common.Interface;
using Microsoft.AspNetCore.Authorization;

namespace KonaAI.Master.API.Handler.Authorize;

/// <summary>
/// Authorization handler that validates whether the current user is authorized to perform
/// a specific <see cref="AccessAuthorizationRequirement"/>. It verifies the user context,
/// role, session audit record, and that the requested navigation/action pair is permitted.
/// </summary>
/// <param name="logger">The logger used to record diagnostic information.</param>
/// <param name="unitOfWork">The unit of work providing access to repositories.</param>
/// <param name="userContextService">The service that supplies the current user context.</param>
public class AccessAuthorizationHandler(
    ILogger<AccessAuthorizationHandler> logger, IUnitOfWork unitOfWork,
    IUserContextService userContextService) : AuthorizationHandler<AccessAuthorizationRequirement>
{
    private const string ClassName = nameof(AccessAuthorizationHandler);

    /// <summary>
    /// Handles the authorization requirement by validating the current user context against
    /// the required role, session audit record, and the requested navigation/action permissions.
    /// </summary>
    /// <param name="context">The authorization context.</param>
    /// <param name="requirement">The requirement containing role, action, and navigation details.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        AccessAuthorizationRequirement requirement)
    {
        const string methodName = $"{ClassName}: {nameof(HandleRequirementAsync)}";

        try
        {
            var userContext = userContextService.UserContext;
            if (userContext == null)
            {
                logger.LogError("{MethodName} - User context is null", methodName);
                context.Fail();
                return;
            }

            var userInformation = (await unitOfWork.UserAudits.GetAsync())
                .FirstOrDefault(x => x.RowId == userContext.SessionRowId);
            if (userInformation == null)
            {
                logger.LogError("{MethodName} - User information not found for SessionRowId: {SessionRowId}",
                    methodName, userContext.SessionRowId);
                context.Fail();
                return;
            }

            var navigationMenu =
                userInformation.RoleNavigation.FirstOrDefault(x => x.NavigationRowId == requirement.NavigationMenu.GetGuid()
                                                                   && x.UserActionRowId == requirement.UserAction.GetGuid());
            if (navigationMenu == null)
            {
                logger.LogError("{MethodName} - User does not have the required navigation menu",
                    methodName);
                context.Fail();
                return;
            }

            context.Succeed(requirement);
        }
        catch (Exception e)
        {
            logger.LogError("{MethodName} - Exception occurred: {ExceptionMessage}", methodName, e.Message);
            context.Fail();
        }
    }
}