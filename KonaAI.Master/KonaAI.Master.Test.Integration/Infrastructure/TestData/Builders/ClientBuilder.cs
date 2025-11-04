using KonaAI.Master.Repository.Domain.Master.App;
using Bogus;

namespace KonaAI.Master.Test.Integration.Infrastructure.TestData.Builders;

/// <summary>
/// Builder for creating Client test data with fluent interface.
/// </summary>
public class ClientBuilder
{
    private string _name = "Test Client";
    private string _displayName = "Test Client Display";
    private string _clientCode = "TC001";
    private string _description = "Test client for integration testing";
    private bool _isActive = true;
    private DateTime _createdOn = DateTime.UtcNow;
    private DateTime _modifiedOn = DateTime.UtcNow;
    private string _createdBy = "testuser";
    private string _modifiedBy = "testuser";

    private static readonly Faker _faker = new Faker();

    public static ClientBuilder Create() => new();

    public static ClientBuilder Create(string name) => new ClientBuilder().WithName(name);

    public ClientBuilder WithName(string name)
    {
        _name = name;
        _displayName = name + " Display";
        return this;
    }

    public ClientBuilder WithDisplayName(string displayName)
    {
        _displayName = displayName;
        return this;
    }

    public ClientBuilder WithCode(string code)
    {
        _clientCode = code;
        return this;
    }

    public ClientBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public ClientBuilder Active()
    {
        _isActive = true;
        return this;
    }

    public ClientBuilder Inactive()
    {
        _isActive = false;
        return this;
    }

    public ClientBuilder CreatedBy(string createdBy)
    {
        _createdBy = createdBy;
        return this;
    }

    public ClientBuilder ModifiedBy(string modifiedBy)
    {
        _modifiedBy = modifiedBy;
        _modifiedOn = DateTime.UtcNow;
        return this;
    }

    public ClientBuilder WithRandomData()
    {
        _name = _faker.Company.CompanyName();
        _displayName = _name + " Display";
        _clientCode = _faker.Random.AlphaNumeric(6).ToUpper();
        _description = _faker.Lorem.Sentence();
        _isActive = _faker.Random.Bool(0.8f); // 80% chance of being active
        return this;
    }

    public Client Build()
    {
        return new Client
        {
            RowId = Guid.NewGuid(),
            Name = _name,
            DisplayName = _displayName,
            ClientCode = _clientCode,
            Description = _description,
            IsActive = _isActive,
            CreatedOn = _createdOn,
            ModifiedOn = _modifiedOn,
            CreatedBy = _createdBy ?? "TestUser",
            ModifiedBy = _modifiedBy ?? "TestUser"
        };
    }

    /// <summary>
    /// Creates multiple clients with sequential data.
    /// </summary>
    public static List<Client> CreateMultiple(int count)
    {
        var clients = new List<Client>();

        for (int i = 1; i <= count; i++)
        {
            var client = Create()
                .WithName($"Test Client {i}")
                .WithCode($"TC{i:D3}")
                .Build();

            clients.Add(client);
        }

        return clients;
    }

    /// <summary>
    /// Creates multiple clients with random data.
    /// </summary>
    public static List<Client> CreateRandomMultiple(int count)
    {
        var clients = new List<Client>();
        var usedNames = new HashSet<string>();
        var usedCodes = new HashSet<string>();

        for (int i = 1; i <= count; i++)
        {
            string name;
            string code;

            // Ensure unique names and codes
            do
            {
                name = _faker.Company.CompanyName();
            } while (usedNames.Contains(name));

            do
            {
                code = _faker.Random.AlphaNumeric(6).ToUpper();
            } while (usedCodes.Contains(code));

            usedNames.Add(name);
            usedCodes.Add(code);

            var client = Create()
                .WithName(name)
                .WithCode(code)
                .WithDescription(_faker.Lorem.Sentence())
                .Active() // Ensure all are active for the test
                .Build();

            clients.Add(client);
        }

        return clients;
    }

    /// <summary>
    /// Creates clients for multi-tenant testing.
    /// </summary>
    public static List<Client> CreateForMultiTenantTesting(int tenantCount = 3)
    {
        var clients = new List<Client>();

        for (int i = 1; i <= tenantCount; i++)
        {
            var client = Create()
                .WithName($"Tenant {i} Client")
                .WithCode($"T{i:D3}")
                .WithDescription($"Test data for tenant {i}")
                .Build();

            clients.Add(client);
        }

        return clients;
    }

    /// <summary>
    /// Creates a client with specific properties for testing edge cases.
    /// </summary>
    public static Client CreateEdgeCase()
    {
        return Create()
            .WithName("A") // Minimum length
            .WithCode("A") // Minimum length
            .WithDescription("") // Empty description
            .Inactive()
            .Build();
    }

    /// <summary>
    /// Creates a client with maximum length properties.
    /// </summary>
    public static Client CreateMaxLength()
    {
        var longName = new string('A', 255);
        var longCode = new string('B', 50);
        var longDescription = new string('C', 1000);

        return Create()
            .WithName(longName)
            .WithCode(longCode)
            .WithDescription(longDescription)
            .Build();
    }
}
