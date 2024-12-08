using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeManagementApp.Services;

namespace UnitTest
{
    [TestClass]
    public class UserSingletonTests
    {
        [TestMethod]
        public void Instance_ShouldReturnSameInstance()
        {
            // Act
            var instance1 = UserSingleton.Instance;
            var instance2 = UserSingleton.Instance;

            // Assert
            Assert.AreSame(instance1, instance2);
        }

        [TestMethod]
        public void Instance_ShouldInitializePropertiesCorrectly()
        {
            // Arrange
            var instance = UserSingleton.Instance;
            instance.Username = "testUser";
            instance.EncryptedPassword = "encryptedPassword";
            instance.Email = "test@example.com";

            // Act
            var retrievedInstance = UserSingleton.Instance;

            // Assert
            Assert.AreEqual("testUser", retrievedInstance.Username);
            Assert.AreEqual("encryptedPassword", retrievedInstance.EncryptedPassword);
            Assert.AreEqual("test@example.com", retrievedInstance.Email);
        }
    }
}
