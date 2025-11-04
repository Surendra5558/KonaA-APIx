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

namespace KonaAI.Master.Test.Unit.Controllers.App;

public class QuestionBankControllerUnitTests
{
    private readonly Mock<ILogger<QuestionBankController>> _logger = new();
    private readonly Mock<IValidator<QuestionBankCreateModel>> _createValidator = new();
    private readonly Mock<IQuestionBankBusiness> _questionBankBusiness = new();
    private readonly QuestionBankController _controller;

    public QuestionBankControllerUnitTests()
    {
        _controller = new QuestionBankController(
            _logger.Object,
            _createValidator.Object,
            _questionBankBusiness.Object);
    }

    [Fact]
    public async Task GetAsync_ReturnsOkResult()
    {
        // Arrange
        var questionBanks = new List<QuestionBankViewModel>
        {
            new() { RowId = Guid.NewGuid(), Description = "Test Bank 1" },
            new() { RowId = Guid.NewGuid(), Description = "Test Bank 2" }
        }.AsQueryable();

        _questionBankBusiness.Setup(b => b.GetAsync()).ReturnsAsync(questionBanks);

        // Act
        var result = await _controller.GetAsync();

        // Assert
        Assert.IsType<OkObjectResult>(result);
        var okResult = (OkObjectResult)result;
        Assert.Equal(questionBanks, okResult.Value);
        _questionBankBusiness.Verify(b => b.GetAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAsync_ThrowsException_ReturnsInternalServerError()
    {
        // Arrange
        _questionBankBusiness.Setup(b => b.GetAsync()).ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.GetAsync();

        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = (ObjectResult)result;
        Assert.Equal(500, objectResult.StatusCode);
        Assert.Equal("Database error", objectResult.Value);
    }

    [Fact]
    public async Task PostAsync_ValidModel_ReturnsCreated()
    {
        // Arrange
        var model = new QuestionBankCreateModel
        {
            Description = "Test Bank",
            IsMandatory = true
        };

        var validationResult = new ValidationResult();
        _createValidator.Setup(v => v.ValidateAsync(model, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);
        _questionBankBusiness.Setup(b => b.CreateAsync(model)).ReturnsAsync(1);

        // Act
        var result = await _controller.PostAsync(model);

        // Assert
        Assert.IsType<CreatedResult>(result);
        _createValidator.Verify(v => v.ValidateAsync(model, It.IsAny<CancellationToken>()), Times.Once);
        _questionBankBusiness.Verify(b => b.CreateAsync(model), Times.Once);
    }

    [Fact]
    public async Task PostAsync_InvalidModel_ReturnsBadRequest()
    {
        // Arrange
        var model = new QuestionBankCreateModel { Description = "" };
        var validationResult = new ValidationResult(new List<ValidationFailure>
        {
            new("Description", "Description is required")
        });

        _createValidator.Setup(v => v.ValidateAsync(model, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _controller.PostAsync(model);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        var badRequestResult = (BadRequestObjectResult)result;
        Assert.Equal(validationResult.Errors, badRequestResult.Value);
        _questionBankBusiness.Verify(b => b.CreateAsync(It.IsAny<QuestionBankCreateModel>()), Times.Never);
    }

    [Fact]
    public async Task PostAsync_ThrowsException_ReturnsInternalServerError()
    {
        // Arrange
        var model = new QuestionBankCreateModel { Description = "Test Bank" };
        var validationResult = new ValidationResult();

        _createValidator.Setup(v => v.ValidateAsync(model, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);
        _questionBankBusiness.Setup(b => b.CreateAsync(model)).ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.PostAsync(model);

        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = (ObjectResult)result;
        Assert.Equal(500, objectResult.StatusCode);
        Assert.Equal("Database error", objectResult.Value);
    }

    [Fact]
    public async Task DeleteAsync_ValidRowId_ReturnsNoContent()
    {
        // Arrange
        var rowId = Guid.NewGuid();
        _questionBankBusiness.Setup(b => b.DeleteAsync(rowId)).ReturnsAsync(1);

        // Act
        var result = await _controller.DeleteAsync(rowId);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _questionBankBusiness.Verify(b => b.DeleteAsync(rowId), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_NotFound_ReturnsNotFound()
    {
        // Arrange
        var rowId = Guid.NewGuid();
        _questionBankBusiness.Setup(b => b.DeleteAsync(rowId))
            .ThrowsAsync(new KeyNotFoundException($"Client with id {rowId} not found"));

        // Act
        var result = await _controller.DeleteAsync(rowId);

        // Assert
        Assert.IsType<NotFoundODataResult>(result);
        // OData results don't expose Value property like standard MVC results
    }

    [Fact]
    public async Task DeleteAsync_ThrowsException_ReturnsInternalServerError()
    {
        // Arrange
        var rowId = Guid.NewGuid();
        _questionBankBusiness.Setup(b => b.DeleteAsync(rowId))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.DeleteAsync(rowId);

        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = (ObjectResult)result;
        Assert.Equal(500, objectResult.StatusCode);
        Assert.Equal("Database error", objectResult.Value);
    }
}