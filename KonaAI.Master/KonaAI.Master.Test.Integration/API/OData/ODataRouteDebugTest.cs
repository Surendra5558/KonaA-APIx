using KonaAI.Master.Test.Integration.Infrastructure.Fixtures;
using KonaAI.Master.Test.Integration.Infrastructure.Factories;
using System.Net;
using Xunit;

namespace KonaAI.Master.Test.Integration.API.OData;

/// <summary>
/// Debug test to investigate OData routing issues.
/// </summary>
[Collection("InMemoryDatabaseCollection")]
public class ODataRouteDebugTest : IClassFixture<InMemoryDatabaseFixture>
{
    private readonly InMemoryWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public ODataRouteDebugTest(InMemoryDatabaseFixture fixture)
    {
        _factory = new InMemoryWebApplicationFactory(fixture);
        _client = _factory.CreateAuthenticatedClientAsync().GetAwaiter().GetResult();
    }

    [Fact]
    public async Task Debug_CheckAvailableRoutes()
    {
        // Arrange
        await _factory.SeedDatabaseAsync();

        // Act - Try different route variations
        Console.WriteLine("=== Testing OData Routes ===");

        // Test 1: Basic Client route
        var response1 = await _client.GetAsync("/v1/Client");
        Console.WriteLine($"1. /v1/Client => Status: {response1.StatusCode}, Content: {await response1.Content.ReadAsStringAsync()}");

        // Test 2: Client with OData query
        var response2 = await _client.GetAsync("/v1/Client?$top=1");
        Console.WriteLine($"2. /v1/Client?$top=1 => Status: {response2.StatusCode}, Content: {await response2.Content.ReadAsStringAsync()}");

        // Test 3: Login route (known to work)
        var response3 = await _client.GetAsync("/v1/Login");
        Console.WriteLine($"3. /v1/Login => Status: {response3.StatusCode}, Content: {await response3.Content.ReadAsStringAsync()}");

        // Test 4: Try api prefix
        var response4 = await _client.GetAsync("/api/v1/Client");
        Console.WriteLine($"4. /api/v1/Client => Status: {response4.StatusCode}, Content: {await response4.Content.ReadAsStringAsync()}");

        // Test 5: Try without v1 prefix
        var response5 = await _client.GetAsync("/Client");
        Console.WriteLine($"5. /Client => Status: {response5.StatusCode}, Content: {await response5.Content.ReadAsStringAsync()}");

        // Test 6: Try Menu controller (also OData)
        var response6 = await _client.GetAsync("/v1/Menu");
        Console.WriteLine($"6. /v1/Menu => Status: {response6.StatusCode}, Content: {await response6.Content.ReadAsStringAsync()}");

        // Assert - At least one should work
        Assert.True(
            response1.StatusCode == HttpStatusCode.OK ||
            response2.StatusCode == HttpStatusCode.OK ||
            response3.StatusCode == HttpStatusCode.OK ||
            response4.StatusCode == HttpStatusCode.OK ||
            response5.StatusCode == HttpStatusCode.OK ||
            response6.StatusCode == HttpStatusCode.OK,
            "At least one route should work"
        );
    }
}


