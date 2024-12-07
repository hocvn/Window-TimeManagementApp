using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Security.Cryptography;
using TimeManagementApp.Services;

namespace UnitTest
{
    [TestClass]
    public class RSAEncryptionServiceTests
    {
        private RSAEncryptionService rsaEncryptionService;

        [TestInitialize]
        public void TestInitialize()
        {
            rsaEncryptionService = new RSAEncryptionService();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Encrypt_ShouldThrowExceptionForNullOrEmptyText()
        {
            // Arrange
            string text = null;

            // Act
            rsaEncryptionService.Encrypt(text);

            // Assert is handled by ExpectedException
        }

        [TestMethod]
        public void Encrypt_ShouldReturnEncryptedTextAndPrivateKey()
        {
            // Arrange
            string text = "Test Text";

            // Act
            var result = rsaEncryptionService.Encrypt(text);

            // Assert
            Assert.IsNotNull(result.EncryptedTextBase64);
            Assert.IsNotNull(result.PrivateKey);
        }

        [TestMethod]
        public void Decrypt_ShouldReturnOriginalText()
        {
            // Arrange
            string text = "Test Text";
            var encryptedResult = rsaEncryptionService.Encrypt(text);

            // Act
            string decryptedText = rsaEncryptionService.Decrypt(encryptedResult.EncryptedTextBase64, encryptedResult.PrivateKey);

            // Assert
            Assert.AreEqual(text, decryptedText);
        }

        [TestMethod]
        public void EncryptDPAPI_ShouldReturnEncryptedText()
        {
            // Arrange
            string text = "Test Text";

            // Act
            string encryptedText = rsaEncryptionService.EncryptDPAPI(text);

            // Assert
            Assert.IsNotNull(encryptedText);
        }

        [TestMethod]
        public void DecryptDPAPI_ShouldReturnOriginalText()
        {
            // Arrange
            string text = "Test Text";
            string encryptedText = rsaEncryptionService.EncryptDPAPI(text);

            // Act
            string decryptedText = rsaEncryptionService.DecryptDPAPI(encryptedText);

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
            rsaEncryptionService.GetPrivateKey(username);

            // Assert is handled by ExpectedException
        }
    }
}
