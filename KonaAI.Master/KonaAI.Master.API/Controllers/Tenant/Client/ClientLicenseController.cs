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
/// OData controller for managing client license information.
/// </summary>
/// <remarks>
/// Supports OData query options ($filter, $select, $orderby, $top, $skip).
/// </remarks>
public class ClientLicenseController(
    ILogger<ClientLicenseController> logger,
    IClientLicenseBusiness licenseBusiness,
    IValidator<ClientLicenseUpdateModel> updateValidator
) : ODataController
{
    private const string ClassName = nameof(ClientLicenseController);

    /// <summary>
    /// Retrieves all client license info records.
    /// </summary>
    [Authorize(Policy = "Permission : Navigation = License; Action = View")]
    [HttpGet]
    [EnableQuery]
    [ProducesResponseType(typeof(IQueryable<ClientLicenseViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAsync()
    {
        const string methodName = $"{ClassName}: {nameof(GetAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - started", methodName);
            var licenses = await licenseBusiness.GetAsync();
            logger.LogInformation("{MethodName} - Retrieved {Count} records", methodName, licenses.Count());
            return Ok(licenses);
        }
        catch (Exception ex)
        {
            logger.LogError("{MethodName} - failed: {Error}", methodName, ex.Message);
            return StatusCode(500, ex.Message);
        }
    }

    /// <summary>
    /// Retrieves a license info record by its unique identifier.
    /// </summary>
    [Authorize(Policy = "Permission : Navigation = License; Action = View")]
    [HttpGet("license-by-rowId/{rowId:Guid}")]
    [EnableQuery]
    [ProducesResponseType(typeof(ClientLicenseViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetByRowIdAsync([FromRoute] Guid rowId)
    {
        const string methodName = $"{ClassName}: {nameof(GetByRowIdAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - started for id: {Id}", methodName, rowId);
            var license = await licenseBusiness.GetByRowIdAsync(rowId);
            if (license == null)
            {
                logger.LogWarning("{MethodName} - not found {Id}", methodName, rowId);
                return NotFound($"License info with id {rowId} not found");
            }
            return Ok(license);
        }
        catch (Exception ex)
        {
            logger.LogError("{MethodName} - failed: {Error}", methodName, ex.Message);
            return StatusCode(500, ex.Message);
        }
    }

    /// <summary>
    /// Updates an existing client license.
    /// </summary>
    /// <param name="rowId">The unique identifier of the client license to update.</param>
    /// <param name="clientLicense">The updated license data.</param>
    /// <remarks>
    /// The request body must contain a valid <see cref="ClientLicenseUpdateModel"/>.
    /// </remarks>
    /// <response code="204">License updated successfully.</response>
    /// <response code="400">If the request is invalid or validation fails.</response>
    /// <response code="404">If the license is not found.</response>
    /// <response code="500">If an internal server error occurs.</response>
    /// <response code="401">If the user is unauthorized.</response>
    [Authorize(Policy = "Permission: Navigation = License; Action = Edit")]
    [HttpPut("license-by-rowId/{rowId:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> PutAsync(Guid rowId, [FromBody] ClientLicenseUpdateModel clientLicense)
    {
        const string methodName = $"{ClassName}: {nameof(PutAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);

            // 1️⃣ Validate input
            var validationResult = await updateValidator.ValidateAsync(clientLicense);
            if (!validationResult.IsValid)
            {
                logger.LogWarning("{MethodName} - Validation failed for license update", methodName);
                return BadRequest(validationResult.Errors);
            }

            // 2️⃣ Perform update
            var result = await licenseBusiness.UpdateAsync(rowId, clientLicense);
            logger.LogInformation("{MethodName} - License updated successfully with id {Id}", methodName, result);

            return NoContent();
        }
        catch (KeyNotFoundException ke)
        {
            logger.LogError("{MethodName} - License not found with error - {EMessage}", methodName, ke.Message);
            return NotFound($"License with id {rowId} not found");
        }
        catch (Exception e)
        {
            logger.LogError("{MethodName} - Internal server error - {EMessage}", methodName, e.Message);
            return StatusCode(500, e.Message);
        }
        finally
        {
            logger.LogInformation("{MethodName} - method execution completed", methodName);
        }
    }

    /// <summary>
    /// Deletes a license info record.
    /// </summary>
    [Authorize(Policy = "Permission : Navigation = License; Action = Delete")]
    [HttpDelete("license-by-rowId/{rowId:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteAsync(Guid rowId)
    {
        const string methodName = $"{ClassName}: {nameof(DeleteAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - started", methodName);
            await licenseBusiness.DeleteAsync(rowId);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"License info with id {rowId} not found");
        }
        catch (Exception ex)
        {
            logger.LogError("{MethodName} - failed: {Error}", methodName, ex.Message);
            return StatusCode(500, ex.Message);
        }
    }
}