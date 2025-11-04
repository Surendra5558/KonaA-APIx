using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Common.Model;
using KonaAI.Master.Repository.Common.Constants;
using KonaAI.Master.Repository.Common.Domain;
using Microsoft.AspNetCore.Http;

namespace KonaAI.Master.Test.Integration.Infrastructure;

/// <summary>
/// Test implementation of IUserContextService for integration testing.
/// </summary>
public class TestUserContextService : IUserContextService
{
    private UserContext _userContext;
    private readonly IHttpContextAccessor? _httpContextAccessor;

    public TestUserContextService()
    {
        _userContext = new UserContext
        {
            ClientId = 1,
            UserRowId = Guid.NewGuid(),
            UserLoginId = 1,
            UserLoginName = "testuser",
            UserLoginEmail = "testuser@konaai.com",
            RoleId = 1,
            RoleName = "Admin"
        };
    }

    public TestUserContextService(IHttpContextAccessor httpContextAccessor) : this()
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public UserContext? UserContext
    {
        get
        {
            // Refresh from current request headers if available
            var ctx = _httpContextAccessor?.HttpContext;
            if (ctx != null)
            {
                if (ctx.Request.Headers.TryGetValue("X-Client-Id", out var clientIdVals))
                {
                    if (long.TryParse(clientIdVals.FirstOrDefault(), out var cid) && cid > 0)
                    {
                        _userContext.ClientId = cid;
                    }
                }
                if (ctx.Request.Headers.TryGetValue("X-User-Id", out var userIdVals))
                {
                    var uid = userIdVals.FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(uid))
                    {
                        _userContext.UserLoginName = uid!;
                    }
                }
            }
            return _userContext;
        }
        set => _userContext = value ?? new UserContext();
    }

    /// <summary>
    /// Gets the current client ID for testing.
    /// </summary>
    public long ClientId => _userContext.ClientId;

    /// <summary>
    /// Gets the current user ID for testing.
    /// </summary>
    public string UserId => _userContext.UserLoginName;

    /// <summary>
    /// Sets the user context for testing.
    /// </summary>
    public void SetUserContext(UserContext userContext)
    {
        _userContext = userContext;
    }

    /// <summary>
    /// Sets the client ID for multi-tenant testing.
    /// </summary>
    public void SetClientId(long clientId)
    {
        _userContext.ClientId = clientId;
    }

    /// <summary>
    /// Sets the user ID for testing.
    /// </summary>
    public void SetUserId(string userId)
    {
        _userContext.UserLoginName = userId;
    }

    /// <summary>
    /// Sets domain defaults for entity creation.
    /// </summary>
    public void SetDomainDefaults<T>(T domain, DataModes dataModes) where T : BaseDomain
    {
        // Set common audit fields based on the entity type
        var now = DateTime.UtcNow;

        switch (dataModes)
        {
            case DataModes.Add:
                domain.CreatedOn = now;
                domain.CreatedBy = _userContext.UserLoginName;
                // Set ModifiedBy for BaseDomain entities (required field)
                domain.ModifiedBy = _userContext.UserLoginName;
                domain.ModifiedOn = now;
                if (domain is BaseClientDomain clientDomain)
                {
                    clientDomain.ClientId = _userContext.ClientId;
                }
                break;
            case DataModes.Edit:
                domain.ModifiedOn = now;
                domain.ModifiedBy = _userContext.UserLoginName;
                break;
            case DataModes.Delete:
                domain.IsDeleted = true;
                domain.ModifiedOn = now;
                domain.ModifiedBy = _userContext.UserLoginName;
                break;
            case DataModes.DeActive:
                domain.IsActive = false;
                domain.ModifiedOn = now;
                domain.ModifiedBy = _userContext.UserLoginName;
                break;
        }
    }

    /// <summary>
    /// Sets domain defaults for a list of entities.
    /// </summary>
    public void SetDomainDefaults<T>(List<T> domains, DataModes dataModes) where T : BaseDomain
    {
        foreach (var domain in domains)
        {
            SetDomainDefaults(domain, dataModes);
        }
    }

    /// <summary>
    /// Creates a test user context with specific values.
    /// </summary>
    public static TestUserContextService Create(long clientId = 1, string userId = "testuser")
    {
        var service = new TestUserContextService();
        service.SetClientId(clientId);
        service.SetUserId(userId);
        return service;
    }

    /// <summary>
    /// Creates a test user context for admin user.
    /// </summary>
    public static TestUserContextService CreateAdmin(long clientId = 1)
    {
        var service = new TestUserContextService();
        service.SetClientId(clientId);
        service.SetUserId("admin");
        service._userContext.RoleName = "Admin";
        service._userContext.RoleId = 1;
        service._userContext.UserLoginName = "Admin User";
        service._userContext.UserLoginEmail = "admin@konaai.com";
        return service;
    }

    /// <summary>
    /// Creates a test user context for regular user.
    /// </summary>
    public static TestUserContextService CreateUser(long clientId = 1)
    {
        var service = new TestUserContextService();
        service.SetClientId(clientId);
        service.SetUserId("user");
        service._userContext.RoleName = "User";
        service._userContext.RoleId = 2;
        service._userContext.UserLoginName = "Regular User";
        service._userContext.UserLoginEmail = "user@konaai.com";
        return service;
    }
}
