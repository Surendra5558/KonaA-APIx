using AutoMapper;
using KonaAI.Master.Business.Master.MetaData.Logic.Interface;
using KonaAI.Master.Model.Common;
using KonaAI.Master.Repository.Common.Interface;
using Microsoft.Extensions.Logging;

namespace KonaAI.Master.Business.Master.MetaData.Logic;

/// <summary>
/// Provides business logic for retrieving Module Source Type metadata.
/// Implements <see cref="IModuleSourceTypeBusiness"/>.
/// </summary>
/// <param name="logger">The logger used for diagnostic and operational logging.</param>
/// <param name="mapper">The AutoMapper instance used to map repository entities to <see cref="MetaDataViewModel"/>.</param>
/// <param name="unitOfWork">The unit-of-work providing access to repositories.</param>
public class ModuleSourceTypeBusiness(ILogger<ModuleSourceTypeBusiness> logger, IMapper mapper, IUnitOfWork unitOfWork)
    : IModuleSourceTypeBusiness
{
    private const string ClassName = nameof(ModuleSourceTypeBusiness);

    /// <summary>
    /// Asynchronously retrieves all Module Source Types as a queryable collection of <see cref="MetaDataViewModel"/>.
    /// </summary>
    /// <returns>
    /// A task that, when completed, provides an <see cref="IQueryable{T}"/> of <see cref="MetaDataViewModel"/> representing all Module Source Types.
    /// </returns>
    /// <remarks>Logs start, completion, and any errors encountered.</remarks>
    /// <exception cref="Exception">Propagates any exception thrown by the underlying repository or the mapping process.</exception>
    public async Task<IQueryable<MetaDataViewModel>> GetAsync()
    {
        const string methodName = $"{ClassName}: {nameof(GetAsync)}";

        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);

            var result = (await unitOfWork.ModuleSourceTypes.GetAsync())
                .Select(item => mapper.Map<MetaDataViewModel>(item));
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
}