using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace KonaAI.Master.Test.Integration.Infrastructure;

/// <summary>
/// Test authentication handler that unconditionally authenticates the request
/// with a basic identity and common claims used by the app.
/// </summary>
public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public const string SchemeName = "TestAuth";

    public TestAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new List<Claim>
        {
            new("Sid", Guid.NewGuid().ToString()),
            new(ClaimTypes.Name, "testuser@konaai.com"),
            new(ClaimTypes.Email, "testuser@konaai.com"),
            new("RoleId", "1"),
            new(ClaimTypes.Role, "Admin"),
            new("ClientId", "1"),
            new("Client", "TestClient")
        };

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}


