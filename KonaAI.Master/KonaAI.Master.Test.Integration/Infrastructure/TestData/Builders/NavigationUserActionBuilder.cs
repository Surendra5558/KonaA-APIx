using KonaAI.Master.Repository.Domain.Master.MetaData;
using System;
using System.Collections.Generic;

namespace KonaAI.Master.Test.Integration.Infrastructure.TestData.Builders;

/// <summary>
/// Builder for NavigationUserAction entities.
/// </summary>
public class NavigationUserActionBuilder
{
    private long _navigationId = 1;
    private long _userActionId = 1;
    private string _name = "Navigation Action";
    private string _description = "Navigation action description";
    private int _orderBy = 1;

    public static NavigationUserActionBuilder Create() => new NavigationUserActionBuilder();

    public NavigationUserActionBuilder WithNavigationId(long navigationId)
    {
        _navigationId = navigationId;
        return this;
    }

    public NavigationUserActionBuilder WithUserActionId(long userActionId)
    {
        _userActionId = userActionId;
        return this;
    }

    public NavigationUserActionBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public NavigationUserActionBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public NavigationUserActionBuilder WithOrderBy(int orderBy)
    {
        _orderBy = orderBy;
        return this;
    }

    public NavigationUserAction Build()
    {
        return new NavigationUserAction
        {
            RowId = Guid.NewGuid(),
            NavigationId = _navigationId,
            UserActionId = _userActionId,
            Name = _name,
            Description = _description,
            OrderBy = _orderBy,
            IsActive = true,
            IsDeleted = false,
            CreatedOn = DateTime.UtcNow,
            CreatedBy = "testsystem",
            ModifiedOn = DateTime.UtcNow,
            ModifiedBy = "testsystem"
        };
    }

    /// <summary>
    /// Creates default navigation user actions for testing.
    /// Assumes Navigation IDs 1, 2, 3 and UserAction IDs 1, 2, 3, 4 exist.
    /// </summary>
    public static List<NavigationUserAction> CreateDefaults()
    {
        var navigationUserActions = new List<NavigationUserAction>();

        // Navigation 1 (Dashboard) with all actions
        for (int actionId = 1; actionId <= 4; actionId++)
        {
            navigationUserActions.Add(Create()
                .WithNavigationId(1)
                .WithUserActionId(actionId)
                .WithName($"Dashboard Action {actionId}")
                .WithDescription($"Dashboard action {actionId} description")
                .WithOrderBy(actionId)
                .Build());
        }

        // Navigation 2 (Clients) with all actions
        for (int actionId = 1; actionId <= 4; actionId++)
        {
            navigationUserActions.Add(Create()
                .WithNavigationId(2)
                .WithUserActionId(actionId)
                .WithName($"Client Action {actionId}")
                .WithDescription($"Client action {actionId} description")
                .WithOrderBy(actionId)
                .Build());
        }

        // Navigation 3 (Projects) with all actions
        for (int actionId = 1; actionId <= 4; actionId++)
        {
            navigationUserActions.Add(Create()
                .WithNavigationId(3)
                .WithUserActionId(actionId)
                .WithName($"Project Action {actionId}")
                .WithDescription($"Project action {actionId} description")
                .WithOrderBy(actionId)
                .Build());
        }

        return navigationUserActions;
    }
}

