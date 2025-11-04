using FluentValidation;
using KonaAI.Master.API.Controllers.Tenant.MetaData;
using KonaAI.Master.Business.Tenant.MetaData.Logic.Interface;
using KonaAI.Master.Model.Common;
using KonaAI.Master.Model.Tenant.Client.SaveModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace KonaAI.Master.Test.Integration.Controllers.Tenant.MetaData;

/// <summary>
/// Integration-style tests for <see cref="KonaAI.Master.API.Controllers.Tenant.MetaData.ClientRoleTypeController"/>.
/// Covers:
/// - GET list (200) and GET by RowId (200/404)
/// - POST with validation success/failure (201/400)
/// - PUT with validation success/failure and not found (204/400/404)
/// - DELETE with success and not found (204/404)
/// - Error handling (500) for server errors
/// </summary>
public class ClientRoleTypeControllerTests
{
    // Overview:
    // - Exercises ClientRoleTypeController actions with mocked business dependencies.
    // - Verifies happy paths (200/201/204) and error scenarios (400/404/500).
    // - This is a CRUD controller, so all operations are tested.
    private static ClientRoleTypeController CreateController(
        out Mock<IClientRoleTypeBusiness> business,
        out Mock<IValidator<ClientRoleTypeCreateModel>> createValidator,
        out Mock<IValidator<ClientRoleTypeUpdateModel>> updateValidator)
    {
        var logger = new Mock<ILogger<ClientRoleTypeController>>();
        business = new Mock<IClientRoleTypeBusiness>(MockBehavior.Strict);
        createValidator = new Mock<IValidator<ClientRoleTypeCreateModel>>(MockBehavior.Strict);
        updateValidator = new Mock<IValidator<ClientRoleTypeUpdateModel>>(MockBehavior.Strict);
        return new ClientRoleTypeController(logger.Object, business.Object, createValidator.Object, updateValidator.Object);
    }

    [Fact]
    public async Task GetAsync_ReturnsOk_WithQueryable()
    {
        // Arrange: business returns a queryable list
        var controller = CreateController(out var business, out var createValidator, out var updateValidator);
        var data = new List<MetaDataViewModel>
        {
            new() { RowId = Guid.NewGuid(), Name = "Role 1", Description = "First role" }
        }.AsQueryable();
        business.Setup(b => b.GetAsync()).ReturnsAsync(data);

        // Act
        var result = await controller.GetAsync();

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Same(data, ok.Value);
        business.VerifyAll();
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsOk_WhenFound()
    {
        // Arrange: business returns data with matching RowId
        var controller = CreateController(out var business, out var createValidator, out var updateValidator);
        var rowId = Guid.NewGuid();
        var role = new MetaDataViewModel { RowId = rowId, Name = "Role 1", Description = "First role" };
        business.Setup(b => b.GetByRowIdAsync(rowId)).ReturnsAsync(role);

        // Act
        var result = await controller.GetByIdAsync(rowId);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Same(role, ok.Value);
        business.VerifyAll();
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNotFound_WhenMissing()
    {
        // Arrange: business returns null
        var controller = CreateController(out var business, out var createValidator, out var updateValidator);
        var rowId = Guid.NewGuid();
        business.Setup(b => b.GetByRowIdAsync(rowId)).ReturnsAsync((MetaDataViewModel)null!);

        // Act
        var result = await controller.GetByIdAsync(rowId);

        // Assert
        var notFound = Assert.IsType<Microsoft.AspNetCore.OData.Results.NotFoundODataResult>(result);
        business.VerifyAll();
    }

    [Fact]
    public async Task PostAsync_ReturnsCreated_WhenValid()
    {
        // Arrange: valid model and successful creation
        var controller = CreateController(out var business, out var createValidator, out var updateValidator);
        var model = new ClientRoleTypeCreateModel { Name = "New Role", Description = "New role description" };
        var validationResult = new FluentValidation.Results.ValidationResult();
        var createdId = 1;

        createValidator.Setup(v => v.ValidateAsync(model, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);
        business.Setup(b => b.CreateAsync(model)).ReturnsAsync(createdId);

        // Act
        var result = await controller.PostAsync(model);

        // Assert
        var created = Assert.IsType<CreatedResult>(result);
        Assert.Equal(201, created.StatusCode);
        business.VerifyAll();
        createValidator.VerifyAll();
    }

    [Fact]
    public async Task PostAsync_ReturnsBadRequest_WhenValidationFails()
    {
        // Arrange: invalid model
        var controller = CreateController(out var business, out var createValidator, out var updateValidator);
        var model = new ClientRoleTypeCreateModel { Name = "", Description = "Invalid role" };
        var validationResult = new FluentValidation.Results.ValidationResult();
        validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure("Name", "Name is required"));

        createValidator.Setup(v => v.ValidateAsync(model, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await controller.PostAsync(model);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequest.StatusCode);
        createValidator.VerifyAll();
    }

    [Fact]
    public async Task PutAsync_ReturnsNoContent_WhenValid()
    {
        // Arrange: valid model and successful update
        var controller = CreateController(out var business, out var createValidator, out var updateValidator);
        var rowId = Guid.NewGuid();
        var model = new ClientRoleTypeUpdateModel { Name = "Updated Role", Description = "Updated role description" };
        var validationResult = new FluentValidation.Results.ValidationResult();
        var updatedId = 1;

        updateValidator.Setup(v => v.ValidateAsync(model, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);
        business.Setup(b => b.UpdateAsync(rowId, model)).ReturnsAsync(updatedId);

        // Act
        var result = await controller.PutAsync(rowId, model);

        // Assert
        var noContent = Assert.IsType<NoContentResult>(result);
        Assert.Equal(204, noContent.StatusCode);
        business.VerifyAll();
        updateValidator.VerifyAll();
    }

    [Fact]
    public async Task PutAsync_ReturnsBadRequest_WhenValidationFails()
    {
        // Arrange: invalid model
        var controller = CreateController(out var business, out var createValidator, out var updateValidator);
        var rowId = Guid.NewGuid();
        var model = new ClientRoleTypeUpdateModel { Name = "", Description = "Invalid role" };
        var validationResult = new FluentValidation.Results.ValidationResult();
        validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure("Name", "Name is required"));

        updateValidator.Setup(v => v.ValidateAsync(model, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await controller.PutAsync(rowId, model);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequest.StatusCode);
        updateValidator.VerifyAll();
    }

    [Fact]
    public async Task PutAsync_ReturnsNotFound_WhenRoleNotFound()
    {
        // Arrange: role not found
        var controller = CreateController(out var business, out var createValidator, out var updateValidator);
        var rowId = Guid.NewGuid();
        var model = new ClientRoleTypeUpdateModel { Name = "Updated Role", Description = "Updated role description" };
        var validationResult = new FluentValidation.Results.ValidationResult();

        updateValidator.Setup(v => v.ValidateAsync(model, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);
        business.Setup(b => b.UpdateAsync(rowId, model)).ThrowsAsync(new KeyNotFoundException($"Role with id {rowId} not found"));

        // Act
        var result = await controller.PutAsync(rowId, model);

        // Assert
        var notFound = Assert.IsType<Microsoft.AspNetCore.OData.Results.NotFoundODataResult>(result);
        business.VerifyAll();
        updateValidator.VerifyAll();
    }

    [Fact]
    public async Task DeleteAsync_ReturnsNoContent_WhenSuccessful()
    {
        // Arrange: successful deletion
        var controller = CreateController(out var business, out var createValidator, out var updateValidator);
        var rowId = Guid.NewGuid();
        var deletedId = 1;

        business.Setup(b => b.DeleteAsync(rowId)).ReturnsAsync(deletedId);

        // Act
        var result = await controller.DeleteAsync(rowId);

        // Assert
        var noContent = Assert.IsType<NoContentResult>(result);
        Assert.Equal(204, noContent.StatusCode);
        business.VerifyAll();
    }

    [Fact]
    public async Task DeleteAsync_ReturnsNotFound_WhenRoleNotFound()
    {
        // Arrange: role not found
        var controller = CreateController(out var business, out var createValidator, out var updateValidator);
        var rowId = Guid.NewGuid();

        business.Setup(b => b.DeleteAsync(rowId)).ThrowsAsync(new KeyNotFoundException($"Role with id {rowId} not found"));

        // Act
        var result = await controller.DeleteAsync(rowId);

        // Assert
        var notFound = Assert.IsType<Microsoft.AspNetCore.OData.Results.NotFoundODataResult>(result);
        business.VerifyAll();
    }

    [Fact]
    public async Task GetAsync_ReturnsServerError_WhenBusinessThrows()
    {
        // Arrange: business throws an exception
        var controller = CreateController(out var business, out var createValidator, out var updateValidator);
        business.Setup(b => b.GetAsync()).ThrowsAsync(new Exception("Database connection failed"));

        // Act
        var result = await controller.GetAsync();

        // Assert
        var statusCode = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCode.StatusCode);
        Assert.Equal("Database connection failed", statusCode.Value);
        business.VerifyAll();
    }
}