using KonaAI.Master.API.Controllers.Authentication;
using KonaAI.Master.Business.Master.MetaData.Logic.Interface;
using KonaAI.Master.Model.Master.MetaData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.Extensions.Logging;
using Moq;
using System.Reflection;

namespace KonaAI.Master.Test.Unit.Controllers.Authentication;

/// <summary>
/// Unit tests for <see cref="AppNavigationController"/>.
/// Covers:
/// - GET list (200, 500)
/// - GET by rowId (200, 404, 500)
/// - Exception handling and logging
/// </summary>
public class AppNavigationControllerUnitTests
{
    private readonly Mock<ILogger<AppNavigationController>> _logger = new();
    private readonly Mock<INavigationBusiness> _business = new();

    private AppNavigationController CreateSut() =>
        new(_logger.Object, _business.Object);

    #region GetAsync

    [Fact]
    public async Task GetAsync_BusinessReturnsData_Returns200Ok()
    {
        // Arrange
        var data = new List<NavigationViewModel>
        {
            new() { RowId = Guid.NewGuid(), Name = "Dashboard", ParentRowId = null, ParentName = null },
            new() { RowId = Guid.NewGuid(), Name = "Users", ParentRowId = null, ParentName = null }
        }.AsQueryable();

        _business.Setup(b => b.GetAsync()).Returns(Task.FromResult(data));

        var sut = CreateSut();

        // Act
        var result = await sut.GetAsync();

        // Assert
        Assert.IsType<OkObjectResult>(result);
        var okResult = (OkObjectResult)result;
        Assert.NotNull(okResult.Value);
        _business.Verify(b => b.GetAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAsync_BusinessThrows_Returns500()
    {
        // Arrange
        _business.Setup(b => b.GetAsync()).ThrowsAsync(new Exception("Database connection failed"));

        var sut = CreateSut();

        // Act
        var result = await sut.GetAsync();

        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = (ObjectResult)result;
        Assert.Equal(500, objectResult.StatusCode);
        Assert.Equal("Database connection failed", objectResult.Value);
    }

    #endregion GetAsync

    #region GetByRowIdAsync

    [Fact]
    public async Task GetByRowIdAsync_ItemExists_Returns200Ok()
    {
        // Arrange
        var rowId = Guid.NewGuid();
        var data = new List<NavigationViewModel>
        {
            new() { RowId = rowId, Name = "Dashboard", ParentRowId = null, ParentName = null }
        }.AsQueryable();

        _business.Setup(b => b.GetAsync()).Returns(Task.FromResult(data));

        var sut = CreateSut();

        // Act
        var result = await sut.GetByRowIdAsync(rowId);

        // Assert
        Assert.IsType<OkObjectResult>(result);
        var okResult = (OkObjectResult)result;
        Assert.NotNull(okResult.Value);
        _business.Verify(b => b.GetAsync(), Times.Once);
    }

    [Fact]
    public async Task GetByRowIdAsync_ItemNotFound_Returns404NotFound()
    {
        // Arrange
        var rowId = Guid.NewGuid();
        var data = new List<NavigationViewModel>
        {
            new() { RowId = Guid.NewGuid(), Name = "Dashboard", ParentRowId = null, ParentName = null }
        }.AsQueryable();

        _business.Setup(b => b.GetAsync()).Returns(Task.FromResult(data));

        var sut = CreateSut();

        // Act
        var result = await sut.GetByRowIdAsync(rowId);

        // Assert
        Assert.IsType<NotFoundODataResult>(result);
        var notFoundResult = (NotFoundODataResult)result;
        Assert.Equal(404, notFoundResult.StatusCode);
        // NotFoundODataResult doesn't have a Value property, just verify the status code
        _business.Verify(b => b.GetAsync(), Times.Once);
    }

    [Fact]
    public async Task GetByRowIdAsync_BusinessThrows_Returns500()
    {
        // Arrange
        var rowId = Guid.NewGuid();
        _business.Setup(b => b.GetAsync()).ThrowsAsync(new Exception("Database connection failed"));

        var sut = CreateSut();

        // Act
        var result = await sut.GetByRowIdAsync(rowId);

        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = (ObjectResult)result;
        Assert.Equal(500, objectResult.StatusCode);
        Assert.Equal("Database connection failed", objectResult.Value);
    }

    #endregion GetByRowIdAsync

    #region Authorization

    [Fact]
    public void Class_ShouldHaveAuthorizeAttribute()
    {
        var attr = typeof(AppNavigationController).GetCustomAttribute<AuthorizeAttribute>(inherit: true);
        Assert.NotNull(attr);
    }

    [Fact]
    public void GetAsync_ShouldHaveAuthorizeAttribute()
    {
        var mi = typeof(AppNavigationController).GetMethod(nameof(AppNavigationController.GetAsync));
        Assert.NotNull(mi);
        var classAttr = typeof(AppNavigationController).GetCustomAttribute<AuthorizeAttribute>(inherit: true);
        var methodHas = mi!.GetCustomAttributes<AuthorizeAttribute>(inherit: true).Any();
        Assert.True(classAttr is not null || methodHas);
    }

    #endregion Authorization
}