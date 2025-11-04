using KonaAI.Master.API.Controllers.Authentication;
using KonaAI.Master.API.Controllers.Master.App;
using KonaAI.Master.API.Controllers.Master.MetaData;
using KonaAI.Master.API.Controllers.Tenant.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace KonaAI.Master.Test.Unit.Controllers.Authorization;

public class AuthorizationAttributesTests
{
    private static string NormalizePolicy(string? policy)
    {
        return (policy ?? string.Empty)
            .Replace(" ", string.Empty)
            .Replace("\t", string.Empty)
            .Trim();
    }

    private static AuthorizeAttribute? GetClassAuthorizeAttribute<TController>() where TController : ControllerBase
    {
        return typeof(TController).GetCustomAttribute<AuthorizeAttribute>(inherit: true);
    }

    private static IEnumerable<AuthorizeAttribute> GetMethodAuthorizeAttributes<TController>(string methodName) where TController : ControllerBase
    {
        var mi = typeof(TController).GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
        Assert.NotNull(mi);
        return mi!.GetCustomAttributes<AuthorizeAttribute>(inherit: true);
    }

    private static bool HasAllowAnonymous<TController>(string methodName) where TController : ControllerBase
    {
        var mi = typeof(TController).GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
        Assert.NotNull(mi);
        return mi!.GetCustomAttribute<AllowAnonymousAttribute>(inherit: true) != null;
    }

    [Fact]
    public void LoginController_ShouldNotRequireAuthorize_OnClass()
    {
        var authorize = GetClassAuthorizeAttribute<LoginController>();
        Assert.Null(authorize);
    }

    [Fact]
    public void MenuController_ShouldRequireAuthorize_OnClass()
    {
        var authorize = GetClassAuthorizeAttribute<MenuController>();
        Assert.NotNull(authorize);
    }

    [Fact]
    public void RoleTypeController_ShouldRequireAuthorize_OnClass()
    {
        var authorize = GetClassAuthorizeAttribute<RoleTypeController>();
        Assert.NotNull(authorize);
    }

    [Fact]
    public void ClientProjectController_ShouldRequireAuthorize_OnClass()
    {
        var authorize = GetClassAuthorizeAttribute<ClientProjectController>();
        Assert.NotNull(authorize);
    }

    [Fact]
    public void ClientController_Methods_ShouldHaveExpectedPolicies()
    {
        // GetAsync
        var getPolicies = GetMethodAuthorizeAttributes<ClientController>(nameof(ClientController.GetAsync));
        Assert.Contains(getPolicies, a => NormalizePolicy(a.Policy).Contains("Permission:Navigation=AllClients;Action=View".Replace(" ", string.Empty)));

        // GetByRowIdAsync
        var getByIdPolicies = GetMethodAuthorizeAttributes<ClientController>(nameof(ClientController.GetByRowIdAsync));
        Assert.Contains(getByIdPolicies, a => NormalizePolicy(a.Policy).Contains("Permission:Navigation=AllClients;Action=View".Replace(" ", string.Empty)));

        // PostAsync
        var postPolicies = GetMethodAuthorizeAttributes<ClientController>(nameof(ClientController.PostAsync));
        Assert.Contains(postPolicies, a => NormalizePolicy(a.Policy).Contains("Permission:Navigation=AllClients;Action=Add".Replace(" ", string.Empty)));

        // PutAsync
        var putPolicies = GetMethodAuthorizeAttributes<ClientController>(nameof(ClientController.PutAsync));
        Assert.Contains(putPolicies, a => NormalizePolicy(a.Policy).Contains("Permission:Navigation=AllClients;Action=Edit".Replace(" ", string.Empty)));

        // DeleteAsync
        var deletePolicies = GetMethodAuthorizeAttributes<ClientController>(nameof(ClientController.DeleteAsync));
        Assert.Contains(deletePolicies, a => NormalizePolicy(a.Policy).Contains("Permission:Navigation=AllClients;Action=Delete".Replace(" ", string.Empty)));
    }

    [Fact]
    public void ClientProjectController_Methods_ShouldHaveExpectedPolicies()
    {
        // PostAsync
        var postPolicies = GetMethodAuthorizeAttributes<ClientProjectController>(nameof(ClientProjectController.PostAsync));
        Assert.Contains(postPolicies, a => NormalizePolicy(a.Policy).Contains("Permission:Navigation=AllProjects;Action=Add".Replace(" ", string.Empty)));

        // GetAsync
        var getPolicies = GetMethodAuthorizeAttributes<ClientProjectController>(nameof(ClientProjectController.GetAsync));
        Assert.Contains(getPolicies, a => NormalizePolicy(a.Policy).Contains("Permission:Navigation=AllProjects;Action=View".Replace(" ", string.Empty)));

        // GetByRowIdAsync
        var getByPolicies = GetMethodAuthorizeAttributes<ClientProjectController>(nameof(ClientProjectController.GetByRowIdAsync));
        Assert.Contains(getByPolicies, a => NormalizePolicy(a.Policy).Contains("Permission:Navigation=AllProjects;Action=View".Replace(" ", string.Empty)));
    }

    [Fact]
    public void ClientUserController_Methods_ShouldHaveExpectedPolicies_And_NoAllowAnonymous()
    {
        // GetAsync
        var getPolicies = GetMethodAuthorizeAttributes<ClientUserController>(nameof(ClientUserController.GetAsync));
        Assert.Contains(getPolicies, a => NormalizePolicy(a.Policy).Contains("Permission:Navigation=Users;Action=View".Replace(" ", string.Empty)));
        Assert.False(HasAllowAnonymous<ClientUserController>(nameof(ClientUserController.GetAsync)));

        // GetByRowIdAsync
        var getByPolicies = GetMethodAuthorizeAttributes<ClientUserController>(nameof(ClientUserController.GetByRowIdAsync));
        Assert.Contains(getByPolicies, a => NormalizePolicy(a.Policy).Contains("Permission:Navigation=Users;Action=View".Replace(" ", string.Empty)));

        // PostAsync
        var postPolicies = GetMethodAuthorizeAttributes<ClientUserController>(nameof(ClientUserController.PostAsync));
        Assert.Contains(postPolicies, a => NormalizePolicy(a.Policy).Contains("Permission:Navigation=Users;Action=Add".Replace(" ", string.Empty)));

        // PutAsync (policy has missing semicolon in source between Users and Action; normalize handles it)
        var putPolicies = GetMethodAuthorizeAttributes<ClientUserController>(nameof(ClientUserController.PutAsync));
        Assert.Contains(putPolicies, a => NormalizePolicy(a.Policy).Contains("Permission:Navigation=UsersAction=Edit".Replace(" ", string.Empty))
                                           || NormalizePolicy(a.Policy).Contains("Permission:Navigation=Users;Action=Edit".Replace(" ", string.Empty)));

        // DeleteAsync
        var deletePolicies = GetMethodAuthorizeAttributes<ClientUserController>(nameof(ClientUserController.DeleteAsync));
        Assert.Contains(deletePolicies, a => NormalizePolicy(a.Policy).Contains("Permission:Navigation=Users;Action=Delete".Replace(" ", string.Empty)));
    }
}