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

namespace TimeManagementApp
{
    /// <summary>
    /// Login window uses to resolve operations about sign in, register.
    /// </summary>
    public sealed partial class LoginWindow : Window
    {
        private Window m_window;
        UserCredential user = new UserCredential();
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        public LoginWindow()
        {
            this.InitializeComponent();
            // Store the user credentials
            //localSettings.Values.Remove("admin");
            //var username = "admin";
            //var passwordRaw = "123456";
            //var email = "admin@gmail.com";
            //user.SaveCredential(username, passwordRaw, email);

            var rememberUsername = localSettings.Values["rememberUsername"] as string;
            if (!String.IsNullOrEmpty(rememberUsername))
            {
                // Automatically fill in the username and password
                usernameTextBox.Text = rememberUsername;
                var rememberPassword = user.GetPassword(rememberUsername);

                if (rememberPassword == null)
                {
                    // The password is not stored
                    return;
                }
                passwordBox.Password = rememberPassword;
                rememberCheckBox.IsChecked = true;
            }
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = usernameTextBox.Text;
            string password = passwordBox.Password;

            if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password))
            {
                errorMessage.Text = "Please enter both username and password.";
                return;
            }
            if (user.CheckCredentials(username, password) == false)
            {
                errorMessage.Text = "Invalid username or password.";
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

        // Hide error message when user starts typing
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
            m_window = new RegisterWindow();
            m_window.Activate();
            this.Close();
        }
    }

}
