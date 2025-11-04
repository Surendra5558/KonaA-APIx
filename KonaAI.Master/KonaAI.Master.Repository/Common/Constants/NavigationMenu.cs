using System.Runtime.Serialization;

namespace KonaAI.Master.Repository.Common.Constants;

/// <summary>
/// Defines the canonical set of application navigation entries.
/// These values are used across authorization (policies/requirements), UI routing,
/// and database seed data. Each member has a fixed string representation via <see cref="EnumMemberAttribute"/>.
/// </summary>
public enum NavigationMenu
{
    /// <summary>Organization menu - contains the user navigation actions according to the user roles.</summary>
    [EnumMember(Value = "Oraganization")] Organization,

    /// <summary>Project menu - views project list and its details.</summary>
    [EnumMember(Value = "Project")] Project,

    /// <summary>All Clients - shows the client details.</summary>
    [EnumMember(Value = "All Clients")] AllClients,

    /// <summary>Summary - shows the client summary and its details.</summary>
    [EnumMember(Value = "Summary")] Summary,

    /// <summary>Members - manage members on the platform, assign roles, and control access seamlessly.</summary>
    [EnumMember(Value = "Members")] Members,

    /// <summary>Roles - create new roles and manage permissions to control access across the platform.</summary>
    [EnumMember(Value = "Roles")] Roles,

    /// <summary>Connectors - manage and monitor your data connections across systems to enable seamless data flow.</summary>
    [EnumMember(Value = "Connectors")] Connectors,

    /// <summary>Configurations - manage modules, risk areas, and source system mappings.</summary>
    [EnumMember(Value = "Configurations")] Configurations,

    /// <summary>Connections - manage and monitor your data connections across systems to enable seamless data flow.</summary>
    [EnumMember(Value = "Connections")] Connections,

    /// <summary>Documents - upload and manage all key project documents in one place.</summary>
    [EnumMember(Value = "Documents")] Documents,

    /// <summary>Questionnaire Builder - add and customise questionnaires for investigations and assessments.</summary>
    [EnumMember(Value = "Questionnaire Builder")] QuestionnaireBuilder,

    /// <summary>Insights Template - choose and customize insight templates for your analysis.</summary>
    [EnumMember(Value = "Insights Template")] InsightsTemplate,

    /// <summary>Users - manage members on the platform, assign roles, and control access seamlessly.</summary>
    [EnumMember(Value = "Users")] Users,

    /// <summary>License - views the license of the client.</summary>
    [EnumMember(Value = "License")] License,

    /// <summary>All Projects - manage and track all the projects created across organisation seamlessly.</summary>
    [EnumMember(Value = "All Projects")] AllProjects,

    /// <summary>Project Summary.</summary>
    [EnumMember(Value = "Summary")] ProjectSummary,

    /// <summary>Workflow Set Up - set criteria and adjust parameters to configure your test for accurate results.</summary>
    [EnumMember(Value = "Workflow Set Up")] WorkflowSetUp,

    /// <summary>Insights - view test results, anomalies, and analyse data trends for deeper insights.</summary>
    [EnumMember(Value = "Insights")] Insights,

    /// <summary>Alerts Dashboard - manage and track all the projects created across organisation seamlessly.</summary>
    [EnumMember(Value = "Alerts Dashboard")] AlertsDashboard,

    /// <summary>Alerts - track and manage alerts triggered by failed tests, anomalies, and policy violations.</summary>
    [EnumMember(Value = "Alerts")] Alerts,

    /// <summary>Project Users - manage members on the platform, assign roles, and control access seamlessly.</summary>
    [EnumMember(Value = "Users")] ProjectUsers,

    /// <summary>Project Roles - create new roles and manage permissions to control access across the platform.</summary>
    [EnumMember(Value = "Roles")] ProjectRoles,

    /// <summary>Visualisations - view test results, anomalies, and analyse data trends for deeper insights.</summary>
    [EnumMember(Value = "Visualisations")] Visualisations,

    /// <summary>Entity View - shows entity profiles.</summary>
    [EnumMember(Value = "Entity View")] EntityView,

    /// <summary>Transaction View - shows transactions and invoice details.</summary>
    [EnumMember(Value = "Transaction View")] TransactionView,

    /// <summary>Scenario Manager - views scenario information.</summary>
    [EnumMember(Value = "Scenario Manager")] ScenarioManager,

    /// <summary>Similar Transaction - access interactive charts and visual analytics to explore project, client, and transaction data.</summary>
    [EnumMember(Value = "Similar Transaction")] SimilarTransaction,

    /// <summary>Project Dashboard - views dashboard for project audits.</summary>
    [EnumMember(Value = "Project Dashboard")] ProjectDashboard
}

/// <summary>
/// Extension helpers for mapping <see cref="NavigationMenu"/> values to and from stable GUIDs.
/// The GUIDs are canonical identifiers used in persistence/seed data and authorization checks.
/// </summary>
public static class NavigationMenuExtensions
{
    /// <summary>
    /// Canonical mapping between persistent GUID identifiers and <see cref="NavigationMenu"/> values.
    /// Do not modify GUIDs for existing entries, as they are used in persisted data and policies.
    /// </summary>
    private static readonly Dictionary<Guid, NavigationMenu> MenusByGuid = new()
    {
        { new Guid("2B7D80F8-9163-4CF6-B03B-9132C49E1B34"), NavigationMenu.Organization },
        { new Guid("B8E3D6CB-B6F0-4023-8E7B-4BCEA6894C03"), NavigationMenu.Project },
        { new Guid("7FFAA26E-0E7E-41F4-8057-2CF4D1951FED"), NavigationMenu.AllClients },
        { new Guid("9EC0D091-74F6-44FA-AAAE-27D303A71CF6"), NavigationMenu.Summary },
        { new Guid("9EC25F34-B66C-4E86-A340-014F6F511990"), NavigationMenu.Members },
        { new Guid("BDCF0A83-8C85-4FD0-A21F-15BCC28DE782"), NavigationMenu.Roles },
        { new Guid("1D1C8349-B551-4A34-8468-E80015425D53"), NavigationMenu.Connectors },
        { new Guid("D17E90F1-D83D-479E-9A98-A0DA491924BA"), NavigationMenu.Configurations },
        { new Guid("BC22B3F0-C8FA-4917-A743-9CA61A32410C"), NavigationMenu.Connections },
        { new Guid("CF8324F3-D559-470F-828A-EC391B593BCF"), NavigationMenu.Documents },
        { new Guid("E7E8B5F4-B732-415C-A8FF-CDC116143B96"), NavigationMenu.QuestionnaireBuilder },
        { new Guid("E8DCF843-425F-4959-9EA6-1DBF63AF5731"), NavigationMenu.InsightsTemplate },
        { new Guid("97C2BFE9-0C66-48C8-9591-1769DCC7A068"), NavigationMenu.Users },
        { new Guid("FE0B6D72-D3D7-4A46-B8E9-F3B187463CF4"), NavigationMenu.License },
        { new Guid("1C4F654B-E3E7-43DB-A0B0-7D6731B933BD"), NavigationMenu.AllProjects },
        { new Guid("AFF447AB-3180-4850-8675-4498A2B8B1E2"), NavigationMenu.ProjectSummary },
        { new Guid("5D534CA5-E26F-45D0-A8B9-1CFDA53ABCC0"), NavigationMenu.WorkflowSetUp },
        { new Guid("6BDDB546-BFCB-4217-9F5C-BC535EF233DD"), NavigationMenu.Insights },
        { new Guid("0D2E6D9B-CA51-410C-9775-EA3583872984"), NavigationMenu.AlertsDashboard },
        { new Guid("A5147898-183E-4EC7-A0A1-D97232EFEE73"), NavigationMenu.Alerts },
        { new Guid("0DBC12A1-7FBF-442C-8583-9F491C8B802E"), NavigationMenu.ProjectUsers },
        { new Guid("9A022CD0-82BD-4855-B25A-E72523D53D3F"), NavigationMenu.ProjectRoles },
        { new Guid("11AD33E4-F72D-4A1D-A468-B252D6864498"), NavigationMenu.ProjectDashboard },
        { new Guid("A814B4CE-BF05-4B13-A728-90AB9A4A0926"), NavigationMenu.Visualisations },
        { new Guid("5B30432E-3307-46E9-AFDE-0E16872624BF"), NavigationMenu.EntityView },
        { new Guid("7487E683-7781-456E-92D0-5E6D7B38F57D"), NavigationMenu.TransactionView },
        { new Guid("32873795-27F4-4FE3-969E-C852BA7D19F4"), NavigationMenu.ScenarioManager },
        { new Guid("5CDE8207-3591-4311-A131-642801FF138E"), NavigationMenu.SimilarTransaction }
    };

    static NavigationMenuExtensions()
    {
        EnumGuidMapper<NavigationMenu>.Initialize(MenusByGuid);
    }

    /// <summary>
    /// Gets the <see cref="NavigationMenu"/> value associated with the provided GUID.
    /// </summary>
    /// <param name="guid">The canonical GUID identifier.</param>
    /// <returns>The mapped <see cref="NavigationMenu"/> value.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the GUID is not mapped.</exception>
    public static NavigationMenu GetMenuByGuid(Guid guid) =>
        EnumGuidMapper<NavigationMenu>.GetEnumByGuid(guid);

    /// <summary>
    /// Attempts to resolve the <see cref="NavigationMenu"/> value associated with the provided GUID.
    /// </summary>
    /// <param name="guid">The canonical GUID identifier.</param>
    /// <param name="menu">When this method returns, contains the resolved <see cref="NavigationMenu"/> if found; otherwise the default value.</param>
    /// <returns><c>true</c> if a mapping exists; otherwise, <c>false</c>.</returns>
    public static bool TryGetMenuByGuid(Guid guid, out NavigationMenu menu) =>
        EnumGuidMapper<NavigationMenu>.TryGetEnumByGuid(guid, out menu);

    /// <summary>
    /// Gets the canonical GUID associated with the specified <see cref="NavigationMenu"/> value.
    /// </summary>
    /// <param name="menu">The menu enum value.</param>
    /// <returns>The mapped GUID.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the menu value is not mapped.</exception>
    public static Guid GetGuid(this NavigationMenu menu) =>
        EnumGuidMapper<NavigationMenu>.GetGuid(menu);

    /// <summary>
    /// Attempts to get the canonical GUID associated with the specified <see cref="NavigationMenu"/> value.
    /// </summary>
    /// <param name="menu">The menu enum value.</param>
    /// <param name="guid">When this method returns, contains the mapped GUID if found; otherwise, <see cref="Guid.Empty"/>.</param>
    /// <returns><c>true</c> if a mapping exists; otherwise, <c>false</c>.</returns>
    public static bool TryGetGuid(this NavigationMenu menu, out Guid guid) =>
        EnumGuidMapper<NavigationMenu>.TryGetGuid(menu, out guid);
}