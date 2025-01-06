using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeManagementApp.Services;

namespace UnitTest.ServicesTests
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
        public void Username_ShouldUpdateUsername()
        {
            // Arrange
            var username = "testuser";

            // Act
            UserSingleton.Instance.Username = username;

            // Assert
            Assert.AreEqual(username, UserSingleton.Instance.Username);
        }
    }
}