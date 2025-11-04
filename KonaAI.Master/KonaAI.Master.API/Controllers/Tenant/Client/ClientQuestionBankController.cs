using FluentValidation;
using KonaAI.Master.Business.Tenant.Client.Logic.Interface;
using KonaAI.Master.Model.Master.App.SaveModel;
using KonaAI.Master.Model.Tenant.Client.SaveModel;
using KonaAI.Master.Repository.Common.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace KonaAI.Master.API.Controllers.Tenant.Client;

/// <summary>
/// Controller for managing client question banks.
/// Provides endpoints to retrieve and query client question bank data.
/// </summary>
[Authorize]
public class ClientQuestionBankController(
    ILogger<ClientQuestionBankController> logger, IValidator<ClientQuestionBankCreateModel> createValidator, IUserContextService userContextService,
    IClientQuestionBankBusiness clientQuestionBankBusiness) : ODataController
{
    private const string ClassName = nameof(ClientQuestionBankController);

    /// <summary>
    /// Retrieves all active client question banks with related metadata and modules.
    /// </summary>
    /// <returns>A collection of client question bank view models.</returns>
    [HttpGet]
    [EnableQuery]
    [ProducesResponseType(typeof(IEnumerable<ClientQuestionBankViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAsync()
    {
        const string methodName = $"{ClassName}: {nameof(GetAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);

            var clientId = userContextService.UserContext!.ClientId;

            var clientQuestionBank = await clientQuestionBankBusiness.GetAsync(clientId);

            logger.LogInformation("{MethodName} - Retrieved {Count} client projects", methodName, clientQuestionBank.Count());
            return Ok(clientQuestionBank);
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
    /// Retrieves a specific client question bank by its unique row identifier (GUID).
    /// </summary>
    /// <param name="rowId">The unique row identifier (GUID) of the client project.</param>
    /// <returns>
    /// A client project view model if found; otherwise, a 404 Not Found response.
    /// </returns>
    [HttpGet("ClientQuestionBankby-rowid/{rowId:guid}")]
    [EnableQuery]
    [ProducesResponseType(typeof(ClientQuestionBankViewModel), StatusCodes.Status200OK)]
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

            var project = await clientQuestionBankBusiness.GetByRowIdAsync(rowId);

            if (project == null)
            {
                logger.LogWarning("{MethodName} - Client project with rowId {RowId} not found", methodName, rowId);
                return NotFound($"Client project with row id {rowId} not found");
            }

            logger.LogInformation("{MethodName} - Client project retrieved successfully for rowId: {RowId}", methodName, rowId);
            return Ok(project);
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
    /// Creates a new client question bank.
    /// </summary>
    /// <param name="questionBankCreateModel">
    /// The <see cref="QuestionBankCreateModel"/> containing the data for the new client question bank.
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
    public async Task<IActionResult> PostAsync([FromBody] ClientQuestionBankCreateModel questionBankCreateModel)
    {
        const string methodName = $"{ClassName}: {nameof(PostAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);

            var validationResult = await createValidator.ValidateAsync(questionBankCreateModel);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var result = await clientQuestionBankBusiness.CreateAsync(questionBankCreateModel);
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