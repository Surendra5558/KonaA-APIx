using KonaAI.Master.Business.Authentication.Logic.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace KonaAI.Master.API.Controllers.Authentication;

/// <summary>
/// Controller for handling application menu and navigation related endpoints.
/// </summary>
[Authorize]
public class MenuController(ILogger<MenuController> logger, IMenuBusiness menuBusiness) : ODataController
{
    private const string ClassName = nameof(MenuController);

    /// <summary>
    /// Retrieves all application navigation entries.
    /// </summary>
    /// <returns>
    /// Returns a list of navigation entries with status 200 if successful, or status 500 if an error occurs.
    /// </returns>
    /// <response code="200">Returns the list of navigation entries.</response>
    /// <response code="401">If the user is unauthorized.</response>
    /// <response code="403">If the user is forbidden from accessing this resource.</response>
    /// <response code="500">If an error occurs, returns the error message.</response>
    [HttpGet]
    [EnableQuery]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAsync()
    {
        const string methodName = $"{ClassName}: {nameof(GetAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);
            var result = await menuBusiness.GetAsync();
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