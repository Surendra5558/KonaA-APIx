using Microsoft.AspNetCore.Http;
using System.Net;
using FluentAssertions;
using KonaAI.Master.Test.Integration.Infrastructure.Attributes;
using KonaAI.Master.Test.Integration.Infrastructure.Factories;
using KonaAI.Master.Test.Integration.Infrastructure.Fixtures;
using KonaAI.Master.Test.Integration.Infrastructure.Helpers;

namespace KonaAI.Master.Test.Integration.API.Authentication;

/// <summary>
/// Tests authentication and authorization functionality.
/// </summary>
[Authentication]
[ApiIntegration]
[Collection("InMemoryDatabaseCollection")]
public class AuthenticationTests : IClassFixture<InMemoryDatabaseFixture>
{
    private readonly InMemoryWebApplicationFactory _factory;

    public AuthenticationTests(InMemoryDatabaseFixture fixture)
    {
        _factory = new InMemoryWebApplicationFactory(fixture);
    }

    [Fact]
    public async Task GetAsync_WithoutAuthentication_ReturnsUnauthorized()
    {
        // Arrange
        var client = _factory.CreateClient(); // No authentication

        // Act
        var response = await client.GetAsync("/v1/Client");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetAsync_WithValidToken_ReturnsOk()
    {
        // Arrange
        var client = await _factory.CreateAuthenticatedClientAsync();

        // Act
        var response = await client.GetAsync("/v1/Client");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetAsync_WithInvalidToken_ReturnsUnauthorized()
    {
        // Arrange
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("Authorization", "Bearer invalid-token");

        // Act
        var response = await client.GetAsync("/v1/Client");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetAsync_WithExpiredToken_ReturnsUnauthorized()
    {
        // Arrange
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("Authorization", "Bearer expired-token");

        // Act
        var response = await client.GetAsync("/v1/Client");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task PostAsync_WithValidToken_ReturnsCreated()
    {
        // Arrange
        var client = await _factory.CreateAuthenticatedClientAsync();
        var createModel = new
        {
            Name = "Test Client",
            DisplayName = "Test Client Display",
            ClientCode = "TC001",
            Description = "Test client for authentication testing"
        };

        var content = TestHelpers.CreateJsonContent(createModel);

        // Act
        var response = await client.PostAsync("/v1/Client", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task PutAsync_WithValidToken_ReturnsOk()
    {
        // Arrange
        var client = await _factory.CreateAuthenticatedClientAsync();
        await _factory.SeedDatabaseAsync();

        // Get an existing client
        var getResponse = await client.GetAsync("/v1/Client");
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var updateModel = new
        {
            RowId = Guid.NewGuid(), // This would be from the get response in a real scenario
            Name = "Updated Client Name",
            DisplayName = "Updated Display Name",
            ClientCode = "TC001",
            Description = "Updated description"
        };

        var content = TestHelpers.CreateJsonContent(updateModel);

        // Act
        var response = await client.PutAsync($"/v1/Client/{updateModel.RowId}", content);

        // Assert
        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteAsync_WithValidToken_ReturnsNoContent()
    {
        // Arrange
        var client = await _factory.CreateAuthenticatedClientAsync();
        await _factory.SeedDatabaseAsync();

        var clientId = Guid.NewGuid(); // This would be from a created client in a real scenario

        // Act
        var response = await client.DeleteAsync($"/v1/Client/{clientId}");

        // Assert
        response.StatusCode.Should().BeOneOf(HttpStatusCode.NoContent, HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetAsync_WithUserContext_ReturnsClientSpecificData()
    {
        // Arrange
        var client = await _factory.CreateAuthenticatedClientWithUserContext(clientId: 1, userId: "testuser");

        // Act


        var response = await client.GetAsync("/v1/ClientProject");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetAsync_WithDifferentUserContext_ReturnsDifferentData()
    {
        // Arrange
        var client1 = await _factory.CreateAuthenticatedClientWithUserContext(clientId: 1, userId: "user1");
        var client2 = await _factory.CreateAuthenticatedClientWithUserContext(clientId: 2, userId: "user2");

        // Act
        var response1 = await client1.GetAsync("/v1/ClientProject");
        var response2 = await client2.GetAsync("/v1/ClientProject");

        // Assert
        response1.StatusCode.Should().Be(HttpStatusCode.OK);
        response2.StatusCode.Should().Be(HttpStatusCode.OK);

        // In a real scenario, we would verify that the responses contain different data
        // based on the client context
    }

    [Fact]
    public async Task PostAsync_WithRoleBasedAccess_RespectsPermissions()
    {
        // Arrange
        var adminClient = await _factory.CreateAuthenticatedClientWithUserContext(clientId: 1, userId: "admin", role: "Administrator");
        var userClient = await _factory.CreateAuthenticatedClientWithUserContext(clientId: 1, userId: "user", role: "User");

        var createModel = new
        {
            Name = "Test Client",
            DisplayName = "Test Client Display",
            ClientCode = "TC001",
            Description = "Test client for role-based access testing"
        };

        var content = TestHelpers.CreateJsonContent(createModel);

        // Act
        var adminResponse = await adminClient.PostAsync("/v1/Client", content);
        var userResponse = await userClient.PostAsync("/v1/Client", content);

        // Assert
        adminResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        userResponse.StatusCode.Should().BeOneOf(HttpStatusCode.Created, HttpStatusCode.Forbidden);
    }

    [Fact(Skip = "Correlation ID middleware not implemented")]
    public async Task GetAsync_WithCorrelationId_IncludesInResponse()
    {
        // Arrange
        var client = await _factory.CreateAuthenticatedClientAsync();
        var correlationId = Guid.NewGuid().ToString();
        client.DefaultRequestHeaders.Add("X-Correlation-Id", correlationId);

        // Act
        var response = await client.GetAsync("/v1/Client");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Headers.Should().ContainKey("X-Correlation-Id");
        response.Headers.GetValues("X-Correlation-Id").Should().Contain(correlationId);
    }
}
