using KonaAI.Master.Business.Tenant.Client.Logic.Interface;
using KonaAI.Master.Model.Tenant.Client.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace KonaAI.Master.API.Controllers.Tenant.Client;

/// <summary>
/// OData controller that exposes client questionnaire sections and their associated questions.
/// </summary>
/// <param name="logger">Logger for diagnostic and operational events.</param>
/// <param name="clientQuestionnaireSectionBusiness">
/// Business service used to retrieve questionnaire section data.
/// </param>
public class ClientQuestionnaireSectionController(ILogger<ClientQuestionnaireSectionController> logger,
    IClientQuestionnaireSectionBusiness clientQuestionnaireSectionBusiness) : ODataController
{
    /// <summary>
    /// Class name used to enrich structured log messages.
    /// </summary>
    private const string ClassName = nameof(ClientQuestionnaireSectionController);

    /// <summary>
    /// Gets client questionnaire sections, including their questions.
    /// </summary>
    /// <remarks>
    /// - Requires authorization.<br/>
    /// - Supports OData query options via <see cref="EnableQueryAttribute"/> (e.g., $filter, $select, $expand, $orderby, $top).<br/>
    /// - The resulting query is deferred and executed by the OData pipeline upon enumeration.
    /// </remarks>
    /// <returns>
    /// An <see cref="IActionResult"/> containing an <see cref="IQueryable{T}"/> of
    /// <see cref="ClientQuestionnaireSectionViewModel"/> when successful.
    /// </returns>
    /// <response code="200">Returns a queryable set of questionnaire sections.</response>
    /// <response code="401">User is not authorized.</response>
    /// <response code="500">An unexpected error occurred.</response>
    [Authorize]
    [HttpGet]
    [EnableQuery]
    [ProducesResponseType(typeof(IQueryable<ClientQuestionnaireSectionViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAsync()
    {
        const string methodName = $"{ClassName}: {nameof(GetAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);

            var response = await clientQuestionnaireSectionBusiness.GetAsync();
            return Ok(response.ToList());
        }
        catch (Exception ex)
        {
            logger.LogError("{MethodName} - Error in execution with error - {Error}", methodName, ex.Message);
            return StatusCode(500, ex.Message);
        }
        finally
        {
            logger.LogInformation("{MethodName} - method execution completed", methodName);
        }
    }
}