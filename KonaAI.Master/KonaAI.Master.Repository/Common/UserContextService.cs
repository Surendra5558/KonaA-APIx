using KonaAI.Master.Repository.Common.Constants;
using KonaAI.Master.Repository.Common.Domain;
using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Common.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;

namespace KonaAI.Master.Repository.Common;

/// <summary>
/// Provides user context information and sets default values on domain entities based on the current authenticated user.
/// </summary>
public class UserContextService : IUserContextService
{
    /// <summary>
    /// Gets or sets the current user context, containing user and client information extracted from the HTTP context.
    /// </summary>
    public UserContext? UserContext { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserContextService"/> class and populates the <see cref="Model.UserContext"/>
    /// from the current HTTP context's claims.
    /// </summary>
    /// <param name="httpContextAccessor">The HTTP context accessor used to retrieve user claims.</param>
    /// <exception cref="UnauthorizedAccessException">Thrown when the user is not found in the HTTP context.</exception>
    /// <exception cref="OverflowException">A claim value represents a number less than <see cref="long.MinValue"/> or greater than <see cref="long.MaxValue"/>.</exception>
    /// <exception cref="ArgumentNullException">A required claim value is <c>null</c>.</exception>
    /// <exception cref="FormatException">A claim value is not in the correct format.</exception>
    public UserContextService(IHttpContextAccessor httpContextAccessor)
    {
        if (httpContextAccessor.HttpContext != null && httpContextAccessor.HttpContext.Request.Path.Value.Contains("Login"))
            return;
        else if (httpContextAccessor.HttpContext == null)
            return;

        if (httpContextAccessor.HttpContext.User.Identity is { IsAuthenticated: false } || httpContextAccessor.HttpContext?.User is not { } claimsPrincipal)
            throw new UnauthorizedAccessException("User not found");

        UserContext = new UserContext
        {
            SessionRowId = Guid.Parse(claimsPrincipal.FindFirst(JwtRegisteredClaimNames.Sid)?.Value!),
            UserRowId = Guid.Parse(claimsPrincipal.FindFirst("UserRowId")?.Value!),
            UserLoginId = long.Parse(claimsPrincipal.FindFirst("UserId")?.Value!),
            UserLoginName = claimsPrincipal.FindFirst(JwtRegisteredClaimNames.Name)?.Value ?? string.Empty,
            UserLoginEmail = claimsPrincipal
                .FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value ?? string.Empty,
            RoleRowId = Guid.Parse(claimsPrincipal.FindFirst("RoleRowId")?.Value!),
            RoleId = long.Parse(claimsPrincipal.FindFirst("RoleId")?.Value!),
            RoleName = claimsPrincipal.FindFirst("Role")?.Value ?? string.Empty,
            ClientId = long.Parse(claimsPrincipal.FindFirst("ClientId")?.Value!),
            ClientName = claimsPrincipal.FindFirst("Client")?.Value ?? string.Empty
        };
    }

    /// <summary>
    /// Sets default values on the specified domain entity according to the current user context
    /// and the provided <paramref name="dataModes"/> operation.
    /// </summary>
    /// <typeparam name="T">The type of the domain entity, derived from <see cref="BaseClientDomain"/>.</typeparam>
    /// <param name="domain">The domain entity to update.</param>
    /// <param name="dataModes">The data operation mode (e.g., Add, Edit, Delete, DeActive).</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="dataModes"/> is not a valid value.</exception>
    public void SetDomainDefaults<T>(T domain, DataModes dataModes) where T : BaseDomain
    {
        switch (dataModes)
        {
            case DataModes.Add:
                domain.CreatedBy = UserContext?.UserLoginName!;
                domain.CreatedById = UserContext!.UserLoginId;
                domain.CreatedOn = DateTime.UtcNow;
                domain.IsActive = true;
                domain.IsDeleted = false;
                break;

            case DataModes.Delete:
                domain.IsActive = false;
                domain.IsDeleted = true;
                break;

            case DataModes.DeActive:
                domain.IsActive = false;
                break;

            case DataModes.Edit:
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(dataModes), dataModes, null);
        }
        domain.ModifiedBy = UserContext?.UserLoginName!;
        domain.ModifiedById = UserContext!.UserLoginId;
        domain.ModifiedOn = DateTime.UtcNow;

        if (domain is not BaseClientDomain clientDomain)
            return;

        if (UserContext != null)
        {
            clientDomain.ClientId = UserContext.ClientId;
        }
    }

    /// <summary>
    /// Sets default values on a list of domain entities according to the current user context
    /// and the provided <paramref name="dataModes"/> operation.
    /// </summary>
    /// <typeparam name="T">The type of the domain entities, derived from <see cref="BaseClientDomain"/>.</typeparam>
    /// <param name="domains">The list of domain entities to update.</param>
    /// <param name="dataModes">The data operation mode (e.g., Add, Edit, Delete, DeActive).</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="dataModes"/> is not a valid value.</exception>
    public void SetDomainDefaults<T>(List<T> domains, DataModes dataModes) where T : BaseDomain
    {
        foreach (var domain in domains)
        {
            SetDomainDefaults(domain, dataModes);
        }
    }
}