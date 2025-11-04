using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace KonaAI.Master.Test.Integration.Infrastructure.Helpers;

/// <summary>
/// Test authentication handler for integration testing.
/// </summary>
public class TestAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public TestAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // If no Authorization header present, simulate unauthenticated
        if (!Request.Headers.ContainsKey("Authorization"))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var authHeader = Request.Headers["Authorization"].FirstOrDefault();
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid authorization header format"));
        }

        var token = authHeader.Substring("Bearer ".Length).Trim();

        // For testing purposes, accept any non-empty token that's not "invalid" or "expired"
        if (string.IsNullOrEmpty(token))
        {
            return Task.FromResult(AuthenticateResult.Fail("Empty token"));
        }

        // Handle specific test scenarios
        if (token == "invalid" || token == "expired" || token == "invalid-token" || token == "expired-token")
        {
            return Task.FromResult(AuthenticateResult.Fail($"Invalid token: {token}"));
        }

        // Resolve client/user from headers if provided to support multi-tenant tests
        var headerClientId = Request.Headers["X-Client-Id"].FirstOrDefault();
        var headerUserId = Request.Headers["X-User-Id"].FirstOrDefault();
        var headerUserRole = Request.Headers["X-User-Role"].FirstOrDefault() ?? "Admin";

        var resolvedClientId = string.IsNullOrWhiteSpace(headerClientId) ? "1" : headerClientId;
        var resolvedUserId = string.IsNullOrWhiteSpace(headerUserId) ? "testuser" : headerUserId;

        // Create a test identity
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, resolvedUserId),
            new Claim(ClaimTypes.NameIdentifier, "1"),
            new Claim(ClaimTypes.Email, "testuser@konaai.com"),
            new Claim("RoleId", headerUserRole.Equals("Admin", StringComparison.OrdinalIgnoreCase) ? "1" : "2"),
            new Claim("Role", headerUserRole),
            new Claim("ClientId", resolvedClientId),
            new Claim("Client", "TestClient")
        };

        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "Test");

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
