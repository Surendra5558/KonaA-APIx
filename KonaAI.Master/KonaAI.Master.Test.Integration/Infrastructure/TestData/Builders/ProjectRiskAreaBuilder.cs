using KonaAI.Master.Repository.Domain.Master.UserMetaData;

namespace KonaAI.Master.Test.Integration.Infrastructure.TestData.Builders;

/// <summary>
/// Builder for creating ProjectRiskArea test data.
/// </summary>
public class ProjectRiskAreaBuilder
{
    private string _name = "Test Risk Area";
    private string _description = "Test risk area for integration testing";
    private bool _isActive = true;

    public static ProjectRiskAreaBuilder Create() => new();

    public ProjectRiskAreaBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public ProjectRiskAreaBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public ProjectRiskAreaBuilder Active()
    {
        _isActive = true;
        return this;
    }

    public ProjectRiskAreaBuilder Inactive()
    {
        _isActive = false;
        return this;
    }

    public ProjectRiskArea Build()
    {
        return new ProjectRiskArea
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
    /// Creates default project risk areas for testing.
    /// </summary>
    public static List<ProjectRiskArea> CreateDefaults()
    {
        return new List<ProjectRiskArea>
        {
            Create().WithName("Security").WithDescription("Security Risk").Build(),
            Create().WithName("Performance").WithDescription("Performance Risk").Build(),
            Create().WithName("Compliance").WithDescription("Compliance Risk").Build(),
            Create().WithName("Financial").WithDescription("Financial Risk").Build(),
            Create().WithName("Operational").WithDescription("Operational Risk").Build()
        };
    }
}
