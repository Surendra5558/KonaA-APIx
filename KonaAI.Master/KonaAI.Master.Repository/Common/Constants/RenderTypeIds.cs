using System.Runtime.Serialization;

namespace KonaAI.Master.Repository.Common.Constants;

/// <summary>
/// Enumerates the available render types for form fields.
/// Used as lookup identifiers instead of magic numbers.
/// </summary>
public enum RenderTypeIds : long
{
    /// <summary>Multi-line text input area.</summary>
    [EnumMember(Value = "TextArea")]
    TextArea = 1,

    /// <summary>Single selection radio button input.</summary>
    [EnumMember(Value = "RadioButton")]
    RadioButton = 2,

    /// <summary>Multi-select checkbox input.</summary>
    [EnumMember(Value = "CheckBox")]
    CheckBox = 3,

    /// <summary>Clickable hyperlink field.</summary>
    [EnumMember(Value = "HyperLink")]
    HyperLink = 4,

    /// <summary>Tabular data display element.</summary>
    [EnumMember(Value = "Table")]
    Table = 5,

    /// <summary>Static label or display text.</summary>
    [EnumMember(Value = "Label")]
    Label = 6
}


/// <summary>
/// Provides helper extensions for mapping <see cref="RenderTypeIds"/> values
/// to and from their canonical GUID identifiers.
/// These mappings ensure stable references in persisted configurations or metadata.
/// </summary>
public static class RenderTypeExtensions
{
    /// <summary>
    /// Canonical mapping between persistent GUID identifiers and <see cref="RenderTypeIds"/> values.
    /// Do not modify existing GUIDs, as they are referenced by persisted data and UI configuration.
    /// </summary>
    private static readonly Dictionary<Guid, RenderTypeIds> RenderTypesByGuid = new()
    {
        { new Guid("1F3A2A01-6B7C-4C9B-905B-72D4D4D3B501"), RenderTypeIds.TextArea },
        { new Guid("A18F2A13-8355-4A4C-B930-5A9E3D12D1D7"), RenderTypeIds.RadioButton },
        { new Guid("78E3C4B0-03CC-4B16-B4C0-7D391F9A4A6E"), RenderTypeIds.CheckBox },
        { new Guid("6BE72A4F-8C2C-41C4-826F-CE473A419D32"), RenderTypeIds.HyperLink },
        { new Guid("5E874B8B-2B87-4A77-87C3-672A7E4A94B4"), RenderTypeIds.Table },
        { new Guid("C4B718E3-6E2F-49A3-8F54-53C83B7A91E0"), RenderTypeIds.Label }
    };

    static RenderTypeExtensions()
    {
        EnumGuidMapper<RenderTypeIds>.Initialize(RenderTypesByGuid);
    }

    /// <summary>
    /// Gets the <see cref="RenderTypeIds"/> value associated with the provided GUID.
    /// </summary>
    /// <param name="guid">The canonical GUID identifier.</param>
    /// <returns>The mapped <see cref="RenderTypeIds"/> value.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the GUID is not mapped.</exception>
    public static RenderTypeIds GetRenderTypeByGuid(Guid guid) =>
        EnumGuidMapper<RenderTypeIds>.GetEnumByGuid(guid);

    /// <summary>
    /// Attempts to resolve the <see cref="RenderTypeIds"/> value associated with the provided GUID.
    /// </summary>
    /// <param name="guid">The canonical GUID identifier.</param>
    /// <param name="type">When this method returns, contains the resolved <see cref="RenderTypeIds"/> if found; otherwise the default value.</param>
    /// <returns><c>true</c> if a mapping exists; otherwise, <c>false</c>.</returns>
    public static bool TryGetRenderTypeByGuid(Guid guid, out RenderTypeIds type) =>
        EnumGuidMapper<RenderTypeIds>.TryGetEnumByGuid(guid, out type);

    /// <summary>
    /// Gets the canonical GUID associated with the specified <see cref="RenderTypeIds"/> value.
    /// </summary>
    /// <param name="type">The render type enum value.</param>
    /// <returns>The mapped GUID.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the type is not mapped.</exception>
    public static Guid GetGuid(this RenderTypeIds type) =>
        EnumGuidMapper<RenderTypeIds>.GetGuid(type);

    /// <summary>
    /// Attempts to get the canonical GUID associated with the specified <see cref="RenderTypeIds"/> value.
    /// </summary>
    /// <param name="type">The render type enum value.</param>
    /// <param name="guid">When this method returns, contains the mapped GUID if found; otherwise, <see cref="Guid.Empty"/>.</param>
    /// <returns><c>true</c> if a mapping exists; otherwise, <c>false</c>.</returns>
    public static bool TryGetGuid(this RenderTypeIds type, out Guid guid) =>
        EnumGuidMapper<RenderTypeIds>.TryGetGuid(type, out guid);
}
