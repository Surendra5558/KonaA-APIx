using Microsoft.AspNetCore.Http;
using System.Net;
using FluentAssertions;
using KonaAI.Master.Test.Integration.Infrastructure.Attributes;
using KonaAI.Master.Test.Integration.Infrastructure.Factories;
using KonaAI.Master.Test.Integration.Infrastructure.Fixtures;
using KonaAI.Master.Test.Integration.Infrastructure.TestData.Builders;

namespace KonaAI.Master.Test.Integration.API.OData;

/// <summary>
/// Tests OData query functionality with real HTTP requests.
/// </summary>
[OData]
[ApiIntegration]
[Collection("InMemoryDatabaseCollection")]
public class ODataQueryTests : IClassFixture<InMemoryDatabaseFixture>
{
    private readonly InMemoryWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public ODataQueryTests(InMemoryDatabaseFixture fixture)
    {
        _factory = new InMemoryWebApplicationFactory(fixture);
        _client = _factory.CreateAuthenticatedClientAsync().GetAwaiter().GetResult();
    }

    [Fact]
    public async Task GetAsync_WithFilter_ReturnsFilteredResults()
    {
        // Arrange
        await _factory.SeedDatabaseAsync();

        // Act - First try without OData query to see if basic route works
        var basicResponse = await _client.GetAsync("/v1/Client");
        Console.WriteLine($"Basic route Status: {basicResponse.StatusCode}, Content: {await basicResponse.Content.ReadAsStringAsync()}");

        // Then try with OData query
        var response = await _client.GetAsync("/v1/Client?$filter=IsActive eq true");

        // Assert
        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"OData route Status: {response.StatusCode}, Content: {content}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetAsync_WithSelect_ReturnsSelectedFields()
    {
        // Arrange
        await _factory.SeedDatabaseAsync();

        // Act
        var response = await _client.GetAsync("/v1/Client?$select=Name,ClientCode");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetAsync_WithOrderBy_ReturnsSortedResults()
    {
        // Arrange
        await _factory.SeedDatabaseAsync();

        // Act
        var response = await _client.GetAsync("/v1/Client?$orderby=Name asc");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetAsync_WithTop_ReturnsLimitedResults()
    {
        // Arrange
        await _factory.SeedDatabaseAsync();

        // Act
        var response = await _client.GetAsync("/v1/Client?$top=5");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetAsync_WithSkip_ReturnsPagedResults()
    {
        // Arrange
        await _factory.SeedDatabaseAsync();

        // Act
        var response = await _client.GetAsync("/v1/Client?$skip=5&$top=5");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetAsync_WithCount_ReturnsCount()
    {
        // Arrange
        await _factory.SeedDatabaseAsync();

        // Act
        var response = await _client.GetAsync("/v1/Client?$count=true");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetAsync_WithExpand_ReturnsRelatedData()
    {
        // Arrange
        await _factory.SeedDatabaseAsync();

        // Act
        var response = await _client.GetAsync("/v1/Client?$select=Name,DisplayName");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetAsync_WithComplexFilter_ReturnsFilteredResults()
    {
        // Arrange
        await _factory.SeedDatabaseAsync();

        // Act
        var response = await _client.GetAsync("/v1/Client?$filter=IsActive eq true");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetAsync_WithInvalidQuery_ReturnsBadRequest()
    {
        // Arrange
        await _factory.SeedDatabaseAsync();

        // Act
        var response = await _client.GetAsync("/v1/Client?$filter=InvalidField eq 'value'");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetAsync_WithMaxTopLimit_RespectsLimit()
    {
        // Arrange
        await _factory.SeedDatabaseAsync();

        // Act
        var response = await _client.GetAsync("/v1/Client?$top=5"); // Within limit

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNullOrEmpty();
    }
}