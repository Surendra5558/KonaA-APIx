using FluentValidation;
using KonaAI.Master.Business.Tenant.Client.Logic.Interface;
using KonaAI.Master.Model.Tenant.Client.SaveModel;
using KonaAI.Master.Repository.Common.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace KonaAI.Master.API.Controllers.Tenant.Client;

/// <summary>
/// Controller for managing client questionnaires.
/// Provides endpoints to retrieve and query client questionnaire data.
/// </summary>
[Authorize]
public class ClientQuestionnaireController(
    ILogger<ClientQuestionnaireController> logger, IValidator<ClientQuestionnaireCreateModel> createValidator, IUserContextService userContextService,
    IClientQuestionnaireBusiness clientQuestionnaireBusiness) : ODataController
{
    private const string ClassName = nameof(ClientQuestionnaireController);

    /// <summary>
    /// Retrieves all active client questionnaires with related metadata.
    /// </summary>
    /// <returns>A collection of client questionnaire view models.</returns>
    [HttpGet]
    [EnableQuery]
    [ProducesResponseType(typeof(IEnumerable<ClientQuestionnaireViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAsync()
    {
        const string methodName = $"{ClassName}: {nameof(GetAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);

            var clientQuestionnaire = await clientQuestionnaireBusiness.GetAsync();

            logger.LogInformation("{MethodName} - Retrieved {Count} client projects", methodName, clientQuestionnaire.Count());
            return Ok(clientQuestionnaire);
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

    /// <summary>
    /// Retrieves a specific client questionnaire by its unique row identifier (GUID).
    /// </summary>
    /// <param name="rowId">The unique row identifier (GUID).</param>
    /// <returns>
    /// The client questionnaire view model if found; otherwise, NotFound.
    /// </returns>
    [HttpGet("v1/clientQuestionnaireBusinessby-rowid/{rowId:guid}")]
    [EnableQuery]
    [ProducesResponseType(typeof(ClientQuestionnaireViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetByRowIdAsync([FromRoute] Guid rowId)
    {
        const string methodName = $"{ClassName}: {nameof(GetByRowIdAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - method execution started for rowId: {RowId}", methodName, rowId);

            if (rowId == Guid.Empty)
            {
                logger.LogWarning("{MethodName} - Invalid rowId provided: {RowId}", methodName, rowId);
                return BadRequest("Invalid project row id provided");
            }

            var clientQuestionnaire = await clientQuestionnaireBusiness.GetQuestionnaireDetailsAsync(rowId);

            if (clientQuestionnaire == null)
            {
                logger.LogWarning("{MethodName} - Client project with rowId {RowId} not found", methodName, rowId);
                return NotFound($"Client project with row id {rowId} not found");
            }

            logger.LogInformation("{MethodName} - Client project retrieved successfully for rowId: {RowId}", methodName, rowId);
            return Ok(clientQuestionnaire);
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

    /// <summary>
    /// Creates a new client questionnaire.
    /// </summary>
    /// <param name="clientQuestionCreateModel">
    /// The <see cref="ClientQuestionnaireCreateModel"/> containing the data for the new client questionnaire.
    /// </param>
    /// <returns>
    /// An <see cref="IActionResult"/> indicating the outcome of the operation.
    /// </returns>
    /// <response code="201">Returns when the question bank is successfully created.</response>
    /// <response code="400">If the input model validation fails.</response>
    /// <response code="401">If the request is unauthorized.</response>
    /// <response code="500">If an internal server error occurs.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> PostAsync([FromBody] ClientQuestionnaireCreateModel clientQuestionCreateModel)
    {
        const string methodName = $"{ClassName}: {nameof(PostAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);

            var validationResult = await createValidator.ValidateAsync(clientQuestionCreateModel);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var clientId = userContextService.UserContext!.ClientId;
            var result = await clientQuestionnaireBusiness.CreateAsync(clientQuestionCreateModel, clientId);
            logger.LogInformation("{MethodName} - Data created successfully with id {Id}", methodName, result);
            return Created();
        }
        catch (Exception e)
        {
            logger.LogError("{MethodName} - Error in execution with error - {EMessage}", methodName, e.Message);
            return StatusCode(500, e.Message);
        }
        finally
        {
            logger.LogInformation("{MethodName} - method execution completed", methodName);
        }
    }
}