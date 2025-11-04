using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.DataAccess.Master.App.Interface;
using KonaAI.Master.Repository.DataAccess.Master.MetaData.Interface;
using KonaAI.Master.Repository.DataAccess.Master.UserMetaData.Interface;
using KonaAI.Master.Repository.DataAccess.Tenant.Client.Interface;
using KonaAI.Master.Repository.DataAccess.Tenant.ClientMetaData.Interface;
using KonaAI.Master.Repository.DataAccess.Tenant.ClientUserMetaData.Interface;

namespace KonaAI.Master.Test.Integration.Infrastructure;

/// <summary>
/// Test-specific UnitOfWork that uses TestDbContext instead of DefaultContext.
/// This avoids the database provider configuration issues in the wrapper approach.
/// </summary>
public class TestUnitOfWork : IUnitOfWork
{
    private readonly TestDbContext _context;

    public TestUnitOfWork(TestDbContext context, IClientRepository clients, IUserRepository users,
        ICountryRepository countries, ILogOnTypeRepository logOnTypes, IModuleSourceTypeRepository moduleSourceTypes, IModuleTypeRepository moduleTypes,
        INavigationRepository navigations, INavigationUserActionRepository navigationUserActions,
        IProjectStatusRepository projectStatuses, IRoleNavigationUserActionRepository roleNavigationUserActions,
        ISourceTypeRepository sourceTypes, IUserActionRepository userActions,
        IRoleTypeRepository roleTypes, IRenderTypeRepository renderTypes,
        IProjectAuditResponsibilityRepository projectAuditResponsibilities,
        IProjectDepartmentRepository projectDepartments, IProjectRiskAreaRepository projectRiskAreas,
        IProjectUnitRepository projectUnits, IClientProjectSourceModuleTypeRepository clientProjectSourceModuleTypes,
        IClientProjectRepository clientProjects, IClientUserRepository clientUsers, IClientProjectAuditResponsibilityRepository clientProjectAuditResponsibilities,
        IClientProjectCountryRepository clientProjectCountries, IClientProjectDepartmentRepository clientProjectDepartments,
        IClientProjectRiskAreaRepository clientProjectRiskAreas, IClientProjectUnitRepository clientProjectUnits,
        IQuestionBankRepository questionBanks, IClientQuestionBankRepository clientQuestionBanks,
        IClientQuestionnaireRepository clientQuestionnaires, IClientQuestionnaireAssociationRepository clientQuestionnaireAssociations,
        IClientQuestionnaireSectionRepository clientQuestionnaireSections, IClientLicenseRepository clientLicenses,
        IClientRoleTypeRepository clientRoleTypes, IUserAuditRepository userAudits,
        IProjectSchedulerRepository projectSchedulers, IClientProjectUserRepository clientProjectUser)
    {
        _context = context;
        Clients = clients;
        Users = users;
        Countries = countries;
        LogOnTypes = logOnTypes;
        ModuleSourceTypes = moduleSourceTypes;
        ModuleTypes = moduleTypes;
        Navigations = navigations;
        NavigationUserActions = navigationUserActions;
        ProjectStatuses = projectStatuses;
        RoleNavigationUserActions = roleNavigationUserActions;
        SourceTypes = sourceTypes;
        UserActions = userActions;
        RoleTypes = roleTypes;
        RenderTypes = renderTypes;
        ProjectAuditResponsibilities = projectAuditResponsibilities;
        ProjectDepartments = projectDepartments;
        ProjectRiskAreas = projectRiskAreas;
        ProjectUnits = projectUnits;
        ClientProjectSourceModuleTypes = clientProjectSourceModuleTypes;
        ClientProjects = clientProjects;
        ClientUsers = clientUsers;
        ClientProjectAuditResponsibilities = clientProjectAuditResponsibilities;
        ClientProjectCountries = clientProjectCountries;
        ClientProjectDepartments = clientProjectDepartments;
        ClientProjectRiskAreas = clientProjectRiskAreas;
        ClientProjectUnits = clientProjectUnits;
        QuestionBanks = questionBanks;
        ClientQuestionBanks = clientQuestionBanks;
        ClientQuestionnaires = clientQuestionnaires;
        ClientQuestionnaireAssociations = clientQuestionnaireAssociations;
        ClientQuestionnaireSections = clientQuestionnaireSections;
        ClientLicenses = clientLicenses;
        ClientRoleTypes = clientRoleTypes;
        UserAudits = userAudits;
        ProjectSchedulers = projectSchedulers;
        ClientProjectUser = clientProjectUser;
    }

    public IClientRepository Clients { get; set; }
    public IUserRepository Users { get; }
    public ICountryRepository Countries { get; }
    public ILogOnTypeRepository LogOnTypes { get; }
    public IModuleSourceTypeRepository ModuleSourceTypes { get; }
    public IModuleTypeRepository ModuleTypes { get; }
    public INavigationRepository Navigations { get; }
    public INavigationUserActionRepository NavigationUserActions { get; }
    public IProjectStatusRepository ProjectStatuses { get; }
    public IRoleNavigationUserActionRepository RoleNavigationUserActions { get; }
    public ISourceTypeRepository SourceTypes { get; }
    public IUserActionRepository UserActions { get; }
    public IRoleTypeRepository RoleTypes { get; }
    public IRenderTypeRepository RenderTypes { get; }
    public IProjectAuditResponsibilityRepository ProjectAuditResponsibilities { get; }
    public IProjectDepartmentRepository ProjectDepartments { get; }
    public IProjectRiskAreaRepository ProjectRiskAreas { get; }
    public IProjectUnitRepository ProjectUnits { get; }
    public IClientProjectSourceModuleTypeRepository ClientProjectSourceModuleTypes { get; }
    public IClientProjectRepository ClientProjects { get; }
    public IClientUserRepository ClientUsers { get; }
    public IClientProjectAuditResponsibilityRepository ClientProjectAuditResponsibilities { get; }
    public IClientProjectCountryRepository ClientProjectCountries { get; }
    public IClientProjectDepartmentRepository ClientProjectDepartments { get; }
    public IClientProjectRiskAreaRepository ClientProjectRiskAreas { get; }
    public IClientProjectUnitRepository ClientProjectUnits { get; }
    public IQuestionBankRepository QuestionBanks { get; }
    public IClientQuestionBankRepository ClientQuestionBanks { get; }
    public IClientQuestionnaireRepository ClientQuestionnaires { get; }
    public IClientQuestionnaireAssociationRepository ClientQuestionnaireAssociations { get; }
    public IClientQuestionnaireSectionRepository ClientQuestionnaireSections { get; }
    public IClientLicenseRepository ClientLicenses { get; }
    public IClientRoleTypeRepository ClientRoleTypes { get; }
    public IUserAuditRepository UserAudits { get; }
    public IProjectSchedulerRepository ProjectSchedulers { get; }
    public IClientProjectUserRepository ClientProjectUser { get; set; }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public int SaveChanges()
    {
        return _context.SaveChanges();
    }

    public async Task ExecuteAsync(Func<Task> action)
    {
        await action();
    }

    public void Dispose()
    {
        _context?.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        return _context?.DisposeAsync() ?? ValueTask.CompletedTask;
    }
}
