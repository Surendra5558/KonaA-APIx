using KonaAI.Master.API.Controllers.Tenant.Client;
using KonaAI.Master.Model.Authentication;
using KonaAI.Master.Model.Common;
using KonaAI.Master.Model.Master.App.ViewModel;
using KonaAI.Master.Model.Tenant.Client.ViewModel;
using KonaAI.Master.Repository.Domain.Tenant.Client;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using QuestionBankViewModel = KonaAI.Master.Model.Tenant.Client.ViewModel.QuestionBankViewModel;

namespace KonaAI.Master.API.Model;

/// <summary>
/// Class for OData EDM Model
/// </summary>
public class ODataEdmModelBuilder
{
    /// <summary>
    /// Gets the EDM Model
    /// </summary>
    /// <returns></returns>
    public static IEdmModel GetModels()
    {
        var builder = new ODataConventionModelBuilder();
        builder.EnableLowerCamelCase();

        // Define base types and keys once at the base level
        builder.EntitySet<BaseViewModel>("Base");
        builder.EntityType<BaseViewModel>().HasKey(x => x.RowId);

        builder.EntitySet<BaseAuditViewModel>("BaseAudit");
        builder.EntityType<BaseAuditViewModel>().DerivesFrom<BaseViewModel>();

        builder.EntitySet<TokenResponse>("Login").EntityType.HasKey(x => x.Name);
        builder.EntitySet<Master.Model.Master.App.ViewModel.QuestionBankViewModel>("QuestionBank");

        // Derived types inherit RowId from BaseViewModel; do not redefine keys here
        builder.EntitySet<ClientViewModel>("Client");
        builder.EntityType<ClientViewModel>().DerivesFrom<BaseAuditViewModel>();

        builder.EntitySet<ClientProjectViewModel>("ClientProject");
        builder.EntityType<ClientProjectViewModel>().DerivesFrom<BaseAuditViewModel>();

        builder.EntitySet<ClientUserViewModel>("ClientUser");
        builder.EntityType<ClientUserViewModel>().DerivesFrom<BaseAuditViewModel>();

        builder.EntitySet<ClientQuestionBankViewModel>("ClientQuestionBank");
        builder.EntitySet<ClientQuestionnaireAssociationViewModel>("ClientQuestionnaireAssociation");
        builder.EntitySet<ClientQuestionnaireViewModel>("ClientQuestionnaire");
        builder.EntityType<ClientQuestionBankViewModel>().DerivesFrom<BaseAuditViewModel>();
        // Register the main entity
        builder.EntitySet<ClientQuestionnaireSectionViewModel>("ClientQuestionnaireSection");
        builder.ComplexType<QuestionBankViewModel>();
        builder.EntityType<ClientQuestionnaireSectionViewModel>()
              .CollectionProperty(x => x.Questions);

        builder.ComplexType<QuestionBankViewModel>();

        builder.EntitySet<ClientLicenseViewModel>("ClientLicense");
        builder.EntityType<ClientLicenseViewModel>().DerivesFrom<BaseAuditViewModel>();

        //RoleType
        builder.EntitySet<MetaDataViewModel>("ClientRoleType");

        builder.EntitySet<MetaDataViewModel>("Menu");

        // Multiple entity sets can share the same entity type
        builder.EntitySet<MetaDataViewModel>("ProjectAuditResponsibility");
        builder.EntitySet<MetaDataViewModel>("Country");
        builder.EntitySet<MetaDataViewModel>("ProjectDepartment");
        builder.EntitySet<MetaDataViewModel>("ProjectRiskArea");
        builder.EntitySet<MetaDataViewModel>("ProjectUnit");
        builder.EntitySet<MetaDataViewModel>("ClientProjectAuditResponsibility");
        builder.EntitySet<MetaDataViewModel>("RenderType");
        builder.EntitySet<MetaDataViewModel>("ClientProjectCountry");
        builder.EntitySet<MetaDataViewModel>("ClientProjectDepartment");
        builder.EntitySet<MetaDataViewModel>("ClientProjectRiskArea");
        builder.EntitySet<MetaDataViewModel>("ClientProjectUnit");
        builder.EntitySet<MetaDataViewModel>("Module");
        builder.EntitySet<MetaDataViewModel>("AppNavigation");
        builder.EntitySet<MetaDataViewModel>("NavigationAction");
        builder.EntitySet<MetaDataViewModel>("RoleNavigationUserAction");
        builder.EntitySet<ClientQuestionnaireSectionViewModel>("ClientQuestionnaireSection");
        builder.EntityType<MetaDataViewModel>().DerivesFrom<BaseViewModel>();

        return builder.GetEdmModel();
    }
}