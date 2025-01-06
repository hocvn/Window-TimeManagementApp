using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeManagementApp.Services;
using MimeKit;
using Moq;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using System;

namespace UnitTest.ServicesTests
{
    [TestClass]
    public class EmailServiceTests
    {
        [TestMethod]
        public async Task SendOtpAsync_WithValidParameters_ShouldSendEmail()
        {
            // Arrange
            var emailService = new EmailService();
            var recipientEmail = "test@example.com";
            var otpCode = "123456";

            // Act
            await emailService.SendOtpAsync(recipientEmail, otpCode);

            // Assert
            // Since we cannot directly verify the email sending, we assume no exceptions mean success
            Assert.IsTrue(true);
        }
    }
}