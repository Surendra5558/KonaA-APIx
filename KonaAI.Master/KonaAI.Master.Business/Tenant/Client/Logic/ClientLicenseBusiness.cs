using AutoMapper;
using KonaAI.Master.Business.Tenant.Client.Logic.Interface;
using KonaAI.Master.Model.Tenant.Client.SaveModel;
using KonaAI.Master.Model.Tenant.Client.ViewModel;
using KonaAI.Master.Repository.Common.Constants;
using KonaAI.Master.Repository.Common.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace KonaAI.Master.Business.Tenant.Client.Logic;

/// <summary>
/// Provides business operations for managing client license info,
/// including retrieval, creation, update, and deletion.
/// </summary>
public class ClientLicenseBusiness(
    ILogger<ClientLicenseBusiness> logger,
    ILicenseService licenseService,
    IUserContextService userContextService,
    IMapper mapper,
    IUnitOfWork unitOfWork)
    : IClientLicenseBusiness
{
    private const string ClassName = nameof(ClientLicenseBusiness);

    /// <summary>
    /// Asynchronously retrieves a queryable collection of all client license info records.
    /// </summary>
    /// <returns>IQueryable&lt;ClientLicenseViewModel&gt;</returns>
    public async Task<IQueryable<ClientLicenseViewModel>> GetAsync()
    {
        const string methodName = $"{ClassName}: {nameof(GetAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);

            var result = (await unitOfWork.ClientLicenses.GetAsync())
                .Where(x => !string.IsNullOrEmpty(x.LicenseKey)) // Filter out records with null/empty LicenseKey
                .Select(x => mapper.Map<ClientLicenseViewModel>(x));

            logger.LogInformation("{MethodName} - Retrieved license info records", methodName);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError("{MethodName} - Error in execution with error - {Error}", methodName, ex.Message);
            throw;
        }
        finally
        {
            logger.LogInformation("{MethodName} - method execution completed", methodName);
        }
    }

    /// <summary>
    /// Retrieves a specific client license info record by its unique identifier.
    /// </summary>
    /// <param name="rowId"></param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public async Task<ClientLicenseViewModel?> GetByRowIdAsync(Guid rowId)
    {
        const string methodName = $"{ClassName}: {nameof(GetByRowIdAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - method execution started for id: {Id}", methodName, rowId);

            var entity = await unitOfWork.ClientLicenses.GetByRowIdAsync(rowId);

            if (entity == null)
            {
                logger.LogError("{MethodName} - License info with id {Id} not found", methodName, rowId);
                throw new KeyNotFoundException($"{methodName} - License info with id {rowId} not found");
            }

            // Check if LicenseKey is null or empty (legacy data issue)
            if (string.IsNullOrEmpty(entity.LicenseKey))
            {
                logger.LogWarning("{MethodName} - License info with id {Id} has null/empty LicenseKey", methodName, rowId);
                throw new KeyNotFoundException($"{methodName} - License info with id {rowId} has invalid license data");
            }

            var viewModel = mapper.Map<ClientLicenseViewModel>(entity);

            logger.LogInformation("{MethodName} - License info retrieved successfully for id: {Id}", methodName, rowId);
            return viewModel;
        }
        catch (Exception ex)
        {
            logger.LogError("{MethodName} - Error in execution with error - {Error}", methodName, ex.Message);
            throw;
        }
        finally
        {
            logger.LogInformation("{MethodName} - method execution completed", methodName);
        }
    }

    /// <summary>
    /// Updates an existing ClientLicense.
    /// </summary>
    /// <param name="rowId">The unique identifier (RowId) of the client license to update.</param>
    /// <param name="payload">The updated license data.</param>
    /// <returns>Number of records affected after update.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the ClientLicense is not found.</exception>
    public async Task<int> UpdateAsync(Guid rowId, ClientLicenseUpdateModel payload)
    {
        const string methodName = $"{ClassName}: {nameof(UpdateAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);

            // 1️ Fetch the existing license
            var licenseEntity = await unitOfWork.ClientLicenses.GetByRowIdAsync(rowId);
            if (licenseEntity == null)
            {
                logger.LogError("{MethodName} - ClientLicense not found for rowId: {RowId}", methodName, rowId);
                throw new KeyNotFoundException($"ClientLicense with id {rowId} not found.");
            }

            // 2️ Map updated fields using AutoMapper
            mapper.Map(payload, licenseEntity);

            // 3️ Ensure valid start and end dates
            licenseEntity.StartDate = payload.StartDate ?? licenseEntity.StartDate;
            licenseEntity.EndDate = payload.EndDate ?? licenseEntity.EndDate;

            // 4️ Rebuild JSON payload for encryption
            var jsonPayload = new
            {
                clientId = licenseEntity.ClientId.ToString(),
                startDate = licenseEntity.StartDate.ToString("o"),
                endDate = licenseEntity.EndDate.ToString("o")
            };

            string jsonPayloadStr = JsonSerializer.Serialize(jsonPayload);

            // 5️ Regenerate license info (license-only approach)
            var licenseInfo = licenseService.EncryptLicense(jsonPayloadStr, licenseEntity.ClientId.ToString());
            licenseEntity.LicenseKey = licenseInfo.EncryptedLicense;
            licenseEntity.PrivateKey = string.Empty; // No private key needed for license-only approach

            // 6️ Update audit fields
            userContextService.SetDomainDefaults(licenseEntity, DataModes.Edit);

            // 7️ Save changes
            _ = await unitOfWork.ClientLicenses.UpdateAsync(licenseEntity);
            var result = await unitOfWork.SaveChangesAsync();

            if (result == 0)
            {
                logger.LogError("{MethodName} - Failed to update license for rowId: {RowId}", methodName, rowId);
                throw new InvalidOperationException("Failed to update client license.");
            }

            logger.LogInformation("{MethodName} - ClientLicense updated successfully for rowId: {RowId}", methodName, rowId);
            return result;
        }
        catch (KeyNotFoundException knf)
        {
            logger.LogError("{MethodName} - Entity not found: {Error}", methodName, knf.Message);
            throw;
        }
        catch (DbUpdateException dex)
        {
            logger.LogError("{MethodName} - Database update exception", methodName);
            foreach (var entry in dex.Entries)
            {
                logger.LogError("{MethodName} - DbUpdateException entry: {Entry}", methodName, entry.Entity.ToString());
            }
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{MethodName} - Unexpected error: {Error}", methodName, ex.Message);
            throw;
        }
        finally
        {
            logger.LogInformation("{MethodName} - method execution completed", methodName);
        }
    }

    /// <summary>
    /// Deletes a license info record by its unique identifier.
    /// </summary>
    /// <param name="rowId"></param>
    /// <returns>int</returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public async Task<int> DeleteAsync(Guid rowId)
    {
        var methodName = nameof(DeleteAsync);
        logger.LogInformation("{MethodName} - method execution started", methodName);

        var result = 0;
        try
        {
            await unitOfWork.ExecuteAsync(async () =>
            {
                var entity = await unitOfWork.ClientLicenses.GetByRowIdAsync(rowId);
                if (entity == null)
                {
                    logger.LogError("{MethodName} - No license info found with id: {Id}", methodName, rowId);
                    throw new KeyNotFoundException($"License info with id {rowId} not found.");
                }

                await unitOfWork.ClientLicenses.DeleteAsync(entity.RowId);
                result = await unitOfWork.SaveChangesAsync();
            });

            return result;
        }
        catch (DbUpdateException dex)
        {
            logger.LogError("{MethodName} - Error in execution due to database update exception", methodName);
            foreach (var entry in dex.Entries)
            {
                logger.LogError("{MethodName} - DbUpdateException entry: {Entry}", methodName, entry.Entity.ToString());
            }
            throw;
        }
        catch (Exception e)
        {
            logger.LogError("{MethodName} - Error in execution with error - {EMessage}", methodName, e.Message);
            throw;
        }
        finally
        {
            logger.LogInformation("{MethodName} - method execution completed", methodName);
        }
    }
}