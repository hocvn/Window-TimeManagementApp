using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;
using static TimeManagementApp.LoginWindow;

namespace TimeManagementApp
{
    internal class UserCredential
    {
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        public class EncrypedPasswordInfo
        {
            public string password { get; set; }
            public string entropy { get; set; }

            public EncrypedPasswordInfo(string password, string entropy)
            {
                this.password = password;
                this.entropy = entropy;
            }
        }
        public void saveCredential(string username, string password)
        {
            // Retrieve existing users data or create a new dictionary
            var usersDataJson = localSettings.Values["usersData"] as string;
            Dictionary<string, EncrypedPasswordInfo> usersData;

            if (String.IsNullOrEmpty(usersDataJson))
            {
                usersData = new Dictionary<string, EncrypedPasswordInfo>();
            }
            else
            {
                usersData = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, EncrypedPasswordInfo>>(usersDataJson);
            }

            // Encrypt the password
            EncrypedPasswordInfo encryptedPasswordInfo = EncryptPassword(password);

            // Save the user's encrypted password and entropy
            usersData[username] = encryptedPasswordInfo;

            // Serialize the dictionary back to JSON
            usersDataJson = System.Text.Json.JsonSerializer.Serialize(usersData);

            // Store the JSON string in local settings
            localSettings.Values["usersData"] = usersDataJson;
        }
        private EncrypedPasswordInfo EncryptPassword(string password)
        {
            // Encrypt the password
            var passwordInBytes = Encoding.UTF8.GetBytes(password);
            var entropyInBytes = new byte[20];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(entropyInBytes);
            }

            var encryptedPasswordInBytes = ProtectedData.Protect(
                   passwordInBytes,
                   entropyInBytes,
                   DataProtectionScope.CurrentUser
            );

            var encryptedPasswordBase64 = Convert.ToBase64String(encryptedPasswordInBytes);
            var entropyInBase64 = Convert.ToBase64String(entropyInBytes);

            return new EncrypedPasswordInfo(encryptedPasswordBase64, entropyInBase64);
        }
        public string getPassword(string username)
        {
            // Retrieve existing users data or create a new dictionary
            var usersDataJson = localSettings.Values["usersData"] as string;
            Dictionary<string, EncrypedPasswordInfo> usersData;

            if (String.IsNullOrEmpty(usersDataJson))
            {
                return null; // There is no data stored for any user
            }

            usersData = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, EncrypedPasswordInfo>>(usersDataJson);

            // Retrieve the encrypted password and entropy
            if (usersData.ContainsKey(username))
            {
                var EncrypedPasswordInfo = usersData[username];
                var encryptedPasswordBase64 = EncrypedPasswordInfo.password;
                var entropyBase64 = EncrypedPasswordInfo.entropy;

                if (encryptedPasswordBase64 == null || entropyBase64 == null)
                {
                    return null;
                }
                var encryptedPasswordInBytes = Convert.FromBase64String(encryptedPasswordBase64);
                var entropyInBytes = Convert.FromBase64String(entropyBase64);

                // Decrypt the password
                var decryptedPasswordInBytes = ProtectedData.Unprotect(
                    encryptedPasswordInBytes,
                    entropyInBytes,
                    DataProtectionScope.CurrentUser
                );

                return Encoding.UTF8.GetString(decryptedPasswordInBytes);
            }
            return null;
        }
        public bool checkCredentials(string username, string password)
        {
            string decryptedPassword = this.getPassword(username);
            // Compare the decrypted password with the entered password
            return decryptedPassword == password;
        }
        private (bool, string) isValidPassword(string password)
        {
            if (password.Length < 6)
            {
                return (false, "Password must be at least 6 characters long");
            }
            if (!password.Any(char.IsUpper))
            {
                return (false, "Password must contain at least one uppercase letter");
            }
            if (!password.Any(char.IsLower))
            {
                return (false, "Password must contain at least one lowercase letter");
            }
            if (!password.Any(char.IsDigit))
            {
                return (false, "Password must contain at least one digit");
            }
            return (true, "");
        }
    }
}
