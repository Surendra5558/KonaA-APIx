using FluentValidation;
using FluentValidation.Results;
using KonaAI.Master.API.Controllers.Tenant.Client;
using KonaAI.Master.Business.Tenant.Client.Logic.Interface;
using KonaAI.Master.Model.Tenant.Client.SaveModel;
using KonaAI.Master.Model.Tenant.Client.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.Extensions.Logging;
using Moq;
using System.Reflection;

namespace KonaAI.Master.Test.Unit.Controllers.Tenant.Client;

public class ClientLicenseControllerUnitTests
{
    private readonly Mock<ILogger<ClientLicenseController>> _logger = new();
    private readonly Mock<IClientLicenseBusiness> _business = new();
    private readonly Mock<IValidator<ClientLicenseUpdateModel>> _updateValidator = new();

    private ClientLicenseController CreateSut() =>
        new(_logger.Object, _business.Object, _updateValidator.Object);

    [Fact]
    public async Task GetAsync_Returns200_WithData()
    {
        var data = new List<ClientLicenseViewModel>
        {
            new() { RowId = Guid.NewGuid(), Name = "Test License", Description = "Test Description", StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddYears(1), LicenseKey = "KEY123" }
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
    }

    [Fact]
    public async Task GetByRowIdAsync_Found_Returns200()
    {
        var id = Guid.NewGuid();
        var data = new ClientLicenseViewModel { RowId = id, LicenseKey = "KEY123" };

        _business.Setup(b => b.GetByRowIdAsync(id)).ReturnsAsync(data);

        var sut = CreateSut();
        var result = await sut.GetByRowIdAsync(id);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(ok.Value);
    }

    [Fact]
    public async Task GetByRowIdAsync_NotFound_Returns404()
    {
        _business.Setup(b => b.GetByRowIdAsync(It.IsAny<Guid>())).ReturnsAsync((ClientLicenseViewModel?)null);

        var sut = CreateSut();
        var result = await sut.GetByRowIdAsync(Guid.NewGuid());

        var notFound = Assert.IsType<NotFoundODataResult>(result);
        Assert.Equal(404, notFound.StatusCode);
    }

    [Fact]
    public async Task PutAsync_ValidModel_Returns204()
    {
        var id = Guid.NewGuid();
        var model = new ClientLicenseUpdateModel { Name = "Updated License", Description = "Updated Description", StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddYears(2) };
        _updateValidator.Setup(v => v.ValidateAsync(model, default)).ReturnsAsync(new ValidationResult());
        _business.Setup(b => b.UpdateAsync(id, model)).ReturnsAsync(1);

        var sut = CreateSut();
        var result = await sut.PutAsync(id, model);

        var noContent = Assert.IsType<NoContentResult>(result);
        Assert.Equal(204, noContent.StatusCode);
    }

    [Fact]
    public async Task PutAsync_ValidationFails_Returns400()
    {
        var id = Guid.NewGuid();
        var model = new ClientLicenseUpdateModel();
        var validationResult = new ValidationResult(new[] { new ValidationFailure("MaxUsers", "Required") });
        _updateValidator.Setup(v => v.ValidateAsync(model, default)).ReturnsAsync(validationResult);

        var sut = CreateSut();
        var result = await sut.PutAsync(id, model);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequest.StatusCode);
    }

    [Fact]
    public async Task PutAsync_NotFound_Returns404()
    {
        var id = Guid.NewGuid();
        var model = new ClientLicenseUpdateModel { Name = "Updated License", Description = "Updated Description", StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddYears(2) };
        _updateValidator.Setup(v => v.ValidateAsync(model, default)).ReturnsAsync(new ValidationResult());
        _business.Setup(b => b.UpdateAsync(id, model)).ThrowsAsync(new KeyNotFoundException());

        var sut = CreateSut();
        var result = await sut.PutAsync(id, model);

        var notFound = Assert.IsType<NotFoundODataResult>(result);
        Assert.Equal(404, notFound.StatusCode);
    }

    #region Authorization

    [Fact]
    public void Class_ShouldNotHaveAuthorizeAttribute()
    {
        var attr = typeof(ClientLicenseController).GetCustomAttribute<AuthorizeAttribute>(inherit: true);
        Assert.Null(attr);
    }

    [Fact]
    public void GetAsync_ShouldHaveAuthorizeAttribute()
    {
        var mi = typeof(ClientLicenseController).GetMethod(nameof(ClientLicenseController.GetAsync));
        Assert.NotNull(mi);
        var attrs = mi!.GetCustomAttributes<AuthorizeAttribute>(inherit: true);
        Assert.True(attrs.Any());
    }

    [Fact]
    public void PutAsync_ShouldHaveAuthorizeAttribute()
    {
        var mi = typeof(ClientLicenseController).GetMethod(nameof(ClientLicenseController.PutAsync));
        Assert.NotNull(mi);
        var attrs = mi!.GetCustomAttributes<AuthorizeAttribute>(inherit: true);
        Assert.True(attrs.Any());
    }

    #endregion Authorization
}