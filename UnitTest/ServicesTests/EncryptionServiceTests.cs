using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Security.Cryptography;
using TimeManagementApp.Services;

namespace UnitTest
{
    [TestClass]
    public class EncryptionServiceTests
    {
        private EncryptionService encryptionService;

        [TestInitialize]
        public void TestInitialize()
        {
            encryptionService = new EncryptionService();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Encrypt_ShouldThrowExceptionForNullOrEmptyText()
        {
            // Arrange
            string text = null;

            // Act
            encryptionService.Encrypt(text);

            // Assert is handled by ExpectedException
        }

        [TestMethod]
        public void Encrypt_ShouldReturnEncryptedTextAndPrivateKey()
        {
            // Arrange
            string text = "Test Text";

            // Act
            var result = encryptionService.Encrypt(text);

            // Assert
            Assert.IsNotNull(result.EncryptedTextBase64);
            Assert.IsNotNull(result.PrivateKey);
        }

        [TestMethod]
        public void Decrypt_ShouldReturnOriginalText()
        {
            // Arrange
            string text = "Test Text";
            var encryptedResult = encryptionService.Encrypt(text);

            // Act
            string decryptedText = encryptionService.Decrypt(encryptedResult.EncryptedTextBase64, encryptedResult.PrivateKey);

            // Assert
            Assert.AreEqual(text, decryptedText);
        }

        [TestMethod]
        public void EncryptDPAPI_ShouldReturnEncryptedText()
        {
            // Arrange
            string text = "Test Text";

            // Act
            string encryptedText = encryptionService.EncryptDPAPI(text);

            // Assert
            Assert.IsNotNull(encryptedText);
        }

        [TestMethod]
        public void DecryptDPAPI_ShouldReturnOriginalText()
        {
            // Arrange
            string text = "Test Text";
            string encryptedText = encryptionService.EncryptDPAPI(text);

            // Act
            string decryptedText = encryptionService.DecryptDPAPI(encryptedText);

            // Assert
            Assert.AreEqual(text, decryptedText);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetPrivateKey_ShouldThrowExceptionWhenKeyNotFound()
        {
            // Arrange
            var username = "nonExistingUser";

            // Act
            encryptionService.GetPrivateKey(username);

            // Assert is handled by ExpectedException
        }
    }
}
