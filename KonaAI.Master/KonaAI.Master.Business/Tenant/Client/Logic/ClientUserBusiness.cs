using AutoMapper;
using KonaAI.Master.Business.Tenant.Client.Logic.Interface;
using KonaAI.Master.Model.Tenant.Client.SaveModel;
using KonaAI.Master.Model.Tenant.Client.ViewModel;
using KonaAI.Master.Repository.Common.Constants;
using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Domain.Master.App;
using KonaAI.Master.Repository.Domain.Tenant.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KonaAI.Master.Business.Tenant.Client.Logic;

/// <summary>
///Provides business operations for managing client users,
///including retrieval of all users and retrieval by unique identifier.
/// </summary>
public class ClientUserBusiness(
    ILogger<ClientUserBusiness> logger,
    IMapper mapper,
    IUserContextService userContextService,
    IUnitOfWork unitOfWork)
    : IClientUserBusiness
{
    private const string ClassName = nameof(ClientUserBusiness);

    /// <summary>
    /// Asynchronously retrieves a queryable collection of all client users
    /// </summary>
    /// <returns>IQueryable&lt;ClientUserViewModel&gt;</returns>
    public async Task<IQueryable<ClientUserViewModel>> GetAsync()
    {
        const string methodName = $"{ClassName}: {nameof(GetAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);

            //Get all client users and join with users to get user details
            var result = from cu in await unitOfWork.ClientUsers.GetAsync()
                         join user in await unitOfWork.Users.GetAsync() on cu.UserId equals user.Id into uGroup
                         from u in uGroup.DefaultIfEmpty()
                         select mapper.Map<ClientUserViewModel>(u);

            logger.LogInformation("{MethodName} - Retrieved client users", methodName);
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
    /// Retrieves a specific client user by its unique identifier.
    /// </summary>
    /// <param name="rowId">The unique identifier of the user.</param>
    /// <returns>The client user view model if found; otherwise, null.</returns>
    public async Task<ClientUserViewModel?> GetByRowIdAsync(Guid rowId)
    {
        const string methodName = $"{ClassName}: {nameof(GetByRowIdAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - method execution started for id: {Id}", methodName, rowId);

            var user = await unitOfWork.Users.GetByRowIdAsync(rowId);

            if (user == null)
            {
                logger.LogError("{MethodName} - User with id {Id} not found", methodName, rowId);
                throw new KeyNotFoundException($"{methodName} - User with id {rowId} not found");
            }
            var viewModel = mapper.Map<ClientUserViewModel>(user);

            logger.LogInformation("{MethodName} - Client user retrieved successfully for id: {Id}", methodName, rowId);
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
    /// Creates a new User for the client. Returns false if there is already a user present.
    /// </summary>
    /// <param name="payload"></param>
    /// <returns>1 if user is created. 0 if user already exists or there is an error.</returns>
    public async Task<int> CreateAsync(ClientUserCreateModel payload)
    {
        var methodName = nameof(CreateAsync);
        logger.LogInformation("{MethodName} - method execution started", methodName);

        var result = 0;
        try
        {
            //Execute Async for transaction
            await unitOfWork.ExecuteAsync(async () =>
            {
                //Check if the user already exists
                var existingUser = unitOfWork.Users.Context.Users
                    .FirstOrDefault(u => u.UserName == payload.UserName && u.IsActive);

                if (existingUser != null)
                {
                    logger.LogError("{MethodName} - User already exists with username: {UserName}", methodName, payload.UserName);
                    throw new KeyNotFoundException($"User already exists with username: {payload.UserName}");
                }

                //Encrypt password and map the payload
                payload.Password = BCrypt.Net.BCrypt.HashPassword(payload.Password);
                var userEntity = mapper.Map<User>(payload);
                userContextService.SetDomainDefaults(userEntity, DataModes.Add);

                var logOnTypeId = unitOfWork.LogOnTypes.Context.LogOnTypes
                    .Where(x => x.Name == "Forms")
                    .Select(x => x.Id)
                    .FirstOrDefault();

                //Add necessary Ids
                if (logOnTypeId == 0)
                {
                    logger.LogError("{MethodName} - No LogOnType found", methodName);
                    throw new Exception("No LogOnType found");
                }
                userEntity.LogOnTypeId = logOnTypeId;

                //Add User
                var createdUser = await unitOfWork.Users.AddAsync(userEntity);
                await unitOfWork.SaveChangesAsync();

                if (createdUser == null)
                {
                    logger.LogError("{MethodName} - User Creation error", methodName);
                    throw new Exception("User Creation error");
                }

                //Map clientUser
                var clientUserEntity = mapper.Map<ClientUserCreateModel, ClientUser>(payload);
                clientUserEntity.UserId = createdUser.Id; // link UserId
                userContextService.SetDomainDefaults(clientUserEntity, DataModes.Add);

                //Add ClientUser
                await unitOfWork.ClientUsers.AddAsync(clientUserEntity);
                result = await unitOfWork.SaveChangesAsync();
            });
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{MethodName} - Exception occurred while creating user", methodName);
            throw;
        }
    }

    /// <summary>
    /// Updates the client entity identified by the specified row ID with the provided update model.
    /// </summary>
    /// <remarks>This method retrieves the client entity associated with the specified <paramref
    /// name="rowId"/> from the database. If the entity is found, it is updated with the values provided in the
    /// <paramref name="payload"/> parameter.  Audit fields are automatically set during the update process. The changes
    /// are then saved to the database.</remarks>
    /// <param name="rowId">The unique identifier of the client entity to update.</param>
    /// <param name="payload">The update model containing the new values for the client entity.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the number of state entries  written
    /// to the database as a result of the update.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if no client entity with the specified <paramref name="rowId"/> is found.</exception>
    public async Task<int> UpdateAsync(Guid rowId, ClientUserUpdateModel payload)
    {
        const string methodName = $"{ClassName}: {nameof(UpdateAsync)}";
        try
        {
            logger.LogInformation("{MethodName} - method execution started", methodName);

            var clientUserEntity = await unitOfWork.Users.GetByRowIdAsync(rowId);
            if (clientUserEntity == null)
            {
                logger.LogError("{MethodName} found no user with id: {rowId}", methodName, rowId);
                throw new KeyNotFoundException($"User with id {rowId} not found.");
            }

            // Map updates into the exisException: Cannot insert explicit value for identity column in table 'User' when IDENTITY_INSERT is set to OFF.ting entity
            mapper.Map(payload, clientUserEntity);

            // Handle password logic
            if (!BCrypt.Net.BCrypt.Verify(payload.Password, clientUserEntity.Password))
            {
                clientUserEntity.Password = BCrypt.Net.BCrypt.HashPassword(payload.Password);
            }

            // Set audit fields using userContextService.SetDomainDefaults
            userContextService.SetDomainDefaults(clientUserEntity, DataModes.Edit);

            //Update and save the User
            _ = await unitOfWork.Users.UpdateAsync(clientUserEntity);

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
        var methodName = nameof(DeleteAsync);
        logger.LogInformation("{MethodName} - method execution started", methodName);

        var result = 0;
        try
        {
            await unitOfWork.ExecuteAsync(async () =>
            {
                // 1. Get user first
                var user = await unitOfWork.Users.GetByRowIdAsync(rowId);
                if (user == null)
                {
                    logger.LogError("{MethodName} - No user found with id: {Id}", methodName, rowId);
                    throw new KeyNotFoundException($"User with id {rowId} not found.");
                }

                // Mark user for delete
                await unitOfWork.Users.DeleteAsync(user.RowId);

                //  Get related client user
                var clientUserId = unitOfWork.ClientUsers.Context.ClientUsers
                    .Where(u => u.UserId == user.Id && u.IsActive)
                    .Select(u => u.RowId)
                    .FirstOrDefault();

                if (clientUserId != Guid.Empty)
                {
                    await unitOfWork.ClientUsers.DeleteAsync(clientUserId);
                }
                else
                {
                    logger.LogError("{MethodName} - No related ClientUser found for userId: {UserId}", methodName, user.Id);
                }

                // Commit all deletes in one SaveChanges
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