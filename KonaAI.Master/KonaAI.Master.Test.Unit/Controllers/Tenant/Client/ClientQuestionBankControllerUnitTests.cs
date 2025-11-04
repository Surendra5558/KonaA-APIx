using FluentValidation;
using FluentValidation.Results;
using KonaAI.Master.API.Controllers.Tenant.Client;
using KonaAI.Master.Business.Tenant.Client.Logic.Interface;
using KonaAI.Master.Model.Tenant.Client.SaveModel;
using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Common.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.Extensions.Logging;
using Moq;

namespace KonaAI.Master.Test.Unit.Controllers.Tenant.Client;

public class ClientQuestionBankControllerUnitTests
{
    private readonly Mock<ILogger<ClientQuestionBankController>> _logger = new();
    private readonly Mock<IValidator<ClientQuestionBankCreateModel>> _createValidator = new();
    private readonly Mock<IUserContextService> _userContextService = new();
    private readonly Mock<IClientQuestionBankBusiness> _clientQuestionBankBusiness = new();
    private readonly ClientQuestionBankController _controller;

    public ClientQuestionBankControllerUnitTests()
    {
        _controller = new ClientQuestionBankController(
            _logger.Object,
            _createValidator.Object,
            _userContextService.Object,
            _clientQuestionBankBusiness.Object);
    }

    [Fact]
    public async Task GetAsync_ReturnsOkResult()
    {
        // Arrange
        var clientId = 1L;
        var userContext = new UserContext { ClientId = clientId };
        var questionBanks = new List<ClientQuestionBankViewModel>
        {
            new() { RowId = Guid.NewGuid(), Description = "Test Question 1" },
            new() { RowId = Guid.NewGuid(), Description = "Test Question 2" }
        }.AsQueryable();

        _userContextService.SetupGet(u => u.UserContext).Returns(userContext);
        _clientQuestionBankBusiness.Setup(b => b.GetAsync(clientId)).ReturnsAsync(questionBanks);

        // Act
        var result = await _controller.GetAsync();

        // Assert
        Assert.IsType<OkObjectResult>(result);
        var okResult = (OkObjectResult)result;
        Assert.Equal(questionBanks, okResult.Value);
        _clientQuestionBankBusiness.Verify(b => b.GetAsync(clientId), Times.Once);
    }

    [Fact]
    public async Task GetAsync_ThrowsException_ReturnsInternalServerError()
    {
        // Arrange
        var clientId = 1L;
        var userContext = new UserContext { ClientId = clientId };

        _userContextService.SetupGet(u => u.UserContext).Returns(userContext);
        _clientQuestionBankBusiness.Setup(b => b.GetAsync(clientId)).ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.GetAsync();

        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = (ObjectResult)result;
        Assert.Equal(500, objectResult.StatusCode);
        Assert.Equal("Database error", objectResult.Value);
    }

    [Fact]
    public async Task GetByRowIdAsync_ValidRowId_ReturnsOkResult()
    {
        // Arrange
        var rowId = Guid.NewGuid();
        var questionBank = new ClientQuestionBankViewModel { RowId = rowId, Description = "Test Question" };

        _clientQuestionBankBusiness.Setup(b => b.GetByRowIdAsync(rowId)).ReturnsAsync(questionBank);

        // Act
        var result = await _controller.GetByRowIdAsync(rowId);

        // Assert
        Assert.IsType<OkObjectResult>(result);
        var okResult = (OkObjectResult)result;
        Assert.Equal(questionBank, okResult.Value);
        _clientQuestionBankBusiness.Verify(b => b.GetByRowIdAsync(rowId), Times.Once);
    }

    [Fact]
    public async Task GetByRowIdAsync_EmptyRowId_ReturnsBadRequest()
    {
        // Arrange
        var emptyRowId = Guid.Empty;

        // Act
        var result = await _controller.GetByRowIdAsync(emptyRowId);

        // Assert
        Assert.IsType<BadRequestODataResult>(result);
        // OData results don't expose Value property like standard MVC results
    }

    [Fact]
    public async Task GetByRowIdAsync_NotFound_ReturnsNotFound()
    {
        // Arrange
        var rowId = Guid.NewGuid();
        _clientQuestionBankBusiness.Setup(b => b.GetByRowIdAsync(rowId)).ReturnsAsync((ClientQuestionBankViewModel?)null);

        // Act
        var result = await _controller.GetByRowIdAsync(rowId);

        // Assert
        Assert.IsType<NotFoundODataResult>(result);
        // OData results don't expose Value property like standard MVC results
    }

    [Fact]
    public async Task GetByRowIdAsync_ThrowsException_ReturnsInternalServerError()
    {
        // Arrange
        var rowId = Guid.NewGuid();
        _clientQuestionBankBusiness.Setup(b => b.GetByRowIdAsync(rowId)).ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.GetByRowIdAsync(rowId);

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
        var model = new ClientQuestionBankCreateModel
        {
            Text = "Test Question",
            Type = "Text",
            Required = true
        };

        var validationResult = new ValidationResult();
        _createValidator.Setup(v => v.ValidateAsync(model, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);
        _clientQuestionBankBusiness.Setup(b => b.CreateAsync(model)).ReturnsAsync(1);

        // Act
        var result = await _controller.PostAsync(model);

        // Assert
        Assert.IsType<CreatedResult>(result);
        _createValidator.Verify(v => v.ValidateAsync(model, It.IsAny<CancellationToken>()), Times.Once);
        _clientQuestionBankBusiness.Verify(b => b.CreateAsync(model), Times.Once);
    }

    [Fact]
    public async Task PostAsync_InvalidModel_ReturnsBadRequest()
    {
        // Arrange
        var model = new ClientQuestionBankCreateModel { Text = "" };
        var validationResult = new ValidationResult(new List<ValidationFailure>
        {
            new("Text", "Question text is required")
        });

        _createValidator.Setup(v => v.ValidateAsync(model, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _controller.PostAsync(model);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        var badRequestResult = (BadRequestObjectResult)result;
        Assert.Equal(validationResult.Errors, badRequestResult.Value);
        _clientQuestionBankBusiness.Verify(b => b.CreateAsync(It.IsAny<ClientQuestionBankCreateModel>()), Times.Never);
    }

    [Fact]
    public async Task PostAsync_ThrowsException_ReturnsInternalServerError()
    {
        // Arrange
        var model = new ClientQuestionBankCreateModel { Text = "Test Question" };
        var validationResult = new ValidationResult();

        _createValidator.Setup(v => v.ValidateAsync(model, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);
        _clientQuestionBankBusiness.Setup(b => b.CreateAsync(model)).ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.PostAsync(model);

        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = (ObjectResult)result;
        Assert.Equal(500, objectResult.StatusCode);
        Assert.Equal("Database error", objectResult.Value);
    }
}