using KonaAI.Master.API.Controllers.Master.MetaData;
using KonaAI.Master.Business.Master.UserMetaData.Logic.Interface;
using KonaAI.Master.Model.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace KonaAI.Master.Test.Integration.Controllers.Master.MetaData;

/// <summary>
/// Integration-style tests for <see cref="KonaAI.Master.API.Controllers.Master.MetaData.ProjectUnitController"/>.
/// Covers:
/// - GET list (200) for read-only metadata controller
/// - GET by RowId (200 or 404)
/// - Error handling (500) for server errors
/// </summary>
public class ProjectUnitControllerTests
{
    // Overview:
    // - Exercises ProjectUnitController actions with mocked business dependencies.
    // - Verifies happy paths (200) and error scenarios (500).
    // - This is a read-only metadata controller, so only GET operations are tested.
    private static ProjectUnitController CreateController(
        out Mock<IProjectUnitBusiness> business)
    {
        var logger = new Mock<ILogger<ProjectUnitController>>();
        business = new Mock<IProjectUnitBusiness>(MockBehavior.Strict);
        return new ProjectUnitController(logger.Object, business.Object);
    }

    [Fact]
    public async Task GetAsync_ReturnsOk_WithQueryable()
    {
        // Arrange: business returns a queryable list
        var controller = CreateController(out var business);
        var data = new List<MetaDataViewModel>
        {
            new() { RowId = Guid.NewGuid(), Name = "Unit 1", Description = "First unit" }
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
    public async Task GetByRowIdAsync_ReturnsOk_WhenFound()
    {
        // Arrange: business returns data with matching RowId
        var controller = CreateController(out var business);
        var rowId = Guid.NewGuid();
        var data = new List<MetaDataViewModel>
        {
            new() { RowId = rowId, Name = "Unit 1", Description = "First unit" }
        }.AsQueryable();
        business.Setup(b => b.GetAsync()).ReturnsAsync(data);

        // Act
        var result = await controller.GetByRowIdAsync(rowId);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(ok.Value);
        business.VerifyAll();
    }

    [Fact]
    public async Task GetByRowIdAsync_ReturnsNotFound_WhenMissing()
    {
        // Arrange: business returns empty data
        var controller = CreateController(out var business);
        var rowId = Guid.NewGuid();
        var data = new List<MetaDataViewModel>().AsQueryable();
        business.Setup(b => b.GetAsync()).ReturnsAsync(data);

        // Act
        var result = await controller.GetByRowIdAsync(rowId);

        // Assert
        var notFound = Assert.IsType<Microsoft.AspNetCore.OData.Results.NotFoundODataResult>(result);
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