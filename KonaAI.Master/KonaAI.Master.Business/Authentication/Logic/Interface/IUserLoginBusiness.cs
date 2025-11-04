using KonaAI.Master.Model.Authentication;

namespace KonaAI.Master.Business.Authentication.Logic.Interface;

/// <summary>
/// Provides business operations for user authentication and login validation.
/// </summary>
public interface IUserLoginBusiness
{
    /// <summary>
    /// Authenticates a user and generates a complete token response including JWT token and user audit.
    /// </summary>
    /// <param name="tokenFormRequest">The authentication request containing user credentials.</param>
    /// <param name="clientIpAddress">The client's IP address.</param>
    /// <param name="userAgent">The client's user agent string.</param>
    /// <returns>A complete token response with JWT token and user information.</returns>
    /// <exception cref="System.Security.Authentication.AuthenticationException">Thrown when authentication fails.</exception>
    /// <exception cref="System.InvalidOperationException">Thrown when token generation or audit creation fails.</exception>
    Task<TokenResponse> AuthenticateUserAsync(TokenFormRequest tokenFormRequest, string clientIpAddress, string userAgent);
}