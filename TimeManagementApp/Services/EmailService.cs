using System;
using System.Threading.Tasks;
using MimeKit;

namespace TimeManagementApp.Services
{
    /// <summary>
    /// This class supports send email by Gmail
    /// </summary>
    public class EmailService
    {
        private const string SmtpServer = "smtp.gmail.com";
        private const int SmtpPort = 587; 
        private const string SenderEmail = "timewindowapp@gmail.com"; // Sender
        private const string SenderPassword = "dvrk stly brzg orbe"; // Use Gmail app password 

        public async Task SendOtpAsync(string recipientEmail, string otpCode)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Time Management App", SenderEmail));
            message.To.Add(new MailboxAddress("User", recipientEmail));
            message.Subject = "Your OTP Code";

            message.Body = new TextPart("plain")
            {
                Text = $"Your OTP code is: {otpCode}"
            };

            using var client = new MailKit.Net.Smtp.SmtpClient();
            try
            {
                await client.ConnectAsync(SmtpServer, SmtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(SenderEmail, SenderPassword);
                await client.SendAsync(message);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to send OTP email", ex);
            }
            finally
            {
                await client.DisconnectAsync(true);
            }
        }
    }
}
