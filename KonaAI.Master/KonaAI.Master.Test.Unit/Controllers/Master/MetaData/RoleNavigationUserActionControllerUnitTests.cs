using KonaAI.Master.API.Controllers.Master.MetaData;
using KonaAI.Master.Business.Master.MetaData.Logic.Interface;
using KonaAI.Master.Model.Master.MetaData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.Extensions.Logging;
using Moq;
using System.Reflection;

namespace KonaAI.Master.Test.Unit.Controllers.Master.MetaData;

public class RoleNavigationUserActionControllerUnitTests
{
    private readonly Mock<ILogger<RoleNavigationUserActionController>> _logger = new();
    private readonly Mock<IRoleNavigationUserActionBusiness> _business = new();

    private RoleNavigationUserActionController CreateSut() =>
        new(_logger.Object, _business.Object);

    [Fact]
    public async Task GetAsync_Returns200_WithData()
    {
        var data = new List<RoleNavigationUserActionViewModel>
        {
            new() { RowId = Guid.NewGuid(), RoleTypeName = "Admin", NavigationName = "Dashboard", UserActionName = "View" },
            new() { RowId = Guid.NewGuid(), RoleTypeName = "User", NavigationName = "Profile", UserActionName = "Edit" }
        }.AsQueryable();

        _business.Setup(b => b.GetAsync()).ReturnsAsync(data);

        var sut = CreateSut();
        var result = await sut.GetAsync();

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(ok.Value);
        _business.Verify(b => b.GetAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAsync_WhenBusinessThrows_Returns500()
    {
        _business.Setup(b => b.GetAsync()).ThrowsAsync(new Exception("db error"));

        var sut = CreateSut();
        var result = await sut.GetAsync();

        var obj = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, obj.StatusCode);
        Assert.Equal("db error", obj.Value);
    }

    [Fact]
    public async Task GetByRowIdAsync_NotFound_Returns404()
    {
        _business.Setup(b => b.GetAsync()).ReturnsAsync(Array.Empty<RoleNavigationUserActionViewModel>().AsQueryable());

        var sut = CreateSut();
        var result = await sut.GetByRowIdAsync(Guid.NewGuid());

        var notFound = Assert.IsType<NotFoundODataResult>(result);
        Assert.Equal(404, notFound.StatusCode);
    }

    [Fact]
    public async Task GetByRowIdAsync_Found_Returns200()
    {
        var id = Guid.NewGuid();
        var data = new List<RoleNavigationUserActionViewModel>
        {
            new() { RowId = id, RoleTypeName = "Admin", NavigationName = "Dashboard", UserActionName = "View" }
        }.AsQueryable();

        _business.Setup(b => b.GetAsync()).ReturnsAsync(data);

        var sut = CreateSut();
        var result = await sut.GetByRowIdAsync(id);

        var ok = Assert.IsType<OkObjectResult>(result);
        var value = Assert.IsType<RoleNavigationUserActionViewModel>(ok.Value);
        Assert.Equal(id, value.RowId);
    }

    [Fact]
    public async Task GetByRowIdAsync_WhenBusinessThrows_Returns500()
    {
        _business.Setup(b => b.GetAsync()).ThrowsAsync(new Exception("boom"));

        var sut = CreateSut();
        var result = await sut.GetByRowIdAsync(Guid.NewGuid());

        var obj = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, obj.StatusCode);
        Assert.Equal("boom", obj.Value);
    }

    [Fact]
    public void Class_ShouldHaveAuthorizeAttribute()
    {
        var attr = typeof(RoleNavigationUserActionController).GetCustomAttribute<AuthorizeAttribute>(inherit: true);
        Assert.NotNull(attr);
    }

    [Fact]
    public void GetAsync_ShouldHaveAuthorizeAttribute()
    {
        var mi = typeof(RoleNavigationUserActionController).GetMethod(nameof(RoleNavigationUserActionController.GetAsync));
        Assert.NotNull(mi);
        var classAttr = typeof(RoleNavigationUserActionController).GetCustomAttribute<AuthorizeAttribute>(inherit: true);
        var methodHas = mi!.GetCustomAttributes<AuthorizeAttribute>(inherit: true).Any();
        Assert.True(classAttr is not null || methodHas);
    }
}