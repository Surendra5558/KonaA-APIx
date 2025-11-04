using FluentValidation;
using KonaAI.Master.Business.Master.App.Logic.Interface;
using KonaAI.Master.Model.Master.App.SaveModel;
using KonaAI.Master.Model.Master.App.ViewModel;
using KonaAI.Master.Model.Tenant.Client.SaveModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace KonaAI.Master.API.Controllers.Master.App;

[Authorize]
public class QuestionBankController(
    ILogger<QuestionBankController> logger,
    IValidator<QuestionBankCreateModel> createValidator,
    IQuestionBankBusiness questionbank
) : ODataController
{
    private const string ClassName = nameof(QuestionBankController);

    /// <summary>
    /// Retrieves a list of question banks.
    /// </summary>
    /// <remarks>
    /// This endpoint supports OData query options such as <c>$filter</c>, <c>$select</c>, and <c>$orderby</c>.
    /// </remarks>
    /// <returns>
    /// An <see cref="IActionResult"/> containing a list of <see cref="QuestionBankViewModel"/> objects if successful.
    /// </returns>
    /// <response code="200">Returns the list of question banks.</response>
    /// <response code="401">If the request is unauthorized.</response>
    /// <response code="500">If an internal server error occurs.</response>
    [HttpGet]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IQueryable<QuestionBankViewModel>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAsync()
    {
        const string methodName = $"{ClassName}: {nameof(GetAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);
            var result = await questionbank.GetAsync();
            return Ok(result);
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


    /// <summary>
    /// Creates a new question bank.
    /// </summary>
    /// <param name="questionBankCreateModel">
    /// The <see cref="QuestionBankCreateModel"/> containing the data for the new question bank.
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
    public async Task<IActionResult> PostAsync([FromBody] QuestionBankCreateModel questionBankCreateModel)
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

            var result = await questionbank.CreateAsync(questionBankCreateModel);
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

    /// <summary>
    /// Updates an existing question.
    /// </summary>
    /// <param name="rowId">The unique identifier of the question to update.</param>
    /// <param name="payload">The updated question data.</param>
    /// <remarks>Updates description, render type, options, etc.</remarks>
    /// <response code="204">question updated successfully.</response>
    /// <response code="400">If the request is invalid or validation fails.</response>
    /// <response code="404">If the question is not found.</response>
    /// <response code="500">If an internal server error occurs.</response>
    [HttpPut("v1/QuestionBank({rowId:guid})")]
    [HttpPut("v1/QuestionBank/{rowId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> PutAsync(Guid rowId, [FromBody] QuestionBankUpdateModel payload)
    {
        const string methodName = $"{ClassName}: {nameof(PutAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);
            //var validationResult = await updateValidator.ValidateAsync(payload);
            //if (!validationResult.IsValid)
            //{
            //    return BadRequest(validationResult.Errors);
            //}
            var result = await questionbank.UpdateAsync(rowId, payload);
            logger.LogInformation("{MethodName} - Data updated successfully with id {Id}", methodName, result);
            return NoContent();
        }
        catch (KeyNotFoundException ke)
        {
            logger.LogError("{MethodName} - Error in execution with error - {EMessage}", methodName, ke.Message);
            return NotFound($"Client with id {rowId} not found");
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

    /// <summary>
    /// Deletes a question by its identifier.
    /// </summary>
    /// <param name="rowId">The unique identifier of the client to delete.</param>
    /// <response code="204">Client deleted successfully.</response>
    /// <response code="404">If the client is not found.</response>
    /// <response code="500">If an internal server error occurs.</response>
    [HttpDelete("v1/QuestionBank/{rowId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteAsync(Guid rowId)
    {
        const string methodName = $"{ClassName}: {nameof(DeleteAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);
            var affected = await questionbank.DeleteAsync(rowId);
            if (affected == 0)
            {
                return NotFound();
            }
            logger.LogInformation("{MethodName} - Data deleted successfully with id {RowId}", methodName, rowId);
            return NoContent();
        }
        catch (KeyNotFoundException ke)
        {
            logger.LogError("{MethodName} - Error in execution with error - {EMessage}", methodName, ke.Message);
            return NotFound($"Client with id {rowId} not found");
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