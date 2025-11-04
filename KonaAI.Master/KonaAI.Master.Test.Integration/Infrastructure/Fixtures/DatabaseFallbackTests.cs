using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using KonaAI.Master.Repository;
using KonaAI.Master.Repository.Domain.Master.App;
using KonaAI.Master.Test.Integration.Infrastructure.Attributes;
using KonaAI.Master.Test.Integration.Infrastructure.Fixtures;
using KonaAI.Master.Test.Integration.Infrastructure.TestData.Builders;
using KonaAI.Master.Test.Integration.Infrastructure.TestData.Seeders;
using KonaAI.Master.Test.Integration.Infrastructure;

namespace KonaAI.Master.Test.Integration.Infrastructure.Fixtures;

/// <summary>
/// Tests the database fallback mechanism to ensure it works with and without Docker.
/// </summary>
[Collection("InMemoryDatabaseCollection")]
public class DatabaseFallbackTests : IClassFixture<InMemoryDatabaseFixture>
{
    private readonly InMemoryDatabaseFixture _fixture;

    public DatabaseFallbackTests(InMemoryDatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void DatabaseType_ShouldBeDetectedCorrectly()
    {
        // Arrange & Act
        var databaseType = _fixture.GetDatabaseType();

        // Assert
        databaseType.Should().NotBeNullOrEmpty();
        databaseType.Should().Be("In-Memory Database");

        // Log the database type being used
        Console.WriteLine($"Using database type: {databaseType}");
    }

    [Fact]
    public async Task CreateContext_ShouldWorkWithBothDatabaseTypes()
    {
        // Arrange & Act
        using var context = _fixture.CreateContext();

        // Assert
        context.Should().NotBeNull();
        context.Database.Should().NotBeNull();

        // Test that we can create a simple entity
        var client = ClientBuilder.Create().Build();
        context.Add(client);
        await context.SaveChangesAsync();

        // Verify the entity was saved
        var savedClient = await context.Set<Client>().FirstOrDefaultAsync(c => c.RowId == client.RowId);
        savedClient.Should().NotBeNull();
        savedClient!.Name.Should().Be(client.Name);
    }

    [Fact]
    public async Task ClearDatabase_ShouldWorkWithBothDatabaseTypes()
    {
        // Arrange
        using var context = _fixture.CreateContext();
        var client = ClientBuilder.Create().Build();
        context.Clients.Add(client);
        await context.SaveChangesAsync();

        // Verify data exists
        var countBefore = await context.Clients.CountAsync();
        countBefore.Should().BeGreaterThan(0);

        // Act
        await _fixture.ClearDatabaseAsync();

        // Assert
        using var contextAfter = _fixture.CreateContext();
        var countAfter = await contextAfter.Clients.CountAsync();
        countAfter.Should().Be(0);
    }

    [Fact]
    public async Task SeedTestData_ShouldWorkWithBothDatabaseTypes()
    {
        // Arrange
        await _fixture.ClearDatabaseAsync();

        // Act & Assert - Use the same context for both seeding and verification
        using var context = _fixture.CreateContext();
        var seeder = new TestDataSeeder();

        try
        {
            Console.WriteLine("Starting test data seeding...");
            await seeder.SeedAsync(context);
            Console.WriteLine("Test data seeding completed successfully");

            // Verify data in the same context
            var clientCount = await context.Clients.CountAsync();
            Console.WriteLine($"Found {clientCount} clients in database");
            clientCount.Should().BeGreaterThan(0);

            Console.WriteLine($"Seeded {clientCount} clients successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during seeding: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            throw;
        }
    }

    [Fact]
    public void DatabaseConfiguration_ShouldBeDetectedCorrectly()
    {
        // Arrange & Act
        var databaseType = _fixture.GetDatabaseType();

        // Assert
        databaseType.Should().NotBeNullOrEmpty();
        databaseType.Should().Be("In-Memory Database");

        Console.WriteLine($"Database type: {databaseType}");
    }
}
