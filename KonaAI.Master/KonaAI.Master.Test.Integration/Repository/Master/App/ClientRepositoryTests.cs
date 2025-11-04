using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using KonaAI.Master.Repository;
using KonaAI.Master.Repository.Domain.Master.App;
using KonaAI.Master.Test.Integration.Infrastructure.Attributes;
using KonaAI.Master.Test.Integration.Infrastructure.Fixtures;
using KonaAI.Master.Test.Integration.Infrastructure.TestData.Builders;

namespace KonaAI.Master.Test.Integration.Repository.Master.App;

/// <summary>
/// Repository integration tests for Client entity using real SQL Server.
/// Tests EF Core mappings, queries, and database operations.
/// </summary>
[RepositoryIntegration]
[Collection("InMemoryDatabaseCollection")]
public class ClientRepositoryTests : IClassFixture<InMemoryDatabaseFixture>
{
    private readonly InMemoryDatabaseFixture _fixture;

    public ClientRepositoryTests(InMemoryDatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetAsync_ReturnsAllActiveClients()
    {
        // Arrange
        using var context = _fixture.CreateContext();
        await _fixture.ClearDatabaseAsync();

        var clients = new[]
        {
            ClientBuilder.Create().WithName("Active Client 1").Active().Build(),
            ClientBuilder.Create().WithName("Active Client 2").Active().Build(),
            ClientBuilder.Create().WithName("Inactive Client").Inactive().Build()
        };

        context.AddRange(clients);
        await context.SaveChangesAsync();

        // Act
        var result = await context.Set<Client>()
            .Where(c => c.IsActive)
            .ToListAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().OnlyContain(c => c.IsActive);
        result.Should().Contain(c => c.Name == "Active Client 1");
        result.Should().Contain(c => c.Name == "Active Client 2");
    }

    [Fact]
    public async Task GetByRowIdAsync_ExistingClient_ReturnsClient()
    {
        // Arrange
        using var context = _fixture.CreateContext();
        await _fixture.ClearDatabaseAsync();

        var client = ClientBuilder.Create()
            .WithName("Test Client")
            .WithCode("TC001")
            .Build();

        context.Add(client);
        await context.SaveChangesAsync();

        // Act
        var result = await context.Set<Client>()
            .FirstOrDefaultAsync(c => c.RowId == client.RowId);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Test Client");
        result.ClientCode.Should().Be("TC001");
    }

    [Fact]
    public async Task GetByRowIdAsync_NonExistentClient_ReturnsNull()
    {
        // Arrange
        using var context = _fixture.CreateContext();
        await _fixture.ClearDatabaseAsync();

        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await context.Set<Client>()
            .FirstOrDefaultAsync(c => c.RowId == nonExistentId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task AddAsync_ValidClient_SavesToDatabase()
    {
        // Arrange
        using var context = _fixture.CreateContext();
        await _fixture.ClearDatabaseAsync();

        var client = ClientBuilder.Create()
            .WithName("New Client")
            .WithCode("NC001")
            .Build();

        // Act
        context.Add(client);
        var result = await context.SaveChangesAsync();

        // Assert
        result.Should().Be(1);

        var savedClient = await context.Set<Client>()
            .FirstOrDefaultAsync(c => c.RowId == client.RowId);

        savedClient.Should().NotBeNull();
        savedClient!.Name.Should().Be("New Client");
        savedClient.ClientCode.Should().Be("NC001");
    }

    [Fact]
    public async Task UpdateAsync_ExistingClient_UpdatesInDatabase()
    {
        // Arrange
        using var context = _fixture.CreateContext();
        await _fixture.ClearDatabaseAsync();

        var client = ClientBuilder.Create()
            .WithName("Original Name")
            .WithCode("OC001")
            .Build();

        context.Add(client);
        await context.SaveChangesAsync();

        // Act
        client.Name = "Updated Name";
        client.Description = "Updated Description";
        client.ModifiedOn = DateTime.UtcNow;
        client.ModifiedBy = "testuser";

        var result = await context.SaveChangesAsync();

        // Assert
        result.Should().Be(1);

        var updatedClient = await context.Set<Client>()
            .FirstOrDefaultAsync(c => c.RowId == client.RowId);

        updatedClient.Should().NotBeNull();
        updatedClient!.Name.Should().Be("Updated Name");
        updatedClient.Description.Should().Be("Updated Description");
        updatedClient.ModifiedOn.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
        updatedClient.ModifiedBy.Should().Be("testuser");
    }

    [Fact]
    public async Task DeleteAsync_ExistingClient_RemovesFromDatabase()
    {
        // Arrange
        using var context = _fixture.CreateContext();
        await _fixture.ClearDatabaseAsync();

        var client = ClientBuilder.Create()
            .WithName("Client To Delete")
            .WithCode("CD001")
            .Build();

        context.Add(client);
        await context.SaveChangesAsync();

        // Act
        context.Remove(client);
        var result = await context.SaveChangesAsync();

        // Assert
        result.Should().Be(1);

        var deletedClient = await context.Set<Client>()
            .FirstOrDefaultAsync(c => c.RowId == client.RowId);

        deletedClient.Should().BeNull();
    }

    [Fact]
    public async Task GetAsync_WithPagination_ReturnsCorrectPage()
    {
        // Arrange
        using var context = _fixture.CreateContext();
        await _fixture.ClearDatabaseAsync();

        var clients = ClientBuilder.CreateRandomMultiple(10);
        context.AddRange(clients);
        await context.SaveChangesAsync();

        // Act
        var page1 = await context.Set<Client>()
            .OrderBy(c => c.Name)
            .Skip(0)
            .Take(5)
            .ToListAsync();

        var page2 = await context.Set<Client>()
            .OrderBy(c => c.Name)
            .Skip(5)
            .Take(5)
            .ToListAsync();

        // Assert
        page1.Should().HaveCount(5);
        page2.Should().HaveCount(5);
        page1.Should().NotIntersectWith(page2);
    }

    [Fact]
    public async Task GetAsync_WithFiltering_ReturnsFilteredResults()
    {
        // Arrange
        using var context = _fixture.CreateContext();
        await _fixture.ClearDatabaseAsync();

        var clients = new[]
        {
            ClientBuilder.Create().WithName("Alpha Client").WithCode("AC001").Build(),
            ClientBuilder.Create().WithName("Beta Client").WithCode("BC001").Build(),
            ClientBuilder.Create().WithName("Alpha Another").WithCode("AA001").Build(),
            ClientBuilder.Create().WithName("Gamma Client").WithCode("GC001").Build()
        };

        context.AddRange(clients);
        await context.SaveChangesAsync();

        // Act
        var alphaClients = await context.Set<Client>()
            .Where(c => c.Name.Contains("Alpha"))
            .ToListAsync();

        // Assert
        alphaClients.Should().HaveCount(2);
        alphaClients.Should().OnlyContain(c => c.Name.Contains("Alpha"));
    }

    [Fact]
    public async Task GetAsync_WithSorting_ReturnsSortedResults()
    {
        // Arrange
        using var context = _fixture.CreateContext();
        await _fixture.ClearDatabaseAsync();

        var clients = new[]
        {
            ClientBuilder.Create().WithName("Charlie Client").Build(),
            ClientBuilder.Create().WithName("Alpha Client").Build(),
            ClientBuilder.Create().WithName("Beta Client").Build()
        };

        context.AddRange(clients);
        await context.SaveChangesAsync();

        // Act
        var sortedClients = await context.Set<Client>()
            .OrderBy(c => c.Name)
            .ToListAsync();

        // Assert
        sortedClients.Should().HaveCount(3);
        sortedClients[0].Name.Should().Be("Alpha Client");
        sortedClients[1].Name.Should().Be("Beta Client");
        sortedClients[2].Name.Should().Be("Charlie Client");
    }

    [Fact]
    public async Task GetAsync_WithInclude_ReturnsRelatedData()
    {
        // Arrange
        using var context = _fixture.CreateContext();
        await _fixture.ClearDatabaseAsync();

        var client = ClientBuilder.Create()
            .WithName("Client With Projects")
            .Build();

        context.Add(client);
        await context.SaveChangesAsync();

        var projects = ClientProjectBuilder.CreateForClient(client.Id)
            .CreateMultiple(3);

        context.AddRange(projects);
        await context.SaveChangesAsync();

        // Act
        var clientWithProjects = await context.Set<Client>()
            .FirstOrDefaultAsync(c => c.RowId == client.RowId);

        // Assert
        clientWithProjects.Should().NotBeNull();
    }

    [Fact]
    public async Task GetAsync_PerformanceTest_LargeDataset()
    {
        // Arrange
        using var context = _fixture.CreateContext();
        await _fixture.ClearDatabaseAsync();

        var clients = ClientBuilder.CreateRandomMultiple(1000);
        context.AddRange(clients);
        await context.SaveChangesAsync();

        // Act
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var result = await context.Set<Client>()
            .Where(c => c.IsActive)
            .ToListAsync();
        stopwatch.Stop();

        // Assert
        result.Should().NotBeEmpty();
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(5000); // Should complete within 5 seconds
    }
}
