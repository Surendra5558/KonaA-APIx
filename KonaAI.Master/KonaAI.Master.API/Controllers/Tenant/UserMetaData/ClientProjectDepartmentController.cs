using KonaAI.Master.Business.Tenant.UserMetaData.Logic.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace KonaAI.Master.API.Controllers.Tenant.UserMetaData;

/// <summary>
/// Controller responsible for handling HTTP requests related to Business Departments.
/// Provides endpoints to retrieve Business Department data.
/// </summary>
[Authorize]
public class ClientProjectDepartmentController(
        ILogger<ClientProjectDepartmentController> logger,
        IClientProjectDepartmentBusiness clientProjectDepartmentBusiness
    ) : ODataController
{
    private const string ClassName = nameof(ClientProjectDepartmentController);

    /// <summary>
    /// Retrieves all business departments asynchronously.
    /// Supports OData query options for filtering, sorting, and paging.
    /// </summary>
    /// <returns>
    /// <see cref="IActionResult"/> containing a list of business departments with HTTP 200 status code on success,
    /// or HTTP 500 status code if an exception occurs.
    /// </returns>
    /// <response code="200">Returns the list of business departments.</response>
    /// <response code="500">If an internal server error occurs.</response>
    [HttpGet]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAsync()
    {
        const string methodName = $"{ClassName}: {nameof(GetAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);
            var result = await clientProjectDepartmentBusiness.GetAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError("{MethodName} - Error: {Error}", methodName, ex.Message);
            return StatusCode(500, ex.Message);
        }
        finally
        {
            logger.LogInformation("{MethodName} - method execution completed", methodName);
        }
    }
}