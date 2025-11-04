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

public class ClientUserControllerUnitTests
{
    private readonly Mock<ILogger<ClientUserController>> _logger = new();
    private readonly Mock<IClientUserBusiness> _business = new();
    private readonly Mock<IValidator<ClientUserCreateModel>> _createValidator = new();
    private readonly Mock<IValidator<ClientUserUpdateModel>> _updateValidator = new();

    private ClientUserController CreateSut() =>
        new(_logger.Object, _business.Object, _createValidator.Object, _updateValidator.Object);

    [Fact]
    public async Task GetAsync_Returns200_WithData()
    {
        var data = new List<ClientUserViewModel>
        {
            new() { RowId = Guid.NewGuid(), UserName = "TestUser", Email = "test@test.com", FirstName = "Test", LastName = "User" }
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
        var data = new ClientUserViewModel { RowId = id, UserName = "TestUser" };

        _business.Setup(b => b.GetByRowIdAsync(id)).ReturnsAsync(data);

        var sut = CreateSut();
        var result = await sut.GetByRowIdAsync(id);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(ok.Value);
    }

    [Fact]
    public async Task GetByRowIdAsync_NotFound_Returns404()
    {
        _business.Setup(b => b.GetByRowIdAsync(It.IsAny<Guid>())).ReturnsAsync((ClientUserViewModel?)null);

        var sut = CreateSut();
        var result = await sut.GetByRowIdAsync(Guid.NewGuid());

        var notFound = Assert.IsType<NotFoundODataResult>(result);
        Assert.Equal(404, notFound.StatusCode);
    }

    [Fact]
    public async Task PostAsync_ValidModel_Returns201()
    {
        var model = new ClientUserCreateModel { UserName = "testuser", Email = "test@test.com", FirstName = "Test", LastName = "User" };
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
        var model = new ClientUserCreateModel();
        var validationResult = new ValidationResult(new[] { new ValidationFailure("UserId", "Required") });
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
        var model = new ClientUserUpdateModel { UserName = "updateduser", Email = "updated@test.com", FirstName = "Updated", LastName = "User" };
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
        var model = new ClientUserUpdateModel { UserName = "updateduser", Email = "updated@test.com", FirstName = "Updated", LastName = "User" };
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
    public void GetAsync_ShouldHaveAuthorizePolicy_ViewUsers()
    {
        var mi = typeof(ClientUserController).GetMethod(nameof(ClientUserController.GetAsync));
        Assert.NotNull(mi);
        var attrs = mi!.GetCustomAttributes<AuthorizeAttribute>(inherit: true);
        Assert.Contains(attrs, a => (a.Policy ?? string.Empty).Replace(" ", string.Empty).Contains("Permission:Navigation=Users;Action=View".Replace(" ", string.Empty)));
    }

    [Fact]
    public void PostAsync_ShouldHaveAuthorizePolicy_AddUsers()
    {
        var mi = typeof(ClientUserController).GetMethod(nameof(ClientUserController.PostAsync));
        Assert.NotNull(mi);
        var attrs = mi!.GetCustomAttributes<AuthorizeAttribute>(inherit: true);
        Assert.Contains(attrs, a => (a.Policy ?? string.Empty).Replace(" ", string.Empty).Contains("Permission:Navigation=Users;Action=Add".Replace(" ", string.Empty)));
    }

    [Fact]
    public void PutAsync_ShouldHaveAuthorizePolicy_EditUsers()
    {
        var mi = typeof(ClientUserController).GetMethod(nameof(ClientUserController.PutAsync));
        Assert.NotNull(mi);
        var attrs = mi!.GetCustomAttributes<AuthorizeAttribute>(inherit: true);
        var normalized = attrs.Select(x => (x.Policy ?? string.Empty).Replace(" ", string.Empty)).ToList();
        Assert.Contains(normalized, p => p.Contains("Permission:Navigation=Users;Action=Edit".Replace(" ", string.Empty)) || p.Contains("Permission:Navigation=UsersAction=Edit"));
    }

    [Fact]
    public void DeleteAsync_ShouldHaveAuthorizePolicy_DeleteUsers()
    {
        var mi = typeof(ClientUserController).GetMethod(nameof(ClientUserController.DeleteAsync));
        Assert.NotNull(mi);
        var attrs = mi!.GetCustomAttributes<AuthorizeAttribute>(inherit: true);
        Assert.Contains(attrs, a => (a.Policy ?? string.Empty).Replace(" ", string.Empty).Contains("Permission:Navigation=Users;Action=Delete".Replace(" ", string.Empty)));
    }

    #endregion Authorization
}