using AutoMapper;
using KonaAI.Master.Business.Tenant.UserMetaData.Logic.Interface;
using KonaAI.Master.Model.Common;
using KonaAI.Master.Repository.Common.Interface;
using Microsoft.Extensions.Logging;

namespace KonaAI.Master.Business.Tenant.UserMetaData.Logic;

/// <summary>
/// Provides business logic operations for managing countries.
/// </summary>
public class ClientProjectCountryBusiness(ILogger<ClientProjectCountryBusiness> logger,
    IMapper mapper,
    IUnitOfWork unitOfWork) : IClientProjectCountryBusiness
{
    private const string ClassName = nameof(ClientProjectCountryBusiness);

    /// <summary>
    /// Retrieves all countries from the repository.
    /// </summary>
    /// <returns>An <see cref="IQueryable{MetaDataViewModel}"/> representing the collection of countries.</returns>
    public async Task<IQueryable<MetaDataViewModel>> GetAsync()
    {
        const string methodName = $"{ClassName}: {nameof(GetAsync)}";

        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);

            var result = from c in await unitOfWork.Countries.GetAsync()
                         join cc in await unitOfWork.ClientProjectCountries.GetAsync()
                             on c.Id equals cc.CountryId
                         select mapper.Map<MetaDataViewModel>(c);

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