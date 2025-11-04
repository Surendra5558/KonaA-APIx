using KonaAI.Master.Repository.Common.Model;

namespace KonaAI.Master.Repository.Common.Interface;

public interface ILicenseService
{
    /// <summary>
    /// Encrypts a license JSON payload for a specific clientId.
    /// </summary>
    /// <param name="jsonPayload">The JSON license payload to encrypt.</param>
    /// <param name="strForIV">The initialization vector string for encryption.</param>
    /// <returns>Encrypted license result (license + encrypted key).</returns>
    LicenseResult EncryptLicense(string jsonPayload, string strForIV);

    /// <summary>
    /// Decrypts an encrypted license for a specific clientId.
    /// </summary>
    /// <param name="license">The encrypted license result.</param>
    /// <param name="stringForIV">The client identifier (GUID).</param>
    /// <returns>The decrypted license JSON string.</returns>
    string DecryptLicense(LicenseResult license, string stringForIV);
}