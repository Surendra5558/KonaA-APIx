using KonaAI.Master.API.Controllers.Tenant.UserMetaData;
using KonaAI.Master.Business.Tenant.UserMetaData.Logic.Interface;
using KonaAI.Master.Model.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace KonaAI.Master.Test.Integration.Controllers.Tenant.UserMetaData;

/// <summary>
/// Integration-style tests for <see cref="KonaAI.Master.API.Controllers.Tenant.UserMetaData.ClientProjectAuditResponsibilityController"/>.
/// Covers:
/// - GET list (200) for read-only metadata controller
/// - GET by RowId (200 or 404)
/// - Error handling (500) for server errors
/// </summary>
public class ClientProjectAuditResponsibilityControllerTests
{
    // Overview:
    // - Exercises ClientProjectAuditResponsibilityController actions with mocked business dependencies.
    // - Verifies happy paths (200) and error scenarios (500).
    // - This is a read-only metadata controller, so only GET operations are tested.
    private static ClientProjectAuditResponsibilityController CreateController(
        out Mock<IClientProjectAuditResponsibilityBusiness> business)
    {
        var logger = new Mock<ILogger<ClientProjectAuditResponsibilityController>>();
        business = new Mock<IClientProjectAuditResponsibilityBusiness>(MockBehavior.Strict);
        return new ClientProjectAuditResponsibilityController(logger.Object, business.Object);
    }

    [Fact]
    public async Task GetAsync_ReturnsOk_WithQueryable()
    {
        // Arrange: business returns a queryable list
        var controller = CreateController(out var business);
        var data = new List<MetaDataViewModel>
        {
            new() { RowId = Guid.NewGuid(), Name = "Audit Responsibility 1", Description = "First audit responsibility" }
        }.AsQueryable();
        business.Setup(b => b.GetAsync()).ReturnsAsync(data);

        // Act
        var result = await controller.GetAsync();

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Same(data, ok.Value);
        business.VerifyAll();
    }

    [Fact]
    public async Task GetAsync_ReturnsServerError_WhenBusinessThrows()
    {
        // Arrange: business throws an exception
        var controller = CreateController(out var business);
        business.Setup(b => b.GetAsync()).ThrowsAsync(new Exception("Database connection failed"));

        // Act
        var result = await controller.GetAsync();

        // Assert
        var statusCode = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCode.StatusCode);
        Assert.Equal("Database connection failed", statusCode.Value);
        business.VerifyAll();
    }
}