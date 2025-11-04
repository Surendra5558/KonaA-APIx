using KonaAI.Master.Business.Tenant.UserMetaData.Logic.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace KonaAI.Master.API.Controllers.Tenant.UserMetaData;

/// <summary>
/// OData controller that exposes client project business unit master data.
/// </summary>
/// <param name="logger">The logger used for structured logging within this controller.</param>
/// <param name="clientProjectUnitBusiness">
/// The business service that provides access to business unit metadata.
/// </param>
/// <remarks>
/// OData query options are enabled via <see cref="EnableQueryAttribute"/> allowing clients to use
/// $filter, $select, $orderby, $top, and $skip.
/// </remarks>
[Authorize]
public class ClientProjectUnitController(
    ILogger<ClientProjectUnitController> logger,
    IClientProjectUnitBusiness clientProjectUnitBusiness) : ODataController
{
    /// <summary>
    /// A constant used to prefix log messages with the controller name.
    /// </summary>
    private const string ClassName = nameof(ClientProjectUnitController);

    /// <summary>
    /// Retrieves all available business units.
    /// </summary>
    /// <returns>
    /// An <see cref="IActionResult"/> containing the queryable collection of business units.
    /// On success returns 200 OK with an <see cref="IQueryable{T}"/> of <c>MetaDataViewModel</c>.
    /// </returns>
    /// <remarks>
    /// - 200 OK: Successfully retrieved the collection.
    /// - 401 Unauthorized: The request is not authenticated.
    /// - 500 Internal Server Error: An unexpected error occurred.
    /// OData query options are supported and applied server-side.
    /// </remarks>
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

            var result = await clientProjectUnitBusiness.GetAsync();

            logger.LogInformation("{MethodName} - retrieved {Count} records", methodName, result.Count());
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