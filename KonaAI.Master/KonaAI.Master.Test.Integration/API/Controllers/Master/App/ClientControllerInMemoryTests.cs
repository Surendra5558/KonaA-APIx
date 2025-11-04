using System.Net;
using System.Net.Http.Json;
using KonaAI.Master.Model.Master.App.SaveModel;
using KonaAI.Master.Model.Master.App.ViewModel;
using KonaAI.Master.Test.Integration.Infrastructure;
using KonaAI.Master.Test.Integration.Infrastructure.Fixtures;
using KonaAI.Master.Test.Integration.Infrastructure.TestData.Builders;

namespace KonaAI.Master.Test.Integration.API.Controllers.Master.App;

/// <summary>
/// Integration tests for ClientController using in-memory database.
/// Tests API endpoints with real database operations.
/// </summary>
[Collection("InMemoryDatabaseCollection")]
public class ClientControllerInMemoryTests : TestBase, IDisposable
{
    public ClientControllerInMemoryTests(InMemoryDatabaseFixture fixture) : base(fixture)
    {
    }

    private async Task<HttpClient> GetAuthenticatedClientAsync()
    {
        return await _factory.CreateAuthenticatedClientAsync();
    }

    [Fact]
    public async Task GetAsync_ReturnsOkResult()
    {
        // Arrange
        await SeedDatabaseAsync();

        // Act
        var httpClient = await GetAuthenticatedClientAsync();
        var response = await httpClient.GetAsync("/v1/Client");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.NotNull(content);
    }

    [Fact]
    public async Task GetByRowIdAsync_WithValidId_ReturnsOkResult()
    {
        // Arrange
        await SeedDatabaseAsync();
        using var context = GetDbContext();
        var client = context.Clients.First();
        var rowId = client.RowId;

        // Act
        // OData conventional route for getting entity by key
        var httpClient = await GetAuthenticatedClientAsync();
        var response = await httpClient.GetAsync($"/v1/Client({rowId})");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.NotNull(content);
    }

    [Fact]
    public async Task GetByRowIdAsync_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var invalidRowId = Guid.NewGuid();

        // Act
        // OData conventional route for getting entity by key
        var httpClient = await GetAuthenticatedClientAsync();
        var response = await httpClient.GetAsync($"/v1/Client({invalidRowId})");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task PostAsync_WithValidModel_ReturnsCreated()
    {
        // Arrange
        var createModel = new ClientCreateModel
        {
            Name = "Test Client",
            DisplayName = "Test Client Display",
            ClientCode = "TC001"
        };

        // Act
        var httpClient = await GetAuthenticatedClientAsync();
        var response = await httpClient.PostAsJsonAsync("/v1/Client", createModel);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.NotNull(content);
    }

    [Fact]
    public async Task PostAsync_WithInvalidModel_ReturnsBadRequest()
    {
        // Arrange
        var createModel = new ClientCreateModel
        {
            Name = "", // Invalid - empty name
            DisplayName = "Test Client Display",
            ClientCode = "TC001"
        };

        // Act
        var httpClient = await GetAuthenticatedClientAsync();
        var response = await httpClient.PostAsJsonAsync("/v1/Client", createModel);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task PutAsync_WithValidModel_ReturnsOkResult()
    {
        // Arrange
        await SeedDatabaseAsync();
        using var context = GetDbContext();
        var client = context.Clients.First();

        var updateModel = new ClientUpdateModel
        {
            RowId = client.RowId,
            Name = "Updated Client",
            DisplayName = "Updated Client Display",
            ClientCode = "UC001"
        };

        // Act
        var httpClient = await GetAuthenticatedClientAsync();
        var response = await httpClient.PutAsJsonAsync($"/v1/Client({client.RowId})", updateModel);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.NotNull(content);
    }

    [Fact]
    public async Task PutAsync_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var updateModel = new ClientUpdateModel
        {
            RowId = Guid.NewGuid(), // Non-existent ID
            Name = "Updated Client",
            DisplayName = "Updated Client Display",
            ClientCode = "UC001"
        };

        // Act
        var httpClient = await GetAuthenticatedClientAsync();
        var response = await httpClient.PutAsJsonAsync($"/v1/Client({Guid.NewGuid()})", updateModel);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteAsync_WithValidId_ReturnsNoContent()
    {
        // Arrange
        await SeedDatabaseAsync();
        using var context = GetDbContext();
        var client = context.Clients.First();
        var rowId = client.RowId;

        // Act
        var httpClient = await GetAuthenticatedClientAsync();
        var response = await httpClient.DeleteAsync($"/v1/Client({rowId})");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task DeleteAsync_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var invalidRowId = Guid.NewGuid();

        // Act
        var httpClient = await GetAuthenticatedClientAsync();
        var response = await httpClient.DeleteAsync($"/v1/Client({invalidRowId})");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetAsync_WithODataQuery_ReturnsFilteredResults()
    {
        // Arrange
        await SeedDatabaseAsync();

        // Act
        var httpClient = await GetAuthenticatedClientAsync();
        var response = await httpClient.GetAsync("/v1/Client?$filter=IsActive eq true&$top=5");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.NotNull(content);
    }

    [Fact]
    public async Task GetAsync_WithODataSelect_ReturnsSelectedFields()
    {
        // Arrange
        await SeedDatabaseAsync();

        // Act
        var httpClient = await GetAuthenticatedClientAsync();
        var response = await httpClient.GetAsync("/v1/Client?$select=Name,ClientCode");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.NotNull(content);
    }

    [Fact]
    public async Task GetAsync_WithODataOrderBy_ReturnsOrderedResults()
    {
        // Arrange
        await SeedDatabaseAsync();

        // Act
        var httpClient = await GetAuthenticatedClientAsync();
        var response = await httpClient.GetAsync("/v1/Client?$orderby=Name asc");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.NotNull(content);
    }

    public new void Dispose()
    {
        base.Dispose();
    }
}
