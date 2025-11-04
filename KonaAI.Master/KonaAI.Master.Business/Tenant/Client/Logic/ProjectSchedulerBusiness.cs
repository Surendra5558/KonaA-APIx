using KonaAI.Master.Business.Tenant.Client.Logic.Interface;
using KonaAI.Master.Model.Common.Constants;
using KonaAI.Master.Repository;
using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Domain.Tenant.Client;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.SqlServer.Dac;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System.Text;
using System.Text.RegularExpressions;

namespace KonaAI.Master.Business.Tenant.Client.Logic;

/// <summary>
/// Executes scheduled provisioning tasks for newly created projects.
/// Creates per-project databases and runs DACPAC/SQL deployments, updating status and errors.
/// Handles database creation, DACPAC deployment, and SQL script execution for project provisioning.
/// </summary>
/// <param name="db">The database context for direct database operations.</param>
/// <param name="configuration">The configuration instance for accessing application settings.</param>
/// <param name="uow">The unit of work pattern implementation for database operations.</param>
/// <param name="logger">The logger instance for recording execution information and errors.</param>
public class ProjectSchedulerBusiness(DefaultContext db, IConfiguration configuration, IUnitOfWork uow, ILogger<ProjectSchedulerBusiness> logger) : IProjectSchedulerBusiness
{
    private readonly DefaultContext _db = db;
    private readonly IConfiguration _configuration = configuration;
    private readonly IUnitOfWork _uow = uow;
    private readonly ILogger<ProjectSchedulerBusiness> _logger = logger;

    /// <summary>
    /// Processes active schedulers (status 1), ensures target DB exists, deploys DACPAC/SQL,
    /// and updates <see cref="ProjectScheduler"/> and related project status.
    /// </summary>
    /// <returns>A task that completes when processing finishes.</returns>
    /// <exception cref="Exception">Thrown when deployment operations fail or database operations encounter errors.</exception>
    public async Task Execute()
    {
        const string methodName = nameof(Execute);
        var startTime = DateTime.UtcNow;

        try
        {
            _logger.LogInformation("{MethodName} - Starting project scheduler execution", methodName);

            // Get deployment artifact paths from Constants
            _logger.LogDebug("{MethodName} - Loading deployment paths from Constants", methodName);
            string dacpacPath = Constants.DacPacPath;
            string sqlScriptPath = Constants.SqlScriptPath;

            _logger.LogDebug("{MethodName} - DacPacPath: {DacPacPath}, SqlScriptPath: {SqlScriptPath}",
                methodName, dacpacPath, sqlScriptPath);

            var configured = _configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(configured))
            {
                _logger.LogWarning("{MethodName} - DefaultConnection missing; skipping execution", methodName);
                await Task.CompletedTask;
                return;
            }

            _logger.LogDebug("{MethodName} - Normalizing connection string", methodName);
            string masterConnectionString = NormalizeSqlConnectionString(configured);

            // 1) Query all active schedulers with ProjectStatusId == 1 (deferred, no ToList)
            _logger.LogDebug("{MethodName} - Querying active schedulers with ProjectStatusId == 1", methodName);
            var schedulersQuery = (await _uow.ProjectSchedulers.GetAsync())
                .Where(ps => ps.IsActive && ps.ProjectStatusId == 1);

            var schedulerCount = 0;
            await foreach (var scheduler in schedulersQuery.AsAsyncEnumerable())
            {
                schedulerCount++;
                bool executionSuccess = false;
                string? errorMessage = null;
                var schedulerStartTime = DateTime.UtcNow;

                try
                {
                    _logger.LogInformation("{MethodName} - Processing scheduler {SchedulerId}: {ProjectName}",
                        methodName, scheduler.ProjectSchedulerId, scheduler.ProjectName);

                    // You said you'll provide DB name dynamically; assuming ProjectName holds desired DB name
                    var targetDatabaseName = scheduler.ProjectName ?? $"Project_{scheduler.ProjectSchedulerId}";

                    // Sanitize database name to avoid special characters
                    targetDatabaseName = SanitizeDatabaseName(targetDatabaseName);
                    _logger.LogDebug("{MethodName} - Sanitized database name: {DatabaseName}", methodName, targetDatabaseName);

                    // 2) Ensure database exists
                    _logger.LogDebug("{MethodName} - Ensuring database exists: {DatabaseName}", methodName, targetDatabaseName);
                    await EnsureDatabaseExistsAsync(masterConnectionString, targetDatabaseName);

                    // 3) Process DacPacPath (can be DACPAC or SQL script)
                    if (!string.IsNullOrWhiteSpace(dacpacPath) && File.Exists(dacpacPath))
                    {
                        _logger.LogInformation("{MethodName} - Processing DACPAC deployment: {DacPacPath}", methodName, dacpacPath);
                        await ProcessDeploymentFileAsync(dacpacPath, targetDatabaseName, masterConnectionString, "DacPacPath");
                    }
                    else
                    {
                        _logger.LogWarning("{MethodName} - DACPAC file not found or empty: {DacPacPath}", methodName, dacpacPath);
                    }

                    // 4) Process DbScriptPath (can be DACPAC or SQL script)
                    if (!string.IsNullOrWhiteSpace(sqlScriptPath) && File.Exists(sqlScriptPath))
                    {
                        _logger.LogInformation("{MethodName} - Processing SQL script deployment: {SqlScriptPath}", methodName, sqlScriptPath);
                        await ProcessDeploymentFileAsync(sqlScriptPath, targetDatabaseName, masterConnectionString, "DbScriptPath");
                    }
                    else
                    {
                        _logger.LogWarning("{MethodName} - SQL script file not found or empty: {SqlScriptPath}", methodName, sqlScriptPath);
                    }

                    executionSuccess = true;
                    var schedulerDuration = DateTime.UtcNow - schedulerStartTime;
                    _logger.LogInformation("{MethodName} - Successfully processed project: {ProjectName} in {Duration}ms",
                        methodName, scheduler.ProjectName, schedulerDuration.TotalMilliseconds);
                }
                catch (Exception ex)
                {
                    var schedulerDuration = DateTime.UtcNow - schedulerStartTime;
                    _logger.LogError(ex, "{MethodName} - Failed to process project {ProjectName} in {Duration}ms: {ErrorMessage}",
                        methodName, scheduler.ProjectName, schedulerDuration.TotalMilliseconds, ex.Message);
                    executionSuccess = false;
                    errorMessage = ex.Message;
                }
                finally
                {
                    // Update status in both tables
                    _logger.LogDebug("{MethodName} - Updating project status for {ProjectName}", methodName, scheduler.ProjectName);
                    await UpdateProjectStatusAsync(scheduler, executionSuccess, errorMessage);
                }
            }

            var totalDuration = DateTime.UtcNow - startTime;
            _logger.LogInformation("{MethodName} - Completed processing {SchedulerCount} schedulers in {Duration}ms",
                methodName, schedulerCount, totalDuration.TotalMilliseconds);
        }
        catch (Exception ex)
        {
            var totalDuration = DateTime.UtcNow - startTime;
            _logger.LogError(ex, "{MethodName} - Deployment failed after {Duration}ms: {ErrorMessage}",
                methodName, totalDuration.TotalMilliseconds, ex.Message);
            throw;
        }
        finally
        {
            _logger.LogInformation("{MethodName} - Project scheduler execution completed", methodName);
        }
    }

    /// <summary>
    /// Splits a SQL script string into executable batches by GO separators.
    /// This method handles SQL Server's batch separator syntax and ensures proper execution order.
    /// </summary>
    /// <param name="script">The full SQL script content to be split into batches.</param>
    /// <returns>Array of SQL batches without GO markers, ready for sequential execution.</returns>
    /// <exception cref="ArgumentNullException">Thrown when script parameter is null.</exception>
    private static string[] SplitSqlScript(string script)
    {
        if (string.IsNullOrWhiteSpace(script))
            return Array.Empty<string>();

        var batches = new List<string>();
        var lines = script.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);
        var currentBatch = new StringBuilder();

        foreach (var line in lines)
        {
            var trimmedLine = line.Trim();

            // Check if this line is a GO statement (case insensitive)
            // GO is SQL Server's batch separator that indicates end of current batch
            if (trimmedLine.Equals("GO", StringComparison.OrdinalIgnoreCase))
            {
                // End current batch if it has content
                var batchContent = currentBatch.ToString().Trim();
                if (!string.IsNullOrWhiteSpace(batchContent))
                {
                    batches.Add(batchContent);
                }
                currentBatch.Clear();
            }
            else
            {
                // Add line to current batch (preserve original formatting)
                currentBatch.AppendLine(line);
            }
        }

        // Add the last batch if it has content (handles scripts without final GO)
        var finalBatch = currentBatch.ToString().Trim();
        if (!string.IsNullOrWhiteSpace(finalBatch))
        {
            batches.Add(finalBatch);
        }

        return batches.ToArray();
    }

    /// <summary>
    /// Normalizes a SQL connection string by converting non-standard keys (e.g., Username, pwd)
    /// to <c>User ID</c> and <c>Password</c> supported by <see cref="SqlConnectionStringBuilder"/>.
    /// This method ensures compatibility with various connection string formats and standardizes them for SqlClient.
    /// </summary>
    /// <param name="connectionString">The input connection string to be normalized.</param>
    /// <returns>A normalized connection string with standard SqlClient parameter names.</returns>
    /// <exception cref="ArgumentNullException">Thrown when connectionString parameter is null.</exception>
    private static string NormalizeSqlConnectionString(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            return connectionString;

        // Replace non-standard keys with SqlClient-supported keys
        // This handles various formats that might be used in configuration files
        string normalized = connectionString;

        // Convert username variations to standard "User ID"
        normalized = Regex.Replace(normalized, "(?i)(^|;)\\s*username\\s*=", "$1User ID=", RegexOptions.Compiled);
        normalized = Regex.Replace(normalized, "(?i)(^|;)\\s*user\\s*=", "$1User ID=", RegexOptions.Compiled);

        // Convert password variations to standard "Password"
        normalized = Regex.Replace(normalized, "(?i)(^|;)\\s*pwd\\s*=", "$1Password=", RegexOptions.Compiled);

        return normalized;
    }

    /// <summary>
    /// Sanitizes a proposed SQL Server database name to valid characters and length.
    /// This method ensures the database name complies with SQL Server naming conventions and constraints.
    /// </summary>
    /// <param name="databaseName">The proposed database name to be sanitized.</param>
    /// <returns>A safe database name that complies with SQL Server naming rules.</returns>
    private static string SanitizeDatabaseName(string databaseName)
    {
        if (string.IsNullOrWhiteSpace(databaseName))
            return "DefaultProject";

        // Remove or replace invalid characters for SQL Server database names
        // SQL Server identifiers cannot contain special characters except underscore
        var sanitized = Regex.Replace(databaseName, Constants.DatabaseNameSanitizationRegex, "_");

        // SQL Server identifiers cannot start with a digit
        if (char.IsDigit(sanitized[0]))
            sanitized = "DB_" + sanitized;

        // SQL Server database names have a maximum length of 128 characters
        if (sanitized.Length > 128)
            sanitized = sanitized.Substring(0, 128);

        // Clean up multiple consecutive underscores
        while (sanitized.Contains("__"))
            sanitized = sanitized.Replace("__", "_");

        // Remove leading/trailing underscores
        sanitized = sanitized.Trim('_');

        // Ensure we have a valid name after sanitization
        return string.IsNullOrWhiteSpace(sanitized) ? "DefaultProject" : sanitized;
    }

    /// <summary>
    /// Ensures the target database exists on the server defined by <paramref name="masterConnectionString"/>.
    /// Creates it if missing. This method connects to the master database to perform the check and creation.
    /// </summary>
    /// <param name="masterConnectionString">Connection string to the server; DB switched to master internally.</param>
    /// <param name="databaseName">Database name to ensure exists.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="SqlException">Thrown when SQL Server operations fail.</exception>
    /// <exception cref="Exception">Thrown when general database operations fail.</exception>
    private static async Task EnsureDatabaseExistsAsync(string masterConnectionString, string databaseName)
    {
        try
        {
            // Log the database check operation
            // Note: This is a static method, so we can't use instance logger
            // In a real implementation, you might want to pass a logger as parameter

            var builder = new SqlConnectionStringBuilder(masterConnectionString) { InitialCatalog = "master" };

            await using var connection = new SqlConnection(builder.ConnectionString);
            await connection.OpenAsync();

            // Check if database exists using DB_ID function
            // DB_ID returns NULL if database doesn't exist
            var checkCmd = $"SELECT DB_ID('{databaseName.Replace("'", "''")}')";
            await using var checkCommand = new SqlCommand(checkCmd, connection);
            var dbId = await checkCommand.ExecuteScalarAsync();

            if (dbId == DBNull.Value)
            {
                // Database doesn't exist, create it
                var createCmd = $"CREATE DATABASE [{databaseName}]";
                await using var createCommand = new SqlCommand(createCmd, connection);
                await createCommand.ExecuteNonQueryAsync();

                // Wait for database to be fully created before proceeding
                // This prevents issues with immediate operations on newly created databases
                await Task.Delay(2000);
            }
        }
        catch (SqlException)
        {
            // Re-throw SQL exceptions with context
            throw;
        }
        catch (Exception)
        {
            // Re-throw general exceptions with context
            throw;
        }
    }

    /// <summary>
    /// Deploys either a DACPAC or executes a SQL script against the target database.
    /// This method handles both DACPAC deployments and SQL script execution based on file extension.
    /// </summary>
    /// <param name="filePath">Path to DACPAC or SQL file to be deployed.</param>
    /// <param name="targetDatabaseName">Target database name for deployment.</param>
    /// <param name="masterConnectionString">Server connection string (DB will be set to target).</param>
    /// <param name="pathType">Label to identify the source of the deployment file for logging purposes.</param>
    /// <returns>A task representing the asynchronous deployment operation.</returns>
    /// <exception cref="ArgumentException">Thrown when file path is invalid or file doesn't exist.</exception>
    /// <exception cref="Exception">Thrown when deployment operations fail.</exception>
    private static async Task ProcessDeploymentFileAsync(string filePath, string targetDatabaseName, string masterConnectionString, string pathType)
    {
        try
        {
            if (filePath.EndsWith(".dacpac", StringComparison.OrdinalIgnoreCase))
            {
                // Deploy DACPAC using SQL Server Data-Tier Application Framework
                var dacpac = DacPackage.Load(filePath);
                var builder = new SqlConnectionStringBuilder(masterConnectionString) { InitialCatalog = targetDatabaseName };
                var dacService = new DacServices(builder.ConnectionString);

                // Configure DACPAC deployment options
                // BlockOnPossibleDataLoss = false allows deployment even if data might be lost
                // CreateNewDatabase = true ensures database is created if it doesn't exist
                dacService.Deploy(dacpac, targetDatabaseName, upgradeExisting: true, options: new DacDeployOptions
                {
                    BlockOnPossibleDataLoss = false,
                    CreateNewDatabase = true
                });
            }
            else if (filePath.EndsWith(".sql", StringComparison.OrdinalIgnoreCase))
            {
                // Execute SQL script against target database
                await EnsureDatabaseExistsAsync(masterConnectionString, targetDatabaseName);

                var builder = new SqlConnectionStringBuilder(masterConnectionString) { InitialCatalog = targetDatabaseName };
                await ExecuteSqlBatchAsync(builder.ConnectionString, filePath);
            }
            else
            {
                // File type not supported - this should be caught earlier but provides safety
                throw new ArgumentException($"File {filePath} from {pathType} is neither a .dacpac nor .sql file. Unsupported file type.");
            }
        }
        catch (Exception ex)
        {
            // Re-throw with additional context for better error handling
            throw new Exception($"Deployment from {pathType} failed: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Executes a SQL script string with basic retry logic.
    /// This method implements retry logic to handle transient SQL Server connection issues.
    /// </summary>
    /// <param name="connectionString">Database connection string for script execution.</param>
    /// <param name="script">SQL script text to be executed.</param>
    /// <returns>A task representing the asynchronous script execution operation.</returns>
    /// <exception cref="SqlException">Thrown when SQL Server operations fail after all retry attempts.</exception>
    /// <exception cref="Exception">Thrown when general script execution fails after all retry attempts.</exception>
    private static async Task ExecuteSqlBatchAsync(string connectionString, string script)
    {
        const int maxRetries = 3;
        const int retryDelayMs = 2000;

        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {
                // Split script into executable batches for proper SQL Server execution
                var batches = SplitSqlScript(script);

                await using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();

                // Execute the script using the CreateProjectDB method
                // This method handles the actual SQL execution with proper error handling
                CreateProjectDB(connection.Database, connection.Database, connectionString, script);
                return;
            }
            catch (SqlException)
            {
                // Handle SQL-specific errors with retry logic
                if (attempt < maxRetries)
                {
                    // Wait before retrying to allow transient issues to resolve
                    await Task.Delay(retryDelayMs);
                }
                else
                {
                    // All retry attempts exhausted, re-throw the exception
                    throw;
                }
            }
            catch (Exception)
            {
                // Handle general errors with retry logic
                if (attempt < maxRetries)
                {
                    // Wait before retrying to allow transient issues to resolve
                    await Task.Delay(retryDelayMs);
                }
                else
                {
                    // All retry attempts exhausted, re-throw the exception
                    throw;
                }
            }
        }
    }

    /// <summary>
    /// Updates scheduler and project status to Completed (4) or Failed (5), and records error message on failure.
    /// This method updates both the ProjectScheduler and ClientProject tables to maintain consistency.
    /// </summary>
    /// <param name="scheduler">The scheduler row being processed.</param>
    /// <param name="success">True if deployment succeeded, false otherwise.</param>
    /// <param name="errorMessage">Optional error message to record when deployment fails.</param>
    /// <returns>A task representing the asynchronous status update operation.</returns>
    private async Task UpdateProjectStatusAsync(ProjectScheduler scheduler, bool success, string? errorMessage)
    {
        const string methodName = nameof(UpdateProjectStatusAsync);
        var startTime = DateTime.UtcNow;

        try
        {
            int statusId = success ? 4 : 5; // 4 = Completed, 5 = Failed
            string statusDescription = success ? "Completed" : "Failed";

            _logger.LogInformation("{MethodName} - Updating status for project {ProjectName} to: {StatusDescription}",
                methodName, scheduler.ProjectName, statusDescription);

            // Update ProjectScheduler table
            var schedulersQueryable = await _uow.ProjectSchedulers.GetAsync();
            var schedulerToUpdate = await schedulersQueryable
                .FirstOrDefaultAsync(ps => ps.ProjectSchedulerId == scheduler.ProjectSchedulerId);

            if (schedulerToUpdate != null)
            {
                schedulerToUpdate.ProjectStatusId = statusId;
                if (!success && !string.IsNullOrWhiteSpace(errorMessage))
                {
                    schedulerToUpdate.ErrorMessage = errorMessage;
                }
                await _uow.ProjectSchedulers.UpdateAsync(schedulerToUpdate);
                await _uow.SaveChangesAsync();
                _logger.LogInformation("{MethodName} - Updated ProjectScheduler status for {ProjectName}",
                    methodName, scheduler.ProjectName);
            }
            else
            {
                _logger.LogWarning("{MethodName} - ProjectScheduler not found for ID {SchedulerId}",
                    methodName, scheduler.ProjectSchedulerId);
            }

            // Update ClientProject table if ProjectId is valid
            if (scheduler.ProjectId > 0)
            {
                var projectToUpdate = await _db.Set<ClientProject>()
                    .FirstOrDefaultAsync(cp => cp.Id == scheduler.ProjectId);

                if (projectToUpdate != null)
                {
                    projectToUpdate.ProjectStatusId = statusId;
                    projectToUpdate.ModifiedOn = DateTime.UtcNow;
                    await _db.SaveChangesAsync();
                    _logger.LogInformation("{MethodName} - Updated ClientProject status for {ProjectName}",
                        methodName, scheduler.ProjectName);
                }
                else
                {
                    _logger.LogWarning("{MethodName} - ClientProject not found for ID {ProjectId}",
                        methodName, scheduler.ProjectId);
                }
            }
            else
            {
                _logger.LogDebug("{MethodName} - No valid ProjectId for scheduler {SchedulerId}, skipping ClientProject update",
                    methodName, scheduler.ProjectSchedulerId);
            }
        }
        catch (Exception ex)
        {
            var executionTime = DateTime.UtcNow - startTime;
            _logger.LogError(ex, "{MethodName} - Failed to update status for project {ProjectName} after {Duration}ms: {ErrorMessage}",
                methodName, scheduler.ProjectName, executionTime.TotalMilliseconds, ex.Message);
            throw;
        }
        finally
        {
            var totalDuration = DateTime.UtcNow - startTime;
            _logger.LogDebug("{MethodName} - Status update completed for {ProjectName} in {Duration}ms",
                methodName, scheduler.ProjectName, totalDuration.TotalMilliseconds);
        }
    }

    /// <summary>
    /// Executes a preloaded SQL script to create and initialize a project database.
    /// This method reads a SQL script file, performs token replacement, and executes it against the target database.
    /// </summary>
    /// <param name="projectName">Project name used for token replacement in the SQL script.</param>
    /// <param name="DBName">Database name (informational parameter).</param>
    /// <param name="connectionstring">Connection string to the target database.</param>
    /// <param name="ProjectDBPath">Path to the SQL script file to be executed.</param>
    /// <exception cref="FileNotFoundException">Thrown when the SQL script file is not found.</exception>
    /// <exception cref="SqlException">Thrown when SQL execution fails.</exception>
    /// <exception cref="Exception">Thrown when general script execution fails.</exception>
    private static void CreateProjectDB(string projectName, string DBName, string connectionstring, string ProjectDBPath)
    {
        Microsoft.Data.SqlClient.SqlConnection? sqlConnection = null;
        Server? server = null;
        try
        {
            // Read the SQL script file content
            string dbScripts = File.ReadAllText(ProjectDBPath);

            // Replace database name tokens in the script
            // This allows the script to be parameterized for different projects
            dbScripts = dbScripts.Replace("$(DatabaseName)", projectName.Replace(" ", ""));

            // Create SQL Server connection and SMO Server object
            sqlConnection = new Microsoft.Data.SqlClient.SqlConnection(connectionstring);
            server = new Server(new ServerConnection(sqlConnection));

            // Skip initial statements until we find "USE [master]" to avoid executing setup commands
            bool skipSQLStatements = true;

            // Split script by GO statements and execute each batch
            // GO is SQL Server's batch separator that must be handled specially
            foreach (var script in dbScripts.Split("GO\r\n", StringSplitOptions.RemoveEmptyEntries))
            {
                // Start executing after we find the "USE [master]" statement
                if (skipSQLStatements && script.Contains("USE [master]", StringComparison.CurrentCultureIgnoreCase))
                    skipSQLStatements = false;

                if (!skipSQLStatements)
                {
                    try
                    {
                        // Execute the SQL batch using SMO
                        server.ConnectionContext.ExecuteNonQuery(script);
                    }
                    catch (Exception)
                    {
                        // Re-throw SQL execution errors
                        throw;
                    }
                }
            }
        }
        catch (Exception)
        {
            // Re-throw all exceptions to maintain error context
            throw;
        }
        finally
        {
            // Ensure proper cleanup of database connections
            if (sqlConnection != null)
            {
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
        }
    }
}
