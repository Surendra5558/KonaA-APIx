using FluentValidation;
using KonaAI.Master.Business.Master.App.Logic.Interface;
using KonaAI.Master.Model.Master.App.SaveModel;
using KonaAI.Master.Model.Master.App.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace KonaAI.Master.API.Controllers.Master.App;

/// <summary>
/// Controller for managing client master data, including retrieval, creation, update, and deletion of clients.
/// </summary>
/// <param name="logger">The logger instance for logging information and errors.</param>
/// <param name="clientBusiness">Business service for client operations.</param>
/// <param name="createValidator">Validator for <see cref="ClientCreateModel"/>.</param>
/// <param name="updateValidator">Validator for <see cref="ClientUpdateModel"/>.</param>
public class ClientController(
    ILogger<ClientController> logger,
    IClientBusiness clientBusiness,
    IValidator<ClientCreateModel> createValidator,
    IValidator<ClientUpdateModel> updateValidator
  ) : ODataController
{
    private const string ClassName = nameof(ClientController);

    /// <summary>
    /// Retrieves all clients.
    /// </summary>
    /// <remarks>Returns a list of all clients in the system.</remarks>
    /// <response code="200">Returns the list of clients.</response>
    /// <response code="500">If an internal server error occurs.</response>
    [Authorize(Policy = "Permission:Navigation=All Clients;Action= View")]
    [HttpGet]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IQueryable<ClientViewModel>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAsync()
    {
        const string methodName = $"{ClassName}: {nameof(GetAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);
            var result = await clientBusiness.GetAsync();
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
    /// Retrieves a client by its row identifier (GUID).
    /// </summary>
    /// <param name="rowId">The unique row identifier (GUID) of the client.</param>
    /// <response code="200">Returns the client with the specified row ID.</response>
    /// <response code="404">If the client is not found.</response>
    /// <response code="500">If an internal server error occurs.</response>
    [Authorize(Policy = "Permission:Navigation=All Clients;Action= View")]
    [HttpGet("v1/Client({rowId:guid})")]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ClientViewModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetByRowIdAsync([FromRoute] Guid rowId)
    {
        const string methodName = $"{ClassName}: {nameof(GetByRowIdAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);
            var result = await clientBusiness.GetByRowIdAsync(rowId);
            logger.LogInformation("{MethodName} - Data retrieved successfully for id {RowId}", methodName, rowId);
            return Ok(result);
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
    /// Creates a new client.
    /// </summary>
    /// <param name="client">The client data to create.</param>
    /// <remarks>
    /// The request body must contain a valid <see cref="ClientCreateModel"/>.
    /// </remarks>
    /// <response code="201">Client created successfully.</response>
    /// <response code="400">If the request is invalid or validation fails.</response>
    /// <response code="500">If an internal server error occurs.</response>
    [Authorize(Policy = "Permission:Navigation=All Clients;Action= Add")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> PostAsync([FromBody] ClientCreateModel client)
    {
        const string methodName = $"{ClassName}: {nameof(PostAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);
            var validationResult = await createValidator.ValidateAsync(client);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var result = await clientBusiness.CreateAsync(client);
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
    /// Updates an existing client.
    /// </summary>
    /// <param name="rowId">The unique identifier of the client to update.</param>
    /// <param name="client">The updated client data.</param>
    /// <remarks>
    /// The request body must contain a valid <see cref="ClientUpdateModel"/>.
    /// </remarks>
    /// <response code="204">Client updated successfully.</response>
    /// <response code="400">If the request is invalid or validation fails.</response>
    /// <response code="404">If the client is not found.</response>
    /// <response code="500">If an internal server error occurs.</response>
    [Authorize(Policy = "Permission:Navigation=All Clients;Action= Edit")]
    [HttpPut("v1/Client({rowId:guid})")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> PutAsync(Guid rowId, [FromBody] ClientUpdateModel client)
    {
        const string methodName = $"{ClassName}: {nameof(PutAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);
            var validationResult = await updateValidator.ValidateAsync(client);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var result = await clientBusiness.UpdateAsync(rowId, client);
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
    /// Deletes a client by its identifier.
    /// </summary>
    /// <param name="rowId">The unique identifier of the client to delete.</param>
    /// <response code="204">Client deleted successfully.</response>
    /// <response code="404">If the client is not found.</response>
    /// <response code="500">If an internal server error occurs.</response>
    [Authorize(Policy = "Permission:Navigation=All Clients;Action= Delete")]
    [HttpDelete("v1/Client({rowId:guid})")]
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
            _ = await clientBusiness.DeleteAsync(rowId);
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