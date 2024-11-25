using System;
using Microsoft.UI.Xaml;
using TimeManagementApp.Login.ForgotPassword;
using TimeManagementApp.Helper;
using TimeManagementApp.Dao;
using System.ComponentModel;

namespace TimeManagementApp
{
    /// <summary>
    /// Login window uses to resolve operations about sign in, register.
    /// </summary>
    public sealed partial class LoginWindow : Window
    {
        public partial class LoginViewModel : INotifyPropertyChanged
        {
            public string ErrorMessage { get; set; }
            private IDao _dao { get; set; }

            public LoginViewModel()
            {
                _dao = new SqlDao();
                ErrorMessage = "";
            }

            public event PropertyChangedEventHandler PropertyChanged;

            public bool CheckCredentials(string username, string password)
            {
                return _dao.CheckCredential(username, password);
            }

            public string GetPassword(string username)
            {
                return _dao.GetPassword(username);
            }
        }

        public LoginViewModel ViewModel { get; set; } = new LoginViewModel();

        public LoginWindow()
        {
            this.InitializeComponent();
            // Set the window size
            WindowInitHelper.SetWindowSize(this);
            WindowInitHelper.SetTitle(this, "Time management");
            var rememberUsername = StorageHelper.GetSetting("rememberUsername");

            if (!String.IsNullOrEmpty(rememberUsername))
            {
                // Automatically fill in the username and password
                usernameTextBox.Text = rememberUsername;
                var rememberPassword = ViewModel.GetPassword(rememberUsername);

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
                ViewModel.ErrorMessage = "Please enter both username and password.";
                return;
            }

            if (ViewModel.CheckCredentials(username, password) == false)
            {
                ViewModel.ErrorMessage = "Invalid username or password.";
                return;
            }
            // Username and password are correct
            ViewModel.ErrorMessage = "";

            App.NavigateWindow(new MainWindow());
        }

        private void RememberCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var username = usernameTextBox.Text;
            StorageHelper.SaveSetting("rememberUsername", username);
        }

        private void RememberCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            // Remove the stored username
            StorageHelper.RemoveSetting("rememberUsername");
        }

        // Hide error message when user starts typing
        private void UsernameTextBox_Focus(object sender, RoutedEventArgs e)
        {
            ViewModel.ErrorMessage = "";
        }

        private void PasswordBox_Focus(object sender, RoutedEventArgs e)
        {
            ViewModel.ErrorMessage = "";
        }

        private void ForgotPasswordHyperLinkButton_Click(object sender, RoutedEventArgs e)
        {
            App.NavigateWindow(new ForgotPasswordWindow());
        }

        private void RegisterHyperLinkButton_Click(object sender, RoutedEventArgs e)
        {
            App.NavigateWindow(new RegisterWindow());
        }
    }
}
