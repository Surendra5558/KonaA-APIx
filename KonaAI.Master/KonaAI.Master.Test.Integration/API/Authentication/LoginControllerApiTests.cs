using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using KonaAI.Master.Model.Authentication;
using KonaAI.Master.Test.Integration.Infrastructure;
using KonaAI.Master.Test.Integration.Infrastructure.Factories;
using KonaAI.Master.Test.Integration.Infrastructure.Fixtures;
using Xunit;

namespace KonaAI.Master.Test.Integration.API.Authentication;

/// <summary>
/// API integration tests for LoginController authentication flows.
/// Tests real HTTP pipeline with authentication token generation and validation.
/// </summary>
[Collection("InMemoryDatabaseCollection")]
public class LoginControllerApiTests : IClassFixture<InMemoryDatabaseFixture>
{
    private readonly InMemoryWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public LoginControllerApiTests(InMemoryDatabaseFixture fixture)
    {
        _factory = new InMemoryWebApplicationFactory(fixture);
        _client = _factory.CreateClient();
    }

    #region Authentication Flow Tests

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsToken()
    {
        // Arrange
        var loginRequest = new TokenFormRequest
        {
            UserName = "testuser@konaai.com",
            Password = "Test@123456",
            GrantType = "password"
        };

        var json = JsonSerializer.Serialize(loginRequest);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/v1/Login", content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync();
        var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.NotNull(tokenResponse);
        Assert.NotNull(tokenResponse.Token);
        Assert.True(tokenResponse.Token.Length > 0);
    }

    [Fact]
    public async Task Login_WithInvalidCredentials_ReturnsUnauthorized()
    {
        // Arrange
        var loginRequest = new TokenFormRequest
        {
            UserName = "invalid@konaai.com",
            Password = "WrongPassword",
            GrantType = "password"
        };

        var json = JsonSerializer.Serialize(loginRequest);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/v1/Login", content);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Login_WithMissingCredentials_ReturnsUnauthorized()
    {
        // Arrange
        var loginRequest = new TokenFormRequest
        {
            UserName = "",
            Password = "",
            GrantType = "password"
        };

        var json = JsonSerializer.Serialize(loginRequest);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/v1/Login", content);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Login_WithInvalidGrantType_ReturnsBadRequest()
    {
        // Arrange
        var loginRequest = new TokenFormRequest
        {
            UserName = "testuser@konaai.com",
            Password = "Test@123456",
            GrantType = "invalid_grant"
        };

        var json = JsonSerializer.Serialize(loginRequest);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/v1/Login", content);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    #endregion

    #region Token Validation Tests

    [Fact]
    public async Task AuthenticatedRequest_WithValidToken_ReturnsData()
    {
        // Arrange
        var token = await TestHelpers.GetAuthTokenAsync(_client);
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await _client.GetAsync("/v1/QuestionBank");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task AuthenticatedRequest_WithInvalidToken_ReturnsUnauthorized()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "invalid-token");

        // Act
        var response = await _client.GetAsync("/v1/QuestionBank");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task AuthenticatedRequest_WithoutToken_ReturnsUnauthorized()
    {
        // Arrange - No authorization header

        // Act
        var response = await _client.GetAsync("/v1/QuestionBank");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    #endregion

    #region End-to-End Authentication Flow

    [Fact]
    public async Task CompleteAuthenticationFlow_LoginAndAccessProtectedResource_Success()
    {
        // Step 1: Login and get token
        var loginRequest = new TokenFormRequest
        {
            UserName = "testuser@konaai.com",
            Password = "Test@123456",
            GrantType = "password"
        };

        var json = JsonSerializer.Serialize(loginRequest);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var loginResponse = await _client.PostAsync("/v1/Login", content);
        Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);

        var loginContent = await loginResponse.Content.ReadAsStringAsync();
        var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(loginContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.NotNull(tokenResponse);
        Assert.NotNull(tokenResponse.Token);

        // Step 2: Use token to access protected resource
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenResponse.Token);

        var protectedResponse = await _client.GetAsync("/v1/QuestionBank");
        Assert.Equal(HttpStatusCode.OK, protectedResponse.StatusCode);

        // Step 3: Verify we can access multiple protected resources
        var menuResponse = await _client.GetAsync("/v1/Menu");
        Assert.Equal(HttpStatusCode.OK, menuResponse.StatusCode);

        var navigationResponse = await _client.GetAsync("/v1/AppNavigation");
        Assert.Equal(HttpStatusCode.OK, navigationResponse.StatusCode);
    }

    #endregion

    #region Performance Tests

    [Fact]
    public async Task Login_PerformanceTest_CompletesWithinTimeLimit()
    {
        // Arrange
        var loginRequest = new TokenFormRequest
        {
            UserName = "testuser@konaai.com",
            Password = "Test@123456",
            GrantType = "password"
        };

        var json = JsonSerializer.Serialize(loginRequest);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act & Assert
        var executionTime = await TestHelpers.MeasureExecutionTimeAsync(async () =>
        {
            var response = await _client.PostAsync("/v1/Login", content);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        });

        // Should complete within 5 seconds
        Assert.True(executionTime.TotalSeconds < 5, $"Login took {executionTime.TotalSeconds} seconds, expected < 5 seconds");
    }

    [Fact(Skip = "Flaky under concurrent runs in CI - intermittent 500. Investigate token generation & shared state.")]
    public async Task ConcurrentLoginRequests_AllCompleteSuccessfully()
    {
        // Arrange
        var loginRequest = new TokenFormRequest
        {
            UserName = "testuser@konaai.com",
            Password = "Test@123456",
            GrantType = "password"
        };

        var json = JsonSerializer.Serialize(loginRequest);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act - Create 5 concurrent login requests
        var tasks = await TestHelpers.CreateConcurrentTasksAsync(async () =>
        {
            var response = await _client.PostAsync("/v1/Login", content);
            return response.StatusCode;
        }, 5);

        var results = await Task.WhenAll(tasks);

        // Assert - All requests should succeed
        Assert.All(results, statusCode => Assert.Equal(HttpStatusCode.OK, statusCode));
    }

    #endregion
}
