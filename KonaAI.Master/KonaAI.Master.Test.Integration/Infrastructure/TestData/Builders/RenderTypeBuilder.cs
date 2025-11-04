using KonaAI.Master.Repository.Domain.Master.MetaData;

namespace KonaAI.Master.Test.Integration.Infrastructure.TestData.Builders;

/// <summary>
/// Builder for creating RenderType test data.
/// </summary>
public class RenderTypeBuilder
{
    private string _name = "Test Render Type";
    private string _description = "Test render type for integration testing";
    private bool _isActive = true;

    public static RenderTypeBuilder Create() => new();

    public RenderTypeBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public RenderTypeBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public RenderTypeBuilder Active()
    {
        _isActive = true;
        return this;
    }

    public RenderTypeBuilder Inactive()
    {
        _isActive = false;
        return this;
    }

    public RenderType Build()
    {
        return new RenderType
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
    /// Creates default render types for testing.
    /// </summary>
    public static List<RenderType> CreateDefaults()
    {
        return new List<RenderType>
        {
            Create().WithName("PDF").WithDescription("PDF Render Type").Build(),
            Create().WithName("Excel").WithDescription("Excel Render Type").Build(),
            Create().WithName("Word").WithDescription("Word Render Type").Build(),
            Create().WithName("HTML").WithDescription("HTML Render Type").Build(),
            Create().WithName("JSON").WithDescription("JSON Render Type").Build()
        };
    }
}
