using AutoMapper;
using KonaAI.Master.Business.Master.App.Logic.Interface;
using AutoMapper.QueryableExtensions;
using KonaAI.Master.Model.Master.App.SaveModel;
using KonaAI.Master.Model.Master.App.ViewModel;
using KonaAI.Master.Model.Tenant.Client.SaveModel;
using KonaAI.Master.Repository.Common.Constants;
using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Domain.Master.App;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KonaAI.Master.Business.Master.App.Logic;

/// <summary>
/// Provides business logic for managing question banks.
/// </summary>
public class QuestionBankBusiness(
ILogger<QuestionBankBusiness> logger,
IMapper mapper,
IUnitOfWork unitOfWork, IUserContextService userContextService) : IQuestionBankBusiness
{
    private const string ClassName = nameof(QuestionBankBusiness);

    /// <summary>
    /// Retrieves all clients as a queryable collection of <see cref="ClientViewModel"/>.
    /// </summary>
    /// <returns>An <see cref="IQueryable{QuestionBankViewModel}"/> representing all clients.</returns>
    /// <exception cref="Exception">Condition.</exception>
    public async Task<IQueryable<QuestionBankViewModel>> GetAsync()
    {
        const string methodName = $"{ClassName}: {nameof(GetAsync)}";

        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);

            var clientId = userContextService.UserContext?.ClientId;
            var query = await unitOfWork.QuestionBanks.GetAsync();
            // Enforce strict tenant isolation: only current tenant's rows
            if (clientId == null || clientId <= 0)
            {
                query = query.Where(_ => false);
            }
            else
            {
                query = query.Where(q => q.ClientId == clientId);
            }

            var canProject = mapper.ConfigurationProvider != null &&
                              query.Provider is Microsoft.EntityFrameworkCore.Query.IAsyncQueryProvider;

            var result = canProject
                ? query.ProjectTo<QuestionBankViewModel>(mapper.ConfigurationProvider)
                : query.Select(q => mapper.Map<QuestionBankViewModel>(q)).AsQueryable();

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
    /// <returns>A <see cref="QuestionBankViewModel"/> representing the client.</returns>
    /// <exception cref="Exception">Condition.</exception>
    public async Task<QuestionBankViewModel> GetByRowIdAsync(Guid rowId)
    {
        const string methodName = $"{ClassName}: {nameof(GetByRowIdAsync)}";

        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);

            var client = await unitOfWork.QuestionBanks.GetByRowIdAsync(rowId);
            if (client == null)
            {
                logger.LogError("{MethodName} found no client with id: {RowId}", methodName, rowId);
                throw new KeyNotFoundException($"Client with id {rowId} not found.");
            }

            var result = mapper.Map<QuestionBankViewModel>(client);
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
    /// Creates a new question using the provided model.
    /// </summary>
    /// <param name="questionBank">The model containing client creation data.</param>
    /// <returns>The number of records affected.</returns>
    /// <exception cref="DbUpdateException">Condition.</exception>
    /// <exception cref="Exception">Condition.</exception>
    public async Task<int> CreateAsync(QuestionBankCreateModel questionBank)
    {
        const string methodName = $"{ClassName}: {nameof(CreateAsync)}";
        var result = 0;
        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);

            await unitOfWork.ExecuteAsync(async () =>
            {
                // Map to the correct namespace for Client entity
                var questionBankEntity = mapper.Map<QuestionBank>(questionBank);

                // Set audit fields using userContextService.SetDomainDefaults
                userContextService.SetDomainDefaults(questionBankEntity, DataModes.Add);

                _ = await unitOfWork.QuestionBanks.AddAsync(questionBankEntity);
                result = await unitOfWork.SaveChangesAsync();

                if (result == 0)
                {
                    logger.LogError("{MethodName} - Failed to create client", methodName);
                    throw new InvalidOperationException("Failed to create client.");
                }
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
    /// Updates an existing questionbank using the provided model.
    /// </summary>
    /// <param name="rowId"></param>
    /// <param name="payload">The model containing questionbank update data.</param>
    /// <returns>The number of records affected.</returns>
    /// <exception cref="DbUpdateException">Condition.</exception>
    /// <exception cref="Exception">Condition.</exception>
    public async Task<int> UpdateAsync(Guid rowId, QuestionBankUpdateModel payload)
    {
        const string methodName = $"{ClassName}: {nameof(UpdateAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);

            var clientEntity = await unitOfWork.QuestionBanks.GetByRowIdAsync(rowId);
            if (clientEntity == null)
            {
                logger.LogError("{MethodName} found no client with id: {Id}", methodName, rowId);
                throw new KeyNotFoundException($"Client with id {rowId} not found.");
            }

            // Map to the correct namespace for Client entity
            clientEntity = mapper.Map<QuestionBank>(payload);

            // Set audit fields using userContextService.SetDomainDefaults
            userContextService.SetDomainDefaults(clientEntity, DataModes.Edit);
            _ = await unitOfWork.QuestionBanks.UpdateAsync(clientEntity);
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
    /// Deletes a question by its unique identifier.
    /// </summary>
    /// <param name="rowId">The unique identifier of the question to delete.</param>
    /// <returns>The number of records affected.</returns>
    /// <exception cref="DbUpdateException">Condition.</exception>
    /// <exception cref="Exception">Condition.</exception>
    public async Task<int> DeleteAsync(Guid rowId)
    {
        const string methodName = $"{ClassName}: {nameof(DeleteAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);
            _ = unitOfWork.QuestionBanks.DeleteAsync(rowId);
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