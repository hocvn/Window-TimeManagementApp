using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using Windows.Services.Maps;
using Windows.System;


namespace TimeManagementApp.Login.ForgotPassword
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ForgotPasswordPage3 : Page
    {
        public ForgotPasswordPage3()
        {
            this.InitializeComponent();
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

        }
    }
}
