using FluentValidation;
using FluentValidation.Results;
using KonaAI.Master.API.Controllers.Master.App;
using KonaAI.Master.Business.Master.App.Logic.Interface;
using KonaAI.Master.Model.Master.App.SaveModel;
using KonaAI.Master.Model.Master.App.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.Extensions.Logging;
using Moq;

namespace KonaAI.Master.Test.Integration.Controllers.App;

/// <summary>
/// Integration tests for QuestionBankController
/// </summary>
public class QuestionBankControllerTests
{
    private static QuestionBankController CreateController(
        out Mock<IQuestionBankBusiness> business,
        out Mock<IValidator<QuestionBankCreateModel>> createValidator)
    {
        var logger = new Mock<ILogger<QuestionBankController>>();
        business = new Mock<IQuestionBankBusiness>();
        createValidator = new Mock<IValidator<QuestionBankCreateModel>>();

        return new QuestionBankController(logger.Object, createValidator.Object, business.Object);
    }

    [Fact]
    public async Task GetAsync_ReturnsOkResult()
    {
        // Arrange: business returns data
        var controller = CreateController(out var business, out var createV);
        var data = new List<QuestionBankViewModel>
        {
            new() { RowId = Guid.NewGuid(), Description = "Test Bank 1" },
            new() { RowId = Guid.NewGuid(), Description = "Test Bank 2" }
        }.AsQueryable();

        business.Setup(b => b.GetAsync()).ReturnsAsync(data);

        // Act
        var result = await controller.GetAsync();

        // Assert
        Assert.IsType<OkObjectResult>(result);
        var okResult = (OkObjectResult)result;
        Assert.Equal(data, okResult.Value);
        business.Verify(b => b.GetAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAsync_ThrowsException_ReturnsInternalServerError()
    {
        // Arrange: business throws
        var controller = CreateController(out var business, out var createV);
        business.Setup(b => b.GetAsync()).ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await controller.GetAsync();

        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = (ObjectResult)result;
        Assert.Equal(500, objectResult.StatusCode);
        Assert.Equal("Database error", objectResult.Value);
    }

    [Fact]
    public async Task PostAsync_ValidModel_ReturnsCreated()
    {
        // Arrange: validation passes, business succeeds
        var controller = CreateController(out var business, out var createV);
        var model = new QuestionBankCreateModel { Description = "Test Bank" };
        var validationResult = new ValidationResult();
        createV.Setup(v => v.ValidateAsync(model, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);
        business.Setup(b => b.CreateAsync(model)).ReturnsAsync(1);

        // Act
        var result = await controller.PostAsync(model);

        // Assert
        Assert.IsType<CreatedResult>(result);
        business.Verify(b => b.CreateAsync(model), Times.Once);
    }

    [Fact]
    public async Task PostAsync_InvalidModel_ReturnsBadRequest()
    {
        // Arrange: validation fails
        var controller = CreateController(out var business, out var createV);
        var model = new QuestionBankCreateModel { Description = "" };
        var validationResult = new ValidationResult(new List<ValidationFailure>
        {
            new("Description", "Description is required")
        });
        createV.Setup(v => v.ValidateAsync(model, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await controller.PostAsync(model);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        var badRequest = (BadRequestObjectResult)result;
        Assert.Equal(validationResult.Errors, badRequest.Value);
        business.Verify(b => b.CreateAsync(It.IsAny<QuestionBankCreateModel>()), Times.Never);
    }

    [Fact]
    public async Task PostAsync_ThrowsException_ReturnsInternalServerError()
    {
        // Arrange: business throws
        var controller = CreateController(out var business, out var createV);
        var model = new QuestionBankCreateModel { Description = "Test Bank" };
        var validationResult = new ValidationResult();
        createV.Setup(v => v.ValidateAsync(model, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);
        business.Setup(b => b.CreateAsync(model)).ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await controller.PostAsync(model);

        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = (ObjectResult)result;
        Assert.Equal(500, objectResult.StatusCode);
        Assert.Equal("Database error", objectResult.Value);
    }

    [Fact]
    public async Task DeleteAsync_ValidRowId_ReturnsNoContent()
    {
        // Arrange: business succeeds
        var controller = CreateController(out var business, out var createV);
        var rowId = Guid.NewGuid();
        business.Setup(b => b.DeleteAsync(rowId)).ReturnsAsync(1);

        // Act
        var result = await controller.DeleteAsync(rowId);

        // Assert
        Assert.IsType<NoContentResult>(result);
        business.Verify(b => b.DeleteAsync(rowId), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_NotFound_ReturnsNotFound()
    {
        // Arrange: business throws KeyNotFoundException
        var controller = CreateController(out var business, out var createV);
        var rowId = Guid.NewGuid();
        business.Setup(b => b.DeleteAsync(rowId))
            .ThrowsAsync(new KeyNotFoundException($"Client with id {rowId} not found"));

        // Act
        var result = await controller.DeleteAsync(rowId);

        // Assert
        Assert.IsType<NotFoundODataResult>(result);
        // OData results don't expose Value property like standard MVC results
    }

    [Fact]
    public async Task DeleteAsync_ThrowsException_ReturnsInternalServerError()
    {
        // Arrange: business throws
        var controller = CreateController(out var business, out var createV);
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