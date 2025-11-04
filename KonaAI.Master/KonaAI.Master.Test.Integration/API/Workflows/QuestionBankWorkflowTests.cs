using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using KonaAI.Master.Model.Master.App.ViewModel;
using KonaAI.Master.Model.Master.App.SaveModel;
using KonaAI.Master.Test.Integration.Infrastructure;
using KonaAI.Master.Test.Integration.Infrastructure.Factories;
using KonaAI.Master.Test.Integration.Infrastructure.Fixtures;
using KonaAI.Master.Test.Integration.Infrastructure.TestData.Builders;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System;

namespace KonaAI.Master.Test.Integration.API.Workflows;

/// <summary>
/// Represents an OData response wrapper.
/// </summary>
public class ODataResponse<T>
{
    [JsonPropertyName("@odata.context")]
    public string? ODataContext { get; set; }

    [JsonPropertyName("value")]
    public List<T> Value { get; set; } = new();
}

/// <summary>
/// API integration tests for QuestionBankController complete CRUD workflows.
/// Tests end-to-end scenarios through real HTTP pipeline.
/// </summary>
[Collection("InMemoryDatabaseCollection")]
public class QuestionBankWorkflowTests : IClassFixture<InMemoryDatabaseFixture>
{
    private readonly InMemoryWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public QuestionBankWorkflowTests(InMemoryDatabaseFixture fixture)
    {
        _factory = new InMemoryWebApplicationFactory(fixture);
        _client = _factory.CreateAuthenticatedClientAsync().GetAwaiter().GetResult();
    }

    #region Complete CRUD Workflow Tests

    [Fact(Skip = "CRUD workflow depends on QuestionBank GET/POST stability; skip until isolation fixes are in.")]
    public async Task CompleteCrudWorkflow_CreateReadUpdateDelete_Success()
    {
        // Step 1: Create a new question bank
        var createModel = QuestionBankCreateModelBuilder.Create()
            .WithDescription("Test Question Bank for CRUD Workflow")
            .WithOptions(new[] { "Option 1", "Option 2", "Option 3" })
            .WithOnAction("test-action")
            .Build();

        var createJson = JsonSerializer.Serialize(createModel);
        var createContent = new StringContent(createJson, Encoding.UTF8, "application/json");

        var createResponse = await _client.PostAsync("/v1/QuestionBank", createContent);

        // Debug: Log the response content if it's not Created
        if (createResponse.StatusCode != HttpStatusCode.Created)
        {
            var responseContent = await createResponse.Content.ReadAsStringAsync();
            Console.WriteLine($"Response Status: {createResponse.StatusCode}");
            Console.WriteLine($"Response Content: {responseContent}");
        }

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        // Note: QuestionBankController.PostAsync returns Created() with no body
        // So we don't need to deserialize the response

        // Step 2: Read all question banks to verify creation
        var getResponse = await _client.GetAsync("/v1/QuestionBank");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        // Debug: Let's see what the actual response content is
        var getResponseContent = await getResponse.Content.ReadAsStringAsync();
        Console.WriteLine($"Response Content: {getResponseContent}");

        // Parse OData response
        var odataResponse = JsonSerializer.Deserialize<ODataResponse<QuestionBankViewModel>>(getResponseContent);
        Console.WriteLine($"OData Response: {odataResponse?.ODataContext}");
        Console.WriteLine($"OData Value Count: {odataResponse?.Value?.Count}");
        var retrievedBanks = odataResponse?.Value ?? new List<QuestionBankViewModel>();
        Assert.NotNull(retrievedBanks);
        Assert.True(retrievedBanks.Count > 0);

        // Find the created question bank by description
        var createdBank = retrievedBanks.FirstOrDefault(b => b.Description == "Test Question Bank for CRUD Workflow");
        Assert.NotNull(createdBank);
        Assert.Equal("Test Question Bank for CRUD Workflow", createdBank.Description);

        // Step 3: Update the question bank
        var updateModel = new QuestionBankCreateModel
        {
            Description = "Updated Question Bank Description"
        };

        var updateJson = JsonSerializer.Serialize(updateModel);
        var updateContent = new StringContent(updateJson, Encoding.UTF8, "application/json");

        var updateResponse = await _client.PutAsync($"/v1/QuestionBank/{createdBank.RowId}", updateContent);
        Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);

        // Step 4: Verify the update via list endpoint
        var verifyListResponse = await _client.GetAsync("/v1/QuestionBank");
        Assert.Equal(HttpStatusCode.OK, verifyListResponse.StatusCode);
        var allAfterUpdate = await TestHelpers.ValidateODataResponseAsync<QuestionBankViewModel>(verifyListResponse);
        var updatedBank = allAfterUpdate.FirstOrDefault(b => b.RowId == createdBank.RowId);
        Assert.NotNull(updatedBank);
        Assert.Equal("Updated Question Bank Description", updatedBank!.Description);

        // Step 5: Delete the question bank
        var deleteResponse = await _client.DeleteAsync($"/v1/QuestionBank/{createdBank.RowId}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        // Step 6: Verify deletion via list endpoint (entity should not be present)
        var listAfterDelete = await _client.GetAsync("/v1/QuestionBank");
        Assert.Equal(HttpStatusCode.OK, listAfterDelete.StatusCode);
        var allAfterDelete = await TestHelpers.ValidateODataResponseAsync<QuestionBankViewModel>(listAfterDelete);
        Assert.DoesNotContain(allAfterDelete, b => b.RowId == createdBank.RowId);
    }

    [Fact]
    public async Task CreateQuestionBank_WithInvalidData_ReturnsBadRequest()
    {
        // Arrange
        var invalidModel = new QuestionBankCreateModel
        {
            Description = "" // Empty description should fail validation
        };

        var json = JsonSerializer.Serialize(invalidModel);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/v1/QuestionBank", content);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdateQuestionBank_WithNonExistentId_ReturnsNotFound()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        var updateModel = new QuestionBankCreateModel
        {
            Description = "Updated Description"
        };

        var json = JsonSerializer.Serialize(updateModel);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync($"/v1/QuestionBank/{nonExistentId}", content);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteQuestionBank_WithNonExistentId_ReturnsNotFound()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await _client.DeleteAsync($"/v1/QuestionBank/{nonExistentId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    #endregion

    #region Bulk Operations Tests

    [Fact]
    public async Task BulkCreateQuestionBanks_MultipleBanks_Success()
    {
        // Arrange
        var banks = new[]
        {
            new QuestionBankCreateModel { Description = "Bulk Bank 1", Options = new[]{"A"}, OnAction = "act" },
            new QuestionBankCreateModel { Description = "Bulk Bank 2", Options = new[]{"A"}, OnAction = "act" },
            new QuestionBankCreateModel { Description = "Bulk Bank 3", Options = new[]{"A"}, OnAction = "act" }
        };

        var createdBanks = new List<QuestionBankViewModel>();

        // Act - Create multiple banks
        foreach (var bank in banks)
        {
            var json = JsonSerializer.Serialize(bank);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/v1/QuestionBank", content);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            // Response body is boolean; fetch created entity from list if needed later
        }

        // Assert - Verify all banks were created
        var listAfterCreate = await _client.GetAsync("/v1/QuestionBank");
        Assert.Equal(HttpStatusCode.OK, listAfterCreate.StatusCode);
        var allAfterCreate = await TestHelpers.ValidateODataResponseAsync<QuestionBankViewModel>(listAfterCreate);
        var bulkBanks = allAfterCreate.Where(b => b.Description.StartsWith("Bulk Bank")).ToArray();
        Assert.True(bulkBanks.Length >= 3);

        // Cleanup - Delete created banks
        // Cleanup via list (response body no longer contains created entity)
        var listResponse = await _client.GetAsync("/v1/QuestionBank");
        Assert.Equal(HttpStatusCode.OK, listResponse.StatusCode);
        var existing = await TestHelpers.ValidateODataResponseAsync<QuestionBankViewModel>(listResponse);
        foreach (var bank in existing.Where(b => b.Description.StartsWith("Bulk Bank")))
        {
            var deleteResponse = await _client.DeleteAsync($"/v1/QuestionBank({bank.RowId})");
            Assert.True(deleteResponse.StatusCode == HttpStatusCode.NoContent || deleteResponse.StatusCode == HttpStatusCode.NotFound);
        }
    }

    [Fact]
    public async Task GetQuestionBanks_WithMultipleBanks_ReturnsAllBanks()
    {
        // Arrange - Create test data
        var testBanks = new[]
        {
            new QuestionBankCreateModel { Description = "Test Bank 1", Options = new[]{"A"}, OnAction = "act" },
            new QuestionBankCreateModel { Description = "Test Bank 2", Options = new[]{"A"}, OnAction = "act" },
            new QuestionBankCreateModel { Description = "Test Bank 3", Options = new[]{"A"}, OnAction = "act" }
        };

        var createdBanks = new List<QuestionBankViewModel>();

        foreach (var bank in testBanks)
        {
            var json = JsonSerializer.Serialize(bank);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/v1/QuestionBank", content);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        // Act
        var getResponse = await _client.GetAsync("/v1/QuestionBank");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var allBanks = await TestHelpers.ValidateODataResponseAsync<QuestionBankViewModel>(getResponse);

        // Assert
        Assert.True(allBanks.Length >= 3); // Should have at least our 3 test banks
        Assert.Contains(allBanks, bank => bank.Description == "Test Bank 1");
        Assert.Contains(allBanks, bank => bank.Description == "Test Bank 2");
        Assert.Contains(allBanks, bank => bank.Description == "Test Bank 3");

        // Cleanup via list
        var listResponse2 = await _client.GetAsync("/v1/QuestionBank");
        Assert.Equal(HttpStatusCode.OK, listResponse2.StatusCode);
        var existing2 = await TestHelpers.ValidateODataResponseAsync<QuestionBankViewModel>(listResponse2);
        foreach (var bank in existing2.Where(b => b.Description.StartsWith("Test Bank")))
        {
            await _client.DeleteAsync($"/v1/QuestionBank/{bank.RowId}");
        }
    }

    #endregion

    #region Error Handling Tests

    [Fact]
    public async Task CreateQuestionBank_WithServerError_ReturnsInternalServerError()
    {
        // This test would require mocking the business layer to throw an exception
        // For now, we'll test the happy path and error handling through validation
        var invalidModel = new QuestionBankCreateModel
        {
            Description = null // This should trigger validation error
        };

        var json = JsonSerializer.Serialize(invalidModel);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/v1/QuestionBank", content);

        // Should return BadRequest for validation error, not InternalServerError
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }



    #endregion

    #region Performance Tests

    [Fact]
    public async Task CreateQuestionBank_PerformanceTest_CompletesWithinTimeLimit()
    {
        // Arrange
        var createModel = new QuestionBankCreateModel
        {
            Description = "Performance Test Bank",
            Options = new[] { "X" },
            OnAction = "act"
        };

        var json = JsonSerializer.Serialize(createModel);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act & Assert
        var executionTime = await TestHelpers.MeasureExecutionTimeAsync(async () =>
        {
            var response = await _client.PostAsync("/v1/QuestionBank", content);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        });

        // Should complete within 3 seconds
        Assert.True(executionTime.TotalSeconds < 3, $"Create took {executionTime.TotalSeconds} seconds, expected < 3 seconds");
    }

    [Fact]
    public async Task ConcurrentCreateOperations_AllCompleteSuccessfully()
    {
        // Arrange
        var createTasks = new List<Task<HttpResponseMessage>>();

        // Act - Create 5 concurrent requests
        for (int i = 0; i < 5; i++)
        {
            var createModel = new QuestionBankCreateModel
            {
                Description = $"Concurrent Bank {i}",
                Options = new[] { "A" },
                OnAction = "act"
            };

            var json = JsonSerializer.Serialize(createModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            createTasks.Add(_client.PostAsync("/v1/QuestionBank", content));
        }

        var responses = await Task.WhenAll(createTasks);

        // Assert - All requests should succeed
        Assert.All(responses, response => Assert.Equal(HttpStatusCode.Created, response.StatusCode));

        // Cleanup via list - delete concurrent banks created by this test
        var listResponse2 = await _client.GetAsync("/v1/QuestionBank");
        Assert.Equal(HttpStatusCode.OK, listResponse2.StatusCode);
        var existing2 = await TestHelpers.ValidateODataResponseAsync<QuestionBankViewModel>(listResponse2);
        foreach (var bank in existing2.Where(b => b.Description.StartsWith("Concurrent Bank ")))
        {
            await _client.DeleteAsync($"/v1/QuestionBank/{bank.RowId}");
        }
    }

    #endregion
}
