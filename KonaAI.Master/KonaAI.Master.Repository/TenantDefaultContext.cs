using KonaAI.Master.Repository.Domain.Tenant.Client;
using KonaAI.Master.Repository.Domain.Tenant.ClientMetaData;
using KonaAI.Master.Repository.Domain.Tenant.ClientUserMetaData;
using Microsoft.EntityFrameworkCore;

namespace KonaAI.Master.Repository;

/// <summary>
/// Partial EF Core context segment that exposes DbSet properties for client/tenant entities.
/// </summary>
public partial class DefaultContext
{
    #region Client

    /// <summary>
    /// Gets or sets the set of client project entities.
    /// </summary>
    /// <remarks>Represents the EF Core set for <see cref="ClientProject"/>.</remarks>
    public virtual DbSet<ClientProject> ClientProjects { get; set; }

    /// <summary>
    /// Gets or sets the set of mappings between client projects and module types.
    /// </summary>
    /// <remarks>Represents the EF Core set for ClientProjectModuleSourceType.</remarks>
    public virtual DbSet<ClientProjectModuleSourceType> ClientProjectModuleSourceTypes { get; set; }

    /// <summary>
    /// Gets or sets the set of client-to-user association entities.
    /// </summary>
    public virtual DbSet<ClientUser> ClientUsers { get; set; }

    /// <summary>
    /// Entity set for <see cref="ClientLicense"/> entities .
    /// </summary>
    public virtual DbSet<ClientLicense> ClientLicenses { get; set; }

    /// <summary>
    /// Gets or sets the client questionnaire.
    /// </summary>
    public DbSet<ClientQuestionnaire> ClientQuestionnaire { get; set; }

    /// <summary>
    /// Gets or sets the client question bank.
    /// </summary>
    public DbSet<ClientQuestionBank> ClientQuestionBank { get; set; }

    /// <summary>
    /// Gets or sets the client questionnaire association.
    /// </summary>
    public DbSet<ClientQuestionnaireAssociation> ClientQuestionnaireAssociation { get; set; }

    /// <summary>
    /// Gets or sets the client questionnaire section.
    /// </summary>
    public DbSet<ClientQuestionnaireSection> ClientQuestionnaireSection { get; set; }

    /// <summary>
    /// Gets or sets the set of client project user association entities.
    /// </summary>
    public DbSet<ClientProjectUser> ClientProjectUsers { get; set; }
    #endregion Client

    #region ClientMetaData

    public virtual DbSet<ClientRoleType> ClientRoleTypes { get; set; }

    #endregion ClientMetaData

    #region ClientUserMetaData

    /// <summary>
    /// Gets or sets the set of project audit responsibility metadata.
    /// </summary>
    public virtual DbSet<ClientProjectAuditResponsibility> ClientProjectAuditResponsibilities { get; set; }

    /// <summary>
    /// Gets or sets the set of client-specific project country metadata.
    /// </summary>
    public virtual DbSet<ClientProjectCountry> ClientProjectCountries { get; set; }

    /// <summary>
    /// Gets or sets the set of business department metadata for client projects.
    /// </summary>
    public virtual DbSet<ClientProjectDepartment> ClientProjectDepartments { get; set; }

    /// <summary>
    /// Gets or sets the set of project risk area metadata.
    /// </summary>
    public virtual DbSet<ClientProjectRiskArea> ClientProjectRiskAreas { get; set; }

    /// <summary>
    /// Gets or sets the set of business unit metadata for client projects.
    /// </summary>
    public virtual DbSet<ClientProjectUnit> ClientProjectUnits { get; set; }

    #endregion ClientUserMetaData
}