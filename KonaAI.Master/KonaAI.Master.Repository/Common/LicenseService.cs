using KonaAI.Master.Repository.Common.Interface;
using KonaAI.Master.Repository.Common.Model;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace KonaAI.Master.Repository.Common;

/// <summary>
/// License service for creating and decrypting licenses using AES-GCM encryption.
/// Provides secure license encryption and decryption with deterministic key derivation.
/// </summary>
/// <param name="logger">The logger instance for recording encryption/decryption operations and errors.</param>
public class LicenseService(ILogger<LicenseService> logger) : ILicenseService
{
    private readonly ILogger<LicenseService> _logger = logger;
    /// <summary>
    /// Derives a 32-byte AES-256 key from the provided string using SHA-256 hashing.
    /// This method provides deterministic key derivation for consistent encryption/decryption.
    /// </summary>
    /// <param name="strForKey">The input string to derive the key from (typically client ID or similar identifier).</param>
    /// <returns>A 32-byte AES-256 key derived from the input string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when strForKey is null or empty.</exception>
    private static byte[] DeriveKeyFromStr(string strForKey)
    {
        if (string.IsNullOrWhiteSpace(strForKey))
            throw new ArgumentNullException(nameof(strForKey), "Input string cannot be null or empty for key derivation");

        try
        {
            // Use SHA-256 to create a deterministic 32-byte key from the input string
            // This ensures the same input always produces the same key
            using var sha = SHA256.Create();
            return sha.ComputeHash(Encoding.UTF8.GetBytes(strForKey));
        }
        catch (Exception ex)
        {
            // Re-throw with context for better error handling
            throw new CryptographicException("Failed to derive key from input string", ex);
        }
    }

    /// <summary>
    /// Derives a 12-byte initialization vector (IV) deterministically from the provided string using SHA-256.
    /// This method ensures consistent IV generation for AES-GCM encryption/decryption.
    /// </summary>
    /// <param name="strForIV">The input string to derive the IV from (typically client ID or similar identifier).</param>
    /// <returns>A 12-byte IV derived from the input string using SHA-256.</returns>
    /// <exception cref="ArgumentNullException">Thrown when strForIV is null or empty.</exception>
    private static byte[] DeriveIv(string strForIV)
    {
        if (string.IsNullOrWhiteSpace(strForIV))
            throw new ArgumentNullException(nameof(strForIV), "Input string cannot be null or empty for IV derivation");

        try
        {
            // Use SHA-256 to create a deterministic IV from the input string
            // Prefix with "IV-" to differentiate from key derivation
            using var sha = SHA256.Create();
            var hash = sha.ComputeHash(Encoding.UTF8.GetBytes("IV-" + strForIV));

            // Extract first 12 bytes for AES-GCM IV (GCM requires 12-byte IV)
            var iv = new byte[12];
            Array.Copy(hash, iv, 12);
            return iv;
        }
        catch (Exception ex)
        {
            // Re-throw with context for better error handling
            throw new CryptographicException("Failed to derive IV from input string", ex);
        }
    }

    /// <summary>
    /// Generates a cryptographically secure random AES-256 key.
    /// This method creates a 32-byte (256-bit) key suitable for AES-GCM encryption.
    /// </summary>
    /// <returns>A 32-byte cryptographically secure random AES-256 key.</returns>
    /// <exception cref="CryptographicException">Thrown when random number generation fails.</exception>
    private static byte[] GenerateAesKey()
    {
        try
        {
            // Create a 32-byte (256-bit) key for AES-256 encryption
            var key = new byte[32];

            // Use cryptographically secure random number generator
            // This ensures the key is truly random and secure
            RandomNumberGenerator.Fill(key);
            return key;
        }
        catch (Exception ex)
        {
            // Re-throw with context for better error handling
            throw new CryptographicException("Failed to generate secure random AES key", ex);
        }
    }

    /// <summary>
    /// Creates an encrypted license using AES-GCM encryption with a two-layer encryption approach.
    /// First encrypts the license payload with a random key, then encrypts that key with a client-derived key.
    /// </summary>
    /// <param name="jsonPayload">The JSON license payload to encrypt.</param>
    /// <param name="strForIV">The initialization vector string for encryption (typically client ID).</param>
    /// <returns>Encrypted license result containing both encrypted license and encrypted private key.</returns>
    /// <exception cref="ArgumentNullException">Thrown when jsonPayload or strForIV is null or empty.</exception>
    /// <exception cref="CryptographicException">Thrown when encryption operations fail.</exception>
    public LicenseResult EncryptLicense(string jsonPayload, string strForIV)
    {
        const string methodName = nameof(EncryptLicense);
        var startTime = DateTime.UtcNow;

        try
        {
            _logger.LogInformation("{MethodName} - Starting license encryption for client: {ClientId}", methodName, strForIV);

            // Validate input parameters
            if (string.IsNullOrWhiteSpace(jsonPayload))
                throw new ArgumentNullException(nameof(jsonPayload), "JSON payload cannot be null or empty");

            if (string.IsNullOrWhiteSpace(strForIV))
                throw new ArgumentNullException(nameof(strForIV), "IV string cannot be null or empty");

            // 1. Generate random AES license key for the license payload
            _logger.LogDebug("{MethodName} - Generating random AES key for license encryption", methodName);
            var licenseKey = GenerateAesKey();

            // 2. Encrypt license JSON with AES-GCM using licenseKey + IV(clientId)
            _logger.LogDebug("{MethodName} - Encrypting license payload with AES-GCM", methodName);
            var iv = DeriveIv(strForIV);
            byte[] plaintext = Encoding.UTF8.GetBytes(jsonPayload);
            byte[] ciphertext = new byte[plaintext.Length];
            byte[] tag = new byte[16]; // 16-byte authentication tag for GCM

            using (var aesGcm = new AesGcm(new ReadOnlySpan<byte>(licenseKey), tagSizeInBytes: 16))
            {
                aesGcm.Encrypt(iv, plaintext, ciphertext, tag);
            }

            // Combine ciphertext and authentication tag
            var outBuf = new byte[ciphertext.Length + tag.Length];
            Buffer.BlockCopy(ciphertext, 0, outBuf, 0, ciphertext.Length);
            Buffer.BlockCopy(tag, 0, outBuf, ciphertext.Length, tag.Length);

            // 3. Encrypt the license key using clientId-derived key (key wrapping)
            _logger.LogDebug("{MethodName} - Encrypting license key with client-derived key", methodName);
            var clientKey = DeriveKeyFromStr(strForIV);
            byte[] encryptedLicenseKey;
            using (var aes = Aes.Create())
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Key = clientKey;
                aes.IV = new byte[16]; // Fixed IV for key wrapping (deterministic)
                using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                encryptedLicenseKey = encryptor.TransformFinalBlock(licenseKey, 0, licenseKey.Length);
            }

            var result = new LicenseResult
            {
                EncryptedLicense = Convert.ToBase64String(outBuf),
                EncryptedPrivateKey = Convert.ToBase64String(encryptedLicenseKey)
            };

            var executionTime = DateTime.UtcNow - startTime;
            _logger.LogInformation("{MethodName} - License encryption completed successfully for client: {ClientId} in {Duration}ms",
                methodName, strForIV, executionTime.TotalMilliseconds);

            return result;
        }
        catch (CryptographicException ex)
        {
            var executionTime = DateTime.UtcNow - startTime;
            _logger.LogError(ex, "{MethodName} - Cryptographic error during license encryption for client: {ClientId} after {Duration}ms",
                methodName, strForIV, executionTime.TotalMilliseconds);
            throw;
        }
        catch (Exception ex)
        {
            var executionTime = DateTime.UtcNow - startTime;
            _logger.LogError(ex, "{MethodName} - Unexpected error during license encryption for client: {ClientId} after {Duration}ms: {ErrorMessage}",
                methodName, strForIV, executionTime.TotalMilliseconds, ex.Message);
            throw;
        }
        finally
        {
            _logger.LogDebug("{MethodName} - License encryption operation completed", methodName);
        }
    }

    /// <summary>
    /// Decrypts a license using the two-layer decryption approach.
    /// First decrypts the license key using client-derived key, then decrypts the license payload.
    /// </summary>
    /// <param name="license">The encrypted license result containing encrypted license and private key.</param>
    /// <param name="strForIV">The initialization vector string for decryption (typically client ID).</param>
    /// <returns>The decrypted JSON license payload as a string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when license or strForIV is null or empty.</exception>
    /// <exception cref="CryptographicException">Thrown when decryption operations fail.</exception>
    /// <exception cref="FormatException">Thrown when base64 decoding fails.</exception>
    public string DecryptLicense(LicenseResult license, string strForIV)
    {
        const string methodName = nameof(DecryptLicense);
        var startTime = DateTime.UtcNow;

        try
        {
            _logger.LogInformation("{MethodName} - Starting license decryption for client: {ClientId}", methodName, strForIV);

            // Validate input parameters
            if (license == null)
                throw new ArgumentNullException(nameof(license), "License cannot be null");

            if (string.IsNullOrWhiteSpace(license.EncryptedLicense))
                throw new ArgumentNullException(nameof(license.EncryptedLicense), "Encrypted license cannot be null or empty");

            if (string.IsNullOrWhiteSpace(license.EncryptedPrivateKey))
                throw new ArgumentNullException(nameof(license.EncryptedPrivateKey), "Encrypted private key cannot be null or empty");

            if (string.IsNullOrWhiteSpace(strForIV))
                throw new ArgumentNullException(nameof(strForIV), "IV string cannot be null or empty");

            // 1. Decrypt license key using clientId-derived key (key unwrapping)
            _logger.LogDebug("{MethodName} - Decrypting license key with client-derived key", methodName);
            var clientKey = DeriveKeyFromStr(strForIV);
            var encryptedKey = Convert.FromBase64String(license.EncryptedPrivateKey);
            byte[] licenseKey;
            using (var aes = Aes.Create())
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Key = clientKey;
                aes.IV = new byte[16]; // Fixed IV for key unwrapping (must match encryption)
                using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                licenseKey = decryptor.TransformFinalBlock(encryptedKey, 0, encryptedKey.Length);
            }

            // 2. Recompute IV from clientId (must match encryption IV)
            _logger.LogDebug("{MethodName} - Deriving IV for license decryption", methodName);
            var iv = DeriveIv(strForIV);

            // 3. Decrypt license JSON using AES-GCM
            _logger.LogDebug("{MethodName} - Decrypting license payload with AES-GCM", methodName);
            var allBytes = Convert.FromBase64String(license.EncryptedLicense);
            int tagLen = 16; // 16-byte authentication tag
            int cipherLen = allBytes.Length - tagLen;

            // Split encrypted data into ciphertext and authentication tag
            var ciphertext = new byte[cipherLen];
            var tag = new byte[tagLen];
            Buffer.BlockCopy(allBytes, 0, ciphertext, 0, cipherLen);
            Buffer.BlockCopy(allBytes, cipherLen, tag, 0, tagLen);

            // Decrypt using AES-GCM with authentication
            var plaintext = new byte[cipherLen];
            using var aesGcm = new AesGcm(new ReadOnlySpan<byte>(licenseKey), tagSizeInBytes: 16);
            aesGcm.Decrypt(iv, ciphertext, tag, plaintext);

            var result = Encoding.UTF8.GetString(plaintext);

            var executionTime = DateTime.UtcNow - startTime;
            _logger.LogInformation("{MethodName} - License decryption completed successfully for client: {ClientId} in {Duration}ms",
                methodName, strForIV, executionTime.TotalMilliseconds);

            return result;
        }
        catch (CryptographicException ex)
        {
            var executionTime = DateTime.UtcNow - startTime;
            _logger.LogError(ex, "{MethodName} - Cryptographic error during license decryption for client: {ClientId} after {Duration}ms",
                methodName, strForIV, executionTime.TotalMilliseconds);
            throw new InvalidOperationException("Failed to decrypt license. Possibly incorrect key or corrupted data.", ex);
        }
        catch (FormatException ex)
        {
            var executionTime = DateTime.UtcNow - startTime;
            _logger.LogError(ex, "{MethodName} - Base64 format error during license decryption for client: {ClientId} after {Duration}ms",
                methodName, strForIV, executionTime.TotalMilliseconds);
            throw;
        }
        catch (Exception ex)
        {
            var executionTime = DateTime.UtcNow - startTime;
            _logger.LogError(ex, "{MethodName} - Unexpected error during license decryption for client: {ClientId} after {Duration}ms: {ErrorMessage}",
                methodName, strForIV, executionTime.TotalMilliseconds, ex.Message);
            throw;
        }
        finally
        {
            _logger.LogDebug("{MethodName} - License decryption operation completed", methodName);
        }
    }
}