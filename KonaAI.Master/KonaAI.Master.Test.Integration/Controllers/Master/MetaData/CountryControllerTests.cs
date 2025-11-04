using KonaAI.Master.API.Controllers.Master.MetaData;
using KonaAI.Master.Business.Master.UserMetaData.Logic.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace KonaAI.Master.Test.Integration.Controllers.Master.MetaData;

/// <summary>
/// Integration-style tests for <see cref="KonaAI.Master.API.Controllers.Master.MetaData.CountryController"/>.
/// Validates:
/// - GET list returns 200 with queryable payload
/// - GET by id returns 200 with matching entity
/// </summary>
public class CountryControllerTests
{
    private static CountryController CreateController(out Mock<ICountryBusiness> business)
    {
        var logger = new Mock<ILogger<CountryController>>();
        business = new Mock<ICountryBusiness>(MockBehavior.Strict);
        return new CountryController(logger.Object, business.Object);
    }

    [Fact]
    public async Task GetAsync_ReturnsOk()
    {
        var controller = CreateController(out var business);
        var queryable = new List<Model.Common.MetaDataViewModel> { new() }.AsQueryable();
        business.Setup(b => b.GetAsync()).ReturnsAsync(queryable);

        var result = await controller.GetAsync();
        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Same(queryable, ok.Value);
        business.VerifyAll();
    }

    [Fact]
    public async Task GetByRowIdAsync_ReturnsOk_WhenFound()
    {
        var controller = CreateController(out var business);
        var id = Guid.NewGuid();
        var list = new List<Model.Common.MetaDataViewModel> { new() { RowId = id } };
        business.Setup(b => b.GetAsync()).ReturnsAsync(list.AsQueryable());

        var result = await controller.GetByRowIdAsync(id);
        var ok = Assert.IsType<OkObjectResult>(result);
        var payload = Assert.IsType<Model.Common.MetaDataViewModel>(ok.Value);
        Assert.Equal(id, payload.RowId);
        business.VerifyAll();
    }
}