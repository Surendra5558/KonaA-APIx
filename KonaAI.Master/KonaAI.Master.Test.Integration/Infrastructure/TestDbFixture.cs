using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using KonaAI.Master.Repository;
using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Common.Model;
using KonaAI.Master.Repository.Domain.Master.App;
using KonaAI.Master.Repository.Domain.Tenant.Client;

namespace KonaAI.Master.Test.Integration.Infrastructure;

/// <summary>
/// Test database fixture for repository integration tests with isolated test data.
/// </summary>
public class TestDbFixture : IAsyncLifetime
{
    private readonly Dictionary<long, DefaultContext> _contexts = new();
    private readonly Dictionary<long, List<object>> _testData = new();

    public async Task InitializeAsync()
    {
        // Initialize test data for different clients
        await SeedTestDataAsync();
    }

    public async Task DisposeAsync()
    {
        // Clean up all contexts
        foreach (var context in _contexts.Values)
        {
            await context.DisposeAsync();
        }
        _contexts.Clear();
        _testData.Clear();
    }

    /// <summary>
    /// Creates a new DbContext for the specified client ID.
    /// </summary>
    public DefaultContext CreateContext(long clientId = 1)
    {
        var options = new DbContextOptionsBuilder<DefaultContext>()
            .UseInMemoryDatabase($"TestDb_{clientId}_{Guid.NewGuid()}")
            .Options;

        var userContext = new Mock<IUserContextService>();
        userContext.SetupGet(u => u.UserContext)
            .Returns(new UserContext { ClientId = clientId });

        var context = new DefaultContext(options, userContext.Object);
        _contexts[clientId] = context;

        return context;
    }

    /// <summary>
    /// Seeds test data for the specified client.
    /// </summary>
    public async Task SeedDataAsync(long clientId, int questionBankCount = 5, int questionnaireCount = 3)
    {
        using var context = CreateContext(clientId);

        // Seed question banks
        for (int i = 1; i <= questionBankCount; i++)
        {
            var questionBank = new QuestionBank
            {
                RowId = Guid.NewGuid(),
                Description = $"Test Question Bank {i}",
                CreatedOn = DateTime.UtcNow.AddDays(-i),
                IsActive = true
            };
            context.Set<QuestionBank>().Add(questionBank);
        }

        // Seed questionnaires
        for (int i = 1; i <= questionnaireCount; i++)
        {
            var questionnaire = new ClientQuestionnaire
            {
                RowId = Guid.NewGuid(),
                Name = $"Test Questionnaire {i}",
                ClientId = clientId,
                CreatedOn = DateTime.UtcNow.AddDays(-i),
                IsActive = true
            };
            context.Set<ClientQuestionnaire>().Add(questionnaire);
        }

        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Seeds multi-tenant test data for isolation testing.
    /// </summary>
    public async Task SeedMultiTenantDataAsync()
    {
        // Client 1 data
        await SeedDataAsync(clientId: 1, questionBankCount: 3, questionnaireCount: 2);

        // Client 2 data
        await SeedDataAsync(clientId: 2, questionBankCount: 2, questionnaireCount: 4);

        // Client 3 data
        await SeedDataAsync(clientId: 3, questionBankCount: 1, questionnaireCount: 1);
    }

    /// <summary>
    /// Cleans up test data for the specified client.
    /// </summary>
    public async Task CleanupDataAsync(long clientId)
    {
        if (_contexts.TryGetValue(clientId, out var context))
        {
            // Remove all test data
            context.Set<QuestionBank>().RemoveRange(context.Set<QuestionBank>());
            context.Set<ClientQuestionnaire>().RemoveRange(context.Set<ClientQuestionnaire>());

            await context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Gets test data count for the specified client.
    /// </summary>
    public async Task<int> GetDataCountAsync<T>(long clientId) where T : class
    {
        if (_contexts.TryGetValue(clientId, out var context))
        {
            return await context.Set<T>().CountAsync();
        }
        return 0;
    }

    private async Task SeedTestDataAsync()
    {
        // Seed basic test data for common scenarios
        await SeedDataAsync(clientId: 1, questionBankCount: 10, questionnaireCount: 5);
    }
}

/// <summary>
/// Collection attribute for database tests to ensure proper cleanup.
/// </summary>
[CollectionDefinition("DbCollection")]
public class DbCollection : ICollectionFixture<TestDbFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}
