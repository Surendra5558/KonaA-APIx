using FluentValidation;
using KonaAI.Master.API.Controllers.Authentication;
using KonaAI.Master.Business.Authentication.Logic.Interface;
using KonaAI.Master.Model.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace KonaAI.Master.Test.Integration.Controllers.Authentication;

/// <summary>
/// Comprehensive integration tests for <see cref="LoginController"/>.
/// Tests authentication flow with real controller execution.
/// </summary>
public class LoginControllerIntegrationTests
{
    private static LoginController CreateController(
        out Mock<IUserLoginBusiness> business,
        out Mock<IValidator<TokenFormRequest>> validator)
    {
        var logger = new Mock<ILogger<LoginController>>();
        business = new Mock<IUserLoginBusiness>(MockBehavior.Strict);
        validator = new Mock<IValidator<TokenFormRequest>>(MockBehavior.Strict);
        var controller = new LoginController(logger.Object, validator.Object, business.Object);
        // Setup HttpContext for IP and User-Agent
        var httpContext = new Mock<HttpContext>();
        var request = new Mock<HttpRequest>();
        var headers = new HeaderDictionary();
        headers["User-Agent"] = "Test-Agent";
        headers["X-Forwarded-For"] = "127.0.0.1";
        request.Setup(r => r.Headers).Returns(headers);
        httpContext.Setup(c => c.Request).Returns(request.Object);
        httpContext.Setup(c => c.Connection.RemoteIpAddress).Returns(System.Net.IPAddress.Parse("127.0.0.1"));
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext.Object
        };

        return controller;
    }

    [Fact]
    public async Task PostAsync_ValidCredentials_Returns200Ok()
    {
        // Arrange
        var controller = CreateController(out var business, out var validator);
        var tokenRequest = new TokenFormRequest
        {
            UserName = "test@example.com",
            Password = "password123",
            GrantType = "password"
        };

        var tokenResponse = new TokenResponse
        {
            Name = "Test User",
            Token = "jwt-token",
            RoleId = 1,
            RefreshToken = "refresh-token",
            RoleName = "Admin",
            ClientId = 1,
            ClientName = "Test Client"
        };

        validator.Setup(v => v.ValidateAsync(tokenRequest, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        business.Setup(b => b.AuthenticateUserAsync(tokenRequest, It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(tokenResponse);

        // Act
        var result = await controller.PostAsync(tokenRequest);

        // Assert
        Assert.IsType<OkObjectResult>(result);
        var okResult = (OkObjectResult)result;
        Assert.NotNull(okResult.Value);
        validator.Verify(v => v.ValidateAsync(tokenRequest, It.IsAny<CancellationToken>()), Times.Once);
        business.Verify(b => b.AuthenticateUserAsync(tokenRequest, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task PostAsync_InvalidCredentials_Returns401Unauthorized()
    {
        // Arrange
        var controller = CreateController(out var business, out var validator);
        var tokenRequest = new TokenFormRequest
        {
            UserName = "test@example.com",
            Password = "wrongpassword"
        };

        validator.Setup(v => v.ValidateAsync(tokenRequest, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        business.Setup(b => b.AuthenticateUserAsync(tokenRequest, It.IsAny<string>(), It.IsAny<string>()))
            .ThrowsAsync(new System.Security.Authentication.AuthenticationException("Invalid credentials"));

        // Act
        var result = await controller.PostAsync(tokenRequest);

        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = (ObjectResult)result;
        Assert.Equal(401, objectResult.StatusCode);
        Assert.Equal("Invalid credentials", objectResult.Value);
        validator.Verify(v => v.ValidateAsync(tokenRequest, It.IsAny<CancellationToken>()), Times.Once);
        business.Verify(b => b.AuthenticateUserAsync(tokenRequest, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task PostAsync_ValidationFails_Returns400BadRequest()
    {
        // Arrange
        var controller = CreateController(out var business, out var validator);
        var tokenRequest = new TokenFormRequest
        {
            UserName = "",
            Password = "",
            GrantType = "password"
        };

        var validationResult = new FluentValidation.Results.ValidationResult();
        validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure("UserName", "UserName is required"));
        validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure("Password", "Password is required"));

        validator.Setup(v => v.ValidateAsync(tokenRequest, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await controller.PostAsync(tokenRequest);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        var badRequestResult = (BadRequestObjectResult)result;
        Assert.IsType<List<FluentValidation.Results.ValidationFailure>>(badRequestResult.Value);
        validator.Verify(v => v.ValidateAsync(tokenRequest, It.IsAny<CancellationToken>()), Times.Once);
        business.Verify(b => b.AuthenticateUserAsync(It.IsAny<TokenFormRequest>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task PostAsync_BusinessThrows_Returns500()
    {
        // Arrange
        var controller = CreateController(out var business, out var validator);
        var tokenRequest = new TokenFormRequest
        {
            UserName = "test@example.com",
            Password = "password123",
            GrantType = "password"
        };

        validator.Setup(v => v.ValidateAsync(tokenRequest, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        business.Setup(b => b.AuthenticateUserAsync(tokenRequest, It.IsAny<string>(), It.IsAny<string>()))
            .ThrowsAsync(new Exception("Database connection failed"));

        // Act
        var result = await controller.PostAsync(tokenRequest);

        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = (ObjectResult)result;
        Assert.Equal(500, objectResult.StatusCode);
        Assert.Equal("An internal server error occurred.", objectResult.Value);
        validator.Verify(v => v.ValidateAsync(tokenRequest, It.IsAny<CancellationToken>()), Times.Once);
        business.Verify(b => b.AuthenticateUserAsync(tokenRequest, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task PostAsync_ValidatorThrows_Returns500()
    {
        // Arrange
        var controller = CreateController(out var business, out var validator);
        var tokenRequest = new TokenFormRequest
        {
            UserName = "test@example.com",
            Password = "password123",
            GrantType = "password"
        };

        validator.Setup(v => v.ValidateAsync(tokenRequest, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Validation service error"));

        // Act
        var result = await controller.PostAsync(tokenRequest);

        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = (ObjectResult)result;
        Assert.Equal(500, objectResult.StatusCode);
        Assert.Equal("An internal server error occurred.", objectResult.Value);
        validator.Verify(v => v.ValidateAsync(tokenRequest, It.IsAny<CancellationToken>()), Times.Once);
        business.Verify(b => b.AuthenticateUserAsync(It.IsAny<TokenFormRequest>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }
}