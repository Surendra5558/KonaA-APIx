using KonaAI.Master.API.Controllers.Tenant.UserMetaData;
using KonaAI.Master.Business.Tenant.UserMetaData.Logic.Interface;
using KonaAI.Master.Model.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Reflection;

namespace KonaAI.Master.Test.Unit.Controllers.Tenant.UserMetaData;

public class ClientProjectCountryControllerUnitTests
{
    private readonly Mock<ILogger<ClientProjectCountryController>> _logger = new();
    private readonly Mock<IClientProjectCountryBusiness> _business = new();

    private ClientProjectCountryController CreateSut() =>
        new(_logger.Object, _business.Object);

    [Fact]
    public async Task GetAsync_Returns200_WithData()
    {
        var data = new List<MetaDataViewModel>
        {
            new() { RowId = Guid.NewGuid(), Name = "India", Description = "IN", OrderBy = 1 }
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

    #region Authorization

    [Fact]
    public void Class_ShouldHaveAuthorizeAttribute()
    {
        var attr = typeof(ClientProjectCountryController).GetCustomAttribute<AuthorizeAttribute>(inherit: true);
        Assert.NotNull(attr);
    }

    [Fact]
    public void GetAsync_ShouldHaveAuthorizeAttribute()
    {
        var mi = typeof(ClientProjectCountryController).GetMethod(nameof(ClientProjectCountryController.GetAsync));
        Assert.NotNull(mi);
        var classAttr = typeof(ClientProjectCountryController).GetCustomAttribute<AuthorizeAttribute>(inherit: true);
        var methodHas = mi!.GetCustomAttributes<AuthorizeAttribute>(inherit: true).Any();
        Assert.True(classAttr is not null || methodHas);
    }

    #endregion Authorization
}