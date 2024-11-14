using System;
using System.Security.Cryptography;
using System.Text;

namespace TimeManagementApp.Services
{
    public class EncryptionService
    {
        public (string EncryptedTextBase64, RSAParameters PrivateKey) Encrypt(string text)
        {
            byte[] dataToEncrypt = Encoding.UTF8.GetBytes(text);
            RSAParameters publicKey;
            RSAParameters privateKey;
            // Create a new RSA to generate a new public and private key pair
            using (var rsa = RSA.Create())
            {
                rsa.KeySize = 2048;
                publicKey = rsa.ExportParameters(false);
                privateKey = rsa.ExportParameters(true);
            }

            using (var rsa = RSA.Create())
            {
                rsa.ImportParameters(publicKey);
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
    }
}
