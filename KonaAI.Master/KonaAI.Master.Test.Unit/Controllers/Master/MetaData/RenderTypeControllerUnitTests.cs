using FluentValidation;
using FluentValidation.Results;
using KonaAI.Master.API.Controllers.Master.MetaData;
using KonaAI.Master.Model.Common;
using KonaAI.Master.Model.Master.SaveModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace KonaAI.Master.Test.Unit.Controllers.Master.MetaData;

public class RenderTypeControllerUnitTests
{
    private readonly Mock<ILogger<RenderTypeController>> _logger = new();
    private readonly Mock<IValidator<RenderTypeCreateModel>> _createValidator = new();
    private readonly Mock<IRenderTypeBusiness> _renderTypeBusiness = new();
    private readonly RenderTypeController _controller;

    public RenderTypeControllerUnitTests()
    {
        _controller = new RenderTypeController(
            _logger.Object,
            _createValidator.Object,
            _renderTypeBusiness.Object);
    }

    [Fact]
    public async Task GetAsync_ReturnsOkResult()
    {
        // Arrange
        var renderTypes = new List<MetaDataViewModel>
        {
            new() { RowId = Guid.NewGuid(), Name = "Text" },
            new() { RowId = Guid.NewGuid(), Name = "Number" }
        }.AsQueryable();

        _renderTypeBusiness.Setup(b => b.GetAsync()).ReturnsAsync(renderTypes);

        // Act
        var result = await _controller.GetAsync();

        // Assert
        Assert.IsType<OkObjectResult>(result);
        var okResult = (OkObjectResult)result;
        Assert.Equal(renderTypes, okResult.Value);
        _renderTypeBusiness.Verify(b => b.GetAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAsync_ThrowsException_ReturnsInternalServerError()
    {
        // Arrange
        _renderTypeBusiness.Setup(b => b.GetAsync()).ThrowsAsync(new Exception("Database error"));

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
        var model = new RenderTypeCreateModel
        {
            Name = "Test Render Type",
            Description = "Test Description"
        };

        var validationResult = new ValidationResult();
        _createValidator.Setup(v => v.ValidateAsync(model, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);
        _renderTypeBusiness.Setup(b => b.CreateAsync(model)).ReturnsAsync(1);

        // Act
        var result = await _controller.PostAsync(model);

        // Assert
        Assert.IsType<CreatedResult>(result);
        _createValidator.Verify(v => v.ValidateAsync(model, It.IsAny<CancellationToken>()), Times.Once);
        _renderTypeBusiness.Verify(b => b.CreateAsync(model), Times.Once);
    }

    [Fact]
    public async Task PostAsync_InvalidModel_ReturnsBadRequest()
    {
        // Arrange
        var model = new RenderTypeCreateModel { Name = "" };
        var validationResult = new ValidationResult(new List<ValidationFailure>
        {
            new("Name", "Name is required")
        });

        _createValidator.Setup(v => v.ValidateAsync(model, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _controller.PostAsync(model);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        var badRequestResult = (BadRequestObjectResult)result;
        Assert.Equal(validationResult.Errors, badRequestResult.Value);
        _renderTypeBusiness.Verify(b => b.CreateAsync(It.IsAny<RenderTypeCreateModel>()), Times.Never);
    }

    [Fact]
    public async Task PostAsync_ThrowsException_ReturnsInternalServerError()
    {
        // Arrange
        var model = new RenderTypeCreateModel { Name = "Test Render Type" };
        var validationResult = new ValidationResult();

        _createValidator.Setup(v => v.ValidateAsync(model, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);
        _renderTypeBusiness.Setup(b => b.CreateAsync(model)).ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.PostAsync(model);

        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = (ObjectResult)result;
        Assert.Equal(500, objectResult.StatusCode);
        Assert.Equal("Database error", objectResult.Value);
    }
}