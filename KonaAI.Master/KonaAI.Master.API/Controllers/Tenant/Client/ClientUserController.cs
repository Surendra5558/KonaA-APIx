using FluentValidation;
using KonaAI.Master.Business.Tenant.Client.Logic.Interface;
using KonaAI.Master.Model.Tenant.Client.SaveModel;
using KonaAI.Master.Model.Tenant.Client.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace KonaAI.Master.API.Controllers.Tenant.Client;

/// <summary>
/// OData controller for managing client users.
/// </summary>
/// <remarks>
/// Endpoints annotated with <see cref="EnableQueryAttribute"/> support OData query options such as $filter, $select, $orderby, $top, and $skip.
/// Standard response codes are documented on each action.
/// </remarks>
/// <param name="logger">Structured logger for diagnostics.</param>
/// <param name="userBusiness">Business service handling client user operations.</param>
/// <param name="createValidator">Validator for <see cref="ClientUserCreateModel"/> requests.</param>
/// <param name="updateValidator">Validator for <see cref="ClientUserUpdateModel"/> requests.</param>
public class ClientUserController(
    ILogger<ClientUserController> logger,
    IClientUserBusiness userBusiness,
    IValidator<ClientUserCreateModel> createValidator,
    IValidator<ClientUserUpdateModel> updateValidator
) : ODataController
{
    private const string ClassName = nameof(ClientUserController);

    /// <summary>
    /// Retrieves all active client users with their related client id.
    /// </summary>
    /// <remarks>GET v1/ClientUser (OData-enabled).</remarks>
    /// <returns>
    /// 200 OK with a queryable sequence of <see cref="ClientUserViewModel"/>; 500 on error; 401 if unauthorized.
    /// </returns>
    [Authorize(Policy = "Permission : Navigation = Users; Action = View")]
    [HttpGet]
    [EnableQuery]
    [ProducesResponseType(typeof(IQueryable<ClientUserViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAsync()
    {
        const string methodName = $"{ClassName}: {nameof(GetAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);

            var users = await userBusiness.GetAsync();

            logger.LogInformation("{MethodName} - Retrieved {Count} client users", methodName, users.Count());
            return Ok(users);
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
    /// Retrieves a specific client user by its unique identifier.
    /// </summary>
    /// <param name="rowId">The unique identifier of the client user.</param>
    /// <returns>
    /// 200 OK with the <see cref="ClientUserViewModel"/> if found; 404 if not found; 500 on error; 401 if unauthorized.
    /// </returns>
    [Authorize(Policy = "Permission : Navigation = Users; Action = View")]
    [HttpGet("user-by-rowId/{rowId:Guid}")]
    [EnableQuery]
    [ProducesResponseType(typeof(ClientUserViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetByRowIdAsync([FromRoute] Guid rowId)
    {
        const string methodName = $"{ClassName}: {nameof(GetByRowIdAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - method execution started for id: {rowId}", methodName, rowId);

            var user = await userBusiness.GetByRowIdAsync(rowId);

            if (user == null)
            {
                logger.LogWarning("{MethodName} - Client user with id {Id} not found", methodName, rowId);
                return NotFound($"Client user with id {rowId} not found");
            }

            logger.LogInformation("{MethodName} - Client user retrieved successfully for id: {Id}", methodName, rowId);
            return Ok(user);
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
    /// Creates a new client user.
    /// </summary>
    /// <param name="clientUser">The user payload to create.</param>
    /// <returns>
    /// 201 Created on success; 400 if validation fails or no records were created; 500 on error; 401 if unauthorized.
    /// </returns>
    [Authorize(Policy = "Permission : Navigation = Users; Action = Add")]
    [HttpPost]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> PostAsync([FromBody] ClientUserCreateModel clientUser)
    {
        const string methodName = $"{ClassName}: {nameof(PostAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);

            var validationResult = await createValidator.ValidateAsync(clientUser);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var result = await userBusiness.CreateAsync(clientUser);
            if (result <= 0)
            {
                logger.LogWarning("{MethodName} - No records were created", methodName);
                return BadRequest("No records were created");
            }

            logger.LogInformation("{MethodName} - User created successfully with {Count} records", methodName, result);
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
    /// <param name="user">The updated user data.</param>
    /// <remarks>
    /// The request body must contain a valid <see cref="ClientUserUpdateModel"/>.
    /// </remarks>
    /// <returns>
    /// 204 No Content on success; 400 if validation fails; 404 if the user is not found; 500 on error; 401 if unauthorized.
    /// </returns>
    [Authorize(Policy = "Permission : Navigation = Users Action = Edit")]
    [HttpPut("user-by-rowId/{rowId:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> PutAsync(Guid rowId, [FromBody] ClientUserUpdateModel user)
    {
        const string methodName = $"{ClassName}: {nameof(PutAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);
            var validationResult = await updateValidator.ValidateAsync(user);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var result = await userBusiness.UpdateAsync(rowId, user);
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
    /// <param name="rowId">The unique identifier of the client user to delete.</param>
    /// <returns>
    /// 204 No Content on success; 404 if the user is not found; 500 on error; 401 if unauthorized.
    /// </returns>
    [Authorize(Policy = "Permission : Navigation = Users; Action = Delete")]
    [HttpDelete("user-by-rowId/{rowId:Guid}")]
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
            _ = await userBusiness.DeleteAsync(rowId);
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