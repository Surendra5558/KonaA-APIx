using KonaAI.Master.Repository.Domain.Tenant.Client;
using Bogus;

namespace KonaAI.Master.Test.Integration.Infrastructure.TestData.Builders;

/// <summary>
/// Builder for creating ClientProject test data with fluent interface.
/// </summary>
public class ClientProjectBuilder
{
    private long _clientId = 1;
    private string _name = "Test Project";
    private string _description = "Test project for integration testing";
    private DateTime _startDate = DateTime.UtcNow;
    private DateTime? _endDate = DateTime.UtcNow.AddMonths(6);
    private bool _isActive = true;
    private DateTime _createdOn = DateTime.UtcNow;
    private string _createdBy = "testuser";
    private string _modules = "T&E";

    private static readonly Faker _faker = new Faker();

    public static ClientProjectBuilder Create() => new();

    public static ClientProjectBuilder CreateForClient(long clientId) => new ClientProjectBuilder().WithClientId(clientId);

    public ClientProjectBuilder WithClientId(long clientId)
    {
        _clientId = clientId;
        return this;
    }

    public ClientProjectBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public ClientProjectBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public ClientProjectBuilder WithStartDate(DateTime startDate)
    {
        _startDate = startDate;
        return this;
    }

    public ClientProjectBuilder WithEndDate(DateTime? endDate)
    {
        _endDate = endDate;
        return this;
    }

    public ClientProjectBuilder Active()
    {
        _isActive = true;
        return this;
    }

    public ClientProjectBuilder Inactive()
    {
        _isActive = false;
        return this;
    }

    public ClientProjectBuilder CreatedBy(string createdBy)
    {
        _createdBy = createdBy;
        return this;
    }

    public ClientProjectBuilder WithModules(string modules)
    {
        _modules = modules;
        return this;
    }

    public ClientProjectBuilder WithRandomData()
    {
        _name = _faker.Commerce.ProductName();
        _description = _faker.Lorem.Paragraph();
        _startDate = _faker.Date.Past();
        _endDate = _faker.Date.Future();
        _isActive = _faker.Random.Bool(0.8f);
        _modules = _faker.PickRandom(new[] { "T&E", "P2P", "O2C", "R2R", "H2R" });
        return this;
    }

    public ClientProject Build()
    {
        return new ClientProject
        {
            RowId = Guid.NewGuid(),
            ClientId = _clientId,
            Name = _name,
            Description = _description,
            StartDate = _startDate,
            EndDate = _endDate,
            IsActive = _isActive,
            CreatedOn = _createdOn,
            CreatedBy = _createdBy,
            ModifiedOn = DateTime.UtcNow,
            ModifiedBy = "testuser",
            ProjectAuditResponsibilityId = 1,
            ProjectRiskAreaId = 1,
            ProjectStatusId = 1,
            Modules = _modules
        };
    }

    /// <summary>
    /// Creates multiple projects for a specific client.
    /// </summary>
    public List<ClientProject> CreateMultiple(int count)
    {
        var projects = new List<ClientProject>();

        for (int i = 1; i <= count; i++)
        {
            var project = WithName($"Project {i}")
                .WithDescription($"Test project {i} for client {_clientId}")
                .Build();

            projects.Add(project);
        }

        return projects;
    }

    /// <summary>
    /// Creates multiple projects with random data.
    /// </summary>
    public List<ClientProject> CreateRandomMultiple(int count)
    {
        var projects = new List<ClientProject>();

        for (int i = 1; i <= count; i++)
        {
            var project = WithRandomData().Build();
            projects.Add(project);
        }

        return projects;
    }

    /// <summary>
    /// Creates projects for performance testing.
    /// </summary>
    public static List<ClientProject> CreateForPerformanceTesting(long clientId, int count = 1000)
    {
        var projects = new List<ClientProject>();

        for (int i = 1; i <= count; i++)
        {
            var project = CreateForClient(clientId)
                .WithName($"Perf Project {i}")
                .WithDescription($"Performance test project {i}")
                .Build();

            projects.Add(project);
        }

        return projects;
    }

    /// <summary>
    /// Creates projects for concurrent testing.
    /// </summary>
    public static List<ClientProject> CreateForConcurrentTesting(long clientId, int count = 10)
    {
        var projects = new List<ClientProject>();

        for (int i = 1; i <= count; i++)
        {
            var project = CreateForClient(clientId)
                .WithName($"Concurrent Project {i}")
                .WithDescription($"Concurrent test project {i}")
                .Build();

            projects.Add(project);
        }

        return projects;
    }
}
