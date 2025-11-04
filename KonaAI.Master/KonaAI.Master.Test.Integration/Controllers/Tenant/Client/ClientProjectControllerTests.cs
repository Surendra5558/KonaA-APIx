using FluentValidation;
using FluentValidation.Results;
using KonaAI.Master.API.Controllers.Tenant.Client;
using KonaAI.Master.Business.Tenant.Client.Logic.Interface;
using KonaAI.Master.Business.Tenant.UserMetaData.Logic.Interface;
using KonaAI.Master.Model.Common;
using KonaAI.Master.Model.Tenant.Client.SaveModel;
using KonaAI.Master.Model.Tenant.Client.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace KonaAI.Master.Test.Integration.Controllers.Tenant.Client;

/// <summary>
/// Integration-style tests for <see cref="KonaAI.Master.API.Controllers.Tenant.Client.ClientProjectController"/>.
/// Ensures:
/// - GET list (200) and detail (200)
/// - POST create (201) invoking department dependency
/// - 400 BadRequest on validator failures for POST
/// </summary>
public class ClientProjectControllerTests
{
    // Overview:
    // - Validates list/detail reads and create path with validator failure branch.
    // - Includes dependency call to department business as exercised by controller.
    private static ClientProjectController CreateController(
        out Mock<IClientProjectBusiness> projectBusiness,
        out Mock<IClientProjectDepartmentBusiness> deptBusiness,
        out Mock<IValidator<ClientProjectCreateModel>> createValidator)
    {
        var logger = new Mock<ILogger<ClientProjectController>>();
        projectBusiness = new Mock<IClientProjectBusiness>(MockBehavior.Strict);
        deptBusiness = new Mock<IClientProjectDepartmentBusiness>(MockBehavior.Strict);
        createValidator = new Mock<IValidator<ClientProjectCreateModel>>(MockBehavior.Strict);
        return new ClientProjectController(logger.Object, projectBusiness.Object, deptBusiness.Object, createValidator.Object);
    }

    private static ClientProjectCreateModel CreateValidPayload() => new()
    {
        Name = "Project A",
        AuditResponsibilityRowId = Guid.NewGuid(),
        RiskAreaRowId = Guid.NewGuid(),
        CountryRowId = null,
        BusinessUnitRowId = null,
        BusinessDepartmentRowId = null,
        Modules = "Audit,Workflow"
    };

    [Fact]
    public async Task GetAsync_ReturnsOk()
    {
        // Arrange
        var controller = CreateController(out var projectBiz, out var deptBiz, out var validator);
        var data = new List<ClientProjectViewModel> { new() }.AsQueryable();
        projectBiz.Setup(b => b.GetAsync()).ReturnsAsync(data);

        // Act
        var result = await controller.GetAsync();
        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Same(data, ok.Value);
        projectBiz.VerifyAll();
    }

    [Fact]
    public async Task GetByRowIdAsync_ReturnsOk_WhenFound()
    {
        // Arrange
        var controller = CreateController(out var projectBiz, out var deptBiz, out var validator);
        var id = Guid.NewGuid();
        var item = new ClientProjectViewModel { RowId = id };
        projectBiz.Setup(b => b.GetByRowIdAsync(id)).ReturnsAsync(item);

        // Act
        var result = await controller.GetByRowIdAsync(id);
        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(item, ok.Value);
        projectBiz.VerifyAll();
    }

    [Fact]
    public async Task PostAsync_ReturnsCreated_OnValidInput()
    {
        // Arrange: validator passes; controller also calls department business
        var controller = CreateController(out var projectBiz, out var deptBiz, out var validator);
        var model = new ClientProjectCreateModel();
        validator.Setup(v => v.ValidateAsync(model, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(new ValidationResult());
        deptBiz.Setup(b => b.GetAsync()).ReturnsAsync(new List<Model.Common.MetaDataViewModel>().AsQueryable());
        projectBiz.Setup(b => b.CreateAsync(model)).ReturnsAsync(1);

        // Act
        var result = await controller.PostAsync(model);
        // Assert
        Assert.IsType<CreatedResult>(result);
        projectBiz.VerifyAll();
        deptBiz.VerifyAll();
    }

    [Fact]
    public async Task PostAsync_ReturnsBadRequest_WhenValidationFails()
    {
        // Arrange: validator fails; business must not be invoked
        var controller = CreateController(out var projectBiz, out var deptBiz, out var validator);
        var model = new ClientProjectCreateModel();
        var failures = new List<ValidationFailure> { new("Name", "required") };
        validator.Setup(v => v.ValidateAsync(model, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(new ValidationResult(failures));

        // Act
        var result = await controller.PostAsync(model);
        // Assert
        var bad = Assert.IsType<BadRequestObjectResult>(result);
        var errors = Assert.IsAssignableFrom<IEnumerable<ValidationFailure>>(bad.Value);
        Assert.Single(errors);
        projectBiz.Verify(b => b.CreateAsync(It.IsAny<ClientProjectCreateModel>()), Times.Never);
    }
}