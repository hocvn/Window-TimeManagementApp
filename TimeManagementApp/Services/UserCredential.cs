﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Windows.Storage;

namespace TimeManagementApp
{
    internal class UserCredential
    {
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        public class UserInfo(string password, string entropy, string email)
        {
            public string Email { get; set; } = email;
            public string Password { get; set; } = password;
            public string Entropy { get; set; } = entropy;
        }

        const int MAX_LENGTH = 50;
        const int MIN_LENGTH = 8;

        public void SaveCredential(string username, string password, string email)
        {
            // Retrieve existing users data or create a new dictionary
            var usersDataJson = localSettings.Values["usersData"] as string;
            Dictionary<string, UserInfo> usersData;

            if (String.IsNullOrEmpty(usersDataJson))
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

            if (String.IsNullOrEmpty(usersDataJson))
            {
                return null; // There is no data stored for any user
            }

            usersData = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, UserInfo>>(usersDataJson);

            // Retrieve the encrypted password and entropy
            if (usersData.ContainsKey(username))
            {
                var UserInfo = usersData[username];
                var encryptedPasswordBase64 = UserInfo.Password;
                var entropyBase64 = UserInfo.Entropy;

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
            string decryptedPassword = this.GetPassword(username);
            // Compare the decrypted password with the entered password
            return decryptedPassword == password;
        }
        public (bool, string) IsValidPassword(string password)
        {
            if (password.Length < MIN_LENGTH)
            {
                return (false, $"Password must be at least {MIN_LENGTH} characters long");
            }

            if (password.Length > MAX_LENGTH)
            {
                return (false, $"Password must be less than {MAX_LENGTH} characters long");
            }

            // Regular expression pattern for validating password
            var regex = new Regex(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d).{8,}$");

            if (!regex.IsMatch(password))
            {
                if (!Regex.IsMatch(password, @"[A-Z]"))
                    return (false, "Password must contain at least one uppercase letter");

                if (!Regex.IsMatch(password, @"[a-z]"))
                    return (false, "Password must contain at least one lowercase letter");

                if (!Regex.IsMatch(password, @"\d"))
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
            Regex regex = new Regex(pattern);
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

            if (String.IsNullOrEmpty(usersDataJson))
            {
                return false; // There is no data stored for any user
            }

            usersData = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, UserInfo>>(usersDataJson);

            for (int i = 0; i < usersData.Count; i++)
            {
                if (usersData.ElementAt(i).Value.Email == email)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsUsernameInUse(string username)
        {
            // Retrieve existing users data or create a new dictionary
            var usersDataJson = localSettings.Values["usersData"] as string;
            Dictionary<string, UserInfo> usersData;

            if (String.IsNullOrEmpty(usersDataJson))
            {
                return false; // There is no data stored for any user
            }

            usersData = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, UserInfo>>(usersDataJson);

            for (int i = 0; i < usersData.Count; i++)
            {
                if (usersData.ElementAt(i).Key == username)
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

            if (String.IsNullOrEmpty(usersDataJson))
            {
                return null; // There is no data stored for any user
            }

            usersData = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, UserInfo>>(usersDataJson);

            for (int i = 0; i < usersData.Count; i++)
            {
                if (usersData.ElementAt(i).Value.Email == email)
                {
                    return usersData.ElementAt(i).Key;
                }
            }

            return null;
        }
    }
}
