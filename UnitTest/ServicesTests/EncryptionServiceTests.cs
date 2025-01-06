using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeManagementApp.Services;

namespace UnitTest.ServicesTests
{
    [TestClass]
    public class EncryptionServiceTests
    {
        [TestMethod]
        public void EncryptDPAPI_WithValidInput_ShouldReturnEncryptedString()
        {
            // Arrange
            var encryptionService = new EncryptionService();
            var input = "testpassword";

            // Act
            var encrypted = encryptionService.EncryptDPAPI(input);

            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(encrypted));
        }

        [TestMethod]
        public void DecryptDPAPI_WithValidInput_ShouldReturnDecryptedString()
        {
            // Arrange
            var encryptionService = new EncryptionService();
            var input = "testpassword";
            var encrypted = encryptionService.EncryptDPAPI(input);

            // Act
            var decrypted = encryptionService.DecryptDPAPI(encrypted);

            // Assert
            Assert.AreEqual(input, decrypted);
        }
    }
}