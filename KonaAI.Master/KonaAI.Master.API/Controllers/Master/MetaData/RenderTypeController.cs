using FluentValidation;
using KonaAI.Master.Model.Master.SaveModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace KonaAI.Master.API.Controllers.Master.MetaData;

/// <summary>
/// Controller responsible for handling operations related to Render Types.
/// </summary>
[Authorize]
public class RenderTypeController(
    ILogger<RenderTypeController> logger, IValidator<RenderTypeCreateModel> createValidator,
    IRenderTypeBusiness renderTypeBusiness) : ODataController
{
    private const string ClassName = nameof(RenderTypeController);

    /// <summary>
    /// Retrieves all available render types.
    /// </summary>
    /// <returns>A collection of render types.</returns>
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
            var result = await renderTypeBusiness.GetAsync();
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
    /// Creates a new render type.
    /// </summary>
    /// <param name="payload">
    /// The <see cref="RenderTypeCreateModel"/> containing the data for the new render type.
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
    public async Task<IActionResult> PostAsync([FromBody] RenderTypeCreateModel payload)
    {
        const string methodName = $"{ClassName}: {nameof(PostAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);

            var validationResult = await createValidator.ValidateAsync(payload);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var result = await renderTypeBusiness.CreateAsync(payload);
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