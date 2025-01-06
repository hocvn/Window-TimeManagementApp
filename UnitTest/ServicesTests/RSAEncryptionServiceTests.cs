using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeManagementApp.Services;
using System.Security.Cryptography;
using System.Text;

namespace UnitTest.ServicesTests
{
    [TestClass]
    public class RSAEncryptionServiceTests
    {
        private RSAEncryptionService _rsaEncryptionService;

        [TestInitialize]
        public void Setup()
        {
            _rsaEncryptionService = new RSAEncryptionService();
        }

        [TestMethod]
        public void EncryptDecrypt_WithValidInput_ShouldReturnOriginalData()
        {
            // Arrange
            var data = "Test data";

            // Act
            var (encryptedData, privateKey) = _rsaEncryptionService.Encrypt(data);
            var decryptedData = _rsaEncryptionService.Decrypt(encryptedData, privateKey);

            // Assert
            Assert.AreEqual(data, decryptedData);
        }
    }
}