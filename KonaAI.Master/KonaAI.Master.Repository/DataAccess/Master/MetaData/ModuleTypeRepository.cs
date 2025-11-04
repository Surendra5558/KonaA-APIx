using KonaAI.Master.Repository.Common;
using KonaAI.Master.Repository.DataAccess.Master.MetaData.Interface;
using KonaAI.Master.Repository.Domain.Master.MetaData;
using System.Reflection;

namespace KonaAI.Master.Repository.DataAccess.Master.MetaData;

/// <summary>
/// Repository for performing CRUD operations on <see cref="Module"/> entities within the <see cref="DefaultContext"/>.
/// Inherits generic data access methods from <see cref="GenericRepository{DefaultContext, ModuleType}"/>
/// and implements IModuleRepository.
/// </summary>
public class ModuleTypeRepository(DefaultContext context)
    : GenericRepository<DefaultContext, ModuleType>(context), IModuleTypeRepository
{
}