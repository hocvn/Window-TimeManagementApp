using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using TimeManagementApp.Helper;

namespace TimeManagementApp.Services
{
    /// <summary>
    /// This class provides encryption and decryption services.
    /// </summary>
    
    public class EncryptionService
    {
        public (string EncryptedTextBase64, RSAParameters PrivateKey) Encrypt(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException("Text to encrypt cannot be null or empty.", nameof(text));
            }

            byte[] dataToEncrypt = Encoding.UTF8.GetBytes(text);

            using (var rsa = RSA.Create())
            {
                rsa.KeySize = 2048; 
                var publicKey = rsa.ExportParameters(false); 
                var privateKey = rsa.ExportParameters(true); 

                byte[] encryptedData = rsa.Encrypt(dataToEncrypt, RSAEncryptionPadding.Pkcs1);

                return (Convert.ToBase64String(encryptedData), privateKey);
            }
        }

        public string Decrypt(string encryptedTextBase64, RSAParameters privateKey)
        {
            byte[] dataToDecrypt = Convert.FromBase64String(encryptedTextBase64);

            using (var rsa = RSA.Create())
            {
                rsa.ImportParameters(privateKey);
                byte[] decryptedData = rsa.Decrypt(dataToDecrypt, RSAEncryptionPadding.Pkcs1);

                return Encoding.UTF8.GetString(decryptedData);
            }
        }

        public string EncryptDPAPI(string text)
        {
            byte[] dataToEncrypt = Encoding.UTF8.GetBytes(text);
            byte[] encryptedData = ProtectedData.Protect(dataToEncrypt, null, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(encryptedData);
        }

        public string DecryptDPAPI(string encryptedTextBase64)
        {
            byte[] dataToDecrypt = Convert.FromBase64String(encryptedTextBase64);
            byte[] decryptedData = ProtectedData.Unprotect(dataToDecrypt, null, DataProtectionScope.CurrentUser);
            return Encoding.UTF8.GetString(decryptedData);
        }

        public void SavePrivateKey(RSAParameters privateKey, string username)
        {
            var rsaSerializable = new RSAParametersSerializable(privateKey);
            string privateKeyJson = JsonSerializer.Serialize(rsaSerializable);
            StorageHelper.SaveSetting(username + "_PrivateKey", privateKeyJson);
        }

        public RSAParameters GetPrivateKey(string username)
        {
            if (StorageHelper.ContainsSetting(username + "_PrivateKey"))
            {
                string privateKeyJson = StorageHelper.GetSetting(username + "_PrivateKey");
                var rsaSerializable = JsonSerializer.Deserialize<RSAParametersSerializable>(privateKeyJson);
                return rsaSerializable.RSAParameters;
            }

            throw new Exception("Private key not found in local settings.");
        }
    }
}
