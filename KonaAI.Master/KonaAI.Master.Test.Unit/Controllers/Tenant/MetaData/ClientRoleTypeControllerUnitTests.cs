using FluentValidation;
using FluentValidation.Results;
using KonaAI.Master.API.Controllers.Tenant.MetaData;
using KonaAI.Master.Business.Tenant.MetaData.Logic.Interface;
using KonaAI.Master.Model.Common;
using KonaAI.Master.Model.Tenant.Client.SaveModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.Extensions.Logging;
using Moq;
using System.Reflection;

namespace KonaAI.Master.Test.Unit.Controllers.Tenant.MetaData;

public class ClientRoleTypeControllerUnitTests
{
    private readonly Mock<ILogger<ClientRoleTypeController>> _logger = new();
    private readonly Mock<IClientRoleTypeBusiness> _business = new();
    private readonly Mock<IValidator<ClientRoleTypeCreateModel>> _createValidator = new();
    private readonly Mock<IValidator<ClientRoleTypeUpdateModel>> _updateValidator = new();

    private ClientRoleTypeController CreateSut() =>
        new(_logger.Object, _business.Object, _createValidator.Object, _updateValidator.Object);

    [Fact]
    public async Task GetAsync_Returns200_WithData()
    {
        var data = new List<MetaDataViewModel>
        {
            new() { RowId = Guid.NewGuid(), Name = "Admin", Description = "Administrator", OrderBy = 1 }
        }.AsQueryable();

        _business.Setup(b => b.GetAsync()).ReturnsAsync((IQueryable<MetaDataViewModel>?)data);

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
    public async Task GetByIdAsync_Found_Returns200()
    {
        var id = Guid.NewGuid();
        var data = new MetaDataViewModel { RowId = id, Name = "Admin" };

        _business.Setup(b => b.GetByRowIdAsync(id)).ReturnsAsync(data);

        var sut = CreateSut();
        var result = await sut.GetByIdAsync(id);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(ok.Value);
    }

    [Fact]
    public async Task GetByIdAsync_NotFound_Returns404()
    {
        _business.Setup(b => b.GetByRowIdAsync(It.IsAny<Guid>())).ReturnsAsync((MetaDataViewModel?)null);

        var sut = CreateSut();
        var result = await sut.GetByIdAsync(Guid.NewGuid());

        var notFound = Assert.IsType<NotFoundODataResult>(result);
        Assert.Equal(404, notFound.StatusCode);
    }

    [Fact]
    public async Task PostAsync_ValidModel_Returns201()
    {
        var model = new ClientRoleTypeCreateModel { Name = "Test Role", Description = "Test Description", OrderBy = 1 };
        _createValidator.Setup(v => v.ValidateAsync(model, default)).ReturnsAsync(new ValidationResult());
        _business.Setup(b => b.CreateAsync(model)).ReturnsAsync(1);

        var sut = CreateSut();
        var result = await sut.PostAsync(model);

        var created = Assert.IsType<CreatedResult>(result);
        Assert.Equal(201, created.StatusCode);
    }

    [Fact]
    public async Task PostAsync_ValidationFails_Returns400()
    {
        var model = new ClientRoleTypeCreateModel();
        var validationResult = new ValidationResult(new[] { new ValidationFailure("RoleTypeId", "Required") });
        _createValidator.Setup(v => v.ValidateAsync(model, default)).ReturnsAsync(validationResult);

        var sut = CreateSut();
        var result = await sut.PostAsync(model);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequest.StatusCode);
    }

    [Fact]
    public async Task PutAsync_ValidModel_Returns204()
    {
        var id = Guid.NewGuid();
        var model = new ClientRoleTypeUpdateModel { RowId = id };
        _updateValidator.Setup(v => v.ValidateAsync(model, default)).ReturnsAsync(new ValidationResult());
        _business.Setup(b => b.UpdateAsync(id, model)).ReturnsAsync(1);

        var sut = CreateSut();
        var result = await sut.PutAsync(id, model);

        var noContent = Assert.IsType<NoContentResult>(result);
        Assert.Equal(204, noContent.StatusCode);
    }

    [Fact]
    public async Task PutAsync_NotFound_Returns404()
    {
        var id = Guid.NewGuid();
        var model = new ClientRoleTypeUpdateModel { RowId = id };
        _updateValidator.Setup(v => v.ValidateAsync(model, default)).ReturnsAsync(new ValidationResult());
        _business.Setup(b => b.UpdateAsync(id, model)).ThrowsAsync(new KeyNotFoundException());

        var sut = CreateSut();
        var result = await sut.PutAsync(id, model);

        var notFound = Assert.IsType<NotFoundODataResult>(result);
        Assert.Equal(404, notFound.StatusCode);
    }

    [Fact]
    public async Task DeleteAsync_Exists_Returns204()
    {
        var id = Guid.NewGuid();
        _business.Setup(b => b.DeleteAsync(id)).ReturnsAsync(1);

        var sut = CreateSut();
        var result = await sut.DeleteAsync(id);

        var noContent = Assert.IsType<NoContentResult>(result);
        Assert.Equal(204, noContent.StatusCode);
    }

    [Fact]
    public async Task DeleteAsync_NotFound_Returns404()
    {
        var id = Guid.NewGuid();
        _business.Setup(b => b.DeleteAsync(id)).ThrowsAsync(new KeyNotFoundException());

        var sut = CreateSut();
        var result = await sut.DeleteAsync(id);

        var notFound = Assert.IsType<NotFoundODataResult>(result);
        Assert.Equal(404, notFound.StatusCode);
    }

    #region Authorization

    [Fact]
    public void Class_ShouldHaveAuthorizeAttribute()
    {
        var attr = typeof(ClientRoleTypeController).GetCustomAttribute<AuthorizeAttribute>(inherit: true);
        Assert.NotNull(attr);
    }

    [Fact]
    public void GetAsync_ShouldHaveAuthorizeAttribute()
    {
        var mi = typeof(ClientRoleTypeController).GetMethod(nameof(ClientRoleTypeController.GetAsync));
        Assert.NotNull(mi);
        var classAttr = typeof(ClientRoleTypeController).GetCustomAttribute<AuthorizeAttribute>(inherit: true);
        var methodHas = mi!.GetCustomAttributes<AuthorizeAttribute>(inherit: true).Any();
        Assert.True(classAttr is not null || methodHas);
    }

    [Fact]
    public void PostAsync_ShouldHaveAuthorizeAttribute()
    {
        var mi = typeof(ClientRoleTypeController).GetMethod(nameof(ClientRoleTypeController.PostAsync));
        Assert.NotNull(mi);
        var classAttr = typeof(ClientRoleTypeController).GetCustomAttribute<AuthorizeAttribute>(inherit: true);
        var methodHas = mi!.GetCustomAttributes<AuthorizeAttribute>(inherit: true).Any();
        Assert.True(classAttr is not null || methodHas);
    }

    [Fact]
    public void PutAsync_ShouldHaveAuthorizeAttribute()
    {
        var mi = typeof(ClientRoleTypeController).GetMethod(nameof(ClientRoleTypeController.PutAsync));
        Assert.NotNull(mi);
        var classAttr = typeof(ClientRoleTypeController).GetCustomAttribute<AuthorizeAttribute>(inherit: true);
        var methodHas = mi!.GetCustomAttributes<AuthorizeAttribute>(inherit: true).Any();
        Assert.True(classAttr is not null || methodHas);
    }

    [Fact]
    public void DeleteAsync_ShouldHaveAuthorizeAttribute()
    {
        var mi = typeof(ClientRoleTypeController).GetMethod(nameof(ClientRoleTypeController.DeleteAsync));
        Assert.NotNull(mi);
        var classAttr = typeof(ClientRoleTypeController).GetCustomAttribute<AuthorizeAttribute>(inherit: true);
        var methodHas = mi!.GetCustomAttributes<AuthorizeAttribute>(inherit: true).Any();
        Assert.True(classAttr is not null || methodHas);
    }

    #endregion Authorization
}