using AutoMapper;
using KonaAI.Master.Business.Tenant.MetaData.Logic.Interface;
using KonaAI.Master.Model.Common;
using KonaAI.Master.Model.Tenant.Client.SaveModel;
using KonaAI.Master.Repository.Common.Constants;
using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Domain.Master.MetaData;
using KonaAI.Master.Repository.Domain.Tenant.ClientMetaData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KonaAI.Master.Business.Tenant.MetaData.Logic;

/// <summary>
/// Provides business operations for managing role types,
/// including retrieval, creation, update, and deletion.
/// </summary>
public class ClientRoleTypeBusiness(
    ILogger<ClientRoleTypeBusiness> logger,
    IMapper mapper,
    IUserContextService userContextService,
    IUnitOfWork unitOfWork)
    : IClientRoleTypeBusiness
{
    private const string ClassName = nameof(ClientRoleTypeBusiness);

    /// <summary>
    /// Retrieves all active role types.
    /// </summary>
    /// <returns></returns>
    public async Task<IQueryable<MetaDataViewModel>> GetAsync()
    {
        const string methodName = $"{ClassName}: {nameof(GetAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);

            var result = from crt in await unitOfWork.ClientRoleTypes.GetAsync()
                         join cr in await unitOfWork.RoleTypes.GetAsync() on crt.RoleTypeId equals cr.Id
                         select mapper.Map<MetaDataViewModel>(cr);

            logger.LogInformation("{MethodName} - Retrieved role types", methodName);
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
    /// Retrieves a role type by its rowId.
    /// </summary>
    /// <param name="rowId"></param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public async Task<MetaDataViewModel> GetByRowIdAsync(Guid rowId)
    {
        const string methodName = $"{ClassName}: {nameof(GetByRowIdAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - method execution started for id: {Id}", methodName, rowId);

            var role = await unitOfWork.RoleTypes.GetByRowIdAsync(rowId);
            if (role == null)
            {
                logger.LogError("{MethodName} - RoleType with id {Id} not found", methodName, rowId);
                throw new KeyNotFoundException($"RoleType with id {rowId} not found.");
            }

            var viewModel = mapper.Map<MetaDataViewModel>(role);

            logger.LogInformation("{MethodName} - RoleType retrieved successfully for id: {Id}", methodName, rowId);
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
    /// Creates a new RoleType.
    /// </summary>
    /// <param name="payload"></param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public async Task<int> CreateAsync(ClientRoleTypeCreateModel payload)
    {
        const string methodName = $"{ClassName}: {nameof(CreateAsync)}";
        logger.LogInformation("{MethodName} - method execution started", methodName);

        int result = 0;
        try
        {
            await unitOfWork.ExecuteAsync(async () =>
            {
                var clientId = userContextService.UserContext!.ClientId;
                var roleName = payload.Name;

                // 1️⃣ Single DB call — get both Role and ClientRole info
                var existing = await (
                    from role in unitOfWork.RoleTypes.Context.RoleTypes
                    join clientRole in unitOfWork.ClientRoleTypes.Context.ClientRoleTypes
                        on role.Id equals clientRole.RoleTypeId into roleClientGroup
                    from clientRole in roleClientGroup
                        .Where(crt => crt.ClientId == clientId && crt.IsActive)
                        .DefaultIfEmpty()
                    where role.Name == roleName && role.IsActive
                    select new
                    {
                        Role = role,
                        ClientRole = clientRole
                    }
                ).FirstOrDefaultAsync();

                long roleTypeId;

                // 2️⃣ Role exists and already assigned
                if (existing?.ClientRole != null)
                {
                    logger.LogError("{MethodName} - Role already assigned to client {ClientId} with role {RoleName}",
                        methodName, clientId, roleName);

                    throw new KeyNotFoundException(
                        $"Role '{roleName}' is already assigned to client {clientId}");
                }

                // 3️⃣ Role exists but not assigned → reuse
                if (existing?.Role != null)
                {
                    roleTypeId = existing.Role.Id;
                }
                else
                {
                    // 4️ Role doesn’t exist → create
                    var newRole = mapper.Map<RoleType>(payload);
                    userContextService.SetDomainDefaults(newRole, DataModes.Add);

                    _ = await unitOfWork.RoleTypes.AddAsync(newRole);
                    await unitOfWork.SaveChangesAsync();

                    roleTypeId = newRole.Id;
                }

                // 5️ Create the ClientRoleType link
                var clientRoleTypeEntity = mapper.Map<ClientRoleType>(payload);
                clientRoleTypeEntity.RoleTypeId = roleTypeId;

                userContextService.SetDomainDefaults(clientRoleTypeEntity, DataModes.Add);

                await unitOfWork.ClientRoleTypes.AddAsync(clientRoleTypeEntity);
                result = await unitOfWork.SaveChangesAsync();
            });
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{MethodName} - Error in execution with error - {Error}", methodName, ex.Message);
            throw;
        }
        finally
        {
            logger.LogInformation("{MethodName} - method execution completed", methodName);
        }
    }

    /// <summary>
    /// Updates an existing RoleType.
    /// </summary>
    /// <param name="rowId"></param>
    /// <param name="payload"></param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public async Task<int> UpdateAsync(Guid rowId, ClientRoleTypeUpdateModel payload)
    {
        const string methodName = $"{ClassName}: {nameof(UpdateAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);

            var roleEntity = await unitOfWork.RoleTypes.GetByRowIdAsync(rowId);
            if (roleEntity == null)
            {
                logger.LogError("{MethodName} - RoleType not found with id: {rowId}", methodName, rowId);
                throw new KeyNotFoundException($"RoleType with id {rowId} not found.");
            }

            var userContext = userContextService.UserContext;
            if (userContext == null)
            {
                logger.LogError("{MethodName} - User context is null", methodName);
                throw new Exception("User context is null");
            }

            mapper.Map(payload, roleEntity);

            userContextService.SetDomainDefaults(roleEntity, DataModes.Edit);

            _ = await unitOfWork.RoleTypes.UpdateAsync(roleEntity);

            return await unitOfWork.SaveChangesAsync();
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
            logger.LogError("{MethodName} - Error in execution with error - {Error}", methodName, ex.Message);
            throw;
        }
        finally
        {
            logger.LogInformation("{MethodName} - method execution completed", methodName);
        }
    }

    /// <summary>
    /// Deletes a RoleType and its related ClientRoleType.
    /// </summary>
    /// <param name="rowId"></param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public async Task<int> DeleteAsync(Guid rowId)
    {
        const string methodName = $"{ClassName}: {nameof(DeleteAsync)}";
        logger.LogInformation("{MethodName} - method execution started", methodName);
        var result = 0;
        try
        {
            await unitOfWork.ExecuteAsync(async () =>
            {
                // Delete RoleType
                var role = await unitOfWork.RoleTypes.DeleteAsync(rowId);
                if (role == null)
                {
                    logger.LogError("{MethodName} - RoleType not found with id: {Id}", methodName, rowId);
                    throw new KeyNotFoundException($"RoleType with id {rowId} not found.");
                }

                // 2. Delete related ClientRoleTypes
                var clientRoleTypes = unitOfWork.ClientRoleTypes.Context.ClientRoleTypes
                    .Where(crt => crt.RoleTypeId == role.Id && crt.IsActive)
                    .Select(crt => crt.RowId)
                    .ToList();

                if (clientRoleTypes.Any())
                {
                    foreach (var crtId in clientRoleTypes)
                    {
                        await unitOfWork.ClientRoleTypes.DeleteAsync(crtId);
                    }
                }
                else
                {
                    logger.LogError("{MethodName} - No ClientRoleTypes found for RoleTypeId: {RoleTypeId}", methodName, role.Id);
                }

                // 3. Save all deletes in one transaction
                result = await unitOfWork.SaveChangesAsync();
            });

            return result;
        }
        catch (DbUpdateException dex)
        {
            logger.LogError(dex, "{MethodName} - Database update exception", methodName);
            foreach (var entry in dex.Entries)
            {
                logger.LogError("{MethodName} - DbUpdateException entry: {Entry}", methodName, entry.Entity.ToString());
            }
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{MethodName} - Error in execution with error - {Error}", methodName, ex.Message);
            throw;
        }
        finally
        {
            logger.LogInformation("{MethodName} - method execution completed", methodName);
        }
    }
}