using FluentValidation;
using KonaAI.Master.API.Controllers.Tenant.Client;
using KonaAI.Master.Business.Tenant.Client.Logic.Interface;
using KonaAI.Master.Model.Tenant.Client.SaveModel;
using KonaAI.Master.Model.Tenant.Client.ViewModel;
using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Common.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace KonaAI.Master.Test.Unit.Controllers.Tenant.Client;

/// <summary>
/// Unit tests for <see cref="ClientQuestionnaireController"/>.
/// </summary>
public class ClientQuestionnaireControllerTests
{
    private Mock<ILogger<ClientQuestionnaireController>> _logger = null!;
    private Mock<IValidator<ClientQuestionnaireCreateModel>> _createValidator = null!;
    private Mock<IUserContextService> _userContextService = null!;
    private Mock<IClientQuestionnaireBusiness> _clientQuestionnaireBusiness = null!;
    private ClientQuestionnaireController _controller = null!;

    public ClientQuestionnaireControllerTests()
    {
        _logger = new Mock<ILogger<ClientQuestionnaireController>>();
        _createValidator = new Mock<IValidator<ClientQuestionnaireCreateModel>>();
        _userContextService = new Mock<IUserContextService>();
        _clientQuestionnaireBusiness = new Mock<IClientQuestionnaireBusiness>();

        _controller = new ClientQuestionnaireController(
            _logger.Object,
            _createValidator.Object,
            _userContextService.Object,
            _clientQuestionnaireBusiness.Object);
    }

    [Fact]
    public async Task GetAsync_ReturnsOkResult()
    {
        // Arrange
        var questionnaires = new List<ClientQuestionnaireViewModel>
        {
            new() { RowId = Guid.NewGuid(), Name = "Test Questionnaire 1" },
            new() { RowId = Guid.NewGuid(), Name = "Test Questionnaire 2" }
        }.AsQueryable();

        _clientQuestionnaireBusiness.Setup(b => b.GetAsync())
            .ReturnsAsync(questionnaires);

        // Act
        var result = await _controller.GetAsync();

        // Assert
        Assert.IsType<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        Assert.Equal(questionnaires, okResult!.Value);
        _clientQuestionnaireBusiness.Verify(b => b.GetAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAsync_ThrowsException_ReturnsInternalServerError()
    {
        // Arrange
        _clientQuestionnaireBusiness.Setup(b => b.GetAsync())
            .ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _controller.GetAsync();

        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = result as ObjectResult;
        Assert.Equal(500, objectResult!.StatusCode);
        Assert.Equal("Test exception", objectResult.Value);
    }

    [Fact]
    public async Task GetByRowIdAsync_ValidRowId_ReturnsOkResult()
    {
        // Arrange
        var rowId = Guid.NewGuid();
        var questionnaire = new ClientQuestionnaireDetailsViewModel
        {
            QuestionnaireRowId = rowId,
            Name = "Test Questionnaire"
        };

        _clientQuestionnaireBusiness.Setup(b => b.GetQuestionnaireDetailsAsync(rowId))
            .ReturnsAsync(questionnaire);

        // Act
        var result = await _controller.GetByRowIdAsync(rowId);

        // Assert
        Assert.IsType<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        Assert.Equal(questionnaire, okResult!.Value);
        _clientQuestionnaireBusiness.Verify(b => b.GetQuestionnaireDetailsAsync(rowId), Times.Once);
    }

    [Fact]
    public async Task GetByRowIdAsync_EmptyRowId_ReturnsBadRequest()
    {
        // Arrange
        var emptyRowId = Guid.Empty;

        // Act
        var result = await _controller.GetByRowIdAsync(emptyRowId);

        // Assert
        Assert.IsType<Microsoft.AspNetCore.OData.Results.BadRequestODataResult>(result);
    }

    [Fact]
    public async Task GetByRowIdAsync_QuestionnaireNotFound_ReturnsNotFound()
    {
        // Arrange
        var rowId = Guid.NewGuid();
        _clientQuestionnaireBusiness.Setup(b => b.GetQuestionnaireDetailsAsync(rowId))
            .ReturnsAsync((ClientQuestionnaireDetailsViewModel?)null);

        // Act
        var result = await _controller.GetByRowIdAsync(rowId);

        // Assert
        Assert.IsType<Microsoft.AspNetCore.OData.Results.NotFoundODataResult>(result);
    }

    [Fact]
    public async Task GetByRowIdAsync_ThrowsException_ReturnsInternalServerError()
    {
        // Arrange
        var rowId = Guid.NewGuid();
        _clientQuestionnaireBusiness.Setup(b => b.GetQuestionnaireDetailsAsync(rowId))
            .ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _controller.GetByRowIdAsync(rowId);

        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = result as ObjectResult;
        Assert.Equal(500, objectResult!.StatusCode);
        Assert.Equal("Test exception", objectResult.Value);
    }

    [Fact]
    public async Task PostAsync_ValidModel_ReturnsCreatedResult()
    {
        // Arrange
        var createModel = new ClientQuestionnaireCreateModel
        {
            Name = "Test Questionnaire",
            Sections = new List<ClientQuestionnaireSectionCreateModel>()
        };

        var validationResult = new FluentValidation.Results.ValidationResult();
        _createValidator.Setup(v => v.ValidateAsync(createModel, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        _userContextService.Setup(u => u.UserContext)
            .Returns(new UserContext { ClientId = 1 });

        _clientQuestionnaireBusiness.Setup(b => b.CreateAsync(createModel, 1))
            .ReturnsAsync(1);

        // Act
        var result = await _controller.PostAsync(createModel);

        // Assert
        Assert.IsType<CreatedResult>(result);
        _createValidator.Verify(v => v.ValidateAsync(createModel, It.IsAny<CancellationToken>()), Times.Once);
        _clientQuestionnaireBusiness.Verify(b => b.CreateAsync(createModel, 1), Times.Once);
    }

    [Fact]
    public async Task PostAsync_InvalidModel_ReturnsBadRequest()
    {
        // Arrange
        var createModel = new ClientQuestionnaireCreateModel();
        var validationResult = new FluentValidation.Results.ValidationResult();
        validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure("Name", "Name is required"));

        _createValidator.Setup(v => v.ValidateAsync(createModel, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _controller.PostAsync(createModel);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        var badRequestResult = result as BadRequestObjectResult;
        Assert.Equal(validationResult.Errors, badRequestResult!.Value);
        _clientQuestionnaireBusiness.Verify(b => b.CreateAsync(It.IsAny<ClientQuestionnaireCreateModel>(), It.IsAny<long>()), Times.Never);
    }

    [Fact]
    public async Task PostAsync_ThrowsException_ReturnsInternalServerError()
    {
        // Arrange
        var createModel = new ClientQuestionnaireCreateModel
        {
            Name = "Test Questionnaire",
            Sections = new List<ClientQuestionnaireSectionCreateModel>()
        };

        var validationResult = new FluentValidation.Results.ValidationResult();
        _createValidator.Setup(v => v.ValidateAsync(createModel, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        _userContextService.Setup(u => u.UserContext)
            .Returns(new UserContext { ClientId = 1 });

        _clientQuestionnaireBusiness.Setup(b => b.CreateAsync(createModel, 1))
            .ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _controller.PostAsync(createModel);

        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = result as ObjectResult;
        Assert.Equal(500, objectResult!.StatusCode);
        Assert.Equal("Test exception", objectResult.Value);
    }
}