using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization.Policy;
using KonaAI.Master.API.Handler.Authorize;

namespace KonaAI.Master.Test.Integration.Infrastructure.Helpers;

/// <summary>
/// Test authorization handler that respects authentication status for testing.
/// </summary>
public class TestAuthorizationHandler : IAuthorizationHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TestAuthorizationHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Task HandleAsync(AuthorizationHandlerContext context)
    {
        var httpContext = _httpContextAccessor.HttpContext;

        // Check if user is authenticated
        var isAuthenticated = httpContext?.User?.Identity?.IsAuthenticated ?? false;

        if (!isAuthenticated)
        {
            // For unauthenticated requests, fail authorization
            foreach (var requirement in context.PendingRequirements.ToList())
            {
                context.Fail();
            }
            return Task.CompletedTask;
        }

        // For authenticated requests, check if they have the required permissions
        // For testing purposes, we'll succeed all authorization requirements
        // In a real scenario, you would check specific permissions here
        foreach (var requirement in context.PendingRequirements.ToList())
        {
            // Check if this is a permission-based requirement
            if (requirement is AccessAuthorizationRequirement accessReq)
            {
                // For testing, we'll assume the user has all permissions
                // In real tests, you might want to check specific permissions
                context.Succeed(requirement);
            }
            else
            {
                // For other requirements, succeed by default in tests
                context.Succeed(requirement);
            }
        }

        return Task.CompletedTask;
    }
}

