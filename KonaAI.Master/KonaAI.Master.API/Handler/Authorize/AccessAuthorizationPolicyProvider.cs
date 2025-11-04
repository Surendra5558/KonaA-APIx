using KonaAI.Master.Repository.Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace KonaAI.Master.API.Handler.Authorize;

/// <summary>
/// Custom <see cref="IAuthorizationPolicyProvider"/> that builds authorization policies
/// from policy names prefixed with <c>"Permission:"</c>.
/// </summary>
public class AccessAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : IAuthorizationPolicyProvider
{
    private const string PolicyPrefix = "Permission:";

    private readonly DefaultAuthorizationPolicyProvider _fallbackPolicyProvider = new(options);

    /// <summary>
    /// Gets a policy by name. Builds a custom policy when the name starts with <c>"Permission:"</c>;
    /// otherwise defers to the default provider.
    /// </summary>
    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (!policyName.StartsWith(PolicyPrefix, StringComparison.OrdinalIgnoreCase))
        {
            return _fallbackPolicyProvider.GetPolicyAsync(policyName);
        }

        // Remove prefix "Permission:"
        var parameters = policyName.Substring(PolicyPrefix.Length);

        // Expected format: "Navigation=ProjectDashboard;Action=Edit;RoleId=3"
        var keyValues = parameters.Split(';', StringSplitOptions.RemoveEmptyEntries)
            .Select(p => p.Split('='))
            .Where(p => p.Length == 2)
            .ToDictionary(p => p[0].Trim(), p => p[1].Trim(), StringComparer.OrdinalIgnoreCase);

        keyValues.TryGetValue("Navigation", out var navigationStr);
        keyValues.TryGetValue("Action", out var actionStr);

        if (!Enum.TryParse<NavigationMenu>(navigationStr, true, out var navigationMenu) ||
            !Enum.TryParse<UserActionMenu>(actionStr, true, out var userAction))
        {
            return _fallbackPolicyProvider.GetPolicyAsync(policyName);
        }

        var requirement = new AccessAuthorizationRequirement(navigationMenu, userAction);

        var policy = new AuthorizationPolicyBuilder()
            .AddRequirements(requirement)
            .Build();

        return Task.FromResult<AuthorizationPolicy?>(policy);
    }

    /// <summary>
    /// Gets the default authorization policy from the fallback provider.
    /// </summary>
    /// <returns>The default <see cref="AuthorizationPolicy"/>.</returns>
    public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
    {
        return _fallbackPolicyProvider.GetDefaultPolicyAsync()!;
    }

    /// <summary>
    /// Gets the fallback authorization policy from the fallback provider.
    /// </summary>
    /// <returns>The fallback <see cref="AuthorizationPolicy"/> if configured; otherwise <see langword="null"/>.</returns>
    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
    {
        return _fallbackPolicyProvider.GetFallbackPolicyAsync()!;
    }
}