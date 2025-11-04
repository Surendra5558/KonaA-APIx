using FluentValidation;
using FluentValidation.Results;
using KonaAI.Master.API.Controllers.Tenant.Client;
using KonaAI.Master.Business.Tenant.Client.Logic.Interface;
using KonaAI.Master.Model.Tenant.Client.SaveModel;
using KonaAI.Master.Model.Tenant.Client.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace KonaAI.Master.Test.Integration.Controllers.Tenant.Client;

/// <summary>
/// Integration-style tests for <see cref="KonaAI.Master.API.Controllers.Tenant.Client.ClientLicenseController"/>.
/// Covers:
/// - GET list (200) and GET by id (200 or 404)
/// - PUT update (204) and 400 on validation failures
/// - DELETE (204) and 404 when resource is missing
/// </summary>
public class ClientLicenseControllerTests
{
    // Overview:
    // - Exercises ClientLicenseController actions with mocked business/validators.
    // - Verifies happy paths (200/204) and negative paths (400/404).
    // - We don't spin up the full host; we set expectations on mocks and assert IActionResult.
    private static ClientLicenseController CreateController(
        out Mock<IClientLicenseBusiness> business,
        out Mock<IValidator<ClientLicenseUpdateModel>> updateValidator)
    {
        var logger = new Mock<ILogger<ClientLicenseController>>();
        business = new Mock<IClientLicenseBusiness>(MockBehavior.Strict);
        updateValidator = new Mock<IValidator<ClientLicenseUpdateModel>>(MockBehavior.Strict);
        return new ClientLicenseController(logger.Object, business.Object, updateValidator.Object);
    }

    [Fact]
    public async Task GetAsync_ReturnsOk_WithQueryable()
    {
        // Arrange: business returns a queryable list
        var controller = CreateController(out var business, out var updateV);
        var data = new List<ClientLicenseViewModel> { new() { RowId = Guid.NewGuid() } }.AsQueryable();
        business.Setup(b => b.GetAsync()).ReturnsAsync(data);

        // Act
        var result = await controller.GetAsync();

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Same(data, ok.Value);
        business.VerifyAll();
    }

    [Fact]
    public async Task GetByRowIdAsync_ReturnsOk_WhenFound()
    {
        // Arrange: business returns a single item for the given id
        var controller = CreateController(out var business, out var updateV);
        var id = Guid.NewGuid();
        var item = new ClientLicenseViewModel { RowId = id };
        business.Setup(b => b.GetByRowIdAsync(id)).ReturnsAsync(item);

        // Act
        var result = await controller.GetByRowIdAsync(id);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(item, ok.Value);
        business.VerifyAll();
    }

    [Fact]
    public async Task GetByRowIdAsync_ReturnsNotFound_WhenMissing()
    {
        // Arrange: business returns null for missing item
        var controller = CreateController(out var business, out var updateV);
        var id = Guid.NewGuid();
        business.Setup(b => b.GetByRowIdAsync(id)).ReturnsAsync((ClientLicenseViewModel?)null);

        // Act
        var result = await controller.GetByRowIdAsync(id);

        // Assert
        var notFound = Assert.IsType<Microsoft.AspNetCore.OData.Results.NotFoundODataResult>(result);
        business.VerifyAll();
    }

    [Fact]
    public async Task PutAsync_ReturnsNoContent_OnValidInput()
    {
        // Arrange: validator passes; business updates successfully
        var controller = CreateController(out var business, out var updateV);
        var id = Guid.NewGuid();
        var model = new ClientLicenseUpdateModel();
        updateV.Setup(v => v.ValidateAsync(model, It.IsAny<CancellationToken>()))
               .ReturnsAsync(new ValidationResult());
        business.Setup(b => b.UpdateAsync(id, model)).ReturnsAsync(1);

        // Act
        var result = await controller.PutAsync(id, model);

        // Assert
        Assert.IsType<NoContentResult>(result);
        business.VerifyAll();
    }

    [Fact]
    public async Task PutAsync_ReturnsBadRequest_WhenValidationFails()
    {
        // Arrange: validator fails; business must not be called
        var controller = CreateController(out var business, out var updateV);
        var id = Guid.NewGuid();
        var model = new ClientLicenseUpdateModel();
        var failures = new List<ValidationFailure> { new("LicenseKey", "required") };
        updateV.Setup(v => v.ValidateAsync(model, It.IsAny<CancellationToken>()))
               .ReturnsAsync(new ValidationResult(failures));

        // Act
        var result = await controller.PutAsync(id, model);

        // Assert
        var bad = Assert.IsType<BadRequestObjectResult>(result);
        var errors = Assert.IsAssignableFrom<IEnumerable<ValidationFailure>>(bad.Value);
        Assert.Single(errors);
        business.Verify(b => b.UpdateAsync(It.IsAny<Guid>(), It.IsAny<ClientLicenseUpdateModel>()), Times.Never);
    }

    [Fact]
    public async Task PutAsync_ReturnsNotFound_WhenLicenseNotFound()
    {
        // Arrange: validator passes but business throws KeyNotFoundException
        var controller = CreateController(out var business, out var updateV);
        var id = Guid.NewGuid();
        var model = new ClientLicenseUpdateModel();
        updateV.Setup(v => v.ValidateAsync(model, It.IsAny<CancellationToken>()))
               .ReturnsAsync(new ValidationResult());
        business.Setup(b => b.UpdateAsync(id, model)).ThrowsAsync(new KeyNotFoundException());

        // Act
        var result = await controller.PutAsync(id, model);

        // Assert
        var notFound = Assert.IsType<Microsoft.AspNetCore.OData.Results.NotFoundODataResult>(result);
        business.VerifyAll();
    }

    [Fact]
    public async Task DeleteAsync_ReturnsNoContent_OnSuccess()
    {
        // Arrange: business deletes successfully
        var controller = CreateController(out var business, out var updateV);
        var id = Guid.NewGuid();
        business.Setup(b => b.DeleteAsync(id)).ReturnsAsync(1);

        // Act
        var result = await controller.DeleteAsync(id);

        // Assert
        Assert.IsType<NoContentResult>(result);
        business.VerifyAll();
    }

    [Fact]
    public async Task DeleteAsync_ReturnsNotFound_WhenMissing()
    {
        // Arrange: business throws KeyNotFoundException
        var controller = CreateController(out var business, out var updateV);
        var id = Guid.NewGuid();
        business.Setup(b => b.DeleteAsync(id)).ThrowsAsync(new KeyNotFoundException());

        // Act
        var result = await controller.DeleteAsync(id);

        // Assert
        var notFound = Assert.IsType<Microsoft.AspNetCore.OData.Results.NotFoundODataResult>(result);
        business.VerifyAll();
    }
}