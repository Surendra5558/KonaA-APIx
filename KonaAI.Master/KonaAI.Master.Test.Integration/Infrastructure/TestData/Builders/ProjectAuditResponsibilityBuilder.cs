using KonaAI.Master.Repository.Domain.Master.UserMetaData;

namespace KonaAI.Master.Test.Integration.Infrastructure.TestData.Builders;

/// <summary>
/// Builder for creating ProjectAuditResponsibility test data.
/// </summary>
public class ProjectAuditResponsibilityBuilder
{
    private string _name = "Test Audit Responsibility";
    private string _description = "Test audit responsibility for integration testing";
    private bool _isActive = true;

    public static ProjectAuditResponsibilityBuilder Create() => new();

    public ProjectAuditResponsibilityBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public ProjectAuditResponsibilityBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public ProjectAuditResponsibilityBuilder Active()
    {
        _isActive = true;
        return this;
    }

    public ProjectAuditResponsibilityBuilder Inactive()
    {
        _isActive = false;
        return this;
    }

    public ProjectAuditResponsibility Build()
    {
        return new ProjectAuditResponsibility
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
    /// Creates default project audit responsibilities for testing.
    /// </summary>
    public static List<ProjectAuditResponsibility> CreateDefaults()
    {
        return new List<ProjectAuditResponsibility>
        {
            Create().WithName("Internal Audit").WithDescription("Internal Audit Responsibility").Build(),
            Create().WithName("External Audit").WithDescription("External Audit Responsibility").Build(),
            Create().WithName("Compliance Audit").WithDescription("Compliance Audit Responsibility").Build(),
            Create().WithName("Financial Audit").WithDescription("Financial Audit Responsibility").Build()
        };
    }
}
