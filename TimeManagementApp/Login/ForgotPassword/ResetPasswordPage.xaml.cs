using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System.ComponentModel;
using TimeManagementApp.Dao;
using TimeManagementApp.Helper;


namespace TimeManagementApp.Login.ForgotPassword
{
    /// <summary>
    /// This page allows the user to reset their password.
    /// </summary>
    public sealed partial class ResetPasswordPage : Page
    {
        public partial class ResetPassViewModel : INotifyPropertyChanged
        {
            public string Email { get; set; }
            public string ErrorMessage { get; set; }
            public string NewPass { get; set; }
            public string ConfirmPass { get; set; }
            private IDao _dao { get; set; }

            public event PropertyChangedEventHandler PropertyChanged;

            public ResetPassViewModel()
            {
                _dao = new SqlDao();
                ErrorMessage = "";
            }

            public string GetUsername()
            {
                return _dao.GetUsername(Email);
            }
            public void ResetPassword()
            {
                string username = GetUsername();
                _dao.ResetPassword(username, NewPass, Email);
            }
        }

        public ResetPassViewModel ViewModel { get; set; } = new ResetPassViewModel();

        public ResetPasswordPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string email = e.Parameter as string;
            if (!string.IsNullOrEmpty(email))
            {
                ViewModel.Email = email;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private async void ResetPassButton_Click(object sender, RoutedEventArgs e)
        {
            (bool isOk, string mess) = CheckingFormatHelper.CheckAll(ViewModel.NewPass, ViewModel.ConfirmPass);
            if (isOk == false)
            {
                ViewModel.ErrorMessage = mess;
                return;
            }
            ViewModel.ErrorMessage = "";

            
            ViewModel.ResetPassword();
            StorageHelper.RemoveSetting("rememberUsername");

            var result = await Dialog.ShowContent
            (
                this.XamlRoot, 
                "SUCCESSFUL".GetLocalized(), 
                "Your_password_has_been_reset".GetLocalized(),
                "Login".GetLocalized(),
                null, 
                null
            );
            if (result == ContentDialogResult.Primary)
            {
                App.NavigateWindow(new LoginWindow());
            }
        }
    }
}
