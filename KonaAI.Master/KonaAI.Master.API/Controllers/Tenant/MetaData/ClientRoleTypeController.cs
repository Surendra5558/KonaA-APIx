using FluentValidation;
using KonaAI.Master.Business.Tenant.MetaData.Logic.Interface;
using KonaAI.Master.Model.Common;
using KonaAI.Master.Model.Tenant.Client.SaveModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace KonaAI.Master.API.Controllers.Tenant.MetaData;

[Authorize]
public class ClientRoleTypeController(ILogger<ClientRoleTypeController> logger,
 IClientRoleTypeBusiness roleTypeBusiness,
 IValidator<ClientRoleTypeCreateModel> createValidator,
 IValidator<ClientRoleTypeUpdateModel> updateValidator) : ODataController
{
    private const string ClassName = nameof(ClientRoleTypeController);

    /// <summary>
    /// Retrieves all active client roles
    /// Exposed as GET /v1/ClientRoleType
    /// </summary>
    [HttpGet]
    [EnableQuery]
    [ProducesResponseType(typeof(IQueryable<MetaDataViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAsync()
    {
        const string methodName = $"{ClassName}: {nameof(GetAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);

            var roles = await roleTypeBusiness.GetAsync();

            logger.LogInformation("{MethodName} - Retrieved {Count} Roles", methodName, roles.Count());
            return Ok(roles);
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
    /// Retrieves a specific roleType by its unique identifier.
    /// </summary>
    /// <param name="rowId">The unique identifier of the client role Type.</param>
    /// <returns>The MetadataViewModel if found; otherwise, NotFound.</returns>
    [HttpGet("roleType-by-rowId/{rowId:Guid}")]
    [EnableQuery]
    [ProducesResponseType(typeof(MetaDataViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid rowId)
    {
        const string methodName = $"{ClassName}: {nameof(GetByIdAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - method execution started for id: {rowId}", methodName, rowId);

            var role = await roleTypeBusiness.GetByRowIdAsync(rowId);

            if (role == null)
            {
                logger.LogWarning("{MethodName} - Role with id {Id} not found", methodName, rowId);
                return NotFound($"Role with id {rowId} not found");
            }

            logger.LogInformation("{MethodName} - Role retrieved successfully for id: {Id}", methodName, rowId);
            return Ok(role);
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
    /// Creates a new client role.
    /// </summary>
    /// <param name="clientRoleType"></param>
    /// <returns></returns>
    [HttpPost]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> PostAsync([FromBody] ClientRoleTypeCreateModel clientRoleType)
    {
        const string methodName = $"{ClassName}: {nameof(PostAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);

            var validationResult = await createValidator.ValidateAsync(clientRoleType);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var result = await roleTypeBusiness.CreateAsync(clientRoleType);
            if (result <= 0)
            {
                logger.LogWarning("{MethodName} - No records were created", methodName);
                return BadRequest("No records were created");
            }

            logger.LogInformation("{MethodName} - Role created successfully with {Count} records", methodName, result);
            return Created(); // returns 201 Created
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
    /// Updates an existing client user.
    /// </summary>
    /// <param name="rowId">The unique identifier of the client user to update.</param>
    /// <param name="clientRoleType">The updated user data.</param>
    /// <remarks>
    /// The request body must contain a valid <see cref="ClientRoleTypeUpdateModel"/>.
    /// </remarks>
    /// <response code="204">User updated successfully.</response>
    /// <response code="400">If the request is invalid or validation fails.</response>
    /// <response code="404">If the client is not found.</response>
    /// <response code="500">If an internal server error occurs.</response>
    [HttpPut("roleType-by-rowId/{rowId:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> PutAsync(Guid rowId, [FromBody] ClientRoleTypeUpdateModel clientRoleType)
    {
        const string methodName = $"{ClassName}: {nameof(PutAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);
            var validationResult = await updateValidator.ValidateAsync(clientRoleType);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var result = await roleTypeBusiness.UpdateAsync(rowId, clientRoleType);
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
    /// Deletes a client user by its identifier.
    /// </summary>
    /// <param name="rowId">The unique identifier of the client to delete.</param>
    /// <response code="204">Client user deleted successfully.</response>
    /// <response code="404">If the client user is not found.</response>
    /// <response code="500">If an internal server error occurs.</response>
    [HttpDelete("roleType-by-rowId/{rowId:Guid}")]
    [EnableQuery]
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
            _ = await roleTypeBusiness.DeleteAsync(rowId);
            logger.LogInformation("{MethodName} - Data deleted successfully with id {RowId}", methodName, rowId);
            return NoContent();
        }
        catch (KeyNotFoundException ke)
        {
            logger.LogError("{MethodName} - Error in execution with error - {EMessage}", methodName, ke.Message);
            return NotFound($"User with id {rowId} not found");
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