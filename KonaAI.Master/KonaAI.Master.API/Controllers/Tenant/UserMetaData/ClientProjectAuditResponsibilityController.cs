using KonaAI.Master.Business.Tenant.UserMetaData.Logic.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace KonaAI.Master.API.Controllers.Tenant.UserMetaData;

/// <summary>
/// Controller for retrieving audit responsibility master data.
/// Provides endpoints to query audit responsibility records using OData.
/// </summary>
[Authorize]
public class ClientProjectAuditResponsibilityController(
        ILogger<ClientProjectAuditResponsibilityController> logger,
        IClientProjectAuditResponsibilityBusiness clientProjectAuditResponsibilityBusiness
    ) : ODataController
{
    private const string ClassName = nameof(ClientProjectAuditResponsibilityController);

    /// <summary>
    /// Gets the asynchronous.
    /// </summary>
    /// <returns></returns>
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
            logger.LogInformation("{MethodName} - execution started", methodName);

            var result = await clientProjectAuditResponsibilityBusiness.GetAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError("{MethodName} - Error: {Error}", methodName, ex.Message);
            return StatusCode(500, ex.Message);
        }
        finally
        {
            logger.LogInformation("{MethodName} - execution completed", methodName);
        }
    }
}