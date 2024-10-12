using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;

namespace TimeManagementApp.Login.ForgotPassword
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ForgotPasswordPage2 : Page
    {
        public string Email { get; set; }
        public ForgotPasswordPage2()
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

        //public string GenerateOtp(int length)
        //{
        //    Random random = new Random();
        //    string otp = "";
        //    for (int i = 0; i < length; i++)
        //    {
        //        otp += random.Next(0, 10).ToString();
        //    }
        //    return otp;
        //}

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ForgotPasswordPage1));
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ForgotPasswordPage3), this.Email);
        }

        private void OtpTextBox_Focus(object sender, RoutedEventArgs e)
        {
            errorMessage.Text = "";
        }
    }
}
