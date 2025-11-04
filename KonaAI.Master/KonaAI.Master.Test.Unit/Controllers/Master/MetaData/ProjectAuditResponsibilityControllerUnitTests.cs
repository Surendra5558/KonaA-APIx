using KonaAI.Master.API.Controllers.Master.MetaData;
using KonaAI.Master.Business.Master.UserMetaData.Logic.Interface;
using KonaAI.Master.Model.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.Extensions.Logging;
using Moq;
using System.Reflection;

namespace KonaAI.Master.Test.Unit.Controllers.Master.MetaData;

public class ProjectAuditResponsibilityControllerUnitTests
{
    private readonly Mock<ILogger<ProjectAuditResponsibilityController>> _logger = new();
    private readonly Mock<IProjectAuditResponsibilityBusiness> _business = new();

    private ProjectAuditResponsibilityController CreateSut() =>
        new(_logger.Object, _business.Object);

    [Fact]
    public async Task GetAsync_Returns200_WithData()
    {
        var data = new List<MetaDataViewModel>
        {
            new() { RowId = Guid.NewGuid(), Name = "Audit Manager", Description = "Audit Manager Role", OrderBy = 1 },
            new() { RowId = Guid.NewGuid(), Name = "Audit Lead", Description = "Audit Lead Role", OrderBy = 2 }
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
        _business.Setup(b => b.GetAsync()).ReturnsAsync(Array.Empty<MetaDataViewModel>().AsQueryable());

        var sut = CreateSut();
        var result = await sut.GetByRowIdAsync(Guid.NewGuid());

        var notFound = Assert.IsType<NotFoundODataResult>(result);
        Assert.Equal(404, notFound.StatusCode);
    }

    [Fact]
    public async Task GetByRowIdAsync_Found_Returns200()
    {
        var id = Guid.NewGuid();
        var data = new List<MetaDataViewModel>
        {
            new() { RowId = id, Name = "Audit Manager", Description = "Audit Manager Role", OrderBy = 1 }
        }.AsQueryable();

        _business.Setup(b => b.GetAsync()).ReturnsAsync(data);

        var sut = CreateSut();
        var result = await sut.GetByRowIdAsync(id);

        var ok = Assert.IsType<OkObjectResult>(result);
        var value = Assert.IsType<MetaDataViewModel>(ok.Value);
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
    public async Task GetAsync_EmptyData_Returns200()
    {
        _business.Setup(b => b.GetAsync()).ReturnsAsync(Array.Empty<MetaDataViewModel>().AsQueryable());

        var sut = CreateSut();
        var result = await sut.GetAsync();

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(ok.Value);
        _business.Verify(b => b.GetAsync(), Times.Once);
    }

    [Fact]
    public async Task GetByRowIdAsync_EmptyGuid_Returns404()
    {
        _business.Setup(b => b.GetAsync()).ReturnsAsync(Array.Empty<MetaDataViewModel>().AsQueryable());

        var sut = CreateSut();
        var result = await sut.GetByRowIdAsync(Guid.Empty);

        var notFound = Assert.IsType<NotFoundODataResult>(result);
        Assert.Equal(404, notFound.StatusCode);
    }

    #region Authorization

    [Fact]
    public void Class_ShouldHaveAuthorizeAttribute()
    {
        var attr = typeof(ProjectAuditResponsibilityController).GetCustomAttribute<AuthorizeAttribute>(inherit: true);
        Assert.NotNull(attr);
    }

    [Fact]
    public void GetAsync_ShouldHaveAuthorizeAttribute()
    {
        var mi = typeof(ProjectAuditResponsibilityController).GetMethod(nameof(ProjectAuditResponsibilityController.GetAsync));
        Assert.NotNull(mi);
        var classAttr = typeof(ProjectAuditResponsibilityController).GetCustomAttribute<AuthorizeAttribute>(inherit: true);
        var methodHas = mi!.GetCustomAttributes<AuthorizeAttribute>(inherit: true).Any();
        Assert.True(classAttr is not null || methodHas);
    }

    #endregion Authorization
}