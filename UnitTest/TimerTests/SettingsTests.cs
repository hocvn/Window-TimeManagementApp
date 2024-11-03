using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeManagementApp.Timer;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;

namespace UnitTest
{
    [TestClass]
    public class SettingsTests
    {
        [UITestMethod]
        public void DefaultSettings_ShouldBeCorrect()
        {
            var settings = new Settings();

            Assert.AreEqual(25, settings.FocusTimeMinutes);
            Assert.AreEqual(5, settings.ShortBreakMinutes);
            Assert.AreEqual(10, settings.LongBreakMinutes);
            Assert.IsTrue(settings.IsNotificationOn);
        }

        [UITestMethod]
        public void SettingProperties_ShouldRaisePropertyChanged()
        {
            var settings = new Settings();
            bool propertyChangedRaised = false;

            settings.PropertyChanged += (sender, args) => {
                if (args.PropertyName == "FocusTimeMinutes")
                {
                    propertyChangedRaised = true;
                }
            };

            settings.FocusTimeMinutes = 30;

            Assert.IsTrue(propertyChangedRaised);
        }

        [UITestMethod]
        public void SettingProperties_ShouldUpdateValues()
        {
            var settings = new Settings();

            settings.FocusTimeMinutes = 40;
            settings.ShortBreakMinutes = 10;
            settings.LongBreakMinutes = 20;
            settings.IsNotificationOn = false;

            Assert.AreEqual(40, settings.FocusTimeMinutes);
            Assert.AreEqual(10, settings.ShortBreakMinutes);
            Assert.AreEqual(20, settings.LongBreakMinutes);
            Assert.IsFalse(settings.IsNotificationOn);
        }
    }
}
