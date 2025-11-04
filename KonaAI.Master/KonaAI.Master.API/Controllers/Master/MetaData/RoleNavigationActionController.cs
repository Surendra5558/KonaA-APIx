using KonaAI.Master.Business.Master.MetaData.Logic.Interface;
using KonaAI.Master.Model.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace KonaAI.Master.API.Controllers.Master.MetaData;

/// <summary>
/// OData controller to manage RoleNavigationUserAction entities.
/// Provides endpoints to retrieve RoleNavigationUserAction data.
/// </summary>
[Authorize]
public class RoleNavigationUserActionController(
        ILogger<RoleNavigationUserActionController> logger,
        IRoleNavigationUserActionBusiness roleNavigationUserActionBusiness
    ) : ODataController
{
    private const string ClassName = nameof(RoleNavigationUserActionController);

    /// <summary>
    /// Retrieves all RoleNavigationUserAction records asynchronously.
    /// </summary>
    /// <returns>
    /// Returns an <see cref="IActionResult"/> containing a collection of RoleNavigationUserAction records with status code 200 on success,
    /// or status code 500 in case of an error.
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
            var result = await roleNavigationUserActionBusiness.GetAsync();
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
    /// Retrieves a role-navigation action mapping by its row identifier (GUID).
    /// </summary>
    /// <param name="rowId">The unique row identifier (GUID) of the mapping.</param>
    /// <returns>200 OK with the mapping; 404 if not found; 500 on error.</returns>
    [HttpGet("v1/RoleNavigationUserAction({rowId:guid})")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByRowIdAsync([FromRoute] Guid rowId)
    {
        const string methodName = $"{ClassName}: {nameof(GetByRowIdAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - method execution started for id: {RowId}", methodName, rowId);
            var item = (await roleNavigationUserActionBusiness.GetAsync()).FirstOrDefault(x => x.RowId == rowId);
            if (item == null)
            {
                logger.LogWarning("{MethodName} - RoleNavigationUserAction with id {Id} not found", methodName, rowId);
                return NotFound($"RoleNavigationUserAction with id {rowId} not found");
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