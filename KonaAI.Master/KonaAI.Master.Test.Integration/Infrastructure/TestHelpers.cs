using System.Net.Http;
using System.Text;
using System.Text.Json;
using KonaAI.Master.Model.Authentication;

namespace KonaAI.Master.Test.Integration.Infrastructure;

/// <summary>
/// Test helper methods for common testing scenarios.
/// </summary>
public static class TestHelpers
{
    /// <summary>
    /// Gets an authentication token for testing.
    /// </summary>
    public static async Task<string> GetAuthTokenAsync(HttpClient client)
    {
        var loginRequest = new TokenFormRequest
        {
            UserName = "testuser@konaai.com",
            Password = "Test@123456",
            GrantType = "password"
        };

        var json = JsonSerializer.Serialize(loginRequest);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PostAsync("/v1/Login", content);

        if (!response.IsSuccessStatusCode)
        {
            // If login fails, return a mock token for testing
            return "mock-test-token-12345";
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return tokenResponse?.Token ?? "mock-test-token-12345";
    }

    /// <summary>
    /// Creates a test user context for multi-tenant testing.
    /// </summary>
    public static HttpClient CreateClientWithUserContext(HttpClient client, long clientId = 1, string userId = "testuser")
    {
        client.DefaultRequestHeaders.Add("X-Client-Id", clientId.ToString());
        client.DefaultRequestHeaders.Add("X-User-Id", userId);
        client.DefaultRequestHeaders.Add("X-Forwarded-For", "127.0.0.1");

        return client;
    }

    /// <summary>
    /// Creates test data for OData query testing.
    /// </summary>
    public static List<T> CreateTestData<T>(int count, Func<int, T> factory) where T : class
    {
        var data = new List<T>();
        for (int i = 1; i <= count; i++)
        {
            data.Add(factory(i));
        }
        return data;
    }

    /// <summary>
    /// Validates OData query response structure.
    /// </summary>
    public static async Task<T[]> ValidateODataResponseAsync<T>(HttpResponseMessage response)
    {
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(content)) return Array.Empty<T>();

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        try
        {
            // Try OData envelope: { "value": [...] }
            using var doc = JsonDocument.Parse(content);
            if (doc.RootElement.ValueKind == JsonValueKind.Object && doc.RootElement.TryGetProperty("value", out var valueEl) && valueEl.ValueKind == JsonValueKind.Array)
            {
                var valueJson = valueEl.GetRawText();
                var arr = JsonSerializer.Deserialize<T[]>(valueJson, options);
                return arr ?? Array.Empty<T>();
            }
        }
        catch
        {
            // fall through to plain array parsing
        }

        // Fallback: plain array
        var result = JsonSerializer.Deserialize<T[]>(content, options);
        return result ?? Array.Empty<T>();
    }

    /// <summary>
    /// Creates a test model with default values.
    /// </summary>
    public static T CreateTestModel<T>() where T : class, new()
    {
        return new T();
    }

    /// <summary>
    /// Generates a test GUID for consistent testing.
    /// </summary>
    public static Guid GenerateTestGuid(int seed = 1)
    {
        var bytes = new byte[16];
        for (int i = 0; i < 16; i++)
        {
            bytes[i] = (byte)(seed + i);
        }
        return new Guid(bytes);
    }

    /// <summary>
    /// Creates test data for pagination testing.
    /// </summary>
    public static async Task<(List<T> Data, int TotalCount)> CreatePaginationTestDataAsync<T>(
        Func<int, T> factory,
        int totalCount = 100,
        int pageSize = 10)
    {
        var data = new List<T>();
        for (int i = 1; i <= totalCount; i++)
        {
            data.Add(factory(i));
        }

        return (data, totalCount);
    }

    /// <summary>
    /// Validates HTTP response status and content.
    /// </summary>
    public static async Task<T> ValidateResponseAsync<T>(HttpResponseMessage response, int expectedStatusCode = 200)
    {
        Assert.Equal(expectedStatusCode, (int)response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return result ?? throw new InvalidOperationException("Failed to deserialize response");
    }

    /// <summary>
    /// Creates test data for multi-tenant isolation testing.
    /// </summary>
    public static async Task<Dictionary<long, List<T>>> CreateMultiTenantTestDataAsync<T>(
        Func<long, int, T> factory,
        params (long ClientId, int Count)[] tenants)
    {
        var result = new Dictionary<long, List<T>>();

        foreach (var (clientId, count) in tenants)
        {
            var data = new List<T>();
            for (int i = 1; i <= count; i++)
            {
                data.Add(factory(clientId, i));
            }
            result[clientId] = data;
        }

        return result;
    }

    /// <summary>
    /// Measures test execution time for performance testing.
    /// </summary>
    public static async Task<TimeSpan> MeasureExecutionTimeAsync(Func<Task> action)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        await action();
        stopwatch.Stop();
        return stopwatch.Elapsed;
    }

    /// <summary>
    /// Creates test data for concurrent testing scenarios.
    /// </summary>
    public static async Task<List<Task<T>>> CreateConcurrentTasksAsync<T>(
        Func<Task<T>> factory,
        int taskCount = 10)
    {
        var tasks = new List<Task<T>>();
        for (int i = 0; i < taskCount; i++)
        {
            tasks.Add(factory());
        }
        return tasks;
    }
}
