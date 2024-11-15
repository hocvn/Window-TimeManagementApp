using System;
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
        public class RegisterViewModel 
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string PasswordConfirmed { get; set; }
            private IDao Dao { get; set; }

            public RegisterViewModel()
            {
                Dao = new SqlDao();
            }

            public void SaveCredential(string username, string password, string email)
            {
                Dao.CreateUser(username, password, email);
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

        UserCredential user = new UserCredential();
        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = usernameTextBox.Text;
            string email = emailTextBox.Text;
            string password = passwordBox.Password;
            string passwordConfirmed = passwordConfirmedBox.Password;
            (bool isOk, string errorMess) = CheckInfomationUserEnter(username, email, password, passwordConfirmed);

            if (!isOk)
            {
                errorMessage.Text = errorMess;
                return;
            }

            // Sign up successfully
            errorMessage.Text = "";
            //user.SaveCredential(username, password, email);
            ViewModel.SaveCredential(username, password, email);
            StorageHelper.RemoveSetting("rememberUsername");

            // Display notification dialog
            //Dialog.ShowContent(((FrameworkElement)sender).XamlRoot, "SUCCESSFUL", "Your account have been created", "Login", null, null);
            ContentDialog dialog = new ContentDialog()
            {
                Title = "SUCCESSFUL",
                Content = "Your account have been created",
                PrimaryButtonText = "Login",
                XamlRoot = ((FrameworkElement)sender).XamlRoot // Set the XamlRoot property
            };
            dialog.PrimaryButtonClick += Dialog_PrimaryButtonClick;
            await dialog.ShowAsync();
        }

        private void Dialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Window loginWindow = new LoginWindow();
            loginWindow.Activate();
            this.Close();
        }

        public (bool, string) CheckInfomationUserEnter(string username, string email, string password, string passwordConfirmed)
        {
            string errorMess;
            if (String.IsNullOrEmpty(username))
            {
                errorMess = "Please fill out your username.";
                return (false, errorMess);
            }

            if (user.IsUsernameInUse(username))
            {
                errorMess = "Username have existed.";
                return (false, errorMess);
            }

            // Check email is in right format
            (bool isOk, string mess) = user.CheckEmailFormat(email);
            if (!isOk)
            {
                errorMess = mess;
                return (false, errorMess);
            }

            // Check if the email is already in use
            if (user.IsEmailInUse(email))
            {
                return (false, "This email is already in use.");
            }

            if (String.IsNullOrEmpty(password))
            {
                errorMess = "Please fill out your password.";
                return (false, errorMess);
            }

            (isOk, mess) = user.IsValidPassword(password);
            if (isOk == false)
            {
                errorMess = mess;
                return (false, errorMess);
            }

            if (password != passwordConfirmed)
            {
                errorMess = "Password confirmation doesn't match the password";
                return (false, errorMess);
            }

            return (true, "");
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Window loginWindow = new LoginWindow();
            loginWindow.Activate();
            this.Close();
        }
    }
}
