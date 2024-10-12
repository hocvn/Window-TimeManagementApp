using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Windows.Storage;
using System;
using System.Diagnostics;
using Windows.Services.Maps;
using Windows.System;


namespace TimeManagementApp.Login.ForgotPassword
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ForgotPasswordPage3 : Page
    {
        public string Email { get; set; }

        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        public ForgotPasswordPage3()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string email = e.Parameter as string;
            if (!string.IsNullOrEmpty(email))
            {
                this.Email = email;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ForgotPasswordPage2));
        }

        UserCredential user = new UserCredential();
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            string password = passwordBox.Password;
            string passwordConfirmed = passwordConfirmedBox.Password;

            if (String.IsNullOrEmpty(password))
            {
                errorMessage.Text = "Please fill out your password.";
                return;
            }

            (bool isOk, string mess) = user.IsValidPassword(password);
            if (isOk == false)
            {
                errorMessage.Text = mess;
                return;
            }

            if (String.IsNullOrEmpty(passwordConfirmed))
            {
                errorMessage.Text = "Please fill out your password confirmation.";
                return;
            }

            if (password != passwordConfirmed)
            {
                errorMessage.Text = "Password confirmation doesn't match the password";
                return;
            }

            errorMessage.Text = "";

            // Get username
            try
            {
                string username = user.GetUsername(this.Email);
                user.SaveCredential(username, password, this.Email);
                localSettings.Values.Remove("rememberUsername");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception caught: {ex.Message}");
            }
        }
    }
}
