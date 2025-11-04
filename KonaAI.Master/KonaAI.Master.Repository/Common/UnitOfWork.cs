using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.DataAccess.Master.App.Interface;
using KonaAI.Master.Repository.DataAccess.Master.MetaData.Interface;
using KonaAI.Master.Repository.DataAccess.Master.UserMetaData.Interface;
using KonaAI.Master.Repository.DataAccess.Tenant.Client.Interface;
using KonaAI.Master.Repository.DataAccess.Tenant.ClientMetaData.Interface;
using KonaAI.Master.Repository.DataAccess.Tenant.ClientUserMetaData.Interface;
using Microsoft.EntityFrameworkCore;

namespace KonaAI.Master.Repository.Common;

/// <summary>
/// Implements the Unit of Work pattern for managing repository transactions.
/// Provides access to all repositories and coordinates saving changes as a single transaction.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly DefaultContext _context;

    /// <summary>
    /// Gets or sets the repository for client application entities.
    /// </summary>
    public IClientRepository Clients { get; set; }

    /// <summary>
    /// Gets the repository for application user entities.
    /// </summary>
    public IUserRepository Users { get; }

    /// <summary>
    /// Gets the repository for country metadata entities.
    /// </summary>
    public ICountryRepository Countries { get; }

    /// <summary>
    /// Gets the repository for logon type metadata entities.
    /// </summary>
    public ILogOnTypeRepository LogOnTypes { get; }

    /// <summary>
    /// Gets the repository for module-source type linkage metadata entities.
    /// </summary>
    public IModuleSourceTypeRepository ModuleSourceTypes { get; }

    /// <summary>
    /// Gets the repository for module types linkage metadata entities.
    /// </summary>
    public IModuleTypeRepository ModuleTypes { get; }

    /// <summary>
    /// Gets the repository for navigation metadata entities.
    /// </summary>
    public INavigationRepository Navigations { get; }

    /// <summary>
    /// Gets the repository for navigation-to-user action mapping metadata entities.
    /// </summary>
    public INavigationUserActionRepository NavigationUserActions { get; }

    /// <summary>
    /// Gets the repository for project status metadata entities.
    /// </summary>
    public IProjectStatusRepository ProjectStatuses { get; }

    /// <summary>
    /// Gets the repository for role-to-navigation user action permission metadata entities.
    /// </summary>
    public IRoleNavigationUserActionRepository RoleNavigationUserActions { get; }

    /// <summary>
    /// Gets the repository for source type metadata entities.
    /// </summary>
    public ISourceTypeRepository SourceTypes { get; }

    /// <summary>
    /// Gets the repository for user action metadata entities.
    /// </summary>
    public IUserActionRepository UserActions { get; }

    /// <summary>
    /// Gets the repository for project audit responsibility metadata entities.
    /// </summary>
    public IProjectAuditResponsibilityRepository ProjectAuditResponsibilities { get; }

    /// <summary>
    /// Gets the repository for project department metadata entities.
    /// </summary>
    public IProjectDepartmentRepository ProjectDepartments { get; }

    /// <summary>
    /// Gets the repository for project risk area metadata entities.
    /// </summary>
    public IProjectRiskAreaRepository ProjectRiskAreas { get; }

    /// <summary>
    /// Gets the repository for project unit metadata entities.
    /// </summary>
    public IProjectUnitRepository ProjectUnits { get; }

    /// <summary>
    /// Gets the repository for mappings between client projects and module types.
    /// </summary>
    public IClientProjectSourceModuleTypeRepository ClientProjectSourceModuleTypes { get; }

    /// <summary>
    /// Gets the repository for client project entities.
    /// </summary>
    public IClientProjectRepository ClientProjects { get; }

    /// <summary>
    /// Gets the repository for client-user association entities.
    /// </summary>
    public IClientUserRepository ClientUsers { get; }

    /// <summary>
    /// Gets the repository for client-specific project audit responsibility metadata entities.
    /// </summary>
    public IClientProjectAuditResponsibilityRepository ClientProjectAuditResponsibilities { get; }

    /// <summary>
    /// Gets the repository for client-specific project country metadata entities.
    /// </summary>
    public IClientProjectCountryRepository ClientProjectCountries { get; }

    /// <summary>
    /// Gets the repository for client-specific project department metadata entities.
    /// </summary>
    public IClientProjectDepartmentRepository ClientProjectDepartments { get; }

    /// <summary>
    /// Gets the repository for client-specific project risk area metadata entities.
    /// </summary>
    public IClientProjectRiskAreaRepository ClientProjectRiskAreas { get; }

    /// <summary>
    /// Gets the repository for client-specific project unit metadata entities.
    /// </summary>
    public IClientProjectUnitRepository ClientProjectUnits { get; }

    /// <summary>
    /// Gets the repository for question bank entities.
    /// </summary>
    public IQuestionBankRepository QuestionBanks { get; }

    /// <summary>
    /// Gets the repository for client questionnaire entities.
    /// </summary>
    public IClientQuestionBankRepository ClientQuestionBanks { get; }

    /// <summary>
    /// Gets the repository for client questionnaire association entities.
    /// </summary>
    public IClientQuestionnaireAssociationRepository ClientQuestionnaireAssociations { get; }

    /// <summary>
    /// Gets the repository for client questionnaire entities.
    /// </summary>
    public IClientQuestionnaireRepository ClientQuestionnaires { get; }

    /// <summary>
    /// Gets the repository for client questionnaire sections entities.
    /// </summary>
    public IClientQuestionnaireSectionRepository ClientQuestionnaireSections { get; }

    /// <summary>
    /// Gets the repository that handles user audit records,
    /// providing access to user activity and change history data.
    /// </summary>
    public IUserAuditRepository UserAudits { get; }

    ///<summary>
    /// Gets or sets the repository for managing client role types.
    /// </summary>
    public IClientRoleTypeRepository ClientRoleTypes { get; set; }

    /// <summary>
    /// Gets the repository for managing and retrieving role type entities,
    /// providing access to role type data and related operations.
    /// </summary>
    public IRoleTypeRepository RoleTypes { get; set; }

    /// <summary>
    /// Gets the repository for managing and retrieving License Info entities,
    /// providing access to LicenseInfo data and related operations.
    /// </summary>
    public IClientLicenseRepository ClientLicenses { get; }

    /// <summary>
    /// Gets the repository for managing and retrieving render type entities,
    /// providing access to role type data and related operations.
    /// </summary>
    public IRenderTypeRepository RenderTypes { get; set; }

    /// <summary>
    /// Gets the repository for managing and retrieving License Info entities,
    /// providing access to LicenseInfo data and related operations.
    /// </summary>
    public IProjectSchedulerRepository ProjectSchedulers { get; }

    /// <summary>
    /// Gets the repository for managing and retrieving client project user entities,
    /// providing access to client-user relationships and related operations.
    /// </summary>
    public IClientProjectUserRepository ClientProjectUser { get; set; }
    /// <summary>
    /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
    /// </summary>
    /// <param name="context">The EF Core context instance.</param>
    /// <param name="clients">The client repository.</param>
    /// <param name="users">The user repository.</param>
    /// <param name="countries">The country repository.</param>
    /// <param name="logOnTypes">The logon type repository.</param>
    /// <param name="moduleSourceTypes">The module-source type repository.</param>
    /// <param name="navigations">The navigation repository.</param>
    /// <param name="navigationUserActions">The navigation user action repository.</param>
    /// <param name="projectStatuses">The project status repository.</param>
    /// <param name="roleNavigationUserActions">The role navigation user action repository.</param>
    /// <param name="sourceTypes">The source type repository.</param>
    /// <param name="userActions">The user action repository.</param>
    /// <param name="projectAuditResponsibilities">The project audit responsibility repository.</param>
    /// <param name="projectDepartments">The project department repository.</param>
    /// <param name="projectRiskAreas">The project risk area repository.</param>
    /// <param name="projectUnits">The project unit repository.</param>
    /// <param name="clientProjectSourceModuleTypes">The client project-module type repository.</param>
    /// <param name="clientProjects">The client project repository.</param>
    /// <param name="clientUsers">The client-user repository.</param>
    /// <param name="clientProjectAuditResponsibilities"></param>
    /// <param name="clientProjectCountries"></param>
    /// <param name="clientProjectDepartments"></param>
    /// <param name="clientProjectRiskAreas"></param>
    /// <param name="clientProjectUnits"></param>
    /// <param name="clientQuestionnaireSections"></param>
    /// <param name="clientRoleTypes"></param>"
    /// <param name="moduleTypes"></param>
    /// <param name="userAudit"></param>
    /// <param name="renderTypes"></param>
    /// <param name="questionBanks"></param>
    /// <param name="clientQuestionBanks"></param>
    /// <param name="questionnaireAssociations"></param>
    /// <param name="clientProjectUser"></param>
    /// <param name="clientQuestionnaires"></param>
    /// <param name="roleTypes"></param>
    /// <param name="clientLicenses"></param>
    /// <param name="schedulerRepository">The project scheduler repository.</param>
    public UnitOfWork(DefaultContext context, IClientRepository clients, IUserRepository users,
        ICountryRepository countries, ILogOnTypeRepository logOnTypes, IModuleSourceTypeRepository moduleSourceTypes,
        INavigationRepository navigations, INavigationUserActionRepository navigationUserActions,
        IProjectStatusRepository projectStatuses, IRoleNavigationUserActionRepository roleNavigationUserActions,
        ISourceTypeRepository sourceTypes, IUserActionRepository userActions,
        IProjectAuditResponsibilityRepository projectAuditResponsibilities,
        IProjectDepartmentRepository projectDepartments, IProjectRiskAreaRepository projectRiskAreas,
        IProjectUnitRepository projectUnits, IClientProjectSourceModuleTypeRepository clientProjectSourceModuleTypes,
        IClientProjectRepository clientProjects, IClientUserRepository clientUsers, IClientProjectAuditResponsibilityRepository clientProjectAuditResponsibilities,
        IClientProjectCountryRepository clientProjectCountries, IClientProjectDepartmentRepository clientProjectDepartments,
        IClientProjectRiskAreaRepository clientProjectRiskAreas, IClientProjectUnitRepository clientProjectUnits,
        IModuleTypeRepository moduleTypes, IUserAuditRepository userAudit,
        IClientRoleTypeRepository clientRoleTypes, IRoleTypeRepository roleTypes, IClientLicenseRepository clientLicenses, IRenderTypeRepository renderTypes,
        IQuestionBankRepository questionBanks, IClientQuestionBankRepository clientQuestionBanks,
        IClientQuestionnaireAssociationRepository questionnaireAssociations, IClientProjectUserRepository clientProjectUser,
        IClientQuestionnaireRepository clientQuestionnaires, IClientQuestionnaireSectionRepository clientQuestionnaireSections,
        IProjectSchedulerRepository schedulerRepository)
    {
        _context = context;
        Clients = clients;
        Users = users;
        Countries = countries;
        LogOnTypes = logOnTypes;
        ModuleSourceTypes = moduleSourceTypes;
        Navigations = navigations;
        NavigationUserActions = navigationUserActions;
        ProjectStatuses = projectStatuses;
        RoleNavigationUserActions = roleNavigationUserActions;
        SourceTypes = sourceTypes;
        UserActions = userActions;
        ProjectAuditResponsibilities = projectAuditResponsibilities;
        ProjectDepartments = projectDepartments;
        ProjectRiskAreas = projectRiskAreas;
        ProjectUnits = projectUnits;
        ClientProjectSourceModuleTypes = clientProjectSourceModuleTypes;
        ClientProjects = clientProjects;
        ClientUsers = clientUsers;
        ClientProjectAuditResponsibilities = clientProjectAuditResponsibilities;
        ClientProjectDepartments = clientProjectDepartments;
        ClientProjectCountries = clientProjectCountries;
        ClientProjectRiskAreas = clientProjectRiskAreas;
        ClientProjectUnits = clientProjectUnits;
        ModuleTypes = moduleTypes;
        UserAudits = userAudit;
        ClientRoleTypes = clientRoleTypes;
        RoleTypes = roleTypes;
        ClientLicenses = clientLicenses;
        QuestionBanks = questionBanks;
        RenderTypes = renderTypes;
        ClientQuestionBanks = clientQuestionBanks;
        ClientQuestionnaireAssociations = questionnaireAssociations;
        ClientQuestionnaires = clientQuestionnaires;
        ClientQuestionnaireSections = clientQuestionnaireSections;
        ProjectSchedulers = schedulerRepository;
        ClientProjectUser = clientProjectUser;
    }

    /// <summary>
    /// Releases all resources used by the <see cref="UnitOfWork"/>.
    /// </summary>
    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Asynchronously commits all changes made in this context to the database.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A task that represents the asynchronous save operation. The task result contains the number of state entries written to the database.
    /// </returns>
    /// <exception cref="DbUpdateException">An error is encountered while saving to the database.</exception>
    /// <exception cref="DbUpdateConcurrencyException">A concurrency violation is encountered while saving to the database.</exception>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken"/> is canceled.</exception>
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Executes the specified asynchronous action within a database transaction and commits the transaction if the action succeeds.
    /// </summary>
    /// <param name="action">The asynchronous action to execute within the transaction.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="Exception">A delegate callback throws an exception.</exception>
    /// <exception cref="DbUpdateException">An error is encountered while saving to the database.</exception>
    /// <exception cref="DbUpdateConcurrencyException">A concurrency violation is encountered while saving to the database.</exception>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken"/> is canceled.</exception>
    public async Task ExecuteAsync(Func<Task> action)
    {
        var strategy = _context.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            // Start a transaction
            await using var transaction = await _context.Database.BeginTransactionAsync();

            await action(); // perform the passed-in logic

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        });
    }

    /// <summary>
    /// Explicit <see cref="IUnitOfWork.SaveChangesAsync(CancellationToken)"/> implementation.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>The number of state entries written to the database.</returns>
    async Task<int> IUnitOfWork.SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            throw;
        }
    }

    /// <summary>
    /// Explicit <see cref="IDisposable.Dispose"/> implementation.
    /// Disposes the underlying context.
    /// </summary>
    void IDisposable.Dispose()
    {
        _context.Dispose();
    }
}