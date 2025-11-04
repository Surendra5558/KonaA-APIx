using System.Runtime.Serialization;

namespace KonaAI.Master.Repository.Common.Constants
{
    /// <summary>
    /// Defines the set of user actions that can be authorized against a given navigation/menu resource.
    /// These values are used in authorization requirements/policies and UI command enablement.
    /// </summary>
    public enum UserActionMenu
    {
        /// <summary>Create or add a new resource.</summary>
        [EnumMember(Value = "Add")] Add,

        /// <summary>View or read an existing resource.</summary>
        [EnumMember(Value = "View")] View,

        /// <summary>Modify or update an existing resource.</summary>
        [EnumMember(Value = "Edit")] Edit,

        /// <summary>Remove or delete an existing resource.</summary>
        [EnumMember(Value = "Delete")] Delete
    }

    /// <summary>
    /// Extension helpers for mapping <see cref="UserActionMenu"/> values to and from stable GUIDs.
    /// The GUIDs are canonical identifiers used by persistence, seed data, and authorization checks.
    /// </summary>
    public static class UserActionMenuExtensions
    {
        /// <summary>
        /// Canonical mapping between persistent GUID identifiers and <see cref="UserActionMenu"/> values.
        /// Do not change GUIDs of existing entries as they are referenced by seeded data and policies.
        /// </summary>
        private static readonly Dictionary<Guid, UserActionMenu> MenusByGuid = new()
        {
            { new Guid("71677AC1-F65E-4A3A-B5BA-30C670ADAF72"), UserActionMenu.Add },
            { new Guid("E2C69446-BCE9-4649-9883-B7CF5DC49ED4"), UserActionMenu.View },
            { new Guid("3EEF0B38-5E82-48AE-8D5F-DB30512FA788"), UserActionMenu.Edit },
            { new Guid("6A567A5C-F8E3-4C30-B2D9-F3BDD59478E3"), UserActionMenu.Delete }
        };

        /// <summary>
        /// Static initializer to register mappings with the shared enum-guid mapper.
        /// </summary>
        static UserActionMenuExtensions()
        {
            EnumGuidMapper<UserActionMenu>.Initialize(MenusByGuid);
        }

        /// <summary>
        /// Gets the <see cref="UserActionMenu"/> value associated with the provided GUID.
        /// </summary>
        /// <param name="guid">The canonical GUID identifier.</param>
        /// <returns>The mapped <see cref="UserActionMenu"/> value.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the GUID is not mapped.</exception>
        public static UserActionMenu GetMenuByGuid(Guid guid) =>
            EnumGuidMapper<UserActionMenu>.GetEnumByGuid(guid);

        /// <summary>
        /// Attempts to resolve the <see cref="UserActionMenu"/> value associated with the provided GUID.
        /// </summary>
        /// <param name="guid">The canonical GUID identifier.</param>
        /// <param name="menu">When this method returns, contains the resolved value if found; otherwise the default.</param>
        /// <returns><c>true</c> if a mapping exists; otherwise, <c>false</c>.</returns>
        public static bool TryGetMenuByGuid(Guid guid, out UserActionMenu menu) =>
            EnumGuidMapper<UserActionMenu>.TryGetEnumByGuid(guid, out menu);

        /// <summary>
        /// Gets the canonical GUID associated with the specified <see cref="UserActionMenu"/> value.
        /// </summary>
        /// <param name="menu">The action enum value.</param>
        /// <returns>The mapped GUID.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the enum value is not mapped.</exception>
        public static Guid GetGuid(this UserActionMenu menu) =>
            EnumGuidMapper<UserActionMenu>.GetGuid(menu);

        /// <summary>
        /// Attempts to get the canonical GUID associated with the specified <see cref="UserActionMenu"/> value.
        /// </summary>
        /// <param name="menu">The action enum value.</param>
        /// <param name="guid">When this method returns, contains the mapped GUID if found; otherwise, <see cref="Guid.Empty"/>.</param>
        /// <returns><c>true</c> if a mapping exists; otherwise, <c>false</c>.</returns>
        public static bool TryGetGuid(this UserActionMenu menu, out Guid guid) =>
            EnumGuidMapper<UserActionMenu>.TryGetGuid(menu, out guid);
    }
}