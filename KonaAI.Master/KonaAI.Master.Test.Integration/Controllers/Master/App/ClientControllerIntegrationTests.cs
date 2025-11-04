using FluentValidation;
using KonaAI.Master.API.Controllers.Master.App;
using KonaAI.Master.Business.Master.App.Logic.Interface;
using KonaAI.Master.Model.Master.App.SaveModel;
using KonaAI.Master.Model.Master.App.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.Extensions.Logging;
using Moq;

namespace KonaAI.Master.Test.Integration.Controllers.Master.App;

/// <summary>
/// Comprehensive integration tests for <see cref="ClientController"/>.
/// Tests all CRUD operations with real controller execution.
/// </summary>
public class ClientControllerIntegrationTests
{
    private static ClientController CreateController(out Mock<IClientBusiness> business, out Mock<IValidator<ClientCreateModel>> createValidator, out Mock<IValidator<ClientUpdateModel>> updateValidator)
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
        // Arrange
        var controller = CreateController(out var business, out var createValidator, out var updateValidator);
        var data = new List<ClientViewModel>
        {
            new() { RowId = Guid.NewGuid(), Name = "Test Client 1", DisplayName = "Test Client 1", ClientCode = "TC001" },
            new() { RowId = Guid.NewGuid(), Name = "Test Client 2", DisplayName = "Test Client 2", ClientCode = "TC002" }
        }.AsQueryable();

        business.Setup(b => b.GetAsync()).Returns(Task.FromResult(data));

        // Act
        var result = await controller.GetAsync();

        // Assert
        Assert.IsType<OkObjectResult>(result);
        var okResult = (OkObjectResult)result;
        Assert.NotNull(okResult.Value);
        business.Verify(b => b.GetAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAsync_BusinessThrows_Returns500()
    {
        // Arrange
        var controller = CreateController(out var business, out var createValidator, out var updateValidator);
        business.Setup(b => b.GetAsync()).ThrowsAsync(new Exception("Database connection failed"));

        // Act
        var result = await controller.GetAsync();

        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = (ObjectResult)result;
        Assert.Equal(500, objectResult.StatusCode);
        Assert.Equal("Database connection failed", objectResult.Value);
    }

    [Fact]
    public async Task GetByRowIdAsync_ItemExists_ReturnsOk()
    {
        // Arrange
        var controller = CreateController(out var business, out var createValidator, out var updateValidator);
        var rowId = Guid.NewGuid();
        var data = new ClientViewModel { RowId = rowId, Name = "Test Client", DisplayName = "Test Client", ClientCode = "TC001" };

        business.Setup(b => b.GetByRowIdAsync(rowId)).Returns(Task.FromResult(data));

        // Act
        var result = await controller.GetByRowIdAsync(rowId);

        // Assert
        Assert.IsType<OkObjectResult>(result);
        var okResult = (OkObjectResult)result;
        Assert.Equal(200, okResult.StatusCode);
        Assert.NotNull(okResult.Value);
        business.Verify(b => b.GetByRowIdAsync(rowId), Times.Once);
    }

    [Fact]
    public async Task GetByRowIdAsync_ItemNotFound_Returns404()
    {
        // Arrange
        var controller = CreateController(out var business, out var createValidator, out var updateValidator);
        var rowId = Guid.NewGuid();
        business.Setup(b => b.GetByRowIdAsync(rowId)).Returns(Task.FromResult((ClientViewModel)null!));

        // Act
        var result = await controller.GetByRowIdAsync(rowId);

        // Assert
        Assert.IsType<OkObjectResult>(result);
        var okResult = (OkObjectResult)result;
        Assert.Equal(200, okResult.StatusCode);
        business.Verify(b => b.GetByRowIdAsync(rowId), Times.Once);
    }

    [Fact]
    public async Task PostAsync_ValidModel_Returns201Created()
    {
        // Arrange
        var controller = CreateController(out var business, out var createValidator, out var updateValidator);
        var createModel = new ClientCreateModel
        {
            Name = "New Client",
            DisplayName = "New Client Display",
            ClientCode = "NC001"
        };

        createValidator.Setup(v => v.ValidateAsync(createModel, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        business.Setup(b => b.CreateAsync(createModel)).ReturnsAsync(1);

        // Act
        var result = await controller.PostAsync(createModel);

        // Assert
        Assert.IsType<CreatedResult>(result);
        var createdResult = (CreatedResult)result;
        Assert.Equal(201, createdResult.StatusCode);
        business.Verify(b => b.CreateAsync(createModel), Times.Once);
    }

    [Fact]
    public async Task PostAsync_BusinessThrows_Returns500()
    {
        // Arrange
        var controller = CreateController(out var business, out var createValidator, out var updateValidator);
        var createModel = new ClientCreateModel
        {
            Name = "New Client",
            DisplayName = "New Client Display",
            ClientCode = "NC001"
        };

        createValidator.Setup(v => v.ValidateAsync(createModel, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        business.Setup(b => b.CreateAsync(createModel)).ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await controller.PostAsync(createModel);

        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = (ObjectResult)result;
        Assert.Equal(500, objectResult.StatusCode);
        Assert.Equal("Database error", objectResult.Value);
    }

    [Fact]
    public async Task PutAsync_ValidModel_Returns200Ok()
    {
        // Arrange
        var controller = CreateController(out var business, out var createValidator, out var updateValidator);
        var rowId = Guid.NewGuid();
        var updateModel = new ClientUpdateModel
        {
            RowId = rowId,
            Name = "Updated Client",
            DisplayName = "Updated Client Display",
            ClientCode = "UC001"
        };

        updateValidator.Setup(v => v.ValidateAsync(updateModel, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        business.Setup(b => b.UpdateAsync(rowId, updateModel)).ReturnsAsync(1);

        // Act
        var result = await controller.PutAsync(rowId, updateModel);

        // Assert
        Assert.IsType<NoContentResult>(result);
        var noContentResult = (NoContentResult)result;
        Assert.Equal(204, noContentResult.StatusCode);
        business.Verify(b => b.UpdateAsync(rowId, updateModel), Times.Once);
    }

    [Fact]
    public async Task PutAsync_NotFound_Returns404()
    {
        // Arrange
        var controller = CreateController(out var business, out var createValidator, out var updateValidator);
        var rowId = Guid.NewGuid();
        var updateModel = new ClientUpdateModel
        {
            RowId = rowId,
            Name = "Updated Client",
            DisplayName = "Updated Client Display",
            ClientCode = "UC001"
        };

        updateValidator.Setup(v => v.ValidateAsync(updateModel, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        business.Setup(b => b.UpdateAsync(rowId, updateModel)).ThrowsAsync(new KeyNotFoundException($"Client with id {rowId} not found."));

        // Act
        var result = await controller.PutAsync(rowId, updateModel);

        // Assert
        Assert.IsType<NotFoundODataResult>(result);
        var notFoundResult = (NotFoundODataResult)result;
        Assert.Equal(404, notFoundResult.StatusCode);
        business.Verify(b => b.UpdateAsync(rowId, updateModel), Times.Once);
    }

    [Fact]
    public async Task PutAsync_BusinessThrows_Returns500()
    {
        // Arrange
        var controller = CreateController(out var business, out var createValidator, out var updateValidator);
        var rowId = Guid.NewGuid();
        var updateModel = new ClientUpdateModel
        {
            RowId = rowId,
            Name = "Updated Client",
            DisplayName = "Updated Client Display",
            ClientCode = "UC001"
        };

        updateValidator.Setup(v => v.ValidateAsync(updateModel, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        business.Setup(b => b.UpdateAsync(rowId, updateModel)).ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await controller.PutAsync(rowId, updateModel);

        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = (ObjectResult)result;
        Assert.Equal(500, objectResult.StatusCode);
        Assert.Equal("Database error", objectResult.Value);
    }

    [Fact]
    public async Task DeleteAsync_ItemExists_Returns200Ok()
    {
        // Arrange
        var controller = CreateController(out var business, out var createValidator, out var updateValidator);
        var rowId = Guid.NewGuid();

        business.Setup(b => b.DeleteAsync(rowId)).ReturnsAsync(1);

        // Act
        var result = await controller.DeleteAsync(rowId);

        // Assert
        Assert.IsType<NoContentResult>(result);
        var noContentResult = (NoContentResult)result;
        Assert.Equal(204, noContentResult.StatusCode);
        business.Verify(b => b.DeleteAsync(rowId), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_NotFound_Returns404()
    {
        // Arrange
        var controller = CreateController(out var business, out var createValidator, out var updateValidator);
        var rowId = Guid.NewGuid();

        business.Setup(b => b.DeleteAsync(rowId)).ReturnsAsync(0);

        // Act
        var result = await controller.DeleteAsync(rowId);

        // Assert
        Assert.IsType<NoContentResult>(result);
        var noContentResult = (NoContentResult)result;
        Assert.Equal(204, noContentResult.StatusCode);
        business.Verify(b => b.DeleteAsync(rowId), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_BusinessThrows_Returns500()
    {
        // Arrange
        var controller = CreateController(out var business, out var createValidator, out var updateValidator);
        var rowId = Guid.NewGuid();

        business.Setup(b => b.DeleteAsync(rowId)).ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await controller.DeleteAsync(rowId);

        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = (ObjectResult)result;
        Assert.Equal(500, objectResult.StatusCode);
        Assert.Equal("Database error", objectResult.Value);
    }
}