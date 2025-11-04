using FluentValidation;
using FluentValidation.Results;
using KonaAI.Master.API.Controllers.Tenant.Client;
using KonaAI.Master.Business.Tenant.Client.Logic.Interface;
using KonaAI.Master.Business.Tenant.UserMetaData.Logic.Interface;
using KonaAI.Master.Model.Tenant.Client.SaveModel;
using KonaAI.Master.Model.Tenant.Client.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Reflection;

namespace KonaAI.Master.Test.Unit.Controllers.Tenant.Client;

public class ClientProjectControllerUnitTests
{
    private readonly Mock<ILogger<ClientProjectController>> _logger = new();
    private readonly Mock<IClientProjectBusiness> _business = new();
    private readonly Mock<IClientProjectDepartmentBusiness> _departmentBusiness = new();
    private readonly Mock<IValidator<ClientProjectCreateModel>> _createValidator = new();

    private readonly ClientProjectController _controller;

    public ClientProjectControllerUnitTests()
    {
        _controller = new ClientProjectController(
            _logger.Object,
            _business.Object,
            _departmentBusiness.Object,
            _createValidator.Object
        );
    }

    [Fact]
    public async Task GetAsync_BusinessReturnsData_Returns200Ok()
    {
        // Arrange
        var clientProjects = new List<ClientProjectViewModel> { new() { RowId = Guid.NewGuid(), Name = "TestProject" } }.AsQueryable();
        _business.Setup(b => b.GetAsync()).ReturnsAsync(clientProjects);

        // Act
        var result = await _controller.GetAsync();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsAssignableFrom<IQueryable<ClientProjectViewModel>>(okResult.Value);
        Assert.Single(returnValue);
        Assert.Equal("TestProject", returnValue.First().Name);
    }

    [Fact]
    public async Task GetAsync_BusinessThrows_Returns500()
    {
        // Arrange
        _business.Setup(b => b.GetAsync()).ThrowsAsync(new Exception("Business error"));

        // Act
        var result = await _controller.GetAsync();

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        Assert.Equal("Business error", statusCodeResult.Value);
    }

    [Fact]
    public async Task PostAsync_ValidModel_Returns201Created()
    {
        // Arrange
        var createModel = new ClientProjectCreateModel { Name = "New Project" };
        var validationResult = new ValidationResult();
        _createValidator.Setup(v => v.ValidateAsync(createModel, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);
        _business.Setup(b => b.CreateAsync(createModel)).ReturnsAsync(1);

        // Act
        var result = await _controller.PostAsync(createModel);

        // Assert
        var createdResult = Assert.IsType<CreatedResult>(result);
        Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);
        Assert.Null(createdResult.Value); // Controller returns Created() without value
    }

    [Fact]
    public async Task PostAsync_ValidationFails_Returns400BadRequest()
    {
        // Arrange
        var createModel = new ClientProjectCreateModel { Name = "" }; // Invalid model
        var validationResult = new ValidationResult(new List<ValidationFailure> { new("Name", "Name is required") });
        _createValidator.Setup(v => v.ValidateAsync(createModel, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

        // Act
        var result = await _controller.PostAsync(createModel);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.IsType<List<ValidationFailure>>(badRequestResult.Value);
    }

    [Fact]
    public async Task PostAsync_BusinessThrows_Returns500()
    {
        // Arrange
        var createModel = new ClientProjectCreateModel { Name = "Test Project" };
        var validationResult = new ValidationResult();
        _createValidator.Setup(v => v.ValidateAsync(createModel, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);
        _business.Setup(b => b.CreateAsync(createModel)).ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.PostAsync(createModel);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        Assert.Equal("Database error", statusCodeResult.Value);
    }

    [Fact]
    public async Task GetAsync_EmptyData_Returns200Ok()
    {
        // Arrange
        _business.Setup(b => b.GetAsync()).ReturnsAsync(Array.Empty<ClientProjectViewModel>().AsQueryable());

        // Act
        var result = await _controller.GetAsync();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsAssignableFrom<IQueryable<ClientProjectViewModel>>(okResult.Value);
        Assert.Empty(returnValue);
    }

    [Fact]
    public async Task PostAsync_NullModel_Returns400BadRequest()
    {
        // Arrange
        ClientProjectCreateModel? createModel = null;
        var validationResult = new ValidationResult(new List<ValidationFailure> { new("Model", "Model is required") });
        _createValidator.Setup(v => v.ValidateAsync(It.IsAny<ClientProjectCreateModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

        // Act
        var result = await _controller.PostAsync(createModel!);

        // Assert
        var badRequestResult = Assert.IsType<Microsoft.AspNetCore.OData.Results.BadRequestODataResult>(result);
    }

    #region Authorization

    [Fact]
    public void Class_ShouldHaveAuthorizeAttribute()
    {
        var attr = typeof(ClientProjectController).GetCustomAttribute<AuthorizeAttribute>(inherit: true);
        Assert.NotNull(attr);
    }

    [Fact]
    public void GetAsync_ShouldHaveAuthorizePolicy_ViewAllProjects()
    {
        var mi = typeof(ClientProjectController).GetMethod(nameof(ClientProjectController.GetAsync));
        Assert.NotNull(mi);
        var attrs = mi!.GetCustomAttributes<AuthorizeAttribute>(inherit: true);
        Assert.Contains(attrs, a => (a.Policy ?? string.Empty).Replace(" ", string.Empty).Contains("Permission:Navigation=AllProjects;Action=View".Replace(" ", string.Empty)));
    }

    [Fact]
    public void PostAsync_ShouldHaveAuthorizePolicy_AddAllProjects()
    {
        var mi = typeof(ClientProjectController).GetMethod(nameof(ClientProjectController.PostAsync));
        Assert.NotNull(mi);
        var attrs = mi!.GetCustomAttributes<AuthorizeAttribute>(inherit: true);
        Assert.Contains(attrs, a => (a.Policy ?? string.Empty).Replace(" ", string.Empty).Contains("Permission:Navigation=AllProjects;Action=Add".Replace(" ", string.Empty)));
    }

    #endregion Authorization
}