using KonaAI.Master.Repository.Domain.Master.MetaData;

namespace KonaAI.Master.Test.Integration.Infrastructure.TestData.Builders;

/// <summary>
/// Builder for creating NavigationAction test data.
/// </summary>
public class NavigationActionBuilder
{
    private string _name = "Test Action";
    private string _displayName = "Test Action Display";
    private string _description = "Test navigation action for integration testing";
    private bool _isActive = true;

    public static NavigationActionBuilder Create() => new();

    public NavigationActionBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public NavigationActionBuilder WithDisplayName(string displayName)
    {
        _displayName = displayName;
        return this;
    }

    public NavigationActionBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public NavigationActionBuilder Active()
    {
        _isActive = true;
        return this;
    }

    public NavigationActionBuilder Inactive()
    {
        _isActive = false;
        return this;
    }

    public Navigation Build()
    {
        return new Navigation
        {
            RowId = Guid.NewGuid(),
            Name = _name,
            Description = _description,
            IsActive = _isActive,
            CreatedOn = DateTime.UtcNow,
            CreatedBy = "testuser",
            ModifiedOn = DateTime.UtcNow,
            ModifiedBy = "testuser"
        };
    }

    /// <summary>
    /// Creates default navigation actions for testing.
    /// </summary>
    public static List<Navigation> CreateDefaults()
    {
        return new List<Navigation>
        {
            Create().WithName("Create").WithDisplayName("Create").Build(),
            Create().WithName("Read").WithDisplayName("View").Build(),
            Create().WithName("Update").WithDisplayName("Edit").Build(),
            Create().WithName("Delete").WithDisplayName("Delete").Build(),
            Create().WithName("Export").WithDisplayName("Export").Build(),
            Create().WithName("Import").WithDisplayName("Import").Build()
        };
    }
}
