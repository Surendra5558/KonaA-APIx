using AutoMapper;
using KonaAI.Master.Business.Master.MetaData.Logic.Interface;
using KonaAI.Master.Model.Common;
using KonaAI.Master.Repository.Common.Interface;
using Microsoft.Extensions.Logging;

namespace KonaAI.Master.Business.Master.MetaData.Logic;

/// <summary>
/// Handles business logic related to <see cref="MetaDataViewModel"/> entities.
/// </summary>
public class ModuleTypeBusiness(ILogger<ModuleTypeBusiness> logger,
    IMapper mapper,
    IUnitOfWork unitOfWork) : IModuleTypeBusiness
{
    private const string ClassName = nameof(ModuleTypeBusiness);

    /// <summary>
    /// Retrieves all <see cref="MetaDataViewModel"/> records asynchronously.
    /// </summary>
    /// <returns>
    /// An <see cref="IQueryable{MetaDataViewModel}"/> containing all modules.
    /// </returns>
    /// <exception cref="Exception">
    /// Throws an exception if an error occurs during data retrieval.
    /// </exception>
    public async Task<IQueryable<MetaDataViewModel>> GetAsync()
    {
        const string methodName = $"{ClassName}: {nameof(GetAsync)}";

        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);

            var result = (await unitOfWork.ModuleTypes.GetAsync())
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