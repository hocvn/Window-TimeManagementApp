using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;

namespace TimeManagementApp.Login.ForgotPassword
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OTPPage : Page
    {
        public string Email { get; set; }
        public string Otp { get; set; }
        public OTPPage()
        {
            this.InitializeComponent();
            Otp = GenerateOtp(6);
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

        void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Otp = GenerateOtp(6);
            myTextBlock.Text = $"Please enter this code '{Otp}' below.";
            //SendOtpEmail(this.Email, Otp);
        }

        public string GenerateOtp(int length)
        {
            Random random = new Random();
            string otp = "";
            for (int i = 0; i < length; i++)
            {
                otp += random.Next(0, 10).ToString();
            }
            return otp;
        }

        //public void SendOtpEmail(string recipientEmail, string otp)
        //{
        //    var smtpClient = new SmtpClient("smtp.gmail.com")
        //    {
        //        Port = 587,
        //        Credentials = new NetworkCredential("hocdothai2004@gmail.com", "your-app-password"),
        //        EnableSsl = true,
        //    };

        //    var mailMessage = new MailMessage
        //    {
        //        From = new MailAddress("hocdothai2004@gmail.com"),
        //        Subject = "Your OTP Code",
        //        Body = $"Your OTP code is: {otp}",
        //        IsBodyHtml = true,
        //    };
        //    mailMessage.To.Add(recipientEmail);

        //    smtpClient.Send(mailMessage);
        //}


        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            //Frame.Navigate(typeof(ForgotPasswordPage1));
            Frame.GoBack();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (otpTextBox.Text == Otp)
            {
                Frame.Navigate(typeof(ResetPasswordPage), this.Email);
            }
            else
            {
                errorMessage.Text = "Invalid OTP code";
            }
        }

        private void OtpTextBox_Focus(object sender, RoutedEventArgs e)
        {
            errorMessage.Text = "";
        }
    }
}
