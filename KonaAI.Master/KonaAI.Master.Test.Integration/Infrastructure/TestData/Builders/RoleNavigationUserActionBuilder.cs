using KonaAI.Master.Repository.Domain.Master.MetaData;
using System;
using System.Collections.Generic;

namespace KonaAI.Master.Test.Integration.Infrastructure.TestData.Builders;

/// <summary>
/// Builder for RoleNavigationUserAction entities.
/// </summary>
public class RoleNavigationUserActionBuilder
{
    private long _navigationUserActionId = 1;
    private long _roleTypeId = 1;
    private string _name = "Role Navigation Action";
    private string _description = "Role navigation action description";
    private int _orderBy = 1;
    private bool _isVisible = true;
    private bool _isAccessible = true;

    public static RoleNavigationUserActionBuilder Create() => new RoleNavigationUserActionBuilder();

    public RoleNavigationUserActionBuilder WithNavigationUserActionId(long navigationUserActionId)
    {
        _navigationUserActionId = navigationUserActionId;
        return this;
    }

    public RoleNavigationUserActionBuilder WithRoleTypeId(long roleTypeId)
    {
        _roleTypeId = roleTypeId;
        return this;
    }

    public RoleNavigationUserActionBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public RoleNavigationUserActionBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public RoleNavigationUserActionBuilder WithOrderBy(int orderBy)
    {
        _orderBy = orderBy;
        return this;
    }

    public RoleNavigationUserActionBuilder WithVisibility(bool isVisible, bool isAccessible)
    {
        _isVisible = isVisible;
        _isAccessible = isAccessible;
        return this;
    }

    public RoleNavigationUserAction Build()
    {
        return new RoleNavigationUserAction
        {
            RowId = Guid.NewGuid(),
            NavigationUserActionId = _navigationUserActionId,
            RoleTypeId = _roleTypeId,
            Name = _name,
            Description = _description,
            OrderBy = _orderBy,
            IsVisible = _isVisible,
            IsAccessible = _isAccessible,
            IsActive = true,
            IsDeleted = false,
            CreatedOn = DateTime.UtcNow,
            CreatedBy = "testsystem",
            ModifiedOn = DateTime.UtcNow,
            ModifiedBy = "testsystem"
        };
    }

    /// <summary>
    /// Creates default role navigation user actions for testing.
    /// Assumes NavigationUserAction IDs 1-12 and RoleType IDs 1, 2, 3 exist.
    /// </summary>
    public static List<RoleNavigationUserAction> CreateDefaults()
    {
        var roleNavigationUserActions = new List<RoleNavigationUserAction>();

        // Admin role (RoleType 1) - has access to all navigation actions
        for (int navigationUserActionId = 1; navigationUserActionId <= 12; navigationUserActionId++)
        {
            roleNavigationUserActions.Add(Create()
                .WithNavigationUserActionId(navigationUserActionId)
                .WithRoleTypeId(1) // Admin role
                .WithName($"Admin Navigation Action {navigationUserActionId}")
                .WithDescription($"Admin navigation action {navigationUserActionId} description")
                .WithOrderBy(navigationUserActionId)
                .WithVisibility(true, true)
                .Build());
        }

        // Manager role (RoleType 2) - has access to most navigation actions
        for (int navigationUserActionId = 1; navigationUserActionId <= 8; navigationUserActionId++)
        {
            roleNavigationUserActions.Add(Create()
                .WithNavigationUserActionId(navigationUserActionId)
                .WithRoleTypeId(2) // Manager role
                .WithName($"Manager Navigation Action {navigationUserActionId}")
                .WithDescription($"Manager navigation action {navigationUserActionId} description")
                .WithOrderBy(navigationUserActionId)
                .WithVisibility(true, true)
                .Build());
        }

        // User role (RoleType 3) - has limited access
        for (int navigationUserActionId = 1; navigationUserActionId <= 4; navigationUserActionId++)
        {
            roleNavigationUserActions.Add(Create()
                .WithNavigationUserActionId(navigationUserActionId)
                .WithRoleTypeId(3) // User role
                .WithName($"User Navigation Action {navigationUserActionId}")
                .WithDescription($"User navigation action {navigationUserActionId} description")
                .WithOrderBy(navigationUserActionId)
                .WithVisibility(true, true)
                .Build());
        }

        return roleNavigationUserActions;
    }
}

