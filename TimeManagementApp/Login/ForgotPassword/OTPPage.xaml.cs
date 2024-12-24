using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using TimeManagementApp.Helper;
using TimeManagementApp.Services;

namespace TimeManagementApp.Login.ForgotPassword
{
    /// <summary>
    /// This page is responsible for sending OTP to user's email and check this OTP to identify user
    /// </summary>
    public sealed partial class OTPPage : Page
    {
        public class OtpViewModel : INotifyPropertyChanged
        {
            public string Email { get; set; }
            public string Otp { get; set; }
            public string ErrorMessage { get; set; }

            public event PropertyChangedEventHandler PropertyChanged;

            public bool ValidateOtp(string otp)
            {
                return otp == Otp;
            }
        }

        public OtpViewModel ViewModel { get; set; } = new OtpViewModel();

        public OTPPage()
        {
            this.InitializeComponent();
            ViewModel.ErrorMessage = "";
            ViewModel.Otp = OtpService.GenerateOtp();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string email = e.Parameter as string;
            if (!string.IsNullOrEmpty(email))
            {
                ViewModel.Email = email;
            }
            _ = SendOtpEmailAsync();
        }

        public async Task SendOtpEmailAsync()
        {
            var emailService = new EmailService();
            string recipientEmail = ViewModel.Email;
            string otp = ViewModel.Otp;

            try
            {
                await emailService.SendOtpAsync(recipientEmail, otp);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending OTP: {ex.Message}");
            }
        }


        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            var otp = otpTextBox.Text;
            if (ViewModel.ValidateOtp(otp))
            {
                Frame.Navigate(typeof(ResetPasswordPage), ViewModel.Email);
            }
            else
            {
                ViewModel.ErrorMessage = "Invalid_code".GetLocalized();
                _ = SendOtpEmailAsync();
            }
        }

        private void OtpTextBox_Focus(object sender, RoutedEventArgs e)
        {
            ViewModel.ErrorMessage = "";
        }
    }
}
