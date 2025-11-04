using FluentValidation;
using KonaAI.Master.Business.Tenant.Client.Logic.Interface;
using KonaAI.Master.Business.Tenant.UserMetaData.Logic.Interface;
using KonaAI.Master.Model.Tenant.Client.SaveModel;
using KonaAI.Master.Model.Tenant.Client.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace KonaAI.Master.API.Controllers.Tenant.Client;

/// <summary>
/// Controller for managing client projects,
/// including creation, update, and retrieval.
/// </summary>
[Authorize]
public class ClientProjectController(
    ILogger<ClientProjectController> logger,
    IClientProjectBusiness projectBusiness, IClientProjectDepartmentBusiness businessDepartment,
    IValidator<ClientProjectCreateModel> createValidator
) : ODataController
{
    private const string ClassName = nameof(ClientProjectController);

    /// <summary>
    /// Creates a new client project (with optional modules).
    /// </summary>
    /// <param name="project">
    /// The project creation payload.
    /// </param>
    /// <returns>
    /// 201 when created; 400 on validation errors; 401 if unauthorized; 500 on unexpected errors.
    /// </returns>
    [Authorize(Policy = "Permission:Navigation=Project;Action=Add")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> PostAsync([FromBody] ClientProjectCreateModel project)
    {
        const string methodName = $"{ClassName}: {nameof(PostAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);

            if (project is null)
            {
                return BadRequest("Request body is required and must be valid JSON.");
            }

            var validationResult = await createValidator.ValidateAsync(project);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var results = await businessDepartment.GetAsync();

            var result = await projectBusiness.CreateAsync(project);
            logger.LogInformation("{MethodName} - Project created successfully with {Count} records", methodName, result);

            return Created();
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
    /// Retrieves all active client projects with their related metadata and modules.
    /// </summary>
    /// <returns>
    /// 200 with a queryable collection of <see cref="ClientProjectViewModel"/>; 401 if unauthorized; 500 on errors.
    /// </returns>
    [Authorize(Policy = "Permission:Navigation=Project;Action=View")]
    [HttpGet]
    [EnableQuery]
    [ProducesResponseType(typeof(IEnumerable<ClientProjectViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAsync()
    {
        const string methodName = $"{ClassName}: {nameof(GetAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);

            var projects = await projectBusiness.GetAsync();

            logger.LogInformation("{MethodName} - Retrieved {Count} client projects", methodName, projects.Count());
            return Ok(projects);
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
    /// Retrieves a specific client project by its unique row identifier (GUID).
    /// </summary>
    /// <param name="rowId">
    /// The unique project row identifier.
    /// </param>
    /// <returns>
    /// 200 with <see cref="ClientProjectViewModel"/> when found; 400 on invalid id; 404 when not found; 401 if unauthorized; 500 on errors.
    /// </returns>
    [Authorize(Policy = "Permission:Navigation=Project;Action=View")]
    [HttpGet("v1/ClientProject({rowId:guid})")]
    [EnableQuery]
    [ProducesResponseType(typeof(ClientProjectViewModel), StatusCodes.Status200OK)]
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

            var project = await projectBusiness.GetByRowIdAsync(rowId);

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
    /// Deletes a client project by its unique row identifier.
    /// </summary>
    /// <param name="rowId">
    /// The unique row identifier (GUID) of the project to delete.
    /// </param>
    /// <returns>
    /// 204 when deleted successfully; 404 if project not found; 401 if unauthorized; 500 on unexpected errors.
    /// </returns>
    [HttpDelete("v1/ClientProject/{rowId}")]
    [EnableQuery]
    [ProducesResponseType(typeof(ClientProjectViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteAsync(Guid rowId)
    {
        const string methodName = $"{ClassName}: {nameof(DeleteAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - method execution started for RowId: {RowId}", methodName, rowId);

            var result = await projectBusiness.DeleteAsync(rowId);

            if (result == 0)
            {
                logger.LogWarning("{MethodName} - Project not found with RowId: {RowId}", methodName, rowId);
                return NotFound($"Project with RowId {rowId} not found.");
            }

            logger.LogInformation("{MethodName} - Project deleted successfully with RowId: {RowId}", methodName, rowId);
            return Ok(new { message = "Project deleted successfully" }); // 200 OK with success message
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