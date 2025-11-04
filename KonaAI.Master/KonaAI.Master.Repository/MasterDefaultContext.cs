using KonaAI.Master.Repository.Domain.Master.App;
using KonaAI.Master.Repository.Domain.Master.MetaData;
using KonaAI.Master.Repository.Domain.Master.UserMetaData;
using Microsoft.EntityFrameworkCore;

namespace KonaAI.Master.Repository;

/// <summary>
/// Partial EF Core context segment exposing entity sets for application,
/// shared metadata, and user metadata domains.
/// </summary>
public partial class DefaultContext
{
    #region Application

    /// <summary>
    /// Entity set for <see cref="Client"/> records.
    /// </summary>
    public virtual DbSet<Client> Clients { get; set; }

    /// <summary>
    /// Gets or sets the question bank.
    /// </summary>
    public DbSet<QuestionBank> QuestionBank { get; set; }

    /// <summary>
    /// Entity set for <see cref="User"/> records.
    /// </summary>
    public virtual DbSet<User> Users { get; set; }

    /// <summary>
    /// Entity set for <see cref="UserAudit"/> snapshots.
    /// </summary>
    public virtual DbSet<UserAudit> UserAudits { get; set; }

    /// <summary>
    /// Entity set for <see cref="QuestionnaireAssociation"/> snapshots.
    /// </summary>
    public virtual DbSet<QuestionnaireAssociation> QuestionnaireAssociation { get; set; }

    #endregion Application

    #region Metadata

    /// <summary>
    /// Entity set for <see cref="Country"/> metadata.
    /// </summary>
    public virtual DbSet<Country> Countries { get; set; }

    /// <summary>
    /// Entity set for <see cref="LogOnType"/> (SSO / authentication types).
    /// </summary>
    public virtual DbSet<LogOnType> LogOnTypes { get; set; }

    /// <summary>
    /// Entity set for <see cref="ModuleSourceType"/> associations.
    /// </summary>
    public virtual DbSet<ModuleSourceType> ModuleSourceTypes { get; set; }

    /// <summary>
    /// Entity set for <see cref="ModuleType"/> metadata.
    /// </summary>
    public virtual DbSet<ModuleType> ModuleTypes { get; set; }

    /// <summary>
    /// Entity set for <see cref="Navigation"/> menu definitions.
    /// </summary>
    public virtual DbSet<Navigation> Navigations { get; set; }

    /// <summary>
    /// Entity set for <see cref="NavigationUserAction"/> mappings.
    /// </summary>
    public virtual DbSet<NavigationUserAction> NavigationUserActions { get; set; }

    /// <summary>
    /// Entity set for <see cref="ProjectStatus"/> metadata.
    /// </summary>
    public virtual DbSet<ProjectStatus> ProjectStatuses { get; set; }

    /// <summary>
    /// Entity set for <see cref="RoleNavigationUserAction"/> permission links.
    /// </summary>
    public virtual DbSet<RoleNavigationUserAction> RoleNavigationUserActions { get; set; }

    /// <summary>
    /// Gets or sets the type of the render.
    /// </summary>
    public DbSet<RenderType> RenderType { get; set; }

    /// <summary>
    /// Entity set for <see cref="RoleType"/> metadata.
    /// </summary>
    public virtual DbSet<RoleType> RoleTypes { get; set; }

    /// <summary>
    /// Entity set for <see cref="SourceType"/> metadata.
    /// </summary>
    public virtual DbSet<SourceType> SourceTypes { get; set; }

    /// <summary>
    /// Entity set for <see cref="UserAction"/> metadata (permission verbs).
    /// </summary>
    public virtual DbSet<UserAction> UserActions { get; set; }

    #endregion Metadata

    #region User Metadata

    /// <summary>
    /// Entity set for <see cref="ProjectAuditResponsibility"/> metadata.
    /// </summary>
    public virtual DbSet<ProjectAuditResponsibility> ProjectAuditResponsibilities { get; set; }

    /// <summary>
    /// Entity set for <see cref="ProjectDepartment"/> metadata.
    /// </summary>
    public virtual DbSet<ProjectDepartment> ProjectDepartments { get; set; }

    /// <summary>
    /// Entity set for <see cref="ProjectRiskArea"/> metadata.
    /// </summary>
    public virtual DbSet<ProjectRiskArea> ProjectRiskAreas { get; set; }

    /// <summary>
    /// Entity set for <see cref="ProjectUnit"/> metadata.
    /// </summary>
    public virtual DbSet<ProjectUnit> ProjectUnits { get; set; }

    #endregion User Metadata
}