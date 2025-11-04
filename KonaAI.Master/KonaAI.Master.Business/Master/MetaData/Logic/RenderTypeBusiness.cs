using AutoMapper;
using KonaAI.Master.Model.Common;
using KonaAI.Master.Model.Master.SaveModel;
using KonaAI.Master.Repository.Common.Constants;
using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Domain.Master.MetaData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KonaAI.Master.Business.Master.MetaData.Logic;

/// <summary>
/// Business logic class for managing render types actions.
/// </summary>

public class RenderTypeBusiness(ILogger<RenderTypeBusiness> logger, IUserContextService userContextService,
    IMapper mapper,
    IUnitOfWork unitOfWork) : IRenderTypeBusiness
{
    private const string ClassName = nameof(RenderTypeBusiness);

    /// <summary>
    /// Retrieves all render types.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// The task result contains a queryable collection of <see cref="MetaDataViewModel"/>.
    /// </returns>
    public async Task<IQueryable<MetaDataViewModel>> GetAsync()
    {
        const string methodName = $"{ClassName}: {nameof(GetAsync)}";

        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);
            var renderTypes = await unitOfWork.RenderTypes.GetAsync();
            var result = renderTypes.Select(item => mapper.Map<MetaDataViewModel>(item));
            return result;
        }
        catch (Exception e)
        {
            logger.LogError("{MethodName} - Error occurred: {EMessage}", methodName, e.Message);
            throw;
        }
        finally
        {
            logger.LogInformation("{MethodName} - method execution completed", methodName);
        }
    }

    /// <summary>
    /// Creates a rendertype using the provided model.
    /// </summary>
    /// <param name="payload">The model containing rendertype creation data.</param>
    /// <returns>The number of records affected.</returns>
    /// <exception cref="DbUpdateException">Condition.</exception>
    /// <exception cref="Exception">Condition.</exception>
    public async Task<int> CreateAsync(RenderTypeCreateModel payload)
    {
        const string methodName = $"{ClassName}: {nameof(CreateAsync)}";
        var result = 0;
        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);
            await unitOfWork.ExecuteAsync(async () =>
            {
                // Map to the correct namespace for Client entity
                var renderTypeeEntity = mapper.Map<RenderType>(payload);
                // Set audit fields using userContextService.SetDomainDefaults
                userContextService.SetDomainDefaults(renderTypeeEntity, DataModes.Add);
                _ = await unitOfWork.RenderTypes.AddAsync(renderTypeeEntity);
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
}