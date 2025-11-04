using KonaAI.Master.Business.Master.UserMetaData.Logic.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace KonaAI.Master.API.Controllers.Master.MetaData;

/// <summary>
/// OData controller for retrieving master countries.
/// </summary>
[Authorize]
public class CountryController(
    ILogger<CountryController> logger,
    ICountryBusiness countryBusiness
) : ODataController
{
    private const string ClassName = nameof(CountryController);

    /// <summary>
    /// Retrieves all master countries (RowId, Name, Description, OrderBy) with OData query support.
    /// </summary>
    /// <returns>
    /// 200 OK with a queryable list of countries; 401 if unauthorized; 500 on errors.
    /// </returns>
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
            var result = await countryBusiness.GetAsync();
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

    /// <summary>
    /// Retrieves a country by its row identifier (GUID).
    /// </summary>
    /// <param name="rowId">The unique row identifier (GUID) of the country.</param>
    /// <returns>200 OK with the country; 404 if not found; 500 on error.</returns>
    [HttpGet("v1/Country({rowId:guid})")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByRowIdAsync([FromRoute] Guid rowId)
    {
        const string methodName = $"{ClassName}: {nameof(GetByRowIdAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - method execution started for id: {RowId}", methodName, rowId);
            var query = await countryBusiness.GetAsync();
            // Force client-side evaluation to avoid EF Core translation issues when filtering on projected DTOs
            var item = query.AsEnumerable().FirstOrDefault(x => x.RowId == rowId);
            if (item == null)
            {
                logger.LogWarning("{MethodName} - Country with id {Id} not found", methodName, rowId);
                return NotFound($"Country with id {rowId} not found");
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