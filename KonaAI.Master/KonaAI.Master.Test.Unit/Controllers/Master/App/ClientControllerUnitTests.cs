using FluentValidation;
using FluentValidation.Results;
using KonaAI.Master.API.Controllers.Master.App;
using KonaAI.Master.Business.Master.App.Logic.Interface;
using KonaAI.Master.Model.Master.App.SaveModel;
using KonaAI.Master.Model.Master.App.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.Extensions.Logging;
using Moq;
using System.Reflection;

namespace KonaAI.Master.Test.Unit.Controllers.Master.App;

/// <summary>
/// Unit tests for <see cref="ClientController"/>.
/// Covers:
/// - HTTP method handling (GET, POST, PUT, DELETE)
/// - Validation error handling (400 responses)
/// - Business exceptions (404, 500 responses)
/// - Logging verification
/// </summary>
public class ClientControllerUnitTests
{
    private readonly Mock<ILogger<ClientController>> _logger = new();
    private readonly Mock<IClientBusiness> _business = new();
    private readonly Mock<IValidator<ClientCreateModel>> _createValidator = new();
    private readonly Mock<IValidator<ClientUpdateModel>> _updateValidator = new();

    private ClientController CreateSut() =>
        new(_logger.Object, _business.Object, _createValidator.Object, _updateValidator.Object);

    #region GetAsync

    [Fact]
    public async Task GetAsync_BusinessReturnsData_Returns200Ok()
    {
        // Arrange
        var data = new List<ClientViewModel>
        {
            new() { RowId = Guid.NewGuid(), Name = "Client1", DisplayName = "Client 1", ClientCode = "C001", IsActive = true },
            new() { RowId = Guid.NewGuid(), Name = "Client2", DisplayName = "Client 2", ClientCode = "C002", IsActive = false }
        }.AsQueryable();

        _business.Setup(b => b.GetAsync()).ReturnsAsync(data);

        var sut = CreateSut();

        // Act
        var result = await sut.GetAsync();

        // Assert
        Assert.IsType<OkObjectResult>(result);
        var okResult = (OkObjectResult)result;
        Assert.NotNull(okResult.Value);
        _business.Verify(b => b.GetAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAsync_BusinessThrows_Returns500()
    {
        // Arrange
        _business.Setup(b => b.GetAsync()).ThrowsAsync(new Exception("Database connection failed"));

        var sut = CreateSut();

        // Act
        var result = await sut.GetAsync();

        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = (ObjectResult)result;
        Assert.Equal(500, objectResult.StatusCode);
        Assert.Equal("Database connection failed", objectResult.Value);
    }

    #endregion GetAsync

    #region PostAsync

    [Fact]
    public async Task PostAsync_ValidModel_Returns201Created()
    {
        // Arrange
        var createModel = new ClientCreateModel
        {
            Name = "NewClient",
            DisplayName = "New Client",
            ClientCode = "C003"
        };

        var validationResult = new ValidationResult();
        _createValidator.Setup(v => v.ValidateAsync(createModel, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        _business.Setup(b => b.CreateAsync(createModel)).ReturnsAsync(1);

        var sut = CreateSut();

        // Act
        var result = await sut.PostAsync(createModel);

        // Assert
        Assert.IsType<CreatedResult>(result);
        var createdResult = (CreatedResult)result;
        Assert.Equal(201, createdResult.StatusCode);
        _createValidator.Verify(v => v.ValidateAsync(createModel, It.IsAny<CancellationToken>()), Times.Once);
        _business.Verify(b => b.CreateAsync(createModel), Times.Once);
    }

    [Fact]
    public async Task PostAsync_ValidationFails_Returns400BadRequest()
    {
        // Arrange
        var createModel = new ClientCreateModel
        {
            Name = "", // Invalid - empty name
            DisplayName = "New Client",
            ClientCode = "C003"
        };

        var validationResult = new ValidationResult();
        validationResult.Errors.Add(new ValidationFailure("Name", "Name is required"));
        _createValidator.Setup(v => v.ValidateAsync(createModel, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        var sut = CreateSut();

        // Act
        var result = await sut.PostAsync(createModel);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        var badRequestResult = (BadRequestObjectResult)result;
        Assert.Equal(400, badRequestResult.StatusCode);
        _createValidator.Verify(v => v.ValidateAsync(createModel, It.IsAny<CancellationToken>()), Times.Once);
        _business.Verify(b => b.CreateAsync(It.IsAny<ClientCreateModel>()), Times.Never);
    }

    [Fact]
    public async Task PostAsync_BusinessThrows_Returns500()
    {
        // Arrange
        var createModel = new ClientCreateModel
        {
            Name = "NewClient",
            DisplayName = "New Client",
            ClientCode = "C003"
        };

        var validationResult = new ValidationResult();
        _createValidator.Setup(v => v.ValidateAsync(createModel, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        _business.Setup(b => b.CreateAsync(createModel)).ThrowsAsync(new Exception("Database error"));

        var sut = CreateSut();

        // Act
        var result = await sut.PostAsync(createModel);

        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = (ObjectResult)result;
        Assert.Equal(500, objectResult.StatusCode);
        Assert.Equal("Database error", objectResult.Value);
    }

    #endregion PostAsync

    #region PutAsync

    [Fact]
    public async Task PutAsync_ValidModel_Returns204NoContent()
    {
        // Arrange
        var updateModel = new ClientUpdateModel
        {
            RowId = Guid.NewGuid(),
            Name = "UpdatedClient",
            DisplayName = "Updated Client",
            ClientCode = "C003"
        };

        var validationResult = new ValidationResult();
        _updateValidator.Setup(v => v.ValidateAsync(updateModel, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        _business.Setup(b => b.UpdateAsync(It.IsAny<Guid>(), updateModel)).ReturnsAsync(1);

        var sut = CreateSut();

        // Act
        var result = await sut.PutAsync(Guid.NewGuid(), updateModel);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _updateValidator.Verify(v => v.ValidateAsync(updateModel, It.IsAny<CancellationToken>()), Times.Once);
        _business.Verify(b => b.UpdateAsync(It.IsAny<Guid>(), updateModel), Times.Once);
    }

    [Fact]
    public async Task PutAsync_ValidationFails_Returns400BadRequest()
    {
        // Arrange
        var updateModel = new ClientUpdateModel
        {
            RowId = Guid.NewGuid(),
            Name = "", // Invalid - empty name
            DisplayName = "Updated Client",
            ClientCode = "C003"
        };

        var validationResult = new ValidationResult();
        validationResult.Errors.Add(new ValidationFailure("Name", "Name is required"));
        _updateValidator.Setup(v => v.ValidateAsync(updateModel, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        var sut = CreateSut();

        // Act
        var result = await sut.PutAsync(Guid.NewGuid(), updateModel);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        var badRequestResult = (BadRequestObjectResult)result;
        Assert.Equal(400, badRequestResult.StatusCode);
        _updateValidator.Verify(v => v.ValidateAsync(updateModel, It.IsAny<CancellationToken>()), Times.Once);
        _business.Verify(b => b.UpdateAsync(It.IsAny<Guid>(), It.IsAny<ClientUpdateModel>()), Times.Never);
    }

    [Fact]
    public async Task PutAsync_NotFound_Returns404()
    {
        // Arrange
        var updateModel = new ClientUpdateModel
        {
            RowId = Guid.NewGuid(),
            Name = "UpdatedClient",
            DisplayName = "Updated Client",
            ClientCode = "C003"
        };

        var validationResult = new ValidationResult();
        _updateValidator.Setup(v => v.ValidateAsync(updateModel, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        _business.Setup(b => b.UpdateAsync(It.IsAny<Guid>(), updateModel)).ThrowsAsync(new KeyNotFoundException("Client not found"));

        var sut = CreateSut();

        // Act
        var result = await sut.PutAsync(Guid.NewGuid(), updateModel);

        // Assert
        Assert.IsType<NotFoundODataResult>(result);
        var notFoundResult = (NotFoundODataResult)result;
        Assert.Equal(404, notFoundResult.StatusCode);
        // NotFoundODataResult doesn't have a Value property, just verify the status code
    }

    #endregion PutAsync

    #region DeleteAsync

    [Fact]
    public async Task DeleteAsync_Exists_Returns204NoContent()
    {
        // Arrange
        var rowId = Guid.NewGuid();
        _business.Setup(b => b.DeleteAsync(rowId)).ReturnsAsync(1);

        var sut = CreateSut();

        // Act
        var result = await sut.DeleteAsync(rowId);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _business.Verify(b => b.DeleteAsync(rowId), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_NotFound_Returns404()
    {
        // Arrange
        var rowId = Guid.NewGuid();
        _business.Setup(b => b.DeleteAsync(rowId)).ThrowsAsync(new KeyNotFoundException("Client not found"));

        var sut = CreateSut();

        // Act
        var result = await sut.DeleteAsync(rowId);

        // Assert
        Assert.IsType<NotFoundODataResult>(result);
        var notFoundResult = (NotFoundODataResult)result;
        Assert.Equal(404, notFoundResult.StatusCode);
        // NotFoundODataResult doesn't have a Value property, just verify the status code
    }

    [Fact]
    public async Task DeleteAsync_BusinessThrows_Returns500()
    {
        // Arrange
        var rowId = Guid.NewGuid();
        _business.Setup(b => b.DeleteAsync(rowId)).ThrowsAsync(new Exception("Database error"));

        var sut = CreateSut();

        // Act
        var result = await sut.DeleteAsync(rowId);

        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = (ObjectResult)result;
        Assert.Equal(500, objectResult.StatusCode);
        Assert.Equal("Database error", objectResult.Value);
    }

    #endregion DeleteAsync

    #region Authorization

    [Fact]
    public void Methods_ShouldHaveExpectedAuthorizePolicies()
    {
        // Get
        var get = typeof(ClientController).GetMethod(nameof(ClientController.GetAsync));
        Assert.NotNull(get);
        var getAuth = get!.GetCustomAttributes<AuthorizeAttribute>(inherit: true);
        Assert.Contains(getAuth, a => (a.Policy ?? string.Empty).Replace(" ", string.Empty).Contains("Permission:Navigation=AllClients;Action=View".Replace(" ", string.Empty)));

        // GetByRowId
        var getBy = typeof(ClientController).GetMethod(nameof(ClientController.GetByRowIdAsync));
        Assert.NotNull(getBy);
        var getByAuth = getBy!.GetCustomAttributes<AuthorizeAttribute>(inherit: true);
        Assert.Contains(getByAuth, a => (a.Policy ?? string.Empty).Replace(" ", string.Empty).Contains("Permission:Navigation=AllClients;Action=View".Replace(" ", string.Empty)));

        // Post
        var post = typeof(ClientController).GetMethod(nameof(ClientController.PostAsync));
        Assert.NotNull(post);
        var postAuth = post!.GetCustomAttributes<AuthorizeAttribute>(inherit: true);
        Assert.Contains(postAuth, a => (a.Policy ?? string.Empty).Replace(" ", string.Empty).Contains("Permission:Navigation=AllClients;Action=Add".Replace(" ", string.Empty)));

        // Put
        var put = typeof(ClientController).GetMethod(nameof(ClientController.PutAsync));
        Assert.NotNull(put);
        var putAuth = put!.GetCustomAttributes<AuthorizeAttribute>(inherit: true);
        Assert.Contains(putAuth, a => (a.Policy ?? string.Empty).Replace(" ", string.Empty).Contains("Permission:Navigation=AllClients;Action=Edit".Replace(" ", string.Empty)));

        // Delete
        var del = typeof(ClientController).GetMethod(nameof(ClientController.DeleteAsync));
        Assert.NotNull(del);
        var delAuth = del!.GetCustomAttributes<AuthorizeAttribute>(inherit: true);
        Assert.Contains(delAuth, a => (a.Policy ?? string.Empty).Replace(" ", string.Empty).Contains("Permission:Navigation=AllClients;Action=Delete".Replace(" ", string.Empty)));
    }

    #endregion Authorization
}