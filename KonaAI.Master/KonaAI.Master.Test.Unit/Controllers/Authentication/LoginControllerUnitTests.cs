using FluentValidation;
using FluentValidation.Results;
using KonaAI.Master.API.Controllers.Authentication;
using KonaAI.Master.Business.Authentication.Logic.Interface;
using KonaAI.Master.Model.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Reflection;

namespace KonaAI.Master.Test.Unit.Controllers.Authentication;

/// <summary>
/// Unit tests for <see cref="LoginController"/>.
/// Covers:
/// - POST authentication (200, 401, 500)
/// - Validation error handling (400 responses)
/// - Business exceptions and logging
/// </summary>
public class LoginControllerUnitTests
{
    private readonly Mock<ILogger<LoginController>> _logger = new();
    private readonly Mock<IUserLoginBusiness> _business = new();
    private readonly Mock<IValidator<TokenFormRequest>> _validator = new();

    private LoginController CreateSut()
    {
        var controller = new LoginController(_logger.Object, _validator.Object, _business.Object);

        // Setup HttpContext
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

    #region PostAsync

    [Fact]
    public async Task PostAsync_ValidCredentials_Returns200Ok()
    {
        // Arrange
        var tokenRequest = new TokenFormRequest
        {
            UserName = "test@example.com",
            Password = "password123",
            GrantType = "password"
        };

        var tokenResponse = new TokenResponse
        {
            Name = "Test User",
            Token = "jwt-token-string",
            RoleId = 1,
            RefreshToken = "refresh-token",
            RoleName = "Admin"
        };

        var validationResult = new ValidationResult();
        _validator.Setup(v => v.ValidateAsync(tokenRequest, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        _business.Setup(b => b.AuthenticateUserAsync(tokenRequest, It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(tokenResponse);

        var sut = CreateSut();

        // Act
        var result = await sut.PostAsync(tokenRequest);

        // Assert
        Assert.IsType<OkObjectResult>(result);
        var okResult = (OkObjectResult)result;
        Assert.NotNull(okResult.Value);
        _validator.Verify(v => v.ValidateAsync(tokenRequest, It.IsAny<CancellationToken>()), Times.Once);
        _business.Verify(b => b.AuthenticateUserAsync(tokenRequest, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task PostAsync_ValidationFails_Returns400BadRequest()
    {
        // Arrange
        var tokenRequest = new TokenFormRequest
        {
            UserName = "", // Invalid - empty username
            Password = "password123",
            GrantType = "password"
        };

        var validationResult = new ValidationResult();
        validationResult.Errors.Add(new ValidationFailure("UserName", "Username is required"));
        _validator.Setup(v => v.ValidateAsync(tokenRequest, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        var sut = CreateSut();

        // Act
        var result = await sut.PostAsync(tokenRequest);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        var badRequestResult = (BadRequestObjectResult)result;
        Assert.Equal(400, badRequestResult.StatusCode);
        _validator.Verify(v => v.ValidateAsync(tokenRequest, It.IsAny<CancellationToken>()), Times.Once);
        _business.Verify(b => b.AuthenticateUserAsync(It.IsAny<TokenFormRequest>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task PostAsync_InvalidCredentials_Returns401Unauthorized()
    {
        // Arrange
        var tokenRequest = new TokenFormRequest
        {
            UserName = "test@example.com",
            Password = "wrongpassword",
            GrantType = "password"
        };

        var validationResult = new ValidationResult();
        _validator.Setup(v => v.ValidateAsync(tokenRequest, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        _business.Setup(b => b.AuthenticateUserAsync(tokenRequest, It.IsAny<string>(), It.IsAny<string>()))
            .ThrowsAsync(new System.Security.Authentication.AuthenticationException("Invalid username or password"));

        var sut = CreateSut();

        // Act
        var result = await sut.PostAsync(tokenRequest);

        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = (ObjectResult)result;
        Assert.Equal(401, objectResult.StatusCode);
        Assert.Equal("Invalid username or password", objectResult.Value);
    }

    [Fact]
    public async Task PostAsync_BusinessThrows_Returns500()
    {
        // Arrange
        var tokenRequest = new TokenFormRequest
        {
            UserName = "test@example.com",
            Password = "password123",
            GrantType = "password"
        };

        var validationResult = new ValidationResult();
        _validator.Setup(v => v.ValidateAsync(tokenRequest, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        _business.Setup(b => b.AuthenticateUserAsync(tokenRequest, It.IsAny<string>(), It.IsAny<string>()))
            .ThrowsAsync(new Exception("Database connection failed"));

        var sut = CreateSut();

        // Act
        var result = await sut.PostAsync(tokenRequest);

        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = (ObjectResult)result;
        Assert.Equal(500, objectResult.StatusCode);
        Assert.Equal("An internal server error occurred.", objectResult.Value);
    }

    #endregion PostAsync

    #region Authorization

    [Fact]
    public void Class_ShouldNotHaveAuthorizeAttribute()
    {
        var attr = typeof(LoginController).GetCustomAttribute<AuthorizeAttribute>(inherit: true);
        Assert.Null(attr);
    }

    #endregion Authorization
}