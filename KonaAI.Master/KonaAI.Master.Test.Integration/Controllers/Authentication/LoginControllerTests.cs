using FluentValidation;
using FluentValidation.Results;
using KonaAI.Master.API.Controllers.Authentication;
using KonaAI.Master.Business.Authentication.Logic.Interface;
using KonaAI.Master.Model.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace KonaAI.Master.Test.Integration.Controllers.Authentication;

/// <summary>
/// Integration-style tests for <see cref="KonaAI.Master.API.Controllers.Authentication.LoginController"/>.
/// Verifies:
/// - 200 OK on valid credentials with token payload
/// - 400 BadRequest on validation failures
/// - 401 Unauthorized on authentication failures
/// - Client IP resolution order (X-Forwarded-For → X-Real-IP → RemoteIp)
/// - User-Agent propagation to business layer
/// </summary>
public class LoginControllerTests
{
    // Helper to build a controller instance with mocked dependencies and a configured HttpContext.
    // We optionally pass header values and a remote IP to exercise client IP resolution logic.
    private static LoginController CreateController(
        out Mock<IUserLoginBusiness> businessMock,
        out Mock<IValidator<TokenFormRequest>> validatorMock,
        string? xForwardedFor = null,
        string? xRealIp = null,
        string? remoteIp = "203.0.113.10",
        string userAgent = "KonaAI.Test/1.0")
    {
        var logger = new Mock<ILogger<LoginController>>();
        businessMock = new Mock<IUserLoginBusiness>(MockBehavior.Strict);
        validatorMock = new Mock<IValidator<TokenFormRequest>>(MockBehavior.Strict);

        var controller = new LoginController(logger.Object, validatorMock.Object, businessMock.Object);

        var httpContext = new DefaultHttpContext();
        // Prefer X-Forwarded-For (first IP) when present
        if (!string.IsNullOrEmpty(xForwardedFor))
            httpContext.Request.Headers["X-Forwarded-For"] = xForwardedFor;
        // Fallback: X-Real-IP if X-Forwarded-For is not provided
        if (!string.IsNullOrEmpty(xRealIp))
            httpContext.Request.Headers["X-Real-IP"] = xRealIp;
        // Always set a test user-agent so we can assert it flows to business layer
        httpContext.Request.Headers["User-Agent"] = userAgent;

        // Final fallback: RemoteIpAddress when no headers are set
        if (!string.IsNullOrEmpty(remoteIp))
            httpContext.Connection.RemoteIpAddress = IPAddress.Parse(remoteIp);

        controller.ControllerContext = new ControllerContext { HttpContext = httpContext };
        return controller;
    }

    // Creates a valid token request payload used across tests
    private static TokenFormRequest CreateValidRequest() =>
        new()
        {
            UserName = "user@example.com",
            Password = "P@ssw0rd!",
            GrantType = "password"
        };

    [Fact]
    public async Task PostAsync_ReturnsOk_OnValidCredentials()
    {
        // Arrange: validation succeeds and business returns a token response
        var controller = CreateController(out var business, out var validator);
        var request = CreateValidRequest();

        validator
            .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        var expectedResponse = new TokenResponse
        {
            Name = "John Doe",
            Token = "jwt-token",
            RefreshToken = "refresh-token",
            RoleId = 1,
            RoleName = "Admin"
        };

        business
            .Setup(b => b.AuthenticateUserAsync(
                It.Is<TokenFormRequest>(r => r.UserName == request.UserName),
                It.IsAny<string>(),
                It.Is<string>(ua => ua == "KonaAI.Test/1.0")))
            .ReturnsAsync(expectedResponse);

        // Act: call the endpoint
        var result = await controller.PostAsync(request);

        // Assert: 200 OK and payload matches business response
        var ok = Assert.IsType<OkObjectResult>(result);
        var payload = Assert.IsType<TokenResponse>(ok.Value);
        Assert.Equal(expectedResponse.Token, payload.Token);
        Assert.Equal(expectedResponse.RefreshToken, payload.RefreshToken);
        Assert.Equal(expectedResponse.RoleId, payload.RoleId);
        Assert.Equal(expectedResponse.RoleName, payload.RoleName);

        validator.Verify(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()), Times.Once);
        business.VerifyAll();
    }

    [Fact]
    public async Task PostAsync_ReturnsBadRequest_WhenValidationFails()
    {
        // Arrange: validator returns two errors; business must not be called
        var controller = CreateController(out var business, out var validator);
        var request = CreateValidRequest();

        var failures = new List<ValidationFailure>
        {
            new("UserName", "UserName is required"),
            new("Password", "Password is required")
        };

        validator
            .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(failures));

        // Act
        var result = await controller.PostAsync(request);

        // Assert: 400 with the same number of validation failures; no authentication attempted
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        var errors = Assert.IsAssignableFrom<IEnumerable<ValidationFailure>>(badRequest.Value);
        Assert.Equal(2, errors.Count());

        business.Verify(b => b.AuthenticateUserAsync(It.IsAny<TokenFormRequest>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        validator.Verify(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task PostAsync_ReturnsUnauthorized_WhenAuthenticationFails()
    {
        // Arrange: validation passes but business throws AuthenticationException
        var controller = CreateController(out var business, out var validator);
        var request = CreateValidRequest();

        validator
            .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        business
            .Setup(b => b.AuthenticateUserAsync(It.IsAny<TokenFormRequest>(), It.IsAny<string>(), It.IsAny<string>()))
            .ThrowsAsync(new System.Security.Authentication.AuthenticationException("Invalid username or password."));

        // Act
        var result = await controller.PostAsync(request);

        // Assert: 401 with error message surfaced by controller
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(401, objectResult.StatusCode);
        Assert.Equal("Invalid username or password.", objectResult.Value);

        validator.Verify(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()), Times.Once);
        business.VerifyAll();
    }

    [Theory]
    [InlineData("198.51.100.7, 10.0.0.1", "198.51.100.7")]
    [InlineData("203.0.113.99", "203.0.113.99")]
    public async Task PostAsync_Uses_XForwardedFor_FirstIP(string xForwardedFor, string expectedClientIp)
    {
        // Arrange: X-Forwarded-For has multiple IPs; expect the first one to be used
        var controller = CreateController(out var business, out var validator, xForwardedFor: xForwardedFor, remoteIp: "127.0.0.1");
        var request = CreateValidRequest();

        validator
            .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        business
            .Setup(b => b.AuthenticateUserAsync(It.IsAny<TokenFormRequest>(), expectedClientIp, "KonaAI.Test/1.0"))
            .ReturnsAsync(new TokenResponse { Name = "n", Token = "t", RefreshToken = "r", RoleId = 1, RoleName = "Admin" });

        // Act
        var result = await controller.PostAsync(request);

        // Assert: authentication is called with the resolved client IP
        Assert.IsType<OkObjectResult>(result);
        business.VerifyAll();
    }

    [Theory]
    [InlineData(null, "203.0.113.55", "127.0.0.1", "203.0.113.55")] // X-Real-IP present
    [InlineData(null, null, "198.51.100.9", "198.51.100.9")]        // Remote IP fallback
    public async Task PostAsync_ResolvesClientIp_FromHeadersOrConnection(
        string? xForwardedFor,
        string? xRealIp,
        string? remoteIp,
        string expectedClientIp)
    {
        // Arrange: exercise fallback order X-Forwarded-For -> X-Real-IP -> RemoteIpAddress
        var controller = CreateController(out var business, out var validator, xForwardedFor, xRealIp, remoteIp);
        var request = CreateValidRequest();

        validator
            .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        business
            .Setup(b => b.AuthenticateUserAsync(It.IsAny<TokenFormRequest>(), expectedClientIp, "KonaAI.Test/1.0"))
            .ReturnsAsync(new TokenResponse { Name = "n", Token = "t", RefreshToken = "r", RoleId = 1, RoleName = "Admin" });

        // Act
        var result = await controller.PostAsync(request);

        // Assert: business is called with the expected client IP based on provided inputs
        Assert.IsType<OkObjectResult>(result);
        business.VerifyAll();
    }
}