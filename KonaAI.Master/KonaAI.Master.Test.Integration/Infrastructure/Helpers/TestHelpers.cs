using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using KonaAI.Master.Repository;
using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Common.Model;
using KonaAI.Master.Repository.Common.Constants;
using KonaAI.Master.Repository.Common.Domain;
using KonaAI.Master.Test.Integration.Infrastructure;

namespace KonaAI.Master.Test.Integration.Infrastructure.Helpers;

/// <summary>
/// Helper methods for integration testing.
/// </summary>
public static class TestHelpers
{
    /// <summary>
    /// Creates JSON content for HTTP requests.
    /// </summary>
    public static StringContent CreateJsonContent<T>(T obj)
    {
        var json = JsonSerializer.Serialize(obj, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        });

        return new StringContent(json, Encoding.UTF8, "application/json");
    }

    /// <summary>
    /// Deserializes JSON response content.
    /// </summary>
    public static async Task<T?> DeserializeResponse<T>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
    }

    /// <summary>
    /// Creates an authenticated HTTP client with JWT token.
    /// </summary>
    public static async Task<HttpClient> CreateAuthenticatedClientAsync(this WebApplicationFactory<Program> factory)
    {
        var client = factory.CreateClient();

        // Add authentication headers
        client.DefaultRequestHeaders.Add("Authorization", "Bearer test-token");
        client.DefaultRequestHeaders.Add("X-Client-Id", "1");
        client.DefaultRequestHeaders.Add("X-User-Id", "testuser");

        return client;
    }

    /// <summary>
    /// Creates an HTTP client with specific user context.
    /// </summary>
    public static HttpClient CreateClientWithUserContext(this WebApplicationFactory<Program> factory,
        long clientId = 1, string userId = "testuser", string role = "User")
    {
        var client = factory.CreateClient();

        client.DefaultRequestHeaders.Add("X-Client-Id", clientId.ToString());
        client.DefaultRequestHeaders.Add("X-User-Id", userId);
        client.DefaultRequestHeaders.Add("X-User-Role", role);
        client.DefaultRequestHeaders.Add("X-Forwarded-For", "127.0.0.1");

        return client;
    }

    /// <summary>
    /// Waits for a condition to be true with timeout.
    /// </summary>
    public static async Task WaitForConditionAsync(Func<Task<bool>> condition,
        TimeSpan timeout = default, TimeSpan interval = default)
    {
        if (timeout == default) timeout = TimeSpan.FromSeconds(30);
        if (interval == default) interval = TimeSpan.FromMilliseconds(100);

        var startTime = DateTime.UtcNow;

        while (DateTime.UtcNow - startTime < timeout)
        {
            if (await condition())
                return;

            await Task.Delay(interval);
        }

        throw new TimeoutException($"Condition not met within {timeout.TotalSeconds} seconds");
    }

    /// <summary>
    /// Creates a test database context with specific client ID.
    /// </summary>
    public static DefaultContext CreateTestContext(IServiceProvider serviceProvider, long clientId = 1)
    {
        var options = serviceProvider.GetRequiredService<DbContextOptions<DefaultContext>>();
        var userContextService = new TestUserContextService();
        return new DefaultContext(options, userContextService);
    }

    /// <summary>
    /// Seeds test data for a specific client.
    /// </summary>
    public static async Task SeedClientDataAsync(DefaultContext context, long clientId, int dataCount = 10)
    {
        // Implementation for seeding client-specific data
        await Task.CompletedTask;
    }

    /// <summary>
    /// Clears all data from the test database.
    /// </summary>
    public static async Task ClearTestDatabaseAsync(DefaultContext context)
    {
        // Disable foreign key constraints temporarily
        await context.Database.ExecuteSqlRawAsync("EXEC sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT all'");

        // Get all table names and delete data
        var tableNames = await context.Database.SqlQueryRaw<string>(
            "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'")
            .ToListAsync();

        foreach (var tableName in tableNames)
        {
            // Table names cannot be parameterized; ensure they come from INFORMATION_SCHEMA and suppress EF1002
#pragma warning disable EF1002
            await context.Database.ExecuteSqlRawAsync($"DELETE FROM [{tableName}]");
#pragma warning restore EF1002
        }

        // Re-enable foreign key constraints
        await context.Database.ExecuteSqlRawAsync("EXEC sp_MSforeachtable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT all'");
    }

    /// <summary>
    /// Measures execution time of an async operation.
    /// </summary>
    public static async Task<(T Result, TimeSpan Duration)> MeasureExecutionTimeAsync<T>(Func<Task<T>> operation)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var result = await operation();
        stopwatch.Stop();

        return (result, stopwatch.Elapsed);
    }

    /// <summary>
    /// Creates a test JWT token for authentication.
    /// </summary>
    public static string CreateTestJwtToken(string userId = "testuser", long clientId = 1, string role = "User")
    {
        // In a real implementation, this would create a proper JWT token
        // For testing purposes, we'll return a simple test token
        return $"test-token-{userId}-{clientId}-{role}";
    }

    /// <summary>
    /// Validates that a response has the expected status code.
    /// </summary>
    public static void AssertStatusCode(HttpResponseMessage response, System.Net.HttpStatusCode expectedStatusCode)
    {
        if (response.StatusCode != expectedStatusCode)
        {
            var content = response.Content.ReadAsStringAsync().Result;
            throw new InvalidOperationException(
                $"Expected status code {expectedStatusCode}, but got {response.StatusCode}. Content: {content}");
        }
    }

    /// <summary>
    /// Validates that a response contains expected content.
    /// </summary>
    public static async Task AssertResponseContainsAsync(HttpResponseMessage response, string expectedContent)
    {
        var content = await response.Content.ReadAsStringAsync();
        if (!content.Contains(expectedContent))
        {
            throw new InvalidOperationException(
                $"Expected response to contain '{expectedContent}', but it didn't. Actual content: {content}");
        }
    }

    /// <summary>
    /// Creates a test user context for multi-tenancy testing.
    /// </summary>
    public static TestUserContextService CreateTestUserContext(long clientId = 1, string userId = "testuser")
    {
        return new TestUserContextService();
    }
}

/// <summary>
/// Test implementation of IUserContextService for testing.
/// </summary>
public class TestUserContextService : IUserContextService
{
    public UserContext? UserContext { get; set; } = new()
    {
        ClientId = 1,
        UserRowId = Guid.NewGuid(),
        UserLoginId = 1,
        UserLoginName = "Test User",
        UserLoginEmail = "testuser@konaai.com",
        RoleId = 1,
        RoleName = "Admin"
    };

    public void SetDomainDefaults<T>(T domain, DataModes dataModes) where T : BaseDomain
    {
        if (UserContext == null) return;

        var now = DateTime.UtcNow;

        switch (dataModes)
        {
            case DataModes.Add:
                domain.CreatedOn = now;
                domain.CreatedBy = UserContext.UserLoginName;
                if (domain is BaseClientDomain clientDomain)
                {
                    clientDomain.ClientId = UserContext.ClientId;
                }
                break;
            case DataModes.Edit:
                domain.ModifiedOn = now;
                domain.ModifiedBy = UserContext.UserLoginName;
                break;
            case DataModes.Delete:
                domain.IsDeleted = true;
                domain.ModifiedOn = now;
                domain.ModifiedBy = UserContext.UserLoginName;
                break;
            case DataModes.DeActive:
                domain.IsActive = false;
                domain.ModifiedOn = now;
                domain.ModifiedBy = UserContext.UserLoginName;
                break;
        }
    }

    public void SetDomainDefaults<T>(List<T> domains, DataModes dataModes) where T : BaseDomain
    {
        foreach (var domain in domains)
        {
            SetDomainDefaults(domain, dataModes);
        }
    }
}

/// <summary>
/// Test authentication handler for integration testing.
/// </summary>
// Duplicate TestAuthHandler removed; use Infrastructure/TestAuthHandler.cs for a single authoritative handler.
