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
            public string Username { get; set; }
            public string Password { get; set; }
            private IDao _dao { get; set; }

            public LoginViewModel()
            {
                _dao = new SqlDao();
                ErrorMessage = "";
            }

            public event PropertyChangedEventHandler PropertyChanged;

            public bool CheckCredentials()
            {
                return _dao.CheckCredential(Username, Password);
            }

            public string GetPassword()
            {
                return _dao.GetPassword(Username);
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
                ViewModel.Username = rememberUsername;
                var rememberPassword = ViewModel.GetPassword();

                if (rememberPassword == null)
                {
                    // The password is not stored
                    return;
                }
                ViewModel.Password = rememberPassword;
                rememberCheckBox.IsChecked = true;
            }
            else
            {
                rememberCheckBox.IsChecked = false;
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(ViewModel.Username) || String.IsNullOrEmpty(ViewModel.Password))
            {
                ViewModel.ErrorMessage = "Please_enter_both_username_and_password".GetLocalized();
                return;
            }

            if (ViewModel.CheckCredentials() == false)
            {
                ViewModel.ErrorMessage = "Invalid_username_or_password".GetLocalized();
                return;
            }
            // Username and password are correct
            ViewModel.ErrorMessage = "";

            App.NavigateWindow(new MainWindow());
        }

        private void RememberCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            StorageHelper.SaveSetting("rememberUsername", ViewModel.Username);
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
