using KonaAI.Master.Repository.Domain.Master.UserMetaData;

namespace KonaAI.Master.Test.Integration.Infrastructure.TestData.Builders;

/// <summary>
/// Builder for creating ProjectDepartment test data.
/// </summary>
public class ProjectDepartmentBuilder
{
    private string _name = "Test Department";
    private string _description = "Test department for integration testing";
    private bool _isActive = true;

    public static ProjectDepartmentBuilder Create() => new();

    public ProjectDepartmentBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public ProjectDepartmentBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public ProjectDepartmentBuilder Active()
    {
        _isActive = true;
        return this;
    }

    public ProjectDepartmentBuilder Inactive()
    {
        _isActive = false;
        return this;
    }

    public ProjectDepartment Build()
    {
        return new ProjectDepartment
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
    /// Creates default project departments for testing.
    /// </summary>
    public static List<ProjectDepartment> CreateDefaults()
    {
        return new List<ProjectDepartment>
        {
            Create().WithName("IT").WithDescription("Information Technology").Build(),
            Create().WithName("Finance").WithDescription("Finance Department").Build(),
            Create().WithName("HR").WithDescription("Human Resources").Build(),
            Create().WithName("Operations").WithDescription("Operations Department").Build(),
            Create().WithName("Marketing").WithDescription("Marketing Department").Build()
        };
    }
}
