using System.Net;
using KonaAI.Master.Test.Integration.Infrastructure;
using KonaAI.Master.Test.Integration.Infrastructure.Factories;
using KonaAI.Master.Test.Integration.Infrastructure.Fixtures;
using Xunit;

namespace KonaAI.Master.Test.Integration.API.OData;

/// <summary>
/// WebApplicationFactory-based smoke tests to ensure all OData entity sets respond under /v1.
/// Splits master-scoped and tenant-scoped entity sets. Only validates 200 responses (content may be empty).
/// </summary>
[Collection("InMemoryDatabaseCollection")]
public class ODataSmokeTests : IClassFixture<InMemoryDatabaseFixture>
{
    private readonly InMemoryWebApplicationFactory _factory;

    public ODataSmokeTests(InMemoryDatabaseFixture fixture)
    {
        _factory = new InMemoryWebApplicationFactory(fixture);
    }

    [Theory]
    [InlineData("Country")]
    [InlineData("Module")]
    [InlineData("AppNavigation")]
    [InlineData("NavigationAction")]
    [InlineData("RoleNavigationUserAction")]
    [InlineData("ProjectAuditResponsibility")]
    [InlineData("ProjectDepartment")]
    [InlineData("ProjectRiskArea")]
    [InlineData("ProjectUnit")]
    [InlineData("RenderType")]
    [InlineData("QuestionBank")]
    [InlineData("Menu")]
    public async Task MasterEntitySets_GetCollection_ReturnsOk(string entitySet)
    {
        var client = await _factory.CreateAuthenticatedClientAsync();

        var response = await client.GetAsync($"/v1/{entitySet}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Theory]
    [InlineData("Client")]
    [InlineData("ClientProject")]
    [InlineData("ClientUser")]
    [InlineData("ClientLicense")]
    [InlineData("ClientQuestionBank")]
    [InlineData("ClientQuestionnaireAssociation")]
    [InlineData("ClientQuestionnaire")]
    [InlineData("ClientRoleType")]
    [InlineData("ClientProjectAuditResponsibility")]
    [InlineData("ClientProjectCountry")]
    [InlineData("ClientProjectDepartment")]
    [InlineData("ClientProjectRiskArea")]
    [InlineData("ClientProjectUnit")]
    public async Task TenantEntitySets_GetCollection_ReturnsOk(string entitySet)
    {
        var client = _factory.CreateClientWithUserContext(clientId: 1, userId: "smoke-user");

        var response = await client.GetAsync($"/v1/{entitySet}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}


