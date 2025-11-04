using KonaAI.Master.Repository.Domain.Master.UserMetaData;

namespace KonaAI.Master.Test.Integration.Infrastructure.TestData.Builders;

/// <summary>
/// Builder for creating ProjectUnit test data.
/// </summary>
public class ProjectUnitBuilder
{
    private string _name = "Test Unit";
    private string _description = "Test unit for integration testing";
    private bool _isActive = true;

    public static ProjectUnitBuilder Create() => new();

    public ProjectUnitBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public ProjectUnitBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public ProjectUnitBuilder Active()
    {
        _isActive = true;
        return this;
    }

    public ProjectUnitBuilder Inactive()
    {
        _isActive = false;
        return this;
    }

    public ProjectUnit Build()
    {
        return new ProjectUnit
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
    /// Creates default project units for testing.
    /// </summary>
    public static List<ProjectUnit> CreateDefaults()
    {
        return new List<ProjectUnit>
        {
            Create().WithName("Development").WithDescription("Development Unit").Build(),
            Create().WithName("Testing").WithDescription("Testing Unit").Build(),
            Create().WithName("Support").WithDescription("Support Unit").Build(),
            Create().WithName("Maintenance").WithDescription("Maintenance Unit").Build()
        };
    }
}
