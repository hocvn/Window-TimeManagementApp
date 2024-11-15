using System;
using Microsoft.UI.Xaml;
using Windows.Storage;
using TimeManagementApp.Login.ForgotPassword;
using Microsoft.UI.Windowing;
using TimeManagementApp.Helper;

namespace TimeManagementApp
{
    /// <summary>
    /// Login window uses to resolve operations about sign in, register.
    /// </summary>
    public sealed partial class LoginWindow : Window
    {
        private Window m_window;
        private UserCredential user = new UserCredential();
        private ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        public LoginWindow()
        {
            this.InitializeComponent();
            // Set the window size
            WindowInitHelper.SetWindowSize(this);
            WindowInitHelper.SetTitle(this, "Time management");

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

        private void LoginButton_Click(object sender, RoutedEventArgs e)
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

        private void RememberCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var username = usernameTextBox.Text;
            localSettings.Values["rememberUsername"] = username;
        }

        private void RememberCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            // Remove the stored username
            localSettings.Values.Remove("rememberUsername");
        }

        // Hide error message when user starts typing
        private void UsernameTextBox_Focus(object sender, RoutedEventArgs e)
        {
            errorMessage.Text = "";
        }

        private void PasswordBox_Focus(object sender, RoutedEventArgs e)
        {
            errorMessage.Text = "";
        }
        private void ForgotPasswordHyperLinkButton_Click(object sender, RoutedEventArgs e)
        {
            m_window = new ForgotPasswordWindow();
            m_window.Activate();
            this.Close();
            // This feature is not working for now
        }

        private void RegisterHyperLinkButton_Click(object sender, RoutedEventArgs e)
        {
            m_window = new RegisterWindow();
            m_window.Activate();
            this.Close();
        }
    }

}
