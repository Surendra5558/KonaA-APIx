namespace KonaAI.Master.Repository.Common.Model;

/// <summary>
/// License generation result containing encrypted license and optional private key.
/// For license-only approach, EncryptedPrivateKey can be null.
/// </summary>
public class LicenseResult
{
    /// <summary>
    /// Encrypted license string.
    /// </summary>
    public string EncryptedLicense { get; set; } = null!;

    /// <summary>
    /// Encrypted private key string (optional for license-only approach).
    /// </summary>
    public string? EncryptedPrivateKey { get; set; }
}

