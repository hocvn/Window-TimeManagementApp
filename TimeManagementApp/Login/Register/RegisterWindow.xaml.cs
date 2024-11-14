using System;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using TimeManagementApp.Helper;
using Windows.Storage;

namespace TimeManagementApp
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RegisterWindow : Window
    {
        private Window m_window;

        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
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
            user.SaveCredential(username, password, email);
            localSettings.Values.Remove("rememberUsername");

            // Display notification dialog
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
            m_window = new LoginWindow();
            m_window.Activate();
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
            m_window = new LoginWindow();
            m_window.Activate();
            this.Close();
        }
    }
}
