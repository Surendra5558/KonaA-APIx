using KonaAI.Master.Repository;
using KonaAI.Master.Repository.Domain.Master.App;
using KonaAI.Master.Repository.Domain.Tenant.Client;
using KonaAI.Master.Test.Integration.Infrastructure.TestData.Builders;
using KonaAI.Master.Test.Integration.Infrastructure;

namespace KonaAI.Master.Test.Integration.Infrastructure.TestData.Seeders;

/// <summary>
/// Seeds test database with comprehensive test data for integration testing.
/// </summary>
public class TestDataSeeder
{
    public async Task SeedAsync(TestDbContext context)
    {
        // Seed master data
        await SeedMasterDataAsync(context);

        // Seed tenant data
        await SeedTenantDataAsync(context);

        await context.SaveChangesAsync();
    }

    private async Task SeedMasterDataAsync(TestDbContext context)
    {
        // Seed countries
        var countries = CountryBuilder.CreateDefaults();
        context.AddRange(countries);

        // Seed modules
        var modules = ModuleBuilder.CreateDefaults();
        context.AddRange(modules);

        // Seed role types
        var roleTypes = RoleTypeBuilder.CreateDefaults();
        context.AddRange(roleTypes);

        // Seed navigation actions
        var navigationActions = NavigationActionBuilder.CreateDefaults();
        context.AddRange(navigationActions);

        // Seed project departments
        var departments = ProjectDepartmentBuilder.CreateDefaults();
        context.AddRange(departments);

        // Seed project units
        var units = ProjectUnitBuilder.CreateDefaults();
        context.AddRange(units);

        // Seed project risk areas
        var riskAreas = ProjectRiskAreaBuilder.CreateDefaults();
        context.AddRange(riskAreas);

        // Seed project audit responsibilities
        var auditResponsibilities = ProjectAuditResponsibilityBuilder.CreateDefaults();
        context.AddRange(auditResponsibilities);

        // Seed render types
        var renderTypes = RenderTypeBuilder.CreateDefaults();
        context.AddRange(renderTypes);

        // Seed user actions (required for authentication audit)
        var userActions = UserActionBuilder.CreateDefaults();
        context.AddRange(userActions);

        // Seed navigation user actions (required for authentication audit)
        var navigationUserActions = NavigationUserActionBuilder.CreateDefaults();
        context.AddRange(navigationUserActions);

        // Seed role navigation user actions (required for authentication audit)
        var roleNavigationUserActions = RoleNavigationUserActionBuilder.CreateDefaults();
        context.AddRange(roleNavigationUserActions);

        // Seed users with authentication credentials for login tests
        var users = UserBuilder.CreateDefaults();
        context.AddRange(users);

        await context.SaveChangesAsync();
    }

    private async Task SeedTenantDataAsync(TestDbContext context)
    {
        // Create test clients
        var clients = new[]
        {
            ClientBuilder.Create()
                .WithName("Test Client 1")
                .WithCode("TC001")
                .Build(),
            ClientBuilder.Create()
                .WithName("Test Client 2")
                .WithCode("TC002")
                .Build(),
            ClientBuilder.Create()
                .WithName("Test Client 3")
                .WithCode("TC003")
                .Inactive()
                .Build()
        };

        context.AddRange(clients);
        await context.SaveChangesAsync();

        // Create client role types (linking roles to clients for authentication)
        var clientRoleTypes = ClientRoleTypeBuilder.CreateDefaults();
        context.AddRange(clientRoleTypes);
        await context.SaveChangesAsync();

        // Create client projects for each client
        foreach (var client in clients)
        {
            var projects = ClientProjectBuilder.CreateForClient(client.Id)
                .CreateMultiple(3);

            context.AddRange(projects);
        }

        // Create client users
        var clientUsers = ClientUserBuilder.CreateDefaults();
        context.AddRange(clientUsers);

        // Create client questionnaires
        var questionnaires = ClientQuestionnaireBuilder.CreateDefaults();
        context.AddRange(questionnaires);

        // Create client question banks
        var questionBanks = ClientQuestionBankBuilder.CreateDefaults();
        context.AddRange(questionBanks);

        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Seeds data for a specific client (multi-tenant testing).
    /// </summary>
    public async Task SeedClientDataAsync(TestDbContext context, long clientId, int dataCount = 10)
    {
        // Create client-specific projects
        var projects = ClientProjectBuilder.CreateForClient(clientId)
            .CreateMultiple(dataCount);
        context.AddRange(projects);

        // Create client-specific questionnaires
        var questionnaires = ClientQuestionnaireBuilder.CreateForClient(clientId)
            .CreateMultiple(dataCount / 2);
        context.AddRange(questionnaires);

        // Create client-specific question banks
        var questionBanks = ClientQuestionBankBuilder.CreateForClient(clientId)
            .CreateMultiple(dataCount);
        context.AddRange(questionBanks);

        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Seeds performance test data (large datasets).
    /// </summary>
    public async Task SeedPerformanceDataAsync(TestDefaultContext context, int recordCount = 1000)
    {
        // Create large number of clients
        var clients = ClientBuilder.CreateMultiple(recordCount / 100);

        context.AddRange(clients);
        await context.SaveChangesAsync();

        // Create large number of projects
        var projects = new List<ClientProject>();
        foreach (var client in clients)
        {
            var clientProjects = ClientProjectBuilder.CreateForClient(client.Id)
                .CreateMultiple(recordCount / clients.Count);
            projects.AddRange(clientProjects);
        }

        context.AddRange(projects);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Seeds data for concurrent testing scenarios.
    /// </summary>
    public async Task SeedConcurrentTestDataAsync(TestDefaultContext context, int clientCount = 5)
    {
        var clients = new List<Client>();

        for (int i = 1; i <= clientCount; i++)
        {
            var client = ClientBuilder.Create()
                .WithName($"Concurrent Client {i}")
                .WithCode($"CC{i:D3}")
                .Build();

            clients.Add(client);
        }

        context.AddRange(clients);
        await context.SaveChangesAsync();

        // Create projects for each client
        foreach (var client in clients)
        {
            var projects = ClientProjectBuilder.CreateForClient(client.Id)
                .CreateMultiple(5);
            context.AddRange(projects);
        }

        await context.SaveChangesAsync();
    }
}
