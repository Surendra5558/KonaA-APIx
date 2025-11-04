using FluentValidation;
using FluentValidation.Results;
using KonaAI.Master.API.Controllers.Master.App;
using KonaAI.Master.Business.Master.App.Logic.Interface;
using KonaAI.Master.Model.Master.App.SaveModel;
using KonaAI.Master.Model.Master.App.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace KonaAI.Master.Test.Integration.Controllers.Master.App;

/// <summary>
/// Integration-style tests for <see cref="KonaAI.Master.API.Controllers.Master.App.ClientController"/>.
/// Covers:
/// - GET list (200) and GET by id (200 or 404)
/// - POST create (201) and 400 on validation failures
/// - PUT update (204) and 400 on validation failures
/// - DELETE (204) and 404 when resource is missing
/// </summary>
public class ClientControllerTests
{
    // Overview:
    // - Exercises ClientController actions with mocked business/validators.
    // - Verifies happy paths (200/201/204) and negative paths (400/404).
    // - We don't spin up the full host; we set expectations on mocks and assert IActionResult.
    private static ClientController CreateController(
        out Mock<IClientBusiness> business,
        out Mock<IValidator<ClientCreateModel>> createValidator,
        out Mock<IValidator<ClientUpdateModel>> updateValidator)
    {
        var logger = new Mock<ILogger<ClientController>>();
        business = new Mock<IClientBusiness>(MockBehavior.Strict);
        createValidator = new Mock<IValidator<ClientCreateModel>>(MockBehavior.Strict);
        updateValidator = new Mock<IValidator<ClientUpdateModel>>(MockBehavior.Strict);
        return new ClientController(logger.Object, business.Object, createValidator.Object, updateValidator.Object);
    }

    [Fact]
    public async Task GetAsync_ReturnsOk_WithQueryable()
    {
        // Arrange: business returns a queryable list
        var controller = CreateController(out var business, out var createV, out var updateV);
        var data = new List<ClientViewModel> { new() }.AsQueryable();
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
        // Arrange: business returns a single item for the given id
        var controller = CreateController(out var business, out var createV, out var updateV);
        var id = Guid.NewGuid();
        var item = new ClientViewModel { RowId = id };
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
        // Arrange: validator passes, business returns created count
        var controller = CreateController(out var business, out var createV, out var updateV);
        var model = new ClientCreateModel();
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
        // Arrange: validator returns failures; business must not be called
        var controller = CreateController(out var business, out var createV, out var updateV);
        var model = new ClientCreateModel();
        var failures = new List<ValidationFailure> { new("Name", "required") };
        createV.Setup(v => v.ValidateAsync(model, It.IsAny<CancellationToken>()))
               .ReturnsAsync(new ValidationResult(failures));

        // Act
        var result = await controller.PostAsync(model);
        // Assert
        var bad = Assert.IsType<BadRequestObjectResult>(result);
        var errors = Assert.IsAssignableFrom<IEnumerable<ValidationFailure>>(bad.Value);
        Assert.Single(errors);
        business.Verify(b => b.CreateAsync(It.IsAny<ClientCreateModel>()), Times.Never);
    }

    [Fact]
    public async Task PutAsync_ReturnsNoContent_OnValidInput()
    {
        // Arrange: validator passes; business updates successfully
        var controller = CreateController(out var business, out var createV, out var updateV);
        var id = Guid.NewGuid();
        var model = new ClientUpdateModel();
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
        // Arrange: validator fails; business must not be called
        var controller = CreateController(out var business, out var createV, out var updateV);
        var id = Guid.NewGuid();
        var model = new ClientUpdateModel();
        var failures = new List<ValidationFailure> { new("Name", "required") };
        updateV.Setup(v => v.ValidateAsync(model, It.IsAny<CancellationToken>()))
               .ReturnsAsync(new ValidationResult(failures));

        // Act
        var result = await controller.PutAsync(id, model);
        // Assert
        var bad = Assert.IsType<BadRequestObjectResult>(result);
        var errors = Assert.IsAssignableFrom<IEnumerable<ValidationFailure>>(bad.Value);
        Assert.Single(errors);
        business.Verify(b => b.UpdateAsync(It.IsAny<Guid>(), It.IsAny<ClientUpdateModel>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsNoContent_OnSuccess()
    {
        // Arrange: business deletes successfully
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
        // Arrange: simulate not found via KeyNotFoundException
        var controller = CreateController(out var business, out var createV, out var updateV);
        var id = Guid.NewGuid();
        business.Setup(b => b.GetByRowIdAsync(id)).ThrowsAsync(new KeyNotFoundException());

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