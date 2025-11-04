using KonaAI.Master.API.Controllers.Authentication;
using KonaAI.Master.Business.Authentication.Logic.Interface;
using KonaAI.Master.Model.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Reflection;

namespace KonaAI.Master.Test.Unit.Controllers.Authentication;

/// <summary>
/// Unit tests for <see cref="MenuController"/>.
/// Covers:
/// - GET list (200, 500)
/// - Exception handling and logging
/// - Authorization scenarios
/// </summary>
public class MenuControllerUnitTests
{
    private readonly Mock<ILogger<MenuController>> _logger = new();
    private readonly Mock<IMenuBusiness> _business = new();

    private MenuController CreateSut() =>
        new(_logger.Object, _business.Object);

    #region GetAsync

    [Fact]
    public async Task GetAsync_BusinessReturnsData_Returns200Ok()
    {
        // Arrange
        var data = new List<MetaDataViewModel>
        {
            new() { RowId = Guid.NewGuid(), Name = "Dashboard", Description = "Main Dashboard", OrderBy = 1 },
            new() { RowId = Guid.NewGuid(), Name = "Users", Description = "User Management", OrderBy = 2 },
            new() { RowId = Guid.NewGuid(), Name = "Settings", Description = "System Settings", OrderBy = 3 }
        }.AsQueryable();

        _business.Setup(b => b.GetAsync()).ReturnsAsync(data);

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
    public async Task GetAsync_EmptyData_Returns200Ok()
    {
        // Arrange
        var data = Array.Empty<MetaDataViewModel>().AsQueryable();
        _business.Setup(b => b.GetAsync()).ReturnsAsync(data);

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
    public async Task GetAsync_UnauthorizedException_Returns500()
    {
        // Arrange
        _business.Setup(b => b.GetAsync()).ThrowsAsync(new UnauthorizedAccessException("User role information is invalid"));

        var sut = CreateSut();

        // Act
        var result = await sut.GetAsync();

        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = (ObjectResult)result;
        Assert.Equal(500, objectResult.StatusCode);
        Assert.Equal("User role information is invalid", objectResult.Value);
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

    #region Authorization

    [Fact]
    public void Class_ShouldHaveAuthorizeAttribute()
    {
        var attr = typeof(MenuController).GetCustomAttribute<AuthorizeAttribute>(inherit: true);
        Assert.NotNull(attr);
    }

    [Fact]
    public void GetAsync_ShouldHaveAuthorizeAttribute()
    {
        var mi = typeof(MenuController).GetMethod(nameof(MenuController.GetAsync));
        Assert.NotNull(mi);
        var attrs = mi!.GetCustomAttributes<AuthorizeAttribute>(inherit: true);
        Assert.True(attrs.Any());
    }

    #endregion Authorization
}