using KonaAI.Master.Repository.Common;
using KonaAI.Master.Repository.DataAccess.Master.MetaData.Interface;
using KonaAI.Master.Repository.Domain.Master.MetaData;

namespace KonaAI.Master.Repository.DataAccess.Master.MetaData;

/// <summary>
/// Repository for performing CRUD operations on <see cref="Country"/> entities within the <see cref="DefaultContext"/>.
/// Inherits generic data access methods from <see cref="GenericRepository{DefaultContext, Country}"/>
/// and implements <see cref="ICountryRepository"/>.
/// </summary>
public class CountryRepository(DefaultContext context)
    : GenericRepository<DefaultContext, Country>(context), ICountryRepository
{
}