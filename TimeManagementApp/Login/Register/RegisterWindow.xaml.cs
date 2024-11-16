using System.ComponentModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using TimeManagementApp.Dao;
using TimeManagementApp.Helper;

namespace TimeManagementApp
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RegisterWindow : Window
    {
        public class RegisterViewModel : INotifyPropertyChanged
        {
            public string ErrorMessage { get; set; }
            private IDao dao { get; set; }

            public RegisterViewModel()
            {
                dao = new SqlDao();
                ErrorMessage = "";
            }

            public event PropertyChangedEventHandler PropertyChanged;

            public void SaveCredential(string username, string password, string email)
            {
                dao.CreateUser(username, password, email);
                //user.SaveCredential(username, password, email); // MockDao
            }

            public bool IsUsernameInUse(string username)
            {
                return dao.IsUsernameInUse(username);
            }

            public bool IsEmailInUse(string username)
            {
                return dao.IsEmailInUse(username);
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
            string username = usernameTextBox.Text;
            string email = emailTextBox.Text;
            string password = passwordBox.Password;
            string passwordConfirmed = passwordConfirmedBox.Password;

            (bool isOk, string errorMess) = CheckingFormatHelper.CheckAll(username, email, password, passwordConfirmed);

            if (!isOk)
            {
                ViewModel.ErrorMessage = errorMess;
                return;
            }
            // Check if the username or email is already in use
            if (ViewModel.IsUsernameInUse(username))
            {
                ViewModel.ErrorMessage = "Username is already in use";
                return;
            }
            if (ViewModel.IsEmailInUse(email))
            {
                ViewModel.ErrorMessage = "Email is already in use";
                return;
            }

            ViewModel.ErrorMessage = ""; // Sign up successfully
            ViewModel.SaveCredential(username, password, email);
            StorageHelper.RemoveSetting("rememberUsername");

            // Display notification dialog
            var result = await Dialog.ShowContent(((FrameworkElement)sender).XamlRoot, "SUCCESSFUL", "Your account have been created", "Login", null, null);

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
