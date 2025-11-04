using KonaAI.Master.Repository.Domain.Master.MetaData;

namespace KonaAI.Master.Test.Integration.Infrastructure.TestData.Builders;

/// <summary>
/// Builder for creating RoleType test data.
/// </summary>
public class RoleTypeBuilder
{
    private string _name = "Test Role Type";
    private string _description = "Test role type for integration testing";
    private bool _isActive = true;

    public static RoleTypeBuilder Create() => new();

    public RoleTypeBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public RoleTypeBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public RoleTypeBuilder Active()
    {
        _isActive = true;
        return this;
    }

    public RoleTypeBuilder Inactive()
    {
        _isActive = false;
        return this;
    }

    public RoleType Build()
    {
        return new RoleType
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
    /// Creates default role types for testing.
    /// </summary>
    public static List<RoleType> CreateDefaults()
    {
        return new List<RoleType>
        {
            Create().WithName("Administrator").WithDescription("System Administrator").Build(),
            Create().WithName("Manager").WithDescription("Project Manager").Build(),
            Create().WithName("User").WithDescription("Regular User").Build(),
            Create().WithName("Auditor").WithDescription("Audit User").Build(),
            Create().WithName("Viewer").WithDescription("Read-only User").Build()
        };
    }
}
