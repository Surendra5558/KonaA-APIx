using AutoMapper;
using AutoMapper.QueryableExtensions;
using KonaAI.Master.Business.Tenant.Client.Logic.Interface;
using KonaAI.Master.Business.Tenant.Client.Profile;
using KonaAI.Master.Model.Common.Constants;
using KonaAI.Master.Model.Tenant.Client.SaveModel;
using KonaAI.Master.Model.Tenant.Client.ViewModel;
using KonaAI.Master.Repository.Common.Constants;
using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Common.Model;
using KonaAI.Master.Repository.Domain.Tenant.Client;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Text.RegularExpressions;
using ProjectSchedulerModel = KonaAI.Master.Repository.Domain.Tenant.Client.ProjectScheduler;

namespace KonaAI.Master.Business.Tenant.Client.Logic;

/// <summary>
/// Provides business logic for managing client projects,
/// including retrieval, creation, update, and deletion operations.
/// Handles project lifecycle management with database creation and scheduler integration.
/// </summary>
/// <param name="logger">The logger instance for recording execution information and errors.</param>
/// <param name="mapper">The AutoMapper instance for object mapping between models and entities.</param>
/// <param name="userContextService">The service for managing user context and tenant scoping.</param>
/// <param name="unitOfWork">The unit of work pattern implementation for database operations.</param>
/// <param name="configuration">The configuration instance for accessing application settings.</param>
/// <param name="licenseService">The service for encrypting and decrypting sensitive data like passwords.</param>
public class ClientProjectBusiness(
    ILogger<ClientProjectBusiness> logger, IMapper mapper,
    IUserContextService userContextService, IUnitOfWork unitOfWork,
    IConfiguration configuration, ILicenseService licenseService)
    : IClientProjectBusiness
{
    private const string ClassName = nameof(ClientProjectBusiness);
    private readonly ILicenseService _licenseService = licenseService;
    /// <summary>
    /// Generates a sanitized SQL Server database name from the provided project name.
    /// Replaces special characters and ensures the name starts with a letter or underscore.
    /// </summary>
    /// <param name="projectName">Raw project name to be sanitized.</param>
    /// <returns>Safe database name compliant with SQL Server naming conventions.</returns>
    private static string GenerateDatabaseName(string projectName)
    {
        if (string.IsNullOrWhiteSpace(projectName))
            return "DefaultProject";

        //Replace all non-alphanumeric characters with underscore
        string sanitized = Regex.Replace(projectName, Constants.DatabaseNameSanitizationRegex, "_");

        //Ensure it doesn't start with a digit (SQL Server restriction)
        if (char.IsDigit(sanitized[0]))
            sanitized = "DB_" + sanitized;

        //Replace multiple underscores with a single underscore
        sanitized = Regex.Replace(sanitized, @"_+", "_");

        //Trim leading and trailing underscores
        sanitized = sanitized.Trim('_');

        //Truncate to the first 10 characters (if longer)
        if (sanitized.Length > 10)
            sanitized = sanitized.Substring(0, 10);

        //Fallback if everything gets sanitized out
        return string.IsNullOrWhiteSpace(sanitized) ? "DefaultProject" : sanitized;
    }

    /// <summary>
    /// Retrieves all active client projects with their related metadata and modules.
    /// </summary>
    /// <remarks>
    /// Loads required lookup sets and performs client-side projection to avoid EF Core
    /// translation limitations for complex joins. Results are tenant-scoped by the
    /// repository's global filter.
    /// </remarks>
    /// <returns>
    /// A queryable projection of <see cref="ClientProjectViewModel"/> representing client projects.
    /// </returns>
    /// <exception cref="Exception">
    /// Propagates unexpected errors during data retrieval or mapping.
    /// </exception>
    public async Task<IQueryable<ClientProjectViewModel>> GetAsync()
    {
        const string methodName = $"{ClassName}: {nameof(GetAsync)}";
        var startTime = DateTime.UtcNow;

        try
        {
            logger.LogInformation("{MethodName} - Retrieving all client projects with metadata", methodName);

            // Load all required lookup data for complex joins
            logger.LogDebug("{MethodName} - Loading lookup data for project metadata", methodName);
            var projects = await unitOfWork.ClientProjects.GetAsync();
            var projectAuditResponsibilitiesQuery = await unitOfWork.ProjectAuditResponsibilities.GetAsync();
            var clientProjectAuditResponsibilitiesQuery = await unitOfWork.ClientProjectAuditResponsibilities.GetAsync();
            var projectRiskAreasQuery = await unitOfWork.ProjectRiskAreas.GetAsync();
            var clientProjectRiskAreasQuery = await unitOfWork.ClientProjectRiskAreas.GetAsync();
            var clientProjectCountriesQuery = await unitOfWork.ClientProjectCountries.GetAsync();
            var countriesQuery = await unitOfWork.Countries.GetAsync();
            var projectUnitsQuery = await unitOfWork.ProjectUnits.GetAsync();
            var clientProjectUnitsQuery = await unitOfWork.ClientProjectUnits.GetAsync();
            var projectDepartmentsQuery = await unitOfWork.ProjectDepartments.GetAsync();
            var clientProjectDepartmentsQuery = await unitOfWork.ClientProjectDepartments.GetAsync();
            var projectStatusesQuery = await unitOfWork.ProjectStatuses.GetAsync();

            logger.LogDebug("{MethodName} - Executing complex join query for project metadata", methodName);
            // Compose a single EF-translatable query using left joins and server-side projection
            var temp =
                from p in projects
                join par in projectAuditResponsibilitiesQuery on p.ProjectAuditResponsibilityId equals par.Id
                join cpar in clientProjectAuditResponsibilitiesQuery on par.Id equals cpar.ProjectAuditResponsibilityId
                join pra in projectRiskAreasQuery on p.ProjectRiskAreaId equals pra.Id
                join cpra in clientProjectRiskAreasQuery on pra.Id equals cpra.ProjectRiskAreaId
                join cpc in clientProjectCountriesQuery on p.ProjectCountryId equals cpc.Id into tempcpc
                from outercpc in tempcpc.DefaultIfEmpty()
                join ctry in countriesQuery on outercpc.CountryId equals ctry.Id into tempc
                from outerc in tempc.DefaultIfEmpty()
                join bu in projectUnitsQuery on p.ProjectUnitId equals bu.Id into tempbu
                from outerbu in tempbu.DefaultIfEmpty()
                join b in clientProjectUnitsQuery on outerbu.Id equals b.ProjectUnitId into tempb
                from outerb in tempb.DefaultIfEmpty()
                join pd in projectDepartmentsQuery on p.ProjectDepartmentId equals pd.Id into temppd
                from outerd in temppd.DefaultIfEmpty()
                join bd in clientProjectDepartmentsQuery on outerd.Id equals bd.ProjectDepartmentId into tempbd
                from outerbd in tempbd.DefaultIfEmpty()
                join ps in projectStatusesQuery on p.ProjectStatusId equals ps.Id
                select new ClientProjectViewProfile.TempMapper
                {
                    ClientProject = p,
                    ProjectAuditResponsibility = par,
                    ProjectRiskArea = pra,
                    Country = outerc,
                    ProjectUnit = outerbu,
                    ProjectDepartment = outerd,
                    ProjectStatus = ps,
                    Modules = p.Modules
                };

            logger.LogDebug("{MethodName} - Mapping to view models using AutoMapper", methodName);
            var result = temp.ProjectTo<ClientProjectViewModel>(mapper.ConfigurationProvider);

            var executionTime = DateTime.UtcNow - startTime;
            logger.LogInformation("{MethodName} - Successfully retrieved client projects - Duration: {Duration}ms",
                methodName, executionTime.TotalMilliseconds);

            return result;
        }
        catch (Exception ex)
        {
            var executionTime = DateTime.UtcNow - startTime;
            logger.LogError(ex, "{MethodName} - Failed to retrieve client projects - Duration: {Duration}ms, Error: {ErrorMessage}",
                methodName, executionTime.TotalMilliseconds, ex.Message);
            throw;
        }
        finally
        {
            logger.LogInformation("{MethodName} - method execution completed", methodName);
        }
    }

    /// <summary>
    /// Retrieves a specific client project by its unique row identifier (GUID) with related metadata and modules.
    /// </summary>
    /// <param name="rowId">
    /// The unique project row identifier.
    /// </param>
    /// <returns>
    /// The client project view model if found; otherwise, null.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown when no project exists for the supplied <paramref name="rowId"/>.
    /// </exception>
    /// <exception cref="Exception">
    /// Propagates unexpected errors encountered during retrieval.
    /// </exception>
    public async Task<ClientProjectViewModel?> GetByRowIdAsync(Guid rowId)
    {
        const string methodName = $"{ClassName}: {nameof(GetByRowIdAsync)}";
        var startTime = DateTime.UtcNow;

        try
        {
            logger.LogInformation("{MethodName} - Retrieving client project by RowId: {RowId}", methodName, rowId);

            // First check if the project exists
            var project = await unitOfWork.ClientProjects.GetByRowIdAsync(rowId);

            if (project == null)
            {
                logger.LogWarning("{MethodName} - Project not found with RowId: {RowId}", methodName, rowId);
                throw new KeyNotFoundException($"Project with id {rowId} not found.");
            }

            logger.LogDebug("{MethodName} - Project found, retrieving full metadata for RowId: {RowId}", methodName, rowId);
            // Get the full project with all metadata using the existing GetAsync method
            var result = (await GetAsync()).First(x => x.RowId == rowId);

            var executionTime = DateTime.UtcNow - startTime;
            logger.LogInformation("{MethodName} - Successfully retrieved client project - RowId: {RowId}, Duration: {Duration}ms",
                methodName, rowId, executionTime.TotalMilliseconds);

            return result;
        }
        catch (KeyNotFoundException)
        {
            // Re-throw KeyNotFoundException without logging as error (it's expected behavior)
            throw;
        }
        catch (Exception ex)
        {
            var executionTime = DateTime.UtcNow - startTime;
            logger.LogError(ex, "{MethodName} - Failed to retrieve client project - RowId: {RowId}, Duration: {Duration}ms, Error: {ErrorMessage}",
                methodName, rowId, executionTime.TotalMilliseconds, ex.Message);
            throw;
        }
        finally
        {
            logger.LogInformation("{MethodName} - method execution completed", methodName);
        }
    }

    /// <summary>
    /// Creates a new client project along with its modules (if provided).
    /// </summary>
    /// <param name="payload">
    /// The model containing project creation data.
    /// </param>
    /// <returns>
    /// The number of records affected.
    /// </returns>
    /// <remarks>
    /// Inserts only the <see cref="ClientProject"/> entity; client-specific metadata upserts are not performed.
    /// Audit fields and tenant scoping are applied via <see cref="IUserContextService"/> defaults.
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    /// Thrown if a project with the same name already exists within the tenant.
    /// </exception>
    /// <exception cref="KeyNotFoundException">
    /// Thrown if any required master reference (AuditResponsibility or RiskArea) cannot be resolved by RowId.
    /// </exception>
    /// <exception cref="DbUpdateException">
    /// Thrown if the database update fails.
    /// </exception>
    public async Task<int> CreateAsync(ClientProjectCreateModel payload)
    {
        const string methodName = $"{ClassName}: {nameof(CreateAsync)}";
        var startTime = DateTime.UtcNow;

        try
        {
            logger.LogInformation("{MethodName} - Creating new client project - Name: {ProjectName}, ClientId: {ClientId}",
                methodName, payload.Name, userContextService.UserContext?.ClientId);

            // Check for duplicate project names within the tenant
            logger.LogDebug("{MethodName} - Checking for duplicate project name: {ProjectName}", methodName, payload.Name);
            var project = (await unitOfWork.ClientProjects.GetAsync()).FirstOrDefault(x => x.Name == payload.Name);

            if (project != null)
            {
                logger.LogWarning("{MethodName} - Duplicate project name found - Name: {ProjectName}", methodName, payload.Name);
                throw new InvalidOperationException($"Project with name {payload.Name} already exists.");
            }

            // Validate required master data references
            logger.LogDebug("{MethodName} - Validating audit responsibility - RowId: {RowId}", methodName, payload.AuditResponsibilityRowId);
            var auditResponsibilitiesQuery = await unitOfWork.ProjectAuditResponsibilities.GetAsync();
            var auditResponsibility = payload.AuditResponsibilityRowId != Guid.Empty
                ? auditResponsibilitiesQuery.FirstOrDefault(x => x.RowId == payload.AuditResponsibilityRowId)
                : auditResponsibilitiesQuery.FirstOrDefault();

            if (auditResponsibility == null)
            {
                logger.LogError("{MethodName} - Audit responsibility not found - RowId: {RowId}", methodName, payload.AuditResponsibilityRowId);
                throw new KeyNotFoundException($"Audit Responsibility with RowId {payload.AuditResponsibilityRowId} not found.");
            }

            logger.LogDebug("{MethodName} - Validating risk area - RowId: {RowId}", methodName, payload.RiskAreaRowId);
            var riskAreasQuery = await unitOfWork.ProjectRiskAreas.GetAsync();
            var riskArea = payload.RiskAreaRowId != Guid.Empty
                ? riskAreasQuery.FirstOrDefault(x => x.RowId == payload.RiskAreaRowId)
                : riskAreasQuery.FirstOrDefault();

            if (riskArea == null)
            {
                logger.LogError("{MethodName} - Risk area not found - RowId: {RowId}", methodName, payload.RiskAreaRowId);
                throw new KeyNotFoundException($"Risk Area with RowId {payload.RiskAreaRowId} not found.");
            }

            // Validate optional master data references
            logger.LogDebug("{MethodName} - Validating optional country - RowId: {RowId}", methodName, payload.CountryRowId);
            var country = payload.CountryRowId.HasValue
                ? (await unitOfWork.Countries.GetAsync())
                    .FirstOrDefault(x => x.RowId == payload.CountryRowId.Value)
                : null;

            if (payload.CountryRowId.HasValue && country == null)
            {
                logger.LogError("{MethodName} - Country not found - RowId: {RowId}", methodName, payload.CountryRowId.Value);
                throw new KeyNotFoundException($"Country with RowId {payload.CountryRowId.Value} not found.");
            }

            logger.LogDebug("{MethodName} - Validating optional business unit - RowId: {RowId}", methodName, payload.BusinessUnitRowId);
            var businessUnit = payload.BusinessUnitRowId.HasValue
                ? (await unitOfWork.ProjectUnits.GetAsync())
                    .FirstOrDefault(x => x.RowId == payload.BusinessUnitRowId.Value)
                : null;

            if (payload.BusinessUnitRowId.HasValue && businessUnit == null)
            {
                logger.LogError("{MethodName} - Business unit not found - RowId: {RowId}", methodName, payload.BusinessUnitRowId.Value);
                throw new KeyNotFoundException($"Business Unit with RowId {payload.BusinessUnitRowId.Value} not found.");
            }

            logger.LogDebug("{MethodName} - Validating optional business department - RowId: {RowId}", methodName, payload.BusinessDepartmentRowId);
            var businessDepartment = payload.BusinessDepartmentRowId.HasValue
                ? (await unitOfWork.ProjectDepartments.GetAsync())
                    .FirstOrDefault(x => x.RowId == payload.BusinessDepartmentRowId.Value)
                : null;

            if (payload.BusinessDepartmentRowId.HasValue && businessDepartment == null)
            {
                logger.LogError("{MethodName} - Business department not found - RowId: {RowId}", methodName, payload.BusinessDepartmentRowId.Value);
                throw new KeyNotFoundException($"Business Department with RowId {payload.BusinessDepartmentRowId.Value} not found.");
            }

            // Map and configure the project domain entity
            logger.LogDebug("{MethodName} - Mapping payload to domain entity", methodName);
            var domain = mapper.Map<ClientProject>(payload);
            domain.ProjectAuditResponsibilityId = auditResponsibility?.Id ?? 0;
            domain.ProjectRiskAreaId = riskArea?.Id ?? 0;
            domain.ProjectCountryId = country?.Id;
            domain.CreatedBy = userContextService.UserContext?.UserLoginName ?? string.Empty;
            domain.ModifiedBy = userContextService.UserContext?.UserLoginName ?? string.Empty;
            domain.CreatedById = userContextService.UserContext?.UserLoginId ?? 0;
            domain.ModifiedById = userContextService.UserContext?.UserLoginId ?? 0;
            domain.ClientId = userContextService.UserContext?.ClientId ?? 0;
            domain.ProjectUnitId = businessUnit?.Id;
            domain.ProjectDepartmentId = businessDepartment?.Id;
            domain.ProjectStatusId = 1; // Default to 'Active' status

            // Store modules as comma-separated string (defaulting to empty when not provided)
            domain.Modules = string.IsNullOrWhiteSpace(payload.Modules) ? string.Empty : payload.Modules;

            logger.LogDebug("{MethodName} - Setting domain defaults and saving project entity", methodName);
            userContextService.SetDomainDefaults(domain, DataModes.Add);
            _ = await unitOfWork.ClientProjects.AddAsync(domain);
            var count = await unitOfWork.SaveChangesAsync();

            logger.LogInformation("{MethodName} - Project entity saved successfully - ProjectId: {ProjectId}, Records affected: {Count}",
                methodName, domain.Id, count);

            // Generate database name and prepare connection string
            logger.LogDebug("{MethodName} - Generating database name for project: {ProjectName}", methodName, domain.Name);
            var databaseName = GenerateDatabaseName(domain.Name);
            var baseConnection = configuration["ConnectionStrings:DefaultConnection"] ?? string.Empty;

            if (string.IsNullOrWhiteSpace(baseConnection))
            {
                logger.LogWarning("{MethodName} - No base connection string found, skipping scheduler creation", methodName);
                // In unit tests or misconfigured environments, skip scheduler creation when no base connection is provided
                return count;
            }

            logger.LogDebug("{MethodName} - Building connection string for database: {DatabaseName}", methodName, databaseName);
            var connectionBuilder = new SqlConnectionStringBuilder(baseConnection)
            {
                InitialCatalog = databaseName
            };

            // Determine authentication method and prepare connection string
            var masterBuilder = new SqlConnectionStringBuilder(configuration["ConnectionStrings:DefaultConnection"] ?? string.Empty);
            var useIntegratedSecurity = masterBuilder.IntegratedSecurity;

            logger.LogDebug("{MethodName} - Using integrated security: {UseIntegratedSecurity}", methodName, useIntegratedSecurity);

            // Configure connection string based on authentication method
            var finalConnectionString = string.Empty;
            var integratedTemplate = configuration["ProjectTemplates:Integrated"];
            var sqlAuthTemplate = configuration["ProjectTemplates:SqlAuth"];
            LicenseResult license = new LicenseResult();
            if (useIntegratedSecurity && !string.IsNullOrWhiteSpace(integratedTemplate))
            {
                logger.LogDebug("{MethodName} - Using integrated security template", methodName);
                finalConnectionString = integratedTemplate;
                connectionBuilder.Password = "";
            }
            else if (!useIntegratedSecurity && !string.IsNullOrWhiteSpace(sqlAuthTemplate))
            {
                logger.LogDebug("{MethodName} - Using SQL authentication with license encryption", methodName);
                var currentClientId = userContextService.UserContext?.ClientId ?? 0;
                var clientId = unitOfWork.Clients.GetAsync().Result.FirstOrDefault(x => x.Id == currentClientId)?.RowId;

                // Use license-only approach - encrypt password and store only EncryptedLicense
                license = _licenseService.EncryptLicense(connectionBuilder.Password, clientId.ToString());
                string? decryptedPassword = null;
                decryptedPassword = license.EncryptedPrivateKey;

                if (!string.IsNullOrEmpty(decryptedPassword))
                {
                    connectionBuilder.Password = decryptedPassword;
                }
                finalConnectionString = sqlAuthTemplate;
            }

            // Create project scheduler for database operations
            var schedulerRepo = unitOfWork.ProjectSchedulers;
            if (schedulerRepo != null)
            {
                logger.LogDebug("{MethodName} - Creating project scheduler for database operations", methodName);
                var nextId = await schedulerRepo.GetNextSchedulerIdAsync();
                var scheduler = new ProjectSchedulerModel
                {
                    ProjectSchedulerId = nextId,
                    ProjectName = domain.Name,
                    ProjectId = domain.Id,
                    ProjectStatusId = 1,
                    IsActive = true,
                    UserName = useIntegratedSecurity ? string.Empty : connectionBuilder.UserID ?? string.Empty,
                    Password = useIntegratedSecurity ? string.Empty : connectionBuilder.Password ?? string.Empty,
                    ConnectionString = finalConnectionString,
                    DatabaseName = databaseName,
                    CreatedBy = userContextService.UserContext.UserLoginName,
                    ModifiedBy = userContextService.UserContext.UserLoginName,
                    CreatedById = userContextService.UserContext.UserLoginId,
                    ModifiedById = userContextService.UserContext.UserLoginId,
                    EncryptedLicenseKey = useIntegratedSecurity ? string.Empty : license.EncryptedLicense ?? string.Empty
                };

                userContextService.SetDomainDefaults(scheduler, DataModes.Add);
                _ = await schedulerRepo.AddAsync(scheduler);
                _ = await unitOfWork.SaveChangesAsync();

                logger.LogInformation("{MethodName} - Project scheduler created successfully - SchedulerId: {SchedulerId}",
                    methodName, scheduler.ProjectSchedulerId);
            }

            var executionTime = DateTime.UtcNow - startTime;
            logger.LogInformation("{MethodName} - Client project created successfully - ProjectName: {ProjectName}, DatabaseName: {DatabaseName}, Duration: {Duration}ms",
                methodName, domain.Name, databaseName, executionTime.TotalMilliseconds);

            return count;
        }
        catch (DbUpdateException dex)
        {
            var executionTime = DateTime.UtcNow - startTime;
            logger.LogError(dex, "{MethodName} - Database update failed - Duration: {Duration}ms, Error: {ErrorMessage}",
                methodName, executionTime.TotalMilliseconds, dex.Message);

            foreach (var entry in dex.Entries)
            {
                logger.LogError("{MethodName} - DbUpdateException entry: {Entry}", methodName, entry.Entity.ToString());
            }
            throw;
        }
        catch (Exception ex)
        {
            var executionTime = DateTime.UtcNow - startTime;
            logger.LogError(ex, "{MethodName} - Failed to create client project - Duration: {Duration}ms, Error: {ErrorMessage}",
                methodName, executionTime.TotalMilliseconds, ex.Message);
            throw;
        }
        finally
        {
            logger.LogInformation("{MethodName} - method execution completed", methodName);
        }
    }

    /// <summary>
    /// Soft deletes a client project by setting IsActive = false.
    /// </summary>
    /// <param name="rowId">
    /// The unique row identifier (GUID) of the project to delete.
    /// </param>
    /// <returns>
    /// The number of records affected (0 if not found, 1 if updated).
    /// </returns>
    /// <exception cref="DbUpdateException">
    /// Thrown if the database update fails.
    /// </exception>
    public async Task<int> DeleteAsync(Guid rowId)
    {
        const string methodName = $"{ClassName}: {nameof(DeleteAsync)}";
        var startTime = DateTime.UtcNow;

        try
        {
            logger.LogInformation("{MethodName} - Soft deleting client project - RowId: {RowId}", methodName, rowId);

            // Find the project by RowId
            logger.LogDebug("{MethodName} - Searching for project with RowId: {RowId}", methodName, rowId);
            var project = (await unitOfWork.ClientProjects.GetAsync())
                .ToArray()
                .FirstOrDefault(x => x.RowId == rowId);

            if (project == null)
            {
                logger.LogWarning("{MethodName} - Project not found for soft deletion - RowId: {RowId}", methodName, rowId);
                return 0;
            }

            logger.LogDebug("{MethodName} - Project found, performing soft deletion - ProjectName: {ProjectName}", methodName, project.Name);
            // Soft delete by setting IsActive = false
            project.IsActive = false;
            // If domain supports soft delete flag, set it too
            try { (project as dynamic).IsDeleted = true; } catch { /* ignore if not present */ }
            project.ModifiedOn = DateTime.UtcNow;
            project.ModifiedBy = userContextService.UserContext?.UserLoginName ?? string.Empty;
            project.ModifiedById = userContextService.UserContext?.UserLoginId ?? 0;

            // Update the project
            logger.LogDebug("{MethodName} - Updating project entity for soft deletion", methodName);
            await unitOfWork.ClientProjects.UpdateAsync(project);
            var result = await unitOfWork.SaveChangesAsync();

            var executionTime = DateTime.UtcNow - startTime;
            logger.LogInformation("{MethodName} - Project soft deleted successfully - RowId: {RowId}, ProjectName: {ProjectName}, Records affected: {Count}, Duration: {Duration}ms",
                methodName, rowId, project.Name, result, executionTime.TotalMilliseconds);

            return result;
        }
        catch (DbUpdateException dex)
        {
            var executionTime = DateTime.UtcNow - startTime;
            logger.LogError(dex, "{MethodName} - Database update failed during soft deletion - RowId: {RowId}, Duration: {Duration}ms, Error: {ErrorMessage}",
                methodName, rowId, executionTime.TotalMilliseconds, dex.Message);

            foreach (var entry in dex.Entries)
            {
                logger.LogError("{MethodName} - DbUpdateException entry: {Entry}", methodName, entry.Entity.ToString());
            }
            throw;
        }
        catch (Exception ex)
        {
            var executionTime = DateTime.UtcNow - startTime;
            logger.LogError(ex, "{MethodName} - Failed to soft delete project - RowId: {RowId}, Duration: {Duration}ms, Error: {ErrorMessage}",
                methodName, rowId, executionTime.TotalMilliseconds, ex.Message);
            throw;
        }
        finally
        {
            logger.LogInformation("{MethodName} - method execution completed", methodName);
        }
    }

}