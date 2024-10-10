using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System.Security.Cryptography;
using System.Text;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace TimeManagementApp
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginWindow : Window
    {
        //Dictionary<string, (string, string)> users = new Dictionary<string, (string, string)>();
        private Window m_window;
        private ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        private StorageFolder localFolder = ApplicationData.Current.LocalFolder;

        public class UserPassword
        {
            public string Password { get; set; }
            public string Entropy { get; set; }
        }
        public LoginWindow()
        {
            this.InitializeComponent();
            // Store the user credentials

            var username = "admin";
            var passwordRaw = "123456";
            saveUserCredential(username, passwordRaw);

            var rememberUsername = localSettings.Values["rememberUsername"] as string;
            if (!String.IsNullOrEmpty(rememberUsername))
            {
                // Automatically fill in the username and password
                usernameTextBox.Text = rememberUsername;
                var rememberPassword = getPassword(rememberUsername);

                if (rememberPassword == null)
                {
                    // The password is not stored
                    return;
                }
                passwordBox.Password = rememberPassword;
                rememberCheckBox.IsChecked = true;
            }
        }

        private void saveUserCredential(string username, string password)
        {
            // Retrieve existing users data or create a new dictionary
            var usersDataJson = localSettings.Values["usersData"] as string;
            Dictionary<string, UserPassword> usersData;

            if (String.IsNullOrEmpty(usersDataJson))
            {
                usersData = new Dictionary<string, UserPassword>();
            }
            else
            {
                usersData = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, UserPassword>>(usersDataJson);
            }

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

            var passData = new UserPassword
            {
                Password = encryptedPasswordBase64,
                Entropy = entropyInBase64
            };

            // Save the user's encrypted password and entropy
            usersData[username] = passData;

            // Serialize the dictionary back to JSON
            usersDataJson = System.Text.Json.JsonSerializer.Serialize(usersData);

            // Store the JSON string in local settings
            localSettings.Values["usersData"] = usersDataJson;
        }

        private String getPassword(string username)
        {
            // Retrieve existing users data or create a new dictionary
            var usersDataJson = localSettings.Values["usersData"] as string;
            Dictionary<string, UserPassword> usersData;
            usersData = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, UserPassword>>(usersDataJson);

            if (String.IsNullOrEmpty(usersDataJson))
            {
                return null; // There is no data stored for any user
            }

            // Retrieve the encrypted password and entropy
            if (usersData.ContainsKey(username))
            {
                var encryptedPasswordBase64 = usersData[username].Password;
                var entropyBase64 = usersData[username].Entropy;

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

        private bool validateUserCredentials(string username, string password)
        {
            return true; // All credentials are valid (for now)
        }
        private bool checkUserCredentials(string username, string password)
        {
            string decryptedPassword = getPassword(username);
            // Compare the decrypted password with the entered password
            return decryptedPassword == password;
        }
        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = usernameTextBox.Text;
            string password = passwordBox.Password;

            if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password))
            {
                errorMessage.Text = "Please enter both username and password";
                return;
            }
            if (checkUserCredentials(username, password) == false)
            {
                errorMessage.Text = "Invalid username or password";
                return;
            }
            // Username and password are correct
            errorMessage.Text = "";
            m_window = new MainWindow();
            m_window.Activate();
            this.Close();
        }
        private void rememberCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var username = usernameTextBox.Text;
            localSettings.Values["rememberUsername"] = username;
        }
        private void rememberCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            // Remove the stored username
            localSettings.Values.Remove("rememberUsername");
        }
        private void usernameTextBox_Focus(object sender, RoutedEventArgs e)
        {
            errorMessage.Text = "";
        }
        private void passwordBox_Focus(object sender, RoutedEventArgs e)
        {
            errorMessage.Text = "";
        }
        private void forgotPasswordHyperLinkButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void registerHyperLinkButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }

}
