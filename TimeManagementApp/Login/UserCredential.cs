using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Windows.Storage;

namespace TimeManagementApp.Login
{
    internal class UserCredential
    {
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        public class UserInfo
        {
            public string email { get; set; }
            public string password { get; set; }
            public string entropy { get; set; }

            public UserInfo(string password, string entropy, string email)
            {
                this.password = password;
                this.entropy = entropy;
                this.email = email;
            }
        }
        public void SaveCredential(string username, string password, string email)
        {
            // Retrieve existing users data or create a new dictionary
            var usersDataJson = localSettings.Values["usersData"] as string;
            Dictionary<string, UserInfo> usersData;

            if (string.IsNullOrEmpty(usersDataJson))
            {
                usersData = new Dictionary<string, UserInfo>();
            }
            else
            {
                usersData = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, UserInfo>>(usersDataJson);
            }

            // Encrypt the password
            (string encryptedPasswordBase64, string entropyInBase64) = EncryptPassword(password);
            UserInfo userInfo = new UserInfo(encryptedPasswordBase64, entropyInBase64, email);

            // Save the user's encrypted password and entropy
            usersData[username] = userInfo;

            // Serialize the dictionary back to JSON
            usersDataJson = System.Text.Json.JsonSerializer.Serialize(usersData);

            // Store the JSON string in local settings
            localSettings.Values["usersData"] = usersDataJson;
        }
        private (string, string) EncryptPassword(string password)
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

            return (encryptedPasswordBase64, entropyInBase64);
        }
        public string GetPassword(string username)
        {
            // Retrieve existing users data or create a new dictionary
            var usersDataJson = localSettings.Values["usersData"] as string;
            Dictionary<string, UserInfo> usersData;

            if (string.IsNullOrEmpty(usersDataJson))
            {
                return null; // There is no data stored for any user
            }

            usersData = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, UserInfo>>(usersDataJson);

            // Retrieve the encrypted password and entropy
            if (usersData.ContainsKey(username))
            {
                var UserInfo = usersData[username];
                var encryptedPasswordBase64 = UserInfo.password;
                var entropyBase64 = UserInfo.entropy;

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
        public bool CheckCredentials(string username, string password)
        {
            string decryptedPassword = GetPassword(username);
            // Compare the decrypted password with the entered password
            return decryptedPassword == password;
        }
        public (bool, string) IsValidPassword(string password)
        {
            if (password.Length < 8)
            {
                return (false, "Password must be at least 8 characters long");
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
        public (bool, string) CheckEmailFormat(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return (false, "Please fill out your email.");
            }
            // Regular expression pattern for validating email
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            // Create a Regex object
            Regex regex = new Regex(pattern);

            // Check if the email matches the pattern
            if (!regex.IsMatch(email))
            {
                return (false, "Please fill out a valid email address format.");
            }

            return (true, "");
        }

        public bool IsEmailInUse(string email)
        {
            // Retrieve existing users data or create a new dictionary
            var usersDataJson = localSettings.Values["usersData"] as string;
            Dictionary<string, UserInfo> usersData;

            if (string.IsNullOrEmpty(usersDataJson))
            {
                return false; // There is no data stored for any user
            }

            usersData = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, UserInfo>>(usersDataJson);

            for (int i = 0; i < usersData.Count; i++)
            {
                if (usersData.ElementAt(i).Value.email == email)
                {
                    return true;
                }
            }

            return false;
        }

        public string GetUsername(string email)
        {
            var usersDataJson = localSettings.Values["usersData"] as string;
            Dictionary<string, UserInfo> usersData;

            if (string.IsNullOrEmpty(usersDataJson))
            {
                return null; // There is no data stored for any user
            }

            usersData = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, UserInfo>>(usersDataJson);

            for (int i = 0; i < usersData.Count; i++)
            {
                if (usersData.ElementAt(i).Value.email == email)
                {
                    return usersData.ElementAt(i).Key;
                }
            }

            return null;
        }
    }
}
