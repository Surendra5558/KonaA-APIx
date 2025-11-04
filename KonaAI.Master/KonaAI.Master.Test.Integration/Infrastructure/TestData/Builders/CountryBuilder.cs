using KonaAI.Master.Repository.Domain.Master.App;
using Bogus;
using KonaAI.Master.Repository.Domain.Master.MetaData;

namespace KonaAI.Master.Test.Integration.Infrastructure.TestData.Builders;

/// <summary>
/// Builder for creating Country test data.
/// </summary>
public class CountryBuilder
{
    private string _name = "Test Country";
    private string _code = "TC";
    private bool _isActive = true;

    private static readonly Faker _faker = new();

    public static CountryBuilder Create() => new();

    public CountryBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public CountryBuilder WithCode(string code)
    {
        _code = code;
        return this;
    }

    public CountryBuilder Active()
    {
        _isActive = true;
        return this;
    }

    public CountryBuilder Inactive()
    {
        _isActive = false;
        return this;
    }

    public Country Build()
    {
        return new Country
        {
            RowId = Guid.NewGuid(),
            Name = _name,
            CountryCode = _code,
            IsActive = _isActive,
            CreatedOn = DateTime.UtcNow,
            CreatedBy = "testuser",
            ModifiedOn = DateTime.UtcNow,
            ModifiedBy = "testuser"
        };
    }

    /// <summary>
    /// Creates default countries for testing.
    /// </summary>
    public static List<Country> CreateDefaults()
    {
        return new List<Country>
        {
            Create().WithName("United States").WithCode("US").Build(),
            Create().WithName("Canada").WithCode("CA").Build(),
            Create().WithName("United Kingdom").WithCode("GB").Build(),
            Create().WithName("Germany").WithCode("DE").Build(),
            Create().WithName("France").WithCode("FR").Build(),
            Create().WithName("Australia").WithCode("AU").Build(),
            Create().WithName("Japan").WithCode("JP").Build(),
            Create().WithName("India").WithCode("IN").Build()
        };
    }
}
