using FluentValidation;
using FluentValidation.Results;
using KonaAI.Master.API.Controllers.Master.MetaData;
using KonaAI.Master.Model.Common;
using KonaAI.Master.Model.Master.SaveModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.Extensions.Logging;
using Moq;

namespace KonaAI.Master.Test.Integration.Controllers.Master.MetaData;

/// <summary>
/// Integration tests for <see cref="KonaAI.Master.API.Controllers.Master.MetaData.RenderTypeController"/>.
/// Covers:
/// - GET list (200) with OData support
/// - POST creation (201, 400 validation)
/// - Error handling (500)
/// - Performance scenarios
/// </summary>
public class RenderTypeControllerTests
{
    private static RenderTypeController CreateController(
        out Mock<IRenderTypeBusiness> business,
        out Mock<IValidator<RenderTypeCreateModel>> createValidator)
    {
        var logger = new Mock<ILogger<RenderTypeController>>();
        business = new Mock<IRenderTypeBusiness>(MockBehavior.Strict);
        createValidator = new Mock<IValidator<RenderTypeCreateModel>>(MockBehavior.Strict);
        return new RenderTypeController(logger.Object, createValidator.Object, business.Object);
    }

    #region GET Tests

    [Fact]
    public async Task GetAsync_ReturnsOk_WithQueryable()
    {
        // Arrange: business returns a queryable list
        var controller = CreateController(out var business, out var createV);
        var data = new List<MetaDataViewModel>
        {
            new() { RowId = Guid.NewGuid(), Name = "Render Type 1" },
            new() { RowId = Guid.NewGuid(), Name = "Render Type 2" }
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
    public async Task GetAsync_WithValidData_ReturnsRenderTypeList()
    {
        // Arrange: business returns render type data
        var controller = CreateController(out var business, out var createV);
        var data = new List<MetaDataViewModel>
        {
            new() { RowId = Guid.NewGuid(), Name = "HTML Renderer", Description = "HTML rendering type" },
            new() { RowId = Guid.NewGuid(), Name = "PDF Renderer", Description = "PDF rendering type" }
        }.AsQueryable();

        business.Setup(b => b.GetAsync()).ReturnsAsync(data);

        // Act
        var result = await controller.GetAsync();

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        var resultData = Assert.IsAssignableFrom<IQueryable<MetaDataViewModel>>(ok.Value);
        Assert.Equal(2, resultData.Count());
        business.VerifyAll();
    }

    [Fact]
    public async Task GetAsync_WithException_ReturnsInternalServerError()
    {
        // Arrange: business throws exception
        var controller = CreateController(out var business, out var createV);
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

    #region POST Tests

    [Fact]
    public async Task PostAsync_ValidModel_ReturnsCreatedResult()
    {
        // Arrange: validation passes, business succeeds
        var controller = CreateController(out var business, out var createV);
        var model = new RenderTypeCreateModel
        {
            Name = "New Render Type"
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
    public async Task PostAsync_ValidModel_CreatesRenderType()
    {
        // Arrange: validation passes, business creates render type
        var controller = CreateController(out var business, out var createV);
        var model = new RenderTypeCreateModel
        {
            Name = "JSON Renderer"
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
        var controller = CreateController(out var business, out var createV);
        var model = new RenderTypeCreateModel { Name = "" };
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
        business.Verify(b => b.CreateAsync(It.IsAny<RenderTypeCreateModel>()), Times.Never);
    }

    [Fact]
    public async Task PostAsync_WithDbException_ReturnsInternalServerError()
    {
        // Arrange: validation passes, business throws exception
        var controller = CreateController(out var business, out var createV);
        var model = new RenderTypeCreateModel { Name = "Test Render Type" };
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
        var controller = CreateController(out var business, out var createV);
        var data = Enumerable.Range(1, 1000)
            .Select(i => new MetaDataViewModel
            {
                RowId = Guid.NewGuid(),
                Name = $"Render Type {i}",
                Description = $"Description for render type {i}"
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
        var controller = CreateController(out var business, out var createV);
        var allData = new List<MetaDataViewModel>
        {
            new() { RowId = Guid.NewGuid(), Name = "HTML Renderer" },
            new() { RowId = Guid.NewGuid(), Name = "PDF Renderer" }
        }.AsQueryable();
        var createModel = new RenderTypeCreateModel { Name = "New Renderer" };
        var validationResult = new ValidationResult();

        business.Setup(b => b.GetAsync()).ReturnsAsync(allData);
        createV.Setup(v => v.ValidateAsync(createModel, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);
        business.Setup(b => b.CreateAsync(createModel)).ReturnsAsync(1);

        // Act & Assert - Get All
        var getAllResult = await controller.GetAsync();
        var getAllOk = Assert.IsType<OkObjectResult>(getAllResult);
        Assert.Equal(allData, getAllOk.Value);

        // Act & Assert - Create
        var createResult = await controller.PostAsync(createModel);
        Assert.IsType<CreatedResult>(createResult);

        // Verify all interactions
        createV.VerifyAll();
        business.VerifyAll();
    }

    #endregion
}