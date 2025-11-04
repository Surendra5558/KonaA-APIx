using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using FluentAssertions;
using KonaAI.Master.Test.Integration.Infrastructure.Attributes;
using KonaAI.Master.Test.Integration.Infrastructure.Factories;
using KonaAI.Master.Test.Integration.Infrastructure.Fixtures;
using KonaAI.Master.Test.Integration.Infrastructure.TestData.Builders;

namespace KonaAI.Master.Test.Integration.API.Controllers.Master.App;

/// <summary>
/// API integration tests for ClientController using real HTTP pipeline.
/// Tests full request/response cycle with real database.
/// </summary>
[ApiIntegration]
[Collection("InMemoryDatabaseCollection")]
public class ClientControllerApiTests : IClassFixture<InMemoryDatabaseFixture>
{
    private readonly IntegrationTestWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public ClientControllerApiTests(InMemoryDatabaseFixture fixture)
    {
        _factory = new IntegrationTestWebApplicationFactory(fixture);
        _client = _factory.CreateClient();
    }

    private async Task<HttpClient> GetAuthenticatedClientAsync()
    {
        return await _factory.CreateAuthenticatedClientAsync();
    }

    [Fact]
    public async Task GetAsync_ReturnsOk_WithClientData()
    {
        // Arrange
        await _factory.SeedDatabaseAsync();
        var client = await GetAuthenticatedClientAsync();

        // Act
        var response = await client.GetAsync("/v1/Client");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetAsync_WithODataQuery_ReturnsFilteredResults()
    {
        // Arrange
        await _factory.SeedDatabaseAsync();
        var client = await GetAuthenticatedClientAsync();

        // Act
        var response = await client.GetAsync("/v1/Client?$filter=IsActive eq true&$top=5");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task PostAsync_ValidModel_ReturnsCreated()
    {
        // Arrange
        var createModel = ClientBuilder.Create()
            .WithName("New Test Client")
            .WithCode("NTC001")
            .Build();

        var json = System.Text.Json.JsonSerializer.Serialize(createModel);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var client = await GetAuthenticatedClientAsync();

        // Act
        var response = await client.PostAsync("/v1/Client", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task PostAsync_InvalidModel_ReturnsBadRequest()
    {
        // Arrange
        var invalidModel = new { Name = "", ClientCode = "" }; // Invalid data
        var json = System.Text.Json.JsonSerializer.Serialize(invalidModel);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        // Act
        var client = await GetAuthenticatedClientAsync();
        var response = await client.PostAsync("/v1/Client", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetByRowIdAsync_ExistingClient_ReturnsOk()
    {
        // Arrange
        await _factory.SeedDatabaseAsync();
        using var context = _factory.GetTestDbContext();
        var client = ClientBuilder.Create().Build();
        context.Add(client);
        await context.SaveChangesAsync();

        // Act
        var httpClient = await GetAuthenticatedClientAsync();
        var response = await httpClient.GetAsync($"/v1/Client({client.RowId})");


        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetByRowIdAsync_NonExistentClient_ReturnsNotFound()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var httpClient = await GetAuthenticatedClientAsync();
        var response = await httpClient.GetAsync($"/v1/Client({nonExistentId})");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task PutAsync_ValidModel_ReturnsOk()
    {
        // Arrange
        await _factory.SeedDatabaseAsync();
        using var context = _factory.GetTestDbContext();
        var client = ClientBuilder.Create().Build();
        context.Add(client);
        await context.SaveChangesAsync();

        var updateModel = new
        {
            RowId = client.RowId,
            Name = "Updated Client Name",
            DisplayName = "Updated Display Name",
            ClientCode = client.ClientCode,
            Description = "Updated description"
        };

        var json = System.Text.Json.JsonSerializer.Serialize(updateModel);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        // Act
        var httpClient = await GetAuthenticatedClientAsync();
        var response = await httpClient.PutAsync($"/v1/Client({client.RowId})", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteAsync_ExistingClient_ReturnsNoContent()
    {
        // Arrange
        await _factory.SeedDatabaseAsync();
        using var context = _factory.GetTestDbContext();
        var client = ClientBuilder.Create().Build();
        context.Add(client);
        await context.SaveChangesAsync();

        // Act
        var httpClient = await GetAuthenticatedClientAsync();
        var response = await httpClient.DeleteAsync($"/v1/Client({client.RowId})");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteAsync_NonExistentClient_ReturnsNotFound()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var httpClient = await GetAuthenticatedClientAsync();
        var response = await httpClient.DeleteAsync($"/v1/Client({nonExistentId})");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
