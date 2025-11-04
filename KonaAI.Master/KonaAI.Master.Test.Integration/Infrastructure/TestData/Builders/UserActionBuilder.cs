using KonaAI.Master.Repository.Domain.Master.MetaData;
using System;
using System.Collections.Generic;

namespace KonaAI.Master.Test.Integration.Infrastructure.TestData.Builders;

/// <summary>
/// Builder for UserAction entities.
/// </summary>
public class UserActionBuilder
{
    private string _name = "Create";
    private string _description = "Create action";
    private int _orderBy = 1;

    public static UserActionBuilder Create() => new UserActionBuilder();

    public UserActionBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public UserActionBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public UserActionBuilder WithOrderBy(int orderBy)
    {
        _orderBy = orderBy;
        return this;
    }

    public UserAction Build()
    {
        return new UserAction
        {
            RowId = Guid.NewGuid(),
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
    /// Creates default user actions for testing.
    /// </summary>
    public static List<UserAction> CreateDefaults()
    {
        return new List<UserAction>
        {
            Create().WithName("Create").WithDescription("Create action").WithOrderBy(1).Build(),
            Create().WithName("Read").WithDescription("Read action").WithOrderBy(2).Build(),
            Create().WithName("Update").WithDescription("Update action").WithOrderBy(3).Build(),
            Create().WithName("Delete").WithDescription("Delete action").WithOrderBy(4).Build(),
            Create().WithName("Export").WithDescription("Export action").WithOrderBy(5).Build(),
            Create().WithName("Import").WithDescription("Import action").WithOrderBy(6).Build()
        };
    }
}

