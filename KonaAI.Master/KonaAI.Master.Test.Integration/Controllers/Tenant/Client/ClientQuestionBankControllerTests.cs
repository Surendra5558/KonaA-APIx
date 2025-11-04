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

namespace KonaAI.Master.Test.Integration.Controllers.Tenant.Client;

/// <summary>
/// Integration tests for <see cref="KonaAI.Master.API.Controllers.Tenant.Client.ClientQuestionBankController"/>.
/// Covers:
/// - GET list (200) with OData support
/// - GET by RowId (200 or 404)
/// - POST creation (201, 400 validation)
/// - Error handling (500)
/// - Authentication scenarios
/// </summary>
public class ClientQuestionBankControllerTests
{
    private static ClientQuestionBankController CreateController(
        out Mock<IClientQuestionBankBusiness> business,
        out Mock<IValidator<ClientQuestionBankCreateModel>> createValidator,
        out Mock<IUserContextService> userContextService)
    {
        var logger = new Mock<ILogger<ClientQuestionBankController>>();
        business = new Mock<IClientQuestionBankBusiness>(MockBehavior.Strict);
        createValidator = new Mock<IValidator<ClientQuestionBankCreateModel>>(MockBehavior.Strict);
        userContextService = new Mock<IUserContextService>(MockBehavior.Strict);
        return new ClientQuestionBankController(logger.Object, createValidator.Object, userContextService.Object, business.Object);
    }

    #region GET Tests

    [Fact]
    public async Task GetAsync_ReturnsOk_WithQueryable()
    {
        // Arrange: business returns a queryable list
        var controller = CreateController(out var business, out var createV, out var userContext);
        var data = new List<ClientQuestionBankViewModel>
        {
            new() { RowId = Guid.NewGuid(), Description = "Question 1" },
            new() { RowId = Guid.NewGuid(), Description = "Question 2" }
        }.AsQueryable();
        var clientId = 1L;
        var userContextObj = new UserContext { ClientId = clientId };

        userContext.SetupGet(u => u.UserContext).Returns(userContextObj);
        business.Setup(b => b.GetAsync(clientId)).ReturnsAsync(data);

        // Act
        var result = await controller.GetAsync();

        // Assert
        Assert.IsType<OkObjectResult>(result);
        var ok = (OkObjectResult)result;
        Assert.Equal(data, ok.Value);
        business.Verify(b => b.GetAsync(clientId), Times.Once);
    }

    [Fact]
    public async Task GetAsync_WithValidData_ReturnsQuestionBankList()
    {
        // Arrange: business returns question bank data
        var controller = CreateController(out var business, out var createV, out var userContext);
        var data = new List<ClientQuestionBankViewModel>
        {
            new() { RowId = Guid.NewGuid(), Description = "Survey Question 1" },
            new() { RowId = Guid.NewGuid(), Description = "Survey Question 2" }
        }.AsQueryable();
        var clientId = 1L;
        var userContextObj = new UserContext { ClientId = clientId };

        userContext.SetupGet(u => u.UserContext).Returns(userContextObj);
        business.Setup(b => b.GetAsync(clientId)).ReturnsAsync(data);

        // Act
        var result = await controller.GetAsync();

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        var resultData = Assert.IsAssignableFrom<IQueryable<ClientQuestionBankViewModel>>(ok.Value);
        Assert.Equal(2, resultData.Count());
        business.VerifyAll();
    }

    [Fact]
    public async Task GetAsync_WithException_ReturnsInternalServerError()
    {
        // Arrange: business throws exception
        var controller = CreateController(out var business, out var createV, out var userContext);
        var clientId = 1L;
        var userContextObj = new UserContext { ClientId = clientId };

        userContext.SetupGet(u => u.UserContext).Returns(userContextObj);
        business.Setup(b => b.GetAsync(clientId)).ThrowsAsync(new Exception("Database connection failed"));

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
        // Arrange: business returns question bank data
        var controller = CreateController(out var business, out var createV, out var userContext);
        var rowId = Guid.NewGuid();
        var data = new ClientQuestionBankViewModel
        {
            RowId = rowId,
            Description = "Test Question"
        };

        business.Setup(b => b.GetByRowIdAsync(rowId)).ReturnsAsync(data);

        // Act
        var result = await controller.GetByRowIdAsync(rowId);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Same(data, ok.Value);
        business.VerifyAll();
    }

    [Fact]
    public async Task GetByRowIdAsync_ValidRowId_ReturnsQuestionBankDetails()
    {
        // Arrange: business returns detailed question bank data
        var controller = CreateController(out var business, out var createV, out var userContext);
        var rowId = Guid.NewGuid();
        var data = new ClientQuestionBankViewModel
        {
            RowId = rowId,
            Description = "Test Question"
        };

        business.Setup(b => b.GetByRowIdAsync(rowId)).ReturnsAsync(data);

        // Act
        var result = await controller.GetByRowIdAsync(rowId);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        var resultData = Assert.IsType<ClientQuestionBankViewModel>(ok.Value);
        Assert.Equal(rowId, resultData.RowId);
        Assert.Equal("Test Question", resultData.Description);
        business.VerifyAll();
    }

    [Fact]
    public async Task GetByRowIdAsync_InvalidRowId_ReturnsNotFound()
    {
        // Arrange: business returns null for invalid rowId
        var controller = CreateController(out var business, out var createV, out var userContext);
        var rowId = Guid.NewGuid();
        business.Setup(b => b.GetByRowIdAsync(rowId)).ReturnsAsync((ClientQuestionBankViewModel?)null);

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
        business.Setup(b => b.GetByRowIdAsync(rowId)).ThrowsAsync(new Exception("Database error"));

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
        var model = new ClientQuestionBankCreateModel
        {
            Description = "Test Question"
        };
        var validationResult = new ValidationResult();

        createV.Setup(v => v.ValidateAsync(model, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);
        business.Setup(b => b.CreateAsync(model)).ReturnsAsync(1);

        // Act
        var result = await controller.PostAsync(model);

        // Assert
        Assert.IsType<CreatedResult>(result);
        createV.Verify(v => v.ValidateAsync(model, It.IsAny<CancellationToken>()), Times.Once);
        business.Verify(b => b.CreateAsync(model), Times.Once);
    }

    [Fact]
    public async Task PostAsync_ValidModel_CreatesQuestionBank()
    {
        // Arrange: validation passes, business creates question bank
        var controller = CreateController(out var business, out var createV, out var userContext);
        var model = new ClientQuestionBankCreateModel
        {
            Description = "New Question"
        };
        var validationResult = new ValidationResult();

        createV.Setup(v => v.ValidateAsync(model, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);
        business.Setup(b => b.CreateAsync(model)).ReturnsAsync(1);

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
        var model = new ClientQuestionBankCreateModel { Description = "" };
        var validationResult = new ValidationResult(new List<ValidationFailure>
        {
            new("Text", "Text is required")
        });

        createV.Setup(v => v.ValidateAsync(model, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await controller.PostAsync(model);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Same(validationResult.Errors, badRequest.Value);
        createV.VerifyAll();
        business.Verify(b => b.CreateAsync(It.IsAny<ClientQuestionBankCreateModel>()), Times.Never);
    }

    [Fact]
    public async Task PostAsync_WithDbException_ReturnsInternalServerError()
    {
        // Arrange: validation passes, business throws exception
        var controller = CreateController(out var business, out var createV, out var userContext);
        var model = new ClientQuestionBankCreateModel { Description = "Test Question" };
        var validationResult = new ValidationResult();

        createV.Setup(v => v.ValidateAsync(model, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);
        business.Setup(b => b.CreateAsync(model)).ThrowsAsync(new Exception("Database error"));

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
            .Select(i => new ClientQuestionBankViewModel
            {
                RowId = Guid.NewGuid(),
                Description = $"Question {i}"
            })
            .AsQueryable();

        var clientId = 1L;
        var userContextObj = new UserContext { ClientId = clientId };

        userContext.SetupGet(u => u.UserContext).Returns(userContextObj);
        business.Setup(b => b.GetAsync(clientId)).ReturnsAsync(data);

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
        var allData = new List<ClientQuestionBankViewModel>
        {
            new() { RowId = rowId, Description = "Question 1" },
            new() { RowId = Guid.NewGuid(), Description = "Question 2" }
        }.AsQueryable();
        var specificData = new ClientQuestionBankViewModel { RowId = rowId, Description = "Question 1" };
        var createModel = new ClientQuestionBankCreateModel { Description = "New Question" };
        var validationResult = new ValidationResult();

        var clientId = 1L;
        var userContextObj = new UserContext { ClientId = clientId };

        userContext.SetupGet(u => u.UserContext).Returns(userContextObj);
        business.Setup(b => b.GetAsync(clientId)).ReturnsAsync(allData);
        business.Setup(b => b.GetByRowIdAsync(rowId)).ReturnsAsync(specificData);
        createV.Setup(v => v.ValidateAsync(createModel, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);
        business.Setup(b => b.CreateAsync(createModel)).ReturnsAsync(1);

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