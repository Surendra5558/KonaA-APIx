using FluentValidation;
using KonaAI.Master.Business.Authentication.Logic.Interface;
using KonaAI.Master.Model.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace KonaAI.Master.API.Controllers.Authentication;

/// <summary>
/// Controller responsible for handling user authentication and issuing JWT tokens.
/// </summary>
/// <param name="logger">The logger instance for logging information and errors.</param>
/// <param name="validator">The validator for validating <see cref="TokenFormRequest"/> instances.</param>
/// <param name="userLoginBusiness">The business service for user login validation.</param>
public class LoginController(
    ILogger<LoginController> logger,
    IValidator<TokenFormRequest> validator,
    IUserLoginBusiness userLoginBusiness)
    : ODataController
{
    private const string ClassName = nameof(LoginController);

    /// <summary>
    /// Authenticates a user and issues a JWT token if the credentials are valid.
    /// </summary>
    /// <param name="tokenFormRequest">The authentication request containing user credentials and grant type.</param>
    /// <returns>
    /// Returns <see cref="TokenResponse"/> with the JWT token and user details if authentication is successful; otherwise an error response.
    /// </returns>
    /// <remarks>
    /// Client IP is resolved using the following precedence:
    /// 1) X-Forwarded-For (first IP when multiple)
    /// 2) X-Real-IP
    /// 3) HttpContext.Connection.RemoteIpAddress
    /// The resolved IP and User-Agent are passed to the business layer for auditing and security checks.
    /// </remarks>
    /// <response code="200">Returns the authentication token and user information.</response>
    /// <response code="400">If the request is invalid or validation fails.</response>
    /// <response code="401">If authentication fails.</response>
    /// <response code="404">If the user is not found.</response>
    /// <response code="500">If an unexpected error occurs.</response>
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PostAsync([FromBody] TokenFormRequest tokenFormRequest)
    {
        const string methodName = $"{ClassName}: {nameof(PostAsync)}";

        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);

            // Validate request
            var validationResult = await validator.ValidateAsync(tokenFormRequest);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            // Get client IP and User Agent for business layer
            var clientIpAddress = GetClientIpAddress();
            var userAgent = Request.Headers["User-Agent"].ToString();

            // Process authentication through business layer
            var tokenResponse = await userLoginBusiness.AuthenticateUserAsync(
                tokenFormRequest,
                clientIpAddress,
                userAgent);

            logger.LogInformation("User {Email} logged in successfully", tokenFormRequest.UserName);
            return Ok(tokenResponse);
        }
        catch (System.Security.Authentication.AuthenticationException ex)
        {
            logger.LogWarning("{MethodName} - Authentication failed: {Message}", methodName, ex.Message);
            return StatusCode(401, ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError("{MethodName} - Error in execution: {Message}", methodName, ex.Message);
            return StatusCode(500, "An internal server error occurred.");
        }
        finally
        {
            logger.LogInformation("{MethodName} - method execution completed", methodName);
        }
    }

    /// <summary>
    /// Gets the client's IP address from the request.
    /// </summary>
    /// <returns>The client's IP address or "Unknown" if not found.</returns>
    private string GetClientIpAddress()
    {
        // Check for forwarded IP first (in case of proxy/load balancer)
        var forwardedFor = Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrEmpty(forwardedFor))
        {
            return forwardedFor.Split(',')[0].Trim();
        }

        // Check for real IP
        var realIp = Request.Headers["X-Real-IP"].FirstOrDefault();
        if (!string.IsNullOrEmpty(realIp))
        {
            return realIp;
        }

        // Fallback to connection remote IP
        return Request.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
    }
}