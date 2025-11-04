using FluentValidation;
using FluentValidation.Results;
using KonaAI.Master.API.Controllers.Tenant.Client;
using KonaAI.Master.Business.Tenant.Client.Logic.Interface;
using KonaAI.Master.Model.Tenant.Client.SaveModel;
using KonaAI.Master.Model.Tenant.Client.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace KonaAI.Master.Test.Integration.Controllers.Tenant.Client;

/// <summary>
/// Integration-style tests for <see cref="KonaAI.Master.API.Controllers.Tenant.Client.ClientUserController"/>.
/// Covers:
/// - GET list (200) and detail (200 or 404)
/// - POST create (201) and 400 on validation failures
/// - PUT update (204) and 400 on validation failures
/// - DELETE (204) and 404 when resource missing
/// </summary>
public class ClientUserControllerTests
{
    // Overview:
    // - Tests list/detail endpoints and write actions with validator and not-found branches.
    // - Uses mock business and validators; asserts IActionResult types and key payload aspects.
    private static ClientUserController CreateController(
        out Mock<IClientUserBusiness> business,
        out Mock<IValidator<ClientUserCreateModel>> createValidator,
        out Mock<IValidator<ClientUserUpdateModel>> updateValidator)
    {
        var logger = new Mock<ILogger<ClientUserController>>();
        business = new Mock<IClientUserBusiness>(MockBehavior.Strict);
        createValidator = new Mock<IValidator<ClientUserCreateModel>>(MockBehavior.Strict);
        updateValidator = new Mock<IValidator<ClientUserUpdateModel>>(MockBehavior.Strict);
        return new ClientUserController(logger.Object, business.Object, createValidator.Object, updateValidator.Object);
    }

    [Fact]
    public async Task GetAsync_ReturnsOk()
    {
        // Arrange
        var controller = CreateController(out var business, out var createV, out var updateV);
        var data = new List<ClientUserViewModel> { new() }.AsQueryable();
        business.Setup(b => b.GetAsync()).ReturnsAsync(data);

        // Act
        var result = await controller.GetAsync();
        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Same(data, ok.Value);
        business.VerifyAll();
    }

    [Fact]
    public async Task GetByRowIdAsync_ReturnsOk_WhenFound()
    {
        // Arrange
        var controller = CreateController(out var business, out var createV, out var updateV);
        var id = Guid.NewGuid();
        var item = new ClientUserViewModel { RowId = id };
        business.Setup(b => b.GetByRowIdAsync(id)).ReturnsAsync(item);

        // Act
        var result = await controller.GetByRowIdAsync(id);
        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(item, ok.Value);
        business.VerifyAll();
    }

    [Fact]
    public async Task PostAsync_ReturnsCreated_OnValidInput()
    {
        // Arrange: validator passes; business create succeeds
        var controller = CreateController(out var business, out var createV, out var updateV);
        var model = new ClientUserCreateModel();
        createV.Setup(v => v.ValidateAsync(model, It.IsAny<CancellationToken>()))
               .ReturnsAsync(new ValidationResult());
        business.Setup(b => b.CreateAsync(model)).ReturnsAsync(1);

        // Act
        var result = await controller.PostAsync(model);
        // Assert
        Assert.IsType<CreatedResult>(result);
        business.VerifyAll();
    }

    [Fact]
    public async Task PostAsync_ReturnsBadRequest_WhenValidationFails()
    {
        // Arrange: validator fails; business must not be invoked
        var controller = CreateController(out var business, out var createV, out var updateV);
        var model = new ClientUserCreateModel();
        var failures = new List<ValidationFailure> { new("UserName", "required") };
        createV.Setup(v => v.ValidateAsync(model, It.IsAny<CancellationToken>()))
               .ReturnsAsync(new ValidationResult(failures));

        // Act
        var result = await controller.PostAsync(model);
        // Assert
        var bad = Assert.IsType<BadRequestObjectResult>(result);
        var errors = Assert.IsAssignableFrom<IEnumerable<ValidationFailure>>(bad.Value);
        Assert.Single(errors);
        business.Verify(b => b.CreateAsync(It.IsAny<ClientUserCreateModel>()), Times.Never);
    }

    [Fact]
    public async Task PutAsync_ReturnsNoContent_OnValidInput()
    {
        // Arrange: validator passes; business updates successfully
        var controller = CreateController(out var business, out var createV, out var updateV);
        var id = Guid.NewGuid();
        var model = new ClientUserUpdateModel();
        updateV.Setup(v => v.ValidateAsync(model, It.IsAny<CancellationToken>()))
               .ReturnsAsync(new ValidationResult());
        business.Setup(b => b.UpdateAsync(id, model)).ReturnsAsync(1);

        // Act
        var result = await controller.PutAsync(id, model);
        // Assert
        Assert.IsType<NoContentResult>(result);
        business.VerifyAll();
    }

    [Fact]
    public async Task PutAsync_ReturnsBadRequest_WhenValidationFails()
    {
        // Arrange: validator fails; business must not be invoked
        var controller = CreateController(out var business, out var createV, out var updateV);
        var id = Guid.NewGuid();
        var model = new ClientUserUpdateModel();
        var failures = new List<ValidationFailure> { new("FirstName", "required") };
        updateV.Setup(v => v.ValidateAsync(model, It.IsAny<CancellationToken>()))
               .ReturnsAsync(new ValidationResult(failures));

        // Act
        var result = await controller.PutAsync(id, model);
        // Assert
        var bad = Assert.IsType<BadRequestObjectResult>(result);
        var errors = Assert.IsAssignableFrom<IEnumerable<ValidationFailure>>(bad.Value);
        Assert.Single(errors);
        business.Verify(b => b.UpdateAsync(It.IsAny<Guid>(), It.IsAny<ClientUserUpdateModel>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsNoContent_OnSuccess()
    {
        // Arrange
        var controller = CreateController(out var business, out var createV, out var updateV);
        var id = Guid.NewGuid();
        business.Setup(b => b.DeleteAsync(id)).ReturnsAsync(1);

        // Act
        var result = await controller.DeleteAsync(id);
        // Assert
        Assert.IsType<NoContentResult>(result);
        business.VerifyAll();
    }

    [Fact]
    public async Task GetByRowIdAsync_ReturnsNotFound_WhenMissing()
    {
        // Arrange: business returns null -> controller should return 404
        var controller = CreateController(out var business, out var createV, out var updateV);
        var id = Guid.NewGuid();
        business.Setup(b => b.GetByRowIdAsync(id)).ReturnsAsync((ClientUserViewModel?)null);

        // Act
        var result = await controller.GetByRowIdAsync(id);
        // Assert
        Assert.IsType<Microsoft.AspNetCore.OData.Results.NotFoundODataResult>(result);
        business.VerifyAll();
    }

    [Fact]
    public async Task DeleteAsync_ReturnsNotFound_WhenMissing()
    {
        // Arrange: simulate not found during delete
        var controller = CreateController(out var business, out var createV, out var updateV);
        var id = Guid.NewGuid();
        business.Setup(b => b.DeleteAsync(id)).ThrowsAsync(new KeyNotFoundException());

        // Act
        var result = await controller.DeleteAsync(id);
        // Assert
        Assert.IsType<Microsoft.AspNetCore.OData.Results.NotFoundODataResult>(result);
        business.VerifyAll();
    }
}