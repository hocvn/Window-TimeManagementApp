using System.ComponentModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using TimeManagementApp.Dao;
using TimeManagementApp.Helper;

namespace TimeManagementApp
{
    /// <summary>
    /// This window is used to register a new account.
    /// </summary>
    public sealed partial class RegisterWindow : Window
    {
        public class RegisterViewModel : INotifyPropertyChanged
        {
            public string ErrorMessage { get; set; }

            public string Username { get; set; }

            public string Email { get; set; }

            public string Password { get; set; }

            public string ConfirmedPass { get; set; }


            private IDao _dao { get; set; }

            public RegisterViewModel()
            {
                _dao = new SqlDao();
                ErrorMessage = "";
            }

            public event PropertyChangedEventHandler PropertyChanged;

            public void SaveCredential()
            {
                _dao.CreateUser(Username, Password, Email);
            }

            public bool IsUsernameInUse()
            {
                return _dao.IsUsernameInUse(Username);
            }

            public bool IsEmailInUse()
            {
                return _dao.IsEmailInUse(Username);
            }
        }

        public RegisterViewModel ViewModel { get; set; } = new RegisterViewModel();

        public RegisterWindow()
        {
            this.InitializeComponent();
            // Set the window size
            WindowInitHelper.SetWindowSize(this);
            WindowInitHelper.SetTitle(this, "Time management");
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            (bool isOk, string errorMess) = CheckingFormatHelper.CheckAll(ViewModel.Username, ViewModel.Email, ViewModel.Password, ViewModel.ConfirmedPass);

            if (!isOk)
            {
                ViewModel.ErrorMessage = errorMess;
                return;
            }
            // Check if the username or email is already in use
            if (ViewModel.IsUsernameInUse())
            {
                ViewModel.ErrorMessage = "Username_is_already_in_use".GetLocalized();
                return;
            }
            if (ViewModel.IsEmailInUse())
            {
                ViewModel.ErrorMessage = "Email_is_already_in_use".GetLocalized();
                return;
            }

            ViewModel.ErrorMessage = ""; // Sign up successfully
            ViewModel.SaveCredential();
            StorageHelper.RemoveSetting("rememberUsername");

            // Display notification dialog
            var result = await Dialog.ShowContent
            (
                ((FrameworkElement)sender).XamlRoot, 
                "SUCCESSFUL".GetLocalized(), 
                "Your_account_have_been_created".GetLocalized(), 
                "Login".GetLocalized(), 
                null, 
                null
            );

            if (result == ContentDialogResult.Primary)
            {
                App.NavigateWindow(new LoginWindow());
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            App.NavigateWindow(new LoginWindow());
        }
    }
}
