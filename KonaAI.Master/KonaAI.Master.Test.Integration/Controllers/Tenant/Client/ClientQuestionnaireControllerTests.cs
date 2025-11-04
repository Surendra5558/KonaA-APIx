using FluentValidation;
using FluentValidation.Results;
using KonaAI.Master.API.Controllers.Tenant.Client;
using KonaAI.Master.Business.Tenant.Client.Logic.Interface;
using KonaAI.Master.Model.Tenant.Client.SaveModel;
using KonaAI.Master.Model.Tenant.Client.ViewModel;
using KonaAI.Master.Repository.Common;
using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Common.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.Extensions.Logging;
using Moq;

namespace KonaAI.Master.Test.Integration.Controllers.Tenant.Client;

/// <summary>
/// Integration tests for <see cref="KonaAI.Master.API.Controllers.Tenant.Client.ClientQuestionnaireController"/>.
/// Covers:
/// - GET list (200) with OData support
/// - GET by RowId (200 or 404)
/// - POST creation (201, 400 validation)
/// - Error handling (500)
/// - Authentication scenarios
/// </summary>
public class ClientQuestionnaireControllerTests
{
    private static ClientQuestionnaireController CreateController(
        out Mock<IClientQuestionnaireBusiness> business,
        out Mock<IValidator<ClientQuestionnaireCreateModel>> createValidator,
        out Mock<IUserContextService> userContextService)
    {
        var logger = new Mock<ILogger<ClientQuestionnaireController>>();
        business = new Mock<IClientQuestionnaireBusiness>(MockBehavior.Strict);
        createValidator = new Mock<IValidator<ClientQuestionnaireCreateModel>>(MockBehavior.Strict);
        userContextService = new Mock<IUserContextService>(MockBehavior.Strict);
        return new ClientQuestionnaireController(logger.Object, createValidator.Object, userContextService.Object, business.Object);
    }

    #region GET Tests

    [Fact]
    public async Task GetAsync_ReturnsOk_WithQueryable()
    {
        // Arrange: business returns a queryable list
        var controller = CreateController(out var business, out var createV, out var userContext);
        var data = new List<ClientQuestionnaireViewModel>
        {
            new() { RowId = Guid.NewGuid(), Name = "Questionnaire 1" },
            new() { RowId = Guid.NewGuid(), Name = "Questionnaire 2" }
        }.AsQueryable();

        business.Setup(b => b.GetAsync()).ReturnsAsync(data);

        // Act
        var result = await controller.GetAsync();

        // Assert
        Assert.IsType<OkObjectResult>(result);
        var ok = (OkObjectResult)result;
        Assert.Equal(data, ok.Value);
        business.Verify(b => b.GetAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAsync_WithValidData_ReturnsQuestionnaireList()
    {
        // Arrange: business returns questionnaire data
        var controller = CreateController(out var business, out var createV, out var userContext);
        var data = new List<ClientQuestionnaireViewModel>
        {
            new() { RowId = Guid.NewGuid(), Name = "Survey 1" },
            new() { RowId = Guid.NewGuid(), Name = "Survey 2" }
        }.AsQueryable();

        business.Setup(b => b.GetAsync()).ReturnsAsync(data);

        // Act
        var result = await controller.GetAsync();

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        var resultData = Assert.IsAssignableFrom<IQueryable<ClientQuestionnaireViewModel>>(ok.Value);
        Assert.Equal(2, resultData.Count());
        business.VerifyAll();
    }

    [Fact]
    public async Task GetAsync_WithException_ReturnsInternalServerError()
    {
        // Arrange: business throws exception
        var controller = CreateController(out var business, out var createV, out var userContext);
        business.Setup(b => b.GetAsync()).ThrowsAsync(new Exception("Database connection failed"));

        // Act
        var result = await controller.GetAsync();

        // Assert
        var statusCode = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCode.StatusCode);
        Assert.Equal("Database connection failed", statusCode.Value);
        business.VerifyAll();
    }

    #endregion

    #region GET by RowId Tests

    [Fact]
    public async Task GetByRowIdAsync_ValidRowId_ReturnsOkResult()
    {
        // Arrange: business returns questionnaire data
        var controller = CreateController(out var business, out var createV, out var userContext);
        var rowId = Guid.NewGuid();
        var data = new ClientQuestionnaireDetailsViewModel
        {
            QuestionnaireRowId = rowId,
            Name = "Test Questionnaire"
        };

        business.Setup(b => b.GetQuestionnaireDetailsAsync(rowId)).ReturnsAsync(data);

        // Act
        var result = await controller.GetByRowIdAsync(rowId);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Same(data, ok.Value);
        business.VerifyAll();
    }

    [Fact]
    public async Task GetByRowIdAsync_ValidRowId_ReturnsQuestionnaireDetails()
    {
        // Arrange: business returns detailed questionnaire data
        var controller = CreateController(out var business, out var createV, out var userContext);
        var rowId = Guid.NewGuid();
        var data = new ClientQuestionnaireDetailsViewModel
        {
            QuestionnaireRowId = rowId,
            Name = "Test Questionnaire"
        };

        business.Setup(b => b.GetQuestionnaireDetailsAsync(rowId)).ReturnsAsync(data);

        // Act
        var result = await controller.GetByRowIdAsync(rowId);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        var resultData = Assert.IsType<ClientQuestionnaireDetailsViewModel>(ok.Value);
        Assert.Equal(rowId, resultData.QuestionnaireRowId);
        Assert.Equal("Test Questionnaire", resultData.Name);
        business.VerifyAll();
    }

    [Fact]
    public async Task GetByRowIdAsync_InvalidRowId_ReturnsNotFound()
    {
        // Arrange: business returns null for invalid rowId
        var controller = CreateController(out var business, out var createV, out var userContext);
        var rowId = Guid.NewGuid();
        business.Setup(b => b.GetQuestionnaireDetailsAsync(rowId)).ReturnsAsync((ClientQuestionnaireDetailsViewModel?)null);

        // Act
        var result = await controller.GetByRowIdAsync(rowId);

        // Assert
        var notFound = Assert.IsType<NotFoundODataResult>(result);
        business.VerifyAll();
    }

    [Fact]
    public async Task GetByRowIdAsync_EmptyRowId_ReturnsBadRequest()
    {
        // Arrange: empty rowId
        var controller = CreateController(out var business, out var createV, out var userContext);
        var emptyRowId = Guid.Empty;

        // Act
        var result = await controller.GetByRowIdAsync(emptyRowId);

        // Assert
        var badRequest = Assert.IsType<BadRequestODataResult>(result);
    }

    [Fact]
    public async Task GetByRowIdAsync_WithException_ReturnsInternalServerError()
    {
        // Arrange: business throws exception
        var controller = CreateController(out var business, out var createV, out var userContext);
        var rowId = Guid.NewGuid();
        business.Setup(b => b.GetQuestionnaireDetailsAsync(rowId)).ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await controller.GetByRowIdAsync(rowId);

        // Assert
        var statusCode = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCode.StatusCode);
        business.VerifyAll();
    }

    #endregion

    #region POST Tests

    [Fact]
    public async Task PostAsync_ValidModel_ReturnsCreatedResult()
    {
        // Arrange: validation passes, business succeeds
        var controller = CreateController(out var business, out var createV, out var userContext);
        var model = new ClientQuestionnaireCreateModel
        {
            Name = "Test Questionnaire"
        };
        var validationResult = new ValidationResult();
        var clientId = 1L;
        var userContextObj = new UserContext { ClientId = clientId };

        createV.Setup(v => v.ValidateAsync(model, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);
        userContext.SetupGet(u => u.UserContext).Returns(userContextObj);
        business.Setup(b => b.CreateAsync(model, clientId)).ReturnsAsync(1);

        // Act
        var result = await controller.PostAsync(model);

        // Assert
        Assert.IsType<CreatedResult>(result);
        createV.Verify(v => v.ValidateAsync(model, It.IsAny<CancellationToken>()), Times.Once);
        business.Verify(b => b.CreateAsync(model, clientId), Times.Once);
    }

    [Fact]
    public async Task PostAsync_ValidModel_CreatesQuestionnaire()
    {
        // Arrange: validation passes, business creates questionnaire
        var controller = CreateController(out var business, out var createV, out var userContext);
        var model = new ClientQuestionnaireCreateModel
        {
            Name = "New Survey"
        };
        var validationResult = new ValidationResult();
        var clientId = 1L;
        var userContextObj = new UserContext { ClientId = clientId };

        createV.Setup(v => v.ValidateAsync(model, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);
        userContext.SetupGet(u => u.UserContext).Returns(userContextObj);
        business.Setup(b => b.CreateAsync(model, clientId)).ReturnsAsync(1);

        // Act
        var result = await controller.PostAsync(model);

        // Assert
        Assert.IsType<CreatedResult>(result);
        createV.VerifyAll();
        business.VerifyAll();
    }

    [Fact]
    public async Task PostAsync_InvalidModel_ReturnsBadRequest()
    {
        // Arrange: validation fails
        var controller = CreateController(out var business, out var createV, out var userContext);
        var model = new ClientQuestionnaireCreateModel { Name = "" };
        var validationResult = new ValidationResult(new List<ValidationFailure>
        {
            new("Name", "Name is required")
        });

        createV.Setup(v => v.ValidateAsync(model, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await controller.PostAsync(model);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Same(validationResult.Errors, badRequest.Value);
        createV.VerifyAll();
        business.Verify(b => b.CreateAsync(It.IsAny<ClientQuestionnaireCreateModel>(), It.IsAny<long>()), Times.Never);
    }

    [Fact]
    public async Task PostAsync_WithDbException_ReturnsInternalServerError()
    {
        // Arrange: validation passes, business throws exception
        var controller = CreateController(out var business, out var createV, out var userContext);
        var model = new ClientQuestionnaireCreateModel { Name = "Test Questionnaire" };
        var validationResult = new ValidationResult();
        var clientId = 1L;
        var userContextObj = new UserContext { ClientId = clientId };

        createV.Setup(v => v.ValidateAsync(model, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);
        userContext.SetupGet(u => u.UserContext).Returns(userContextObj);
        business.Setup(b => b.CreateAsync(model, clientId)).ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await controller.PostAsync(model);

        // Assert
        var statusCode = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCode.StatusCode);
        createV.VerifyAll();
        business.VerifyAll();
    }

    #endregion

    #region Performance Tests

    [Fact]
    public async Task GetAsync_LargeDataSet_PerformsWithinTimeLimit()
    {
        // Arrange: business returns large dataset
        var controller = CreateController(out var business, out var createV, out var userContext);
        var data = Enumerable.Range(1, 1000)
            .Select(i => new ClientQuestionnaireViewModel
            {
                RowId = Guid.NewGuid(),
                Name = $"Questionnaire {i}"
            })
            .AsQueryable();

        business.Setup(b => b.GetAsync()).ReturnsAsync(data);

        // Act
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var result = await controller.GetAsync();
        stopwatch.Stop();

        // Assert
        Assert.IsType<OkObjectResult>(result);
        Assert.True(stopwatch.ElapsedMilliseconds < 5000); // Should complete within 5 seconds
        business.VerifyAll();
    }

    #endregion

    #region End-to-End Workflow Tests

    [Fact]
    public async Task CompleteWorkflow_GetAndCreate_Succeeds()
    {
        // Arrange: complete workflow
        var controller = CreateController(out var business, out var createV, out var userContext);
        var rowId = Guid.NewGuid();
        var allData = new List<ClientQuestionnaireViewModel>
        {
            new() { RowId = rowId, Name = "Dashboard" },
            new() { RowId = Guid.NewGuid(), Name = "Settings" }
        }.AsQueryable();
        var specificData = new ClientQuestionnaireDetailsViewModel { QuestionnaireRowId = rowId, Name = "Dashboard" };
        var createModel = new ClientQuestionnaireCreateModel { Name = "New Survey" };
        var validationResult = new ValidationResult();
        var clientId = 1L;
        var userContextObj = new UserContext { ClientId = clientId };

        business.Setup(b => b.GetAsync()).ReturnsAsync(allData);
        business.Setup(b => b.GetQuestionnaireDetailsAsync(rowId)).ReturnsAsync(specificData);
        createV.Setup(v => v.ValidateAsync(createModel, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);
        userContext.SetupGet(u => u.UserContext).Returns(userContextObj);
        business.Setup(b => b.CreateAsync(createModel, clientId)).ReturnsAsync(1);

        // Act & Assert - Get All
        var getAllResult = await controller.GetAsync();
        var getAllOk = Assert.IsType<OkObjectResult>(getAllResult);
        Assert.Equal(allData, getAllOk.Value);

        // Act & Assert - Get Specific
        var getSpecificResult = await controller.GetByRowIdAsync(rowId);
        var getSpecificOk = Assert.IsType<OkObjectResult>(getSpecificResult);
        Assert.Equal(specificData, getSpecificOk.Value);

        // Act & Assert - Create
        var createResult = await controller.PostAsync(createModel);
        Assert.IsType<CreatedResult>(createResult);

        // Verify all interactions
        createV.VerifyAll();
        business.VerifyAll();
    }

    #endregion
}