using KonaAI.Master.Business.Master.MetaData.Logic.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace KonaAI.Master.API.Controllers.Master.MetaData;

/// <summary>
/// OData controller for retrieving master modules.
/// </summary>
[Authorize]
public class ModuleController(
    ILogger<ModuleController> logger,
    IModuleTypeBusiness moduleBusiness) : ODataController
{
    private const string ClassName = nameof(ModuleController);

    /// <summary>
    /// Retrieves all master modules (RowId, Name, Description, OrderBy).
    /// </summary>
    /// <returns>
    /// 200 with a queryable list; 401 if unauthorized; 500 on errors.
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
            var result = await moduleBusiness.GetAsync();
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
    /// Retrieves a module by its row identifier (GUID).
    /// </summary>
    /// <param name="rowId">The unique row identifier (GUID) of the module.</param>
    /// <returns>200 OK with the module; 404 if not found; 500 on error.</returns>
    [HttpGet("v1/Module({rowId:guid})")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByRowIdAsync([FromRoute] Guid rowId)
    {
        const string methodName = $"{ClassName}: {nameof(GetByRowIdAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - method execution started for id: {RowId}", methodName, rowId);
            var query = await moduleBusiness.GetAsync();
            var item = query.FirstOrDefault(x => x.RowId == rowId);
            if (item == null)
            {
                logger.LogWarning("{MethodName} - Module with id {Id} not found", methodName, rowId);
                return NotFound($"Module with id {rowId} not found");
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