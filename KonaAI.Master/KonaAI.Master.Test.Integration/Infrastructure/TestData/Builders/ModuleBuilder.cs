using KonaAI.Master.Repository.Domain.Master.App;
using Bogus;
using KonaAI.Master.Repository.Domain.Master.MetaData;

namespace KonaAI.Master.Test.Integration.Infrastructure.TestData.Builders;

/// <summary>
/// Builder for creating Module test data.
/// </summary>
public class ModuleBuilder
{
    private string _name = "Test Module";
    private string _displayName = "Test Module Display";
    private string _description = "Test module for integration testing";
    private bool _isActive = true;

    private static readonly Faker _faker = new();

    public static ModuleBuilder Create() => new();

    public ModuleBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public ModuleBuilder WithDisplayName(string displayName)
    {
        _displayName = displayName;
        return this;
    }

    public ModuleBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public ModuleBuilder Active()
    {
        _isActive = true;
        return this;
    }

    public ModuleBuilder Inactive()
    {
        _isActive = false;
        return this;
    }

    public ModuleType Build()
    {
        return new ModuleType
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
    /// Creates default modules for testing.
    /// </summary>
    public static List<ModuleType> CreateDefaults()
    {
        return new List<ModuleType>
        {
            Create().WithName("UserManagement").WithDisplayName("User Management").Build(),
            Create().WithName("ProjectManagement").WithDisplayName("Project Management").Build(),
            Create().WithName("CaseManagement").WithDisplayName("Case Management").Build(),
            Create().WithName("DataManagement").WithDisplayName("Data Management").Build(),
            Create().WithName("Reporting").WithDisplayName("Reporting").Build(),
            Create().WithName("Audit").WithDisplayName("Audit").Build(),
            Create().WithName("Settings").WithDisplayName("Settings").Build()
        };
    }
}
