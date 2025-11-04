using KonaAI.Master.Repository.Common.Constants;
using Microsoft.AspNetCore.Authorization;

namespace KonaAI.Master.API.Handler.Authorize;

/// <summary>
/// Represents an authorization requirement that specifies the required role and the
/// allowed user action against a specific navigation menu item.
/// </summary>
public class AccessAuthorizationRequirement(NavigationMenu navigationMenu, UserActionMenu userAction)
    : IAuthorizationRequirement
{
    /// <summary>
    /// Gets the user action being authorized.
    /// </summary>
    public UserActionMenu UserAction { get; } = userAction;

    /// <summary>
    /// Gets the navigation menu item associated with the authorization request.
    /// </summary>
    public NavigationMenu NavigationMenu { get; } = navigationMenu;
}