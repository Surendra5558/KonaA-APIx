using KonaAI.Master.Repository.DataAccess.Master.App.Interface;
using KonaAI.Master.Repository.DataAccess.Master.MetaData.Interface;
using KonaAI.Master.Repository.DataAccess.Master.UserMetaData.Interface;
using KonaAI.Master.Repository.DataAccess.Tenant.Client.Interface;
using KonaAI.Master.Repository.DataAccess.Tenant.ClientMetaData.Interface;
using KonaAI.Master.Repository.DataAccess.Tenant.ClientUserMetaData.Interface;

namespace KonaAI.Master.Repository.Common.Interface;

/// <summary>
/// Defines the contract for the Unit of Work pattern, coordinating repository operations and database transactions.
/// Provides access to all repositories and a method to commit changes as a single transaction.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    #region App

    /// <summary>
    /// Gets or sets the repository for client application entities.
    /// </summary>
    IClientRepository Clients { get; set; }

    /// <summary>
    /// Gets the repository for application user entities.
    /// </summary>
    IUserRepository Users { get; }

    /// <summary>
    /// Gets the repository for managing and accessing user audit records,
    /// which track user activities and changes for auditing and compliance purposes.
    /// </summary>
    IUserAuditRepository UserAudits { get; }

    /// <summary>
    /// Gets the repository for application-level question bank entities.
    /// </summary>
    IQuestionBankRepository QuestionBanks { get; }

    #endregion App

    #region Meta Data

    /// <summary>
    /// Gets the repository for country metadata entities.
    /// </summary>
    ICountryRepository Countries { get; }

    /// <summary>
    /// Gets the repository for logon type metadata entities.
    /// </summary>
    ILogOnTypeRepository LogOnTypes { get; }

    /// <summary>
    /// Gets the repository for module-source type linkage metadata entities.
    /// </summary>
    IModuleSourceTypeRepository ModuleSourceTypes { get; }

    /// <summary>
    /// Gets the repository for module types linkage metadata entities.
    /// </summary>
    IModuleTypeRepository ModuleTypes { get; }

    /// <summary>
    /// Gets the repository for navigation metadata entities.
    /// </summary>
    INavigationRepository Navigations { get; }

    /// <summary>
    /// Gets the repository for navigation-to-user action mapping metadata entities.
    /// </summary>
    INavigationUserActionRepository NavigationUserActions { get; }

    /// <summary>
    /// Gets the repository for project status metadata entities.
    /// </summary>
    IProjectStatusRepository ProjectStatuses { get; }

    /// <summary>
    /// Gets the repository for role-to-navigation user action permission metadata entities.
    /// </summary>
    IRoleNavigationUserActionRepository RoleNavigationUserActions { get; }

    /// <summary>
    /// Gets the repository for source type metadata entities.
    /// </summary>
    ISourceTypeRepository SourceTypes { get; }

    /// <summary>
    /// Gets the repository for user action metadata entities.
    /// </summary>
    IUserActionRepository UserActions { get; }

    /// <summary>
    /// Gets the repository for client-user association entities.
    /// </summary>
    IClientUserRepository ClientUsers { get; }

    /// <summary>
    /// Gets the repository for managing and retrieving role type entities and provides access to role type data and related operations
    /// </summary>
    IRoleTypeRepository RoleTypes { get; }

    /// <summary>
    /// Gets the repository for render type metadata entities.
    /// </summary>
    IRenderTypeRepository RenderTypes { get; }

    /// <summary>
    /// Gets the repository for project scheduler metadata entities.
    /// </summary>
    IProjectSchedulerRepository ProjectSchedulers { get; }

    #endregion Meta Data

    #region User Meta Data

    /// <summary>
    /// Gets the repository for project audit responsibility metadata entities.
    /// </summary>
    IProjectAuditResponsibilityRepository ProjectAuditResponsibilities { get; }

    /// <summary>
    /// Gets the repository for project department metadata entities.
    /// </summary>
    IProjectDepartmentRepository ProjectDepartments { get; }

    /// <summary>
    /// Gets the repository for project risk area metadata entities.
    /// </summary>
    IProjectRiskAreaRepository ProjectRiskAreas { get; }

    /// <summary>
    /// Gets the repository for project unit metadata entities.
    /// </summary>
    IProjectUnitRepository ProjectUnits { get; }

    #endregion User Meta Data

    #region Client

    /// <summary>
    /// Gets the repository for mappings between client projects and module types.
    /// </summary>
    IClientProjectSourceModuleTypeRepository ClientProjectSourceModuleTypes { get; }

    /// <summary>
    /// Gets the repository for client project entities.
    /// </summary>
    IClientProjectRepository ClientProjects { get; }

    /// <summary>
    /// Gets or sets the repository for client project user entities.
    /// </summary>
    IClientProjectUserRepository ClientProjectUser { get; set; }

    /// <summary>
    /// Gets the repository for client-specific question bank entities.
    /// </summary>
    IClientQuestionBankRepository ClientQuestionBanks { get; }

    /// <summary>
    /// Gets the repository for client questionnaire entities.
    /// </summary>
    IClientQuestionnaireRepository ClientQuestionnaires { get; }

    /// <summary>
    /// Gets the repository for associations between client questionnaires, sections, and question bank entries.
    /// </summary>
    IClientQuestionnaireAssociationRepository ClientQuestionnaireAssociations { get; }

    /// <summary>
    /// Gets the repository for client questionnaire section entities.
    /// </summary>
    IClientQuestionnaireSectionRepository ClientQuestionnaireSections { get; }

    /// <summary>
    /// Gets the repository for managing and retrieving License Info entities and provides access to LicenseInfo data and related operations
    /// </summary>
    IClientLicenseRepository ClientLicenses { get; }

    #endregion Client

    #region Client User MetaData

    /// <summary>
    /// Gets the repository for client-specific project audit responsibility metadata entities.
    /// </summary>
    IClientProjectAuditResponsibilityRepository ClientProjectAuditResponsibilities { get; }

    /// <summary>
    /// Gets the repository for client-specific project country metadata entities.
    /// </summary>
    IClientProjectCountryRepository ClientProjectCountries { get; }

    /// <summary>
    /// Gets the repository for client-specific project department metadata entities.
    /// </summary>
    IClientProjectDepartmentRepository ClientProjectDepartments { get; }

    /// <summary>
    /// Gets the repository for client-specific project risk area metadata entities.
    /// </summary>
    IClientProjectRiskAreaRepository ClientProjectRiskAreas { get; }

    /// <summary>
    /// Gets the repository for client-specific project unit metadata entities.
    /// </summary>
    IClientProjectUnitRepository ClientProjectUnits { get; }

    #endregion Client User MetaData

    #region Client Meta Data

    /// <summary>
    /// Gets the repository for client-specific RoleTypes metadata entities.
    /// </summary>
    IClientRoleTypeRepository ClientRoleTypes { get; }

    #endregion Client Meta Data

    /// <summary>
    /// Commits all pending changes to the database asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>The number of state entries written to the database.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes the specified asynchronous action within the unit-of-work scope.
    /// Implementations may wrap the action in a transaction, committing on success and rolling back on failure.
    /// </summary>
    /// <param name="action">The asynchronous action to execute.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task ExecuteAsync(Func<Task> action);
}