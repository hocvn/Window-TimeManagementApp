using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using TimeManagementApp.Helper;
using TimeManagementApp.Services;


namespace TimeManagementApp.Account
{
    /// <summary>
    /// This page is responsible for displaying the account information of the user.
    /// </summary>
    public sealed partial class AccountPage : Page
    {
        public AccountViewModel ViewModel { get; set; } = new AccountViewModel();
        public AccountPage()
        {
            this.InitializeComponent();
            ViewModel.Init();
        }

        private void SignOutButton_Click(object sender, RoutedEventArgs e)
        {
            UserSingleton.Instance.SignOut();
            App.NavigateWindow(new LoginWindow());
        }

        private async void ResetPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            string currentPassword = ViewModel.CurrentPass;

            if (ViewModel.CheckPass() == false)
            {
                ViewModel.ErrorMessage = "Current_password_is_incorrect".GetLocalized();
                return;
            }

            string newPassword = ViewModel.NewPass;
            string passwordConfirmed = ViewModel.ConfirmPass;

            (bool isOk, string mess) = CheckingFormatHelper.CheckAll(newPassword, passwordConfirmed);
            if (isOk == false)
            {
                ViewModel.ErrorMessage = mess;
                return;
            }

            ViewModel.ErrorMessage = "";
            ViewModel.ResetPassword();

            // Show dialog to notify the user that the password has been reset
            var result = await Dialog.ShowContent
            (
                this.XamlRoot,
                "SUCCESSFUL".GetLocalized(),
                "Your_password_has_been_reset".GetLocalized(),
                "OK".GetLocalized(),
                null,
                null
            );
        }

    }
}
