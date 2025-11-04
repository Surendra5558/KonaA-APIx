using FluentValidation;
using KonaAI.Master.API.Controllers.Tenant.Client;
using KonaAI.Master.Business.Tenant.Client.Logic.Interface;
using KonaAI.Master.Business.Tenant.UserMetaData.Logic.Interface;
using KonaAI.Master.Model.Common;
using KonaAI.Master.Model.Tenant.Client.SaveModel;
using KonaAI.Master.Model.Tenant.Client.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.Extensions.Logging;
using Moq;

namespace KonaAI.Master.Test.Integration.Controllers.Tenant.Client;

/// <summary>
/// Comprehensive integration tests for <see cref="ClientProjectController"/>.
/// Tests all CRUD operations with real controller execution.
/// </summary>
public class ClientProjectControllerIntegrationTests
{
    private static ClientProjectController CreateController(
        out Mock<IClientProjectBusiness> business,
        out Mock<IValidator<ClientProjectCreateModel>> createValidator,
        out Mock<IClientProjectDepartmentBusiness> departmentBusiness)
    {
        var logger = new Mock<ILogger<ClientProjectController>>();
        business = new Mock<IClientProjectBusiness>(MockBehavior.Strict);
        createValidator = new Mock<IValidator<ClientProjectCreateModel>>(MockBehavior.Strict);
        departmentBusiness = new Mock<IClientProjectDepartmentBusiness>(MockBehavior.Strict);

        return new ClientProjectController(
            logger.Object,
            business.Object,
            departmentBusiness.Object,
            createValidator.Object);
    }

    [Fact]
    public async Task GetAsync_ReturnsOk_WithQueryable()
    {
        // Arrange
        var controller = CreateController(out var business, out var createValidator,
            out var departmentBusiness);

        var data = new List<ClientProjectViewModel>
        {
            new() { RowId = Guid.NewGuid(), Name = "Project 1", Description = "Test Project 1" },
            new() { RowId = Guid.NewGuid(), Name = "Project 2", Description = "Test Project 2" }
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
        var controller = CreateController(out var business, out var createValidator,
            out var departmentBusiness);

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
        var controller = CreateController(out var business, out var createValidator,
            out var departmentBusiness);

        var rowId = Guid.NewGuid();
        var data = new ClientProjectViewModel { RowId = rowId, Name = "Test Project", Description = "Test Description" };

        business.Setup(b => b.GetByRowIdAsync(rowId)).Returns(Task.FromResult((ClientProjectViewModel?)data));

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
        var controller = CreateController(out var business, out var createValidator,
            out var departmentBusiness);

        var rowId = Guid.NewGuid();
        business.Setup(b => b.GetByRowIdAsync(rowId)).Returns(Task.FromResult((ClientProjectViewModel?)null));

        // Act
        var result = await controller.GetByRowIdAsync(rowId);

        // Assert
        Assert.IsType<NotFoundODataResult>(result);
        var notFoundResult = (NotFoundODataResult)result;
        Assert.Equal(404, notFoundResult.StatusCode);
        business.Verify(b => b.GetByRowIdAsync(rowId), Times.Once);
    }

    [Fact]
    public async Task PostAsync_ValidModel_Returns201Created()
    {
        // Arrange
        var controller = CreateController(out var business, out var createValidator,
            out var departmentBusiness);

        var createModel = new ClientProjectCreateModel
        {
            Name = "New Project",
            Description = "New Project Description"
        };

        createValidator.Setup(v => v.ValidateAsync(createModel, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        business.Setup(b => b.CreateAsync(createModel)).ReturnsAsync(1);
        departmentBusiness.Setup(d => d.GetAsync()).Returns(Task.FromResult(Array.Empty<MetaDataViewModel>().AsQueryable()));

        // Act
        var result = await controller.PostAsync(createModel);

        // Assert
        Assert.IsType<CreatedResult>(result);
        var createdResult = (CreatedResult)result;
        Assert.Equal(201, createdResult.StatusCode);
        createValidator.Verify(v => v.ValidateAsync(createModel, It.IsAny<CancellationToken>()), Times.Once);
        business.Verify(b => b.CreateAsync(createModel), Times.Once);
    }

    [Fact]
    public async Task PostAsync_ValidationFails_Returns400BadRequest()
    {
        // Arrange
        var controller = CreateController(out var business, out var createValidator,
            out var departmentBusiness);

        var createModel = new ClientProjectCreateModel
        {
            Name = "",
            Description = ""
        };

        var validationResult = new FluentValidation.Results.ValidationResult();
        validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure("Name", "Name is required"));

        createValidator.Setup(v => v.ValidateAsync(createModel, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await controller.PostAsync(createModel);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        var badRequestResult = (BadRequestObjectResult)result;
        Assert.IsType<List<FluentValidation.Results.ValidationFailure>>(badRequestResult.Value);
        createValidator.Verify(v => v.ValidateAsync(createModel, It.IsAny<CancellationToken>()), Times.Once);
        business.Verify(b => b.CreateAsync(It.IsAny<ClientProjectCreateModel>()), Times.Never);
    }

    [Fact]
    public async Task PostAsync_BusinessThrows_Returns500()
    {
        // Arrange
        var controller = CreateController(out var business, out var createValidator,
            out var departmentBusiness);

        var createModel = new ClientProjectCreateModel
        {
            Name = "New Project",
            Description = "New Project Description"
        };

        createValidator.Setup(v => v.ValidateAsync(createModel, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        departmentBusiness.Setup(d => d.GetAsync()).Returns(Task.FromResult(Array.Empty<MetaDataViewModel>().AsQueryable()));
        business.Setup(b => b.CreateAsync(createModel)).ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await controller.PostAsync(createModel);

        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = (ObjectResult)result;
        Assert.Equal(500, objectResult.StatusCode);
        Assert.Equal("Database error", objectResult.Value);
    }
}