using KonaAI.Master.Business.Master.MetaData.Logic.Interface;
using KonaAI.Master.Model.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace KonaAI.Master.API.Controllers.Master.MetaData;

/// <summary>
/// OData controller for managing navigation actions.
/// Provides endpoints to retrieve navigation action data.
/// </summary>
[Authorize]
public class NavigationActionController(
        ILogger<NavigationActionController> logger,
        INavigationUserActionBusiness navigationUserActionBusiness
    ) : ODataController
{
    private const string ClassName = nameof(NavigationActionController);

    /// <summary>
    /// Retrieves all navigation actions.
    /// </summary>
    /// <remarks>
    /// Supports OData query options such as $filter, $select, $expand, $orderby, and $top.
    /// </remarks>
    /// <returns>
    /// Returns a <see cref="IActionResult"/> containing the list of navigation actions with HTTP 200 status code on success.
    /// Returns HTTP 500 status code if an unhandled exception occurs.
    /// </returns>
    [HttpGet]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAsync()
    {
        const string methodName = $"{ClassName}: {nameof(GetAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);
            var result = await navigationUserActionBusiness.GetAsync();
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

    /// <summary>
    /// Retrieves a navigation action by its row identifier (GUID).
    /// </summary>
    /// <param name="rowId">The unique row identifier (GUID) of the navigation action.</param>
    /// <returns>200 OK with the action; 404 if not found; 500 on error.</returns>
    [HttpGet("v1/NavigationAction({rowId:guid})")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByRowIdAsync([FromRoute] Guid rowId)
    {
        const string methodName = $"{ClassName}: {nameof(GetByRowIdAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - method execution started for id: {RowId}", methodName, rowId);
            var item = (await navigationUserActionBusiness.GetAsync()).FirstOrDefault(x => x.RowId == rowId);
            if (item == null)
            {
                logger.LogWarning("{MethodName} - Navigation action with id {Id} not found", methodName, rowId);
                return NotFound($"Navigation action with id {rowId} not found");
            }
            return Ok(item);
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