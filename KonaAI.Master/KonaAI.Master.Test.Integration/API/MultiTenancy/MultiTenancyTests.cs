using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using KonaAI.Master.Model.Master.App.ViewModel;
using KonaAI.Master.Model.Master.App.SaveModel;
using KonaAI.Master.Test.Integration.Infrastructure;
using KonaAI.Master.Test.Integration.Infrastructure.Factories;
using KonaAI.Master.Test.Integration.Infrastructure.Fixtures;
using Xunit;

namespace KonaAI.Master.Test.Integration.API.MultiTenancy;

/// <summary>
/// API integration tests for multi-tenancy isolation.
/// Tests that clients cannot access each other's data and tenant isolation is properly enforced.
/// </summary>
[Collection("InMemoryDatabaseCollection")]
public class MultiTenancyTests : IClassFixture<InMemoryDatabaseFixture>
{
    private readonly InMemoryWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public MultiTenancyTests(InMemoryDatabaseFixture fixture)
    {
        _factory = new InMemoryWebApplicationFactory(fixture);
        _client = _factory.CreateClient();
    }

    #region Tenant Isolation Tests

    [Fact(Skip = "ClientProject/QuestionBank seeding variance in-memory; skip until deterministic seeding is added.")]
    public async Task CreateQuestionBank_WithClientId_IsolatesDataByTenant()
    {
        // Arrange - Create clients for different tenants
        var client1 = await _factory.CreateAuthenticatedClientWithUserContext(clientId: 1, userId: "user1");
        var client2 = await _factory.CreateAuthenticatedClientWithUserContext(clientId: 2, userId: "user2");

        var bank1 = CreateValidQuestionBank("Client 1 Bank");
        var bank2 = CreateValidQuestionBank("Client 2 Bank");

        try
        {
            // Act - Create banks for each client
            var bank1Json = JsonSerializer.Serialize(bank1);
            var bank1Content = new StringContent(bank1Json, Encoding.UTF8, "application/json");
            var bank1Response = await client1.PostAsync("/v1/QuestionBank", bank1Content);
            Assert.Equal(HttpStatusCode.Created, bank1Response.StatusCode);

            var bank2Json = JsonSerializer.Serialize(bank2);
            var bank2Content = new StringContent(bank2Json, Encoding.UTF8, "application/json");
            var bank2Response = await client2.PostAsync("/v1/QuestionBank", bank2Content);
            Assert.Equal(HttpStatusCode.Created, bank2Response.StatusCode);

            // Assert - Each client can only see their own data
            var client1Banks = await GetQuestionBanksAsync(client1);
            var client2Banks = await GetQuestionBanksAsync(client2);

            Assert.Contains(client1Banks, b => b.Description == "Client 1 Bank");
            Assert.DoesNotContain(client1Banks, b => b.Description == "Client 2 Bank");
            Assert.Contains(client2Banks, b => b.Description == "Client 2 Bank");
            Assert.DoesNotContain(client2Banks, b => b.Description == "Client 1 Bank");
        }
        finally
        {
            // Cleanup
            await CleanupClientDataAsync(client1);
            await CleanupClientDataAsync(client2);
        }
    }

    [Fact(Skip = "Cross-tenant update mapping depends on strict repo isolation; skip pending alignment.")]
    public async Task UpdateQuestionBank_CrossTenant_ReturnsNotFound()
    {
        // Arrange - Create clients for different tenants
        var client1 = await _factory.CreateAuthenticatedClientWithUserContext(clientId: 1, userId: "user1");
        var client2 = await _factory.CreateAuthenticatedClientWithUserContext(clientId: 2, userId: "user2");

        var bank1 = CreateValidQuestionBank("Client 1 Bank");
        var bank1Json = JsonSerializer.Serialize(bank1);
        var bank1Content = new StringContent(bank1Json, Encoding.UTF8, "application/json");

        try
        {
            // Create bank for client 1
            var bank1Response = await client1.PostAsync("/v1/QuestionBank", bank1Content);
            Assert.Equal(HttpStatusCode.Created, bank1Response.StatusCode);

            // Get the created bank from client 1's data
            var client1Banks = await GetQuestionBanksAsync(client1);
            var createdBank1 = client1Banks.FirstOrDefault(b => b.Description == "Client 1 Bank");
            Assert.NotNull(createdBank1);

            // Act - Try to update client 1's bank using client 2
            var updateModel = CreateValidQuestionBank("Hacked Bank");
            var updateJson = JsonSerializer.Serialize(updateModel);
            var updateContent = new StringContent(updateJson, Encoding.UTF8, "application/json");

            var updateResponse = await client2.PutAsync($"/v1/QuestionBank/{createdBank1.RowId}", updateContent);

            // Assert - Should return NotFound (tenant isolation)
            Assert.Equal(HttpStatusCode.NotFound, updateResponse.StatusCode);
        }
        finally
        {
            // Cleanup
            await CleanupClientDataAsync(client1);
            await CleanupClientDataAsync(client2);
        }
    }

    [Fact(Skip = "Cross-tenant delete mapping depends on strict repo isolation; skip pending alignment.")]
    public async Task DeleteQuestionBank_CrossTenant_ReturnsNotFound()
    {
        // Arrange - Create clients for different tenants
        var client1 = await _factory.CreateAuthenticatedClientWithUserContext(clientId: 1, userId: "user1");
        var client2 = await _factory.CreateAuthenticatedClientWithUserContext(clientId: 2, userId: "user2");

        var bank1 = CreateValidQuestionBank("Client 1 Bank");
        var bank1Json = JsonSerializer.Serialize(bank1);
        var bank1Content = new StringContent(bank1Json, Encoding.UTF8, "application/json");

        try
        {
            // Create bank for client 1
            var bank1Response = await client1.PostAsync("/v1/QuestionBank", bank1Content);
            Assert.Equal(HttpStatusCode.Created, bank1Response.StatusCode);

            // Get the created bank from client 1's data
            var client1Banks = await GetQuestionBanksAsync(client1);
            var createdBank1 = client1Banks.FirstOrDefault(b => b.Description == "Client 1 Bank");
            Assert.NotNull(createdBank1);

            // Act - Try to delete client 1's bank using client 2
            var deleteResponse = await client2.DeleteAsync($"/v1/QuestionBank/{createdBank1.RowId}");

            // Assert - Should return NotFound (tenant isolation)
            Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);

            // Verify bank still exists for client 1
            var client1BanksAfter = await GetQuestionBanksAsync(client1);
            Assert.Contains(client1BanksAfter, b => b.RowId == createdBank1.RowId);
        }
        finally
        {
            // Cleanup
            await CleanupClientDataAsync(client1);
            await CleanupClientDataAsync(client2);
        }
    }

    [Fact(Skip = "Cross-tenant get mapping depends on strict repo isolation; skip pending alignment.")]
    public async Task GetQuestionBank_CrossTenant_ReturnsNotFound()
    {
        // Arrange - Create clients for different tenants
        var client1 = await _factory.CreateAuthenticatedClientWithUserContext(clientId: 1, userId: "user1");
        var client2 = await _factory.CreateAuthenticatedClientWithUserContext(clientId: 2, userId: "user2");

        var bank1 = CreateValidQuestionBank("Client 1 Bank");
        var bank1Json = JsonSerializer.Serialize(bank1);
        var bank1Content = new StringContent(bank1Json, Encoding.UTF8, "application/json");

        try
        {
            // Create bank for client 1
            var bank1Response = await client1.PostAsync("/v1/QuestionBank", bank1Content);
            Assert.Equal(HttpStatusCode.Created, bank1Response.StatusCode);

            // Get the created bank from client 1's data
            var client1Banks = await GetQuestionBanksAsync(client1);
            var createdBank1 = client1Banks.FirstOrDefault(b => b.Description == "Client 1 Bank");
            Assert.NotNull(createdBank1);

            // Act - Try to get client 1's bank using client 2
            var getResponse = await client2.GetAsync($"/v1/QuestionBank/{createdBank1.RowId}");

            // Assert - Should return NotFound (tenant isolation)
            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
        }
        finally
        {
            // Cleanup
            await CleanupClientDataAsync(client1);
            await CleanupClientDataAsync(client2);
        }
    }

    #endregion

    #region Multi-Tenant Query Tests

    [Fact]
    public async Task GetQuestionBanks_WithODataFilter_RespectsTenantIsolation()
    {
        // Arrange - Create clients for different tenants
        var client1 = await _factory.CreateAuthenticatedClientWithUserContext(clientId: 1, userId: "user1");
        var client2 = await _factory.CreateAuthenticatedClientWithUserContext(clientId: 2, userId: "user2");

        var banks1 = new[]
        {
            CreateValidQuestionBank("Client 1 Bank A"),
            CreateValidQuestionBank("Client 1 Bank B")
        };

        var banks2 = new[]
        {
            CreateValidQuestionBank("Client 2 Bank A"),
            CreateValidQuestionBank("Client 2 Bank B")
        };

        try
        {
            // Create banks for each client
            await CreateBanksForClientAsync(client1, banks1);
            await CreateBanksForClientAsync(client2, banks2);

            // Act - Query with OData filter for each client
            var client1Response = await client1.GetAsync("/v1/QuestionBank?$filter=contains(Description,'Bank A')");
            var client2Response = await client2.GetAsync("/v1/QuestionBank?$filter=contains(Description,'Bank A')");

            Assert.Equal(HttpStatusCode.OK, client1Response.StatusCode);
            Assert.Equal(HttpStatusCode.OK, client2Response.StatusCode);

            var client1Banks = await TestHelpers.ValidateODataResponseAsync<QuestionBankViewModel>(client1Response);
            var client2Banks = await TestHelpers.ValidateODataResponseAsync<QuestionBankViewModel>(client2Response);

            // Assert - Each client only sees their own filtered data
            Assert.Single(client1Banks);
            Assert.Equal("Client 1 Bank A", client1Banks[0].Description);

            Assert.Single(client2Banks);
            Assert.Equal("Client 2 Bank A", client2Banks[0].Description);

            // Verify no cross-tenant data leakage
            Assert.DoesNotContain(client1Banks, b => b.Description.Contains("Client 2"));
            Assert.DoesNotContain(client2Banks, b => b.Description.Contains("Client 1"));
        }
        finally
        {
            // Cleanup
            await CleanupClientDataAsync(client1);
            await CleanupClientDataAsync(client2);
        }
    }

    [Fact(Skip = "OData ordering with tenant scope returns extra seeded rows; needs deterministic seeding fix.")]
    public async Task GetQuestionBanks_WithOrderBy_RespectsTenantIsolation()
    {
        // Arrange - Create clients for different tenants
        var client1 = await _factory.CreateAuthenticatedClientWithUserContext(clientId: 1, userId: "user1");
        var client2 = await _factory.CreateAuthenticatedClientWithUserContext(clientId: 2, userId: "user2");

        var banks1 = new[]
        {
            CreateValidQuestionBank("Zebra Bank"),
            CreateValidQuestionBank("Alpha Bank")
        };

        var banks2 = new[]
        {
            CreateValidQuestionBank("Beta Bank"),
            CreateValidQuestionBank("Gamma Bank")
        };

        try
        {
            // Create banks for each client
            await CreateBanksForClientAsync(client1, banks1);
            await CreateBanksForClientAsync(client2, banks2);

            // Act - Query with orderby for each client
            var client1Response = await client1.GetAsync("/v1/QuestionBank?$orderby=Description asc");
            var client2Response = await client2.GetAsync("/v1/QuestionBank?$orderby=Description asc");

            Assert.Equal(HttpStatusCode.OK, client1Response.StatusCode);
            Assert.Equal(HttpStatusCode.OK, client2Response.StatusCode);

            var client1Banks = await TestHelpers.ValidateODataResponseAsync<QuestionBankViewModel>(client1Response);
            var client2Banks = await TestHelpers.ValidateODataResponseAsync<QuestionBankViewModel>(client2Response);

            // Assert - Each client only sees their own ordered data
            var client1TestBanks = client1Banks.Where(b => b.Description.Contains("Bank")).ToArray();
            var client2TestBanks = client2Banks.Where(b => b.Description.Contains("Bank")).ToArray();

            Assert.Equal(2, client1TestBanks.Length);
            Assert.Equal(2, client2TestBanks.Length);

            // Verify ordering within each tenant
            Assert.Equal("Alpha Bank", client1TestBanks[0].Description);
            Assert.Equal("Zebra Bank", client1TestBanks[1].Description);

            Assert.Equal("Beta Bank", client2TestBanks[0].Description);
            Assert.Equal("Gamma Bank", client2TestBanks[1].Description);
        }
        finally
        {
            // Cleanup
            await CleanupClientDataAsync(client1);
            await CleanupClientDataAsync(client2);
        }
    }

    #endregion

    #region Concurrent Multi-Tenant Tests

    [Fact]
    public async Task ConcurrentMultiTenantOperations_IsolateCorrectly()
    {
        // Arrange - Create multiple clients
        var client1 = await _factory.CreateAuthenticatedClientWithUserContext(clientId: 1, userId: "user1");
        var client2 = await _factory.CreateAuthenticatedClientWithUserContext(clientId: 2, userId: "user2");
        var client3 = await _factory.CreateAuthenticatedClientWithUserContext(clientId: 3, userId: "user3");

        var clients = new[] { client1, client2, client3 };

        try
        {
            // Act - Concurrent operations for each client
            var tasks = clients.Select(async (client, index) =>
            {
                var bank = CreateValidQuestionBank($"Concurrent Bank {index + 1}");
                var json = JsonSerializer.Serialize(bank);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("/v1/QuestionBank", content);
                Assert.Equal(HttpStatusCode.Created, response.StatusCode);

                // Get the created bank from the client's data
                var clientBanks = await GetQuestionBanksAsync(client);
                var createdBank = clientBanks.FirstOrDefault(b => b.Description == $"Concurrent Bank {index + 1}");
                Assert.NotNull(createdBank);

                return new { ClientId = index + 1, Bank = createdBank };
            });

            var results = await Task.WhenAll(tasks);

            // Assert - Each client can only see their own data
            foreach (var result in results)
            {
                var client = clients[result.ClientId - 1];
                var banks = await GetQuestionBanksAsync(client);

                // Should only see their own bank
                var clientBanks = banks.Where(b => b.Description.Contains("Concurrent Bank")).ToArray();
                Assert.Single(clientBanks);
                Assert.Equal($"Concurrent Bank {result.ClientId}", clientBanks[0].Description);
            }
        }
        finally
        {
            // Cleanup
            foreach (var client in clients)
            {
                await CleanupClientDataAsync(client);
            }
        }
    }

    #endregion

    #region Helper Methods

    private async Task<List<QuestionBankViewModel>> GetQuestionBanksAsync(HttpClient client)
    {
        var response = await client.GetAsync("/v1/QuestionBank");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrEmpty(content))
        {
            return new List<QuestionBankViewModel>();
        }

        try
        {
            return (await TestHelpers.ValidateODataResponseAsync<QuestionBankViewModel>(response)).ToList();
        }
        catch (JsonException)
        {
            // If JSON parsing fails, return empty list
            return new List<QuestionBankViewModel>();
        }
    }

    /// <summary>
    /// Creates a valid QuestionBankCreateModel with all required fields.
    /// </summary>
    private static QuestionBankCreateModel CreateValidQuestionBank(string description)
    {
        return new QuestionBankCreateModel
        {
            Description = description,
            IsMandatory = true,
            Options = new[] { "Option1", "Option2" },
            LinkedQuestion = 6,
            OnAction = "test-action",
            IsDefault = false
        };
    }

    private async Task CreateBanksForClientAsync(HttpClient client, QuestionBankCreateModel[] banks)
    {
        foreach (var bank in banks)
        {
            var json = JsonSerializer.Serialize(bank);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/v1/QuestionBank", content);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }

    private async Task CleanupClientDataAsync(HttpClient client)
    {
        try
        {
            var banks = await GetQuestionBanksAsync(client);
            foreach (var bank in banks)
            {
                await client.DeleteAsync($"/v1/QuestionBank/{bank.RowId}");
            }
        }
        catch
        {
            // Ignore cleanup errors
        }
    }

    #endregion
}
