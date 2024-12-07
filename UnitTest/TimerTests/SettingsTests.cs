using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel;
using TimeManagementApp.Timer;

namespace UnitTest
{
    [TestClass]
    public class SettingsTests
    {
        private Settings settings;

        [TestInitialize]
        public void TestInitialize()
        {
            settings = new Settings();
        }

        [TestMethod]
        public void Settings_DefaultConstructor_ShouldInitializeProperties()
        {
            // Assert
            Assert.AreEqual(25, settings.FocusTimeMinutes);
            Assert.AreEqual(5, settings.ShortBreakMinutes);
            Assert.AreEqual(10, settings.LongBreakMinutes);
            Assert.IsTrue(settings.IsNotificationOn);
            Assert.AreEqual("Studying", settings.Tag);
        }

        [TestMethod]
        public void Settings_SetProperties_ShouldRaisePropertyChangedEvent()
        {
            // Arrange
            bool eventRaised = false;
            settings.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(settings.FocusTimeMinutes))
                {
                    eventRaised = true;
                }
            };

            // Act
            settings.FocusTimeMinutes = 30;

            // Assert
            Assert.IsTrue(eventRaised);
        }

        [TestMethod]
        public void Settings_ShouldSetPropertiesCorrectly()
        {
            // Act
            settings.FocusTimeMinutes = 30;
            settings.ShortBreakMinutes = 10;
            settings.LongBreakMinutes = 20;
            settings.IsNotificationOn = false;
            settings.Tag = "Work";

            // Assert
            Assert.AreEqual(30, settings.FocusTimeMinutes);
            Assert.AreEqual(10, settings.ShortBreakMinutes);
            Assert.AreEqual(20, settings.LongBreakMinutes);
            Assert.IsFalse(settings.IsNotificationOn);
            Assert.AreEqual("Work", settings.Tag);
        }
    }
}
