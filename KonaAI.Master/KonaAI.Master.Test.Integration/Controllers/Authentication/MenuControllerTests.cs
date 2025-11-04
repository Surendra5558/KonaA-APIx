using KonaAI.Master.API.Controllers.Authentication;
using KonaAI.Master.Business.Authentication.Logic.Interface;
using KonaAI.Master.Model.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.Extensions.Logging;
using Moq;

namespace KonaAI.Master.Test.Integration.Controllers.Authentication;

/// <summary>
/// Integration tests for <see cref="KonaAI.Master.API.Controllers.Authentication.MenuController"/>.
/// Covers:
/// - GET list (200) with OData support
/// - Error handling (500)
/// - Performance scenarios
/// </summary>
public class MenuControllerTests
{
    private static MenuController CreateController(
        out Mock<IMenuBusiness> business)
    {
        var logger = new Mock<ILogger<MenuController>>();
        business = new Mock<IMenuBusiness>(MockBehavior.Strict);
        return new MenuController(logger.Object, business.Object);
    }

    #region GET Tests

    [Fact]
    public async Task GetAsync_ReturnsOk_WithQueryable()
    {
        // Arrange: business returns a queryable list
        var controller = CreateController(out var business);
        var data = new List<MetaDataViewModel>
        {
            new() { RowId = Guid.NewGuid(), Name = "Menu Item 1" },
            new() { RowId = Guid.NewGuid(), Name = "Menu Item 2" }
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
    public async Task GetAsync_WithValidData_ReturnsMenuList()
    {
        // Arrange: business returns menu data
        var controller = CreateController(out var business);
        var data = new List<MetaDataViewModel>
        {
            new() { RowId = Guid.NewGuid(), Name = "Dashboard", Description = "Main dashboard", OrderBy = 1 },
            new() { RowId = Guid.NewGuid(), Name = "Reports", Description = "Reports section", OrderBy = 2 },
            new() { RowId = Guid.NewGuid(), Name = "Settings", Description = "Application settings", OrderBy = 3 }
        }.AsQueryable();

        business.Setup(b => b.GetAsync()).ReturnsAsync(data);

        // Act
        var result = await controller.GetAsync();

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        var resultData = Assert.IsAssignableFrom<IQueryable<MetaDataViewModel>>(ok.Value);
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
            .Select(i => new MetaDataViewModel
            {
                RowId = Guid.NewGuid(),
                Name = $"Menu Item {i}",
                Description = $"Description for menu item {i}",
                OrderBy = i
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
    public async Task CompleteWorkflow_GetMenu_Succeeds()
    {
        // Arrange: complete workflow
        var controller = CreateController(out var business);
        var allData = new List<MetaDataViewModel>
        {
            new() { RowId = Guid.NewGuid(), Name = "Dashboard", Description = "Main dashboard", OrderBy = 1 },
            new() { RowId = Guid.NewGuid(), Name = "Reports", Description = "Reports section", OrderBy = 2 },
            new() { RowId = Guid.NewGuid(), Name = "Settings", Description = "Application settings", OrderBy = 3 }
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