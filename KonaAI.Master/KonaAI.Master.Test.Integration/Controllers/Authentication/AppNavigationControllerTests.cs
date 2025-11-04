using KonaAI.Master.API.Controllers.Authentication;
using KonaAI.Master.Business.Master.MetaData.Logic.Interface;
using KonaAI.Master.Model.Master.MetaData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.Extensions.Logging;
using Moq;

namespace KonaAI.Master.Test.Integration.Controllers.Authentication;

/// <summary>
/// Integration tests for <see cref="KonaAI.Master.API.Controllers.Authentication.AppNavigationController"/>.
/// Covers:
/// - GET list (200) with OData support
/// - Error handling (500)
/// - Performance scenarios
/// </summary>
public class AppNavigationControllerTests
{
    private static AppNavigationController CreateController(
        out Mock<INavigationBusiness> business)
    {
        var logger = new Mock<ILogger<AppNavigationController>>();
        business = new Mock<INavigationBusiness>(MockBehavior.Strict);
        return new AppNavigationController(logger.Object, business.Object);
    }

    #region GET Tests

    [Fact]
    public async Task GetAsync_ReturnsOk_WithQueryable()
    {
        // Arrange: business returns a queryable list
        var controller = CreateController(out var business);
        var data = new List<NavigationViewModel>
        {
            new() { RowId = Guid.NewGuid(), Name = "Dashboard", ParentName = "Root" },
            new() { RowId = Guid.NewGuid(), Name = "Reports", ParentName = "Root" }
        }.AsQueryable();

        business.Setup(b => b.GetAsync()).ReturnsAsync(data);

        // Act
        var result = await controller.GetAsync();

        // Assert
        Assert.IsType<OkObjectResult>(result);
        var ok = (OkObjectResult)result;
        Assert.Equal(data, ok.Value);
        business.Verify(b => b.GetAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAsync_WithValidData_ReturnsNavigationList()
    {
        // Arrange: business returns navigation data
        var controller = CreateController(out var business);
        var data = new List<NavigationViewModel>
        {
            new() { RowId = Guid.NewGuid(), Name = "Dashboard", ParentName = "Root" },
            new() { RowId = Guid.NewGuid(), Name = "Settings", ParentName = "Root" },
            new() { RowId = Guid.NewGuid(), Name = "User Management", ParentName = "Settings" }
        }.AsQueryable();

        business.Setup(b => b.GetAsync()).ReturnsAsync(data);

        // Act
        var result = await controller.GetAsync();

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        var resultData = Assert.IsAssignableFrom<IQueryable<NavigationViewModel>>(ok.Value);
        Assert.Equal(3, resultData.Count());
        business.VerifyAll();
    }

    [Fact]
    public async Task GetAsync_WithException_ReturnsInternalServerError()
    {
        // Arrange: business throws exception
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

    #endregion

    #region Performance Tests

    [Fact]
    public async Task GetAsync_LargeDataSet_PerformsWithinTimeLimit()
    {
        // Arrange: business returns large dataset
        var controller = CreateController(out var business);
        var data = Enumerable.Range(1, 1000)
            .Select(i => new NavigationViewModel
            {
                RowId = Guid.NewGuid(),
                Name = $"Navigation Item {i}",
                ParentName = i % 2 == 0 ? "Root" : "Sub Menu"
            })
            .AsQueryable();

        business.Setup(b => b.GetAsync()).ReturnsAsync(data);

        // Act
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var result = await controller.GetAsync();
        stopwatch.Stop();

        // Assert
        Assert.IsType<OkObjectResult>(result);
        Assert.True(stopwatch.ElapsedMilliseconds < 5000); // Should complete within 5 seconds
        business.VerifyAll();
    }

    #endregion

    #region End-to-End Workflow Tests

    [Fact]
    public async Task CompleteWorkflow_GetNavigation_Succeeds()
    {
        // Arrange: complete workflow
        var controller = CreateController(out var business);
        var allData = new List<NavigationViewModel>
        {
            new() { RowId = Guid.NewGuid(), Name = "Dashboard", ParentName = "Root" },
            new() { RowId = Guid.NewGuid(), Name = "Reports", ParentName = "Root" },
            new() { RowId = Guid.NewGuid(), Name = "Settings", ParentName = "Root" }
        }.AsQueryable();

        business.Setup(b => b.GetAsync()).ReturnsAsync(allData);

        // Act & Assert - Get All
        var getAllResult = await controller.GetAsync();
        var getAllOk = Assert.IsType<OkObjectResult>(getAllResult);
        Assert.Equal(allData, getAllOk.Value);

        // Verify all interactions
        business.VerifyAll();
    }

    #endregion
}