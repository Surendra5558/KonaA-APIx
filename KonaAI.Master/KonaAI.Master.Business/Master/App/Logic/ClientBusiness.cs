using AutoMapper;
using AutoMapper.QueryableExtensions;
using KonaAI.Master.Business.Master.App.Logic.Interface;
using KonaAI.Master.Model.Master.App.SaveModel;
using KonaAI.Master.Model.Master.App.ViewModel;
using KonaAI.Master.Repository.Common.Constants;
using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Domain.Master.App;
using KonaAI.Master.Repository.Domain.Tenant.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace KonaAI.Master.Business.Master.App.Logic;

/// <summary>
/// Provides business logic for managing clients, including retrieval, creation, update, and deletion operations.
/// </summary>
public class ClientBusiness(
    ILogger<ClientBusiness> logger,
    IMapper mapper,
    IUserContextService userContextService,
    ILicenseService licenseService,
    IUnitOfWork unitOfWork)
    : IClientBusiness
{
    private const string ClassName = nameof(ClientBusiness);

    /// <summary>
    /// Retrieves all clients as a queryable collection of <see cref="ClientViewModel"/>.
    /// </summary>
    /// <returns>An <see cref="IQueryable{ClientViewModel}"/> representing all clients.</returns>
    /// <exception cref="Exception">Condition.</exception>
    public async Task<IQueryable<ClientViewModel>> GetAsync()
    {
        const string methodName = $"{ClassName}: {nameof(GetAsync)}";

        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);

            // Get the queryable from the repository
            var queryable = await unitOfWork.Clients.GetAsync();

            // Filter to active clients
            var filtered = queryable.Where(c => c.IsActive);

            // If backed by EF (IAsyncQueryProvider), use ProjectTo for server-side translation;
            // otherwise fall back to in-memory mapping for mocks/unit tests
            var canProject = mapper.ConfigurationProvider != null &&
                              filtered.Provider is Microsoft.EntityFrameworkCore.Query.IAsyncQueryProvider;

            IQueryable<ClientViewModel> result = canProject
                ? filtered.ProjectTo<ClientViewModel>(mapper.ConfigurationProvider)
                : filtered.Select(c => mapper.Map<ClientViewModel>(c)).AsQueryable();

            return result;
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

    /// <summary>
    /// Retrieves a client by its unique row identifier (GUID).
    /// </summary>
    /// <param name="rowId">The unique row identifier (GUID) of the client.</param>
    /// <returns>A <see cref="ClientViewModel"/> representing the client.</returns>
    /// <exception cref="Exception">Condition.</exception>
    public async Task<ClientViewModel> GetByRowIdAsync(Guid rowId)
    {
        const string methodName = $"{ClassName}: {nameof(GetByRowIdAsync)}";

        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);

            var client = await unitOfWork.Clients.GetByRowIdAsync(rowId);
            if (client == null)
            {
                logger.LogError("{MethodName} found no client with id: {RowId}", methodName, rowId);
                throw new KeyNotFoundException($"Client with id {rowId} not found.");
            }

            var result = mapper.Map<ClientViewModel>(client);
            return result;
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

    /// <summary>
    /// Creates a new client using the provided model.
    /// </summary>
    /// <param name="payload">The model containing client creation data.</param>
    /// <returns>The number of records affected.</returns>
    /// <exception cref="DbUpdateException">Condition.</exception>
    /// <exception cref="Exception">Condition.</exception>
    public async Task<int> CreateAsync(ClientCreateModel payload)
    {
        const string methodName = $"{ClassName}: {nameof(CreateAsync)}";
        var result = 0;
        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);

            await unitOfWork.ExecuteAsync(async () =>
            {
                // Map to the correct namespace for Client entity
                var clientEntity = mapper.Map<Client>(payload);

                // Set audit fields using userContextService.SetDomainDefaults
                userContextService.SetDomainDefaults(clientEntity, DataModes.Add);

                _ = await unitOfWork.Clients.AddAsync(clientEntity);
                result = await unitOfWork.SaveChangesAsync();

                if (result == 0)
                {
                    logger.LogError("{MethodName} - Failed to create client", methodName);
                    throw new InvalidOperationException("Failed to create client.");
                }

                payload.LicenseStartDate ??= DateTime.UtcNow;
                payload.LicenseEndDate ??= DateTime.UtcNow.AddMonths(6);

                //client rowId for license generation
                Guid clientRowId = unitOfWork.Clients.GetByIdAsync(clientEntity.Id).Result?.RowId
                               ?? Guid.Empty;

                // Creating a JSON payload
                var jsonPayload = new
                {
                    clientId = clientRowId.ToString(),
                    startDate = payload!.LicenseStartDate!.Value.ToString("o"),
                    endDate = payload!.LicenseEndDate!.Value.ToString("o")
                };

                string jsonPayloadStr = JsonSerializer.Serialize(jsonPayload);


                // Generating license information using license-only approach
                var licenseinfo = licenseService.EncryptLicense(jsonPayloadStr, clientRowId.ToString());

                var clientLicense = new ClientLicense
                {
                    Name = $"{clientEntity.Name}-License",
                    Description = $"License for client {clientEntity.Name}",
                    ClientId = clientEntity.Id,
                    LicenseKey = licenseinfo.EncryptedLicense,
                    PrivateKey = licenseinfo.EncryptedPrivateKey, // No private key needed for license-only approach
                    StartDate = payload.LicenseStartDate.Value,
                    EndDate = payload.LicenseEndDate.Value,
                };

                // Set audit fields using userContextService.SetDomainDefaults
                userContextService?.SetDomainDefaults(clientLicense, DataModes.Add);

                await unitOfWork.ClientLicenses.AddAsync(clientLicense);
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

    /// <summary>
    /// Updates an existing client using the provided model.
    /// </summary>
    /// <param name="rowId"></param>
    /// <param name="payload">The model containing client update data.</param>
    /// <returns>The number of records affected.</returns>
    /// <exception cref="DbUpdateException">Condition.</exception>
    /// <exception cref="Exception">Condition.</exception>
    public async Task<int> UpdateAsync(Guid rowId, ClientUpdateModel payload)
    {
        const string methodName = $"{ClassName}: {nameof(UpdateAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);

            var clientEntity = await unitOfWork.Clients.GetByRowIdAsync(rowId);
            if (clientEntity == null)
            {
                logger.LogError("{MethodName} found no client with id: {Id}", methodName, rowId);
                throw new KeyNotFoundException($"Client with id {rowId} not found.");
            }

            // Map to the correct namespace for Client entity
            mapper.Map(payload, clientEntity);

            // Set audit fields using userContextService.SetDomainDefaults
            userContextService.SetDomainDefaults(clientEntity, DataModes.Edit);
            _ = await unitOfWork.Clients.UpdateAsync(clientEntity);
            return await unitOfWork.SaveChangesAsync();
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

    /// <summary>
    /// Deletes a client by its unique identifier.
    /// </summary>
    /// <param name="rowId">The unique identifier of the client to delete.</param>
    /// <returns>The number of records affected.</returns>
    /// <exception cref="DbUpdateException">Condition.</exception>
    /// <exception cref="Exception">Condition.</exception>
    public async Task<int> DeleteAsync(Guid rowId)
    {
        const string methodName = $"{ClassName}: {nameof(DeleteAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);
            _ = await unitOfWork.Clients.DeleteAsync(rowId);
            return await unitOfWork.SaveChangesAsync();
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