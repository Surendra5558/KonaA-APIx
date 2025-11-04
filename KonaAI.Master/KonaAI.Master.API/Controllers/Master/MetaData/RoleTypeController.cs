using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace KonaAI.Master.API.Controllers.Master.MetaData;

/// <summary>
/// Controller for retrieving role type master data.
/// </summary>
[Authorize]
public class RoleTypeController() : ODataController
{
    private const string ClassName = nameof(RoleTypeController);

    /// <summary>
    /// Retrieves all role types asynchronously with OData query support.
    /// </summary>
    /// <returns>An <see cref="IActionResult"/> containing the role type data.</returns>
    [HttpGet]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult Get()
    {
        const string methodName = $"{ClassName}: {nameof(Get)}";
        try
        {
            //logger.LogInformation("{MethodName} - execution started", methodName);

            //var result = await roleTypeBusiness.GetAsync();

            //logger.LogInformation("{MethodName} - retrieved {Count} records", methodName, result.Count());
            return Ok();
        }
        catch (Exception ex)
        {
            // logger.LogError("{MethodName} - Error: {Error}", methodName, ex.Message);
            return StatusCode(500, ex.Message);
        }
        finally
        {
            // logger.LogInformation("{MethodName} - execution completed", methodName);
        }
    }
}