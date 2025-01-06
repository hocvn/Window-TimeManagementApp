using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeManagementApp.Timer;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;

namespace UnitTest.TimerTests
{
    [TestClass]
    public class SettingsTests
    {
        [UITestMethod]
        public void Constructor_DefaultSettings_ShouldBeCorrect()
        {
            var settings = new Settings();

            Assert.AreEqual(25, settings.FocusTimeMinutes);
            Assert.AreEqual(5, settings.ShortBreakMinutes);
            Assert.AreEqual(10, settings.LongBreakMinutes);
            Assert.IsTrue(settings.IsNotificationOn);
            Assert.AreEqual("Studying", settings.Tag);
        }

        [UITestMethod]
        public void PropertyChanged_FocusTimeMinutes_ShouldRaisePropertyChanged()
        {
            var settings = new Settings();
            bool propertyChangedRaised = false;

            settings.PropertyChanged += (sender, args) => {
                if (args.PropertyName == nameof(Settings.FocusTimeMinutes))
                {
                    propertyChangedRaised = true;
                }
            };

            settings.FocusTimeMinutes = 30;

            Assert.IsTrue(propertyChangedRaised);
        }

        [UITestMethod]
        public void PropertyChanged_SettingProperties_ShouldUpdateValues()
        {
            var settings = new Settings();

            settings.FocusTimeMinutes = 40;
            settings.ShortBreakMinutes = 10;
            settings.LongBreakMinutes = 20;
            settings.IsNotificationOn = false;
            settings.Tag = "Working";

            Assert.AreEqual(40, settings.FocusTimeMinutes);
            Assert.AreEqual(10, settings.ShortBreakMinutes);
            Assert.AreEqual(20, settings.LongBreakMinutes);
            Assert.IsFalse(settings.IsNotificationOn);
            Assert.AreEqual("Working", settings.Tag);
        }
    }
}