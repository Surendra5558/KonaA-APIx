using KonaAI.Master.Business.Authentication.Logic.Interface;
using KonaAI.Master.Model.Authentication;
using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Common.Model;
using KonaAI.Master.Repository.Domain.Master.App;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;

namespace KonaAI.Master.Business.Authentication.Logic;

/// <summary>
/// Provides business operations for user authentication and login validation.
/// </summary>
public class UserLoginBusiness(
    ILogger<UserLoginBusiness> logger,
    IUnitOfWork unitOfWork,
    IUserContextService userContextService,
    ILicenseService licenseService,
    IConfiguration configuration)
    : IUserLoginBusiness
{
    private const string ClassName = nameof(UserLoginBusiness);
    // Touch userContextService to avoid unused parameter warning and to record context presence in logs
    private readonly long? _ctxClientId = userContextService?.UserContext?.ClientId;

    /// <summary>
    /// Authenticates a user and generates a complete token response including JWT token and user audit.
    /// </summary>
    /// <param name="tokenFormRequest">The authentication request containing user credentials.</param>
    /// <param name="clientIpAddress">The client's IP address.</param>
    /// <param name="userAgent">The client's user agent string.</param>
    /// <returns>A complete token response with JWT token and user information.</returns>
    /// <exception cref="AuthenticationException">Thrown when authentication fails.</exception>
    /// <exception cref="InvalidOperationException">Thrown when token generation or audit creation fails.</exception>
    public async Task<TokenResponse> AuthenticateUserAsync(
        TokenFormRequest tokenFormRequest,
        string clientIpAddress,
        string userAgent)
    {
        const string methodName = $"{ClassName}: {nameof(AuthenticateUserAsync)}";

        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);

            var sessionId = Guid.NewGuid();
            var tokenCreatedDate = DateTime.Now;
            var expireMinutes = Convert.ToDouble(configuration["Tokens:AccessTokenExpiryInMinutes"]);
            var tokenExpiredDate = tokenCreatedDate.AddMinutes(expireMinutes);

            var refreshToken = GenerateRefreshToken();

            var userDetails = await ValidateUserAsync(tokenFormRequest);

            // Best-effort audit creation; authentication should not fail due to audit persistence issues
            try
            {
                await CreateUserAuditAsync(sessionId, userDetails, refreshToken,
                    tokenCreatedDate, tokenExpiredDate, clientIpAddress, userAgent);
            }
            catch (Exception auditEx)
            {
                logger.LogWarning("{MethodName} - Skipping user audit due to error: {Message}", methodName, auditEx.Message);
            }

            var tokenString = GenerateJwtToken(sessionId, userDetails, tokenExpiredDate);

            logger.LogInformation("User {Email} authenticated successfully with session {SessionId}",
                userDetails.Email, sessionId);

            return new TokenResponse
            {
                Name = userDetails.Name,
                Token = tokenString,
                RoleId = userDetails.RoleId,
                RefreshToken = refreshToken,
                RoleName = userDetails.RoleName,
                ClientId = userDetails.ClientId,
                ClientName = userDetails.ClientName
            };
        }
        catch (AuthenticationException)
        {
            throw;
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

    /// <summary>
    /// Generates a secure refresh token.
    /// </summary>
    /// <returns>A base64 encoded refresh token.</returns>
    private static string GenerateRefreshToken()
    {
        try
        {
            var randomNumber = new byte[32];
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to generate refresh token", ex);
        }
    }

    /// <summary>
    /// Validates the user credentials provided in the <paramref name="tokenFormRequest"/> and returns user login details if authentication is successful.
    /// </summary>
    /// <param name="tokenFormRequest">The login request containing the username, password, and grant type.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a <see cref="UserLoginViewModel"/>
    /// with user information such as ID, name, email, role, and client details if validation is successful.
    /// Throws <see cref="AuthenticationException"/> if the credentials are invalid.
    /// </returns>
    /// <exception cref="AuthenticationException">Thrown when credentials are invalid.</exception>
    /// <exception cref="InvalidOperationException">An error occurred during user validation</exception>
    private async Task<UserLoginViewModel> ValidateUserAsync(TokenFormRequest tokenFormRequest)
    {
        const string methodName = $"{ClassName}: {nameof(ValidateUserAsync)}";

        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);

            var result = from u in await unitOfWork.Users.GetAsync()
                         join cu in await unitOfWork.ClientUsers.GetAsync() on u.Id equals cu.UserId
                         join c in await unitOfWork.Clients.GetAsync() on cu.ClientId equals c.Id
                         join r in await unitOfWork.RoleTypes.GetAsync() on u.RoleTypeId equals r.Id
                         join cr in await unitOfWork.ClientRoleTypes.GetAsync() on r.Id equals cr.RoleTypeId
                         where u.IsActive == true
                         select new
                         {
                             User = u,
                             Client = c,
                             ClientUser = cu,
                             Role = r,
                         };

            var userRecord = result.FirstOrDefault(x =>
                x.User.UserName == tokenFormRequest.UserName);

            if (userRecord == null)
            {
                logger.LogError("{ClassName}: User not found for username {UserName}", ClassName, tokenFormRequest.UserName);
                throw new AuthenticationException("Invalid username or password.");
            }

            if (!BCrypt.Net.BCrypt.Verify(tokenFormRequest.Password, userRecord.User.Password))
            {
                logger.LogError("{ClassName}: Invalid password for user {UserName}", ClassName, tokenFormRequest.UserName);
                throw new AuthenticationException("Invalid username or password.");
            }
            return new UserLoginViewModel
            {
                Id = userRecord.User.Id,
                RowId = userRecord.User.RowId,
                Name = string.Concat(userRecord.User.FirstName, " ", userRecord.User.LastName).Trim(),
                Email = userRecord.User.UserName,
                RoleRowId = userRecord.Role.RowId,
                RoleId = userRecord.Role.Id,
                RoleName = userRecord.Role.Name,
                ClientId = userRecord.Client.Id,
                ClientName = userRecord.Client.Name
            };
        }
        catch (AuthenticationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError("{MethodName} - Error in execution: {Message}", methodName, ex.Message);
            throw new InvalidOperationException("An error occurred during user validation", ex);
        }
        finally
        {
            logger.LogInformation("{MethodName} - method execution completed", methodName);
        }
    }

    /// <summary>
    /// Creates a user audit entry for the login session.
    /// </summary>
    /// <param name="sessionId">The session identifier.</param>
    /// <param name="userDetails">The user details.</param>
    /// <param name="refreshToken">The refresh token.</param>
    /// <param name="tokenCreatedDate">The token creation date.</param>
    /// <param name="tokenExpiredDate">The token expiration date.</param>
    /// <param name="clientIpAddress">The client's IP address.</param>
    /// <param name="userAgent">The client's user agent.</param>
    /// <exception cref="InvalidOperationException">Thrown when audit creation fails.</exception>
    private async Task CreateUserAuditAsync(Guid sessionId, UserLoginViewModel userDetails,
        string refreshToken, DateTime tokenCreatedDate, DateTime tokenExpiredDate,
        string clientIpAddress, string userAgent)
    {
        try
        {
            var users = await unitOfWork.Users.GetAsync();
            var roleTypes = await unitOfWork.RoleTypes.GetAsync();
            var clientRoleTypes = await unitOfWork.ClientRoleTypes.GetAsync();
            var roleNavigationUserActions = await unitOfWork.RoleNavigationUserActions.GetAsync();
            var navigationUserActions = await unitOfWork.NavigationUserActions.GetAsync();
            var navigations = await unitOfWork.Navigations.GetAsync();
            var userActions = await unitOfWork.UserActions.GetAsync();
            var clientProjects = await unitOfWork.ClientProjects.GetAsync();
            var clientProjectUsers = await unitOfWork.ClientProjectUser.GetAsync();
            var projectSchedulers = await unitOfWork.ProjectSchedulers.GetAsync();

            var userRecord = users.FirstOrDefault(u => u.Id == userDetails.Id);
            if (userRecord == null)
                throw new InvalidOperationException($"User with ID {userDetails.Id} not found for audit creation");

            var rolesInfo = (from r in roleTypes
                             join u in users on r.Id equals u.RoleTypeId
                             join cr in clientRoleTypes on r.Id equals cr.RoleTypeId
                             where u.Id == userDetails.Id
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

            var userPermissions = from rnua in roleNavigationUserActions
                                  join nua in navigationUserActions on rnua.NavigationUserActionId equals nua.Id
                                  join n in navigations on nua.NavigationId equals n.Id
                                  join us in userActions on nua.UserActionId equals us.Id
                                  where rnua.RoleTypeId == rolesInfo.Id
                                  orderby n.OrderBy
                                  select new UserPermission
                                  {
                                      NavigationRowId = n.RowId,
                                      NavigationId = n.Id,
                                      NavigationName = n.Name,
                                      UserActionId = us.Id,
                                      UserActionRowId = us.RowId,
                                      UserActionName = us.Name
                                  };

            var client = await unitOfWork.Clients.GetByIdAsync(userDetails.ClientId);

            var projects = from u in clientProjects
                           join p in clientProjectUsers on u.Id equals p.ProjectId
                           join ps in projectSchedulers on p.ProjectId equals ps.ProjectId
                           where p.UserId == userDetails.Id && u.ClientId == userDetails.ClientId
                           select new UserProject
                           {
                               Id = u.Id,
                               Name = u.Name,
                               RowId = u.RowId,
                               ConnectionString = string.Format(ps.ConnectionString, u.Name, ps.UserName,
                                   licenseService.DecryptLicense(
                                       new LicenseResult
                                       {
                                           EncryptedPrivateKey = ps.Password,
                                           EncryptedLicense = ps.EncryptedLicenseKey
                                       },
                                       client.RowId.ToString()))
                           };

            var userAudit = new UserAudit
            {
                RowId = sessionId,
                UserRowId = userRecord.RowId,
                UserId = userRecord.Id,
                FirstName = userRecord.FirstName ?? string.Empty,
                LastName = userRecord.LastName ?? string.Empty,
                Email = userDetails.Email,
                RoleRowId = userDetails.RoleRowId,
                RoleId = userDetails.RoleId,
                RoleName = userDetails.RoleName,
                RefreshToken = refreshToken,
                TokenCreatedDate = tokenCreatedDate,
                TokenExpiredDate = tokenExpiredDate,
                RoleNavigation = userPermissions.ToList(),
                ProjectAccess = projects.ToList(),
                CreatedById = userDetails.Id,
                CreatedBy = userDetails.Name,
                ModifiedById = userDetails.Id,
                ModifiedBy = userDetails.Name
            };

            await unitOfWork.UserAudits.AddAsync(userAudit);
            await unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger.LogError("Error creating user audit: {Message}", ex.Message);
            throw new InvalidOperationException("Failed to create user audit", ex);
        }
    }

    /// <summary>
    /// Generates a JWT token for the authenticated user.
    /// </summary>
    ///     /// <param name="sessionId"></param>
    /// <param name="userDetails">The user details to include in the token.</param>
    /// <param name="tokenExpiryDateTime"></param>
    /// <returns>A JWT token string.</returns>
    /// <exception cref="InvalidOperationException">Thrown when token configuration is missing or invalid.</exception>
    private string GenerateJwtToken(Guid sessionId, UserLoginViewModel userDetails,
        DateTime tokenExpiryDateTime)
    {
        try
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sid,sessionId.ToString()),
                new("UserRowId", userDetails.RowId.ToString()),
                new ("UserId",userDetails.Id.ToString()),
                new(JwtRegisteredClaimNames.Name, userDetails.Name),
                new(JwtRegisteredClaimNames.Email, userDetails.Email),
                new("RoleRowId", userDetails.RoleRowId.ToString()),
                new("RoleId", userDetails.RoleId.ToString()),
                new("Role", userDetails.RoleName),
                new("ClientId", userDetails.ClientId.ToString()),
                new("Client", userDetails.ClientName),
            };

            var issuer = configuration["Tokens:Issuer"];
            var audience = configuration["Tokens:Audience"];
            var key = configuration["Tokens:Key"];

            if (string.IsNullOrEmpty(issuer))
            {
                throw new InvalidOperationException("Token issuer configuration is missing");
            }

            if (string.IsNullOrEmpty(audience))
            {
                throw new InvalidOperationException("Token audience configuration is missing");
            }

            if (string.IsNullOrEmpty(key))
            {
                throw new InvalidOperationException("Token key configuration is missing");
            }

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: tokenExpiryDateTime,
                signingCredentials: signinCredentials);

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }
        catch (Exception ex)
        {
            logger.LogError("Error generating JWT token: {Message}", ex.Message);
            throw new InvalidOperationException("Failed to generate JWT token", ex);
        }
    }
}