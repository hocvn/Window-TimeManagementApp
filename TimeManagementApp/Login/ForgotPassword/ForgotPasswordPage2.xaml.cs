using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace TimeManagementApp.Login.ForgotPassword
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ForgotPasswordPage2 : Page
    {
        public ForgotPasswordPage2()
        {
            this.InitializeComponent();
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
            Frame.Navigate(typeof(ForgotPasswordPage3));
        }

        private void OtpTextBox_Focus(object sender, RoutedEventArgs e)
        {
            errorMessage.Text = "";
        }
    }
}
