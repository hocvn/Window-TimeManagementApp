using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeManagementApp.Timer;
using System;
using Microsoft.UI.Xaml;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;

namespace UnitTest
{
    [TestClass]
    public class PomodoroTimerTests
    {
        private Settings settings;

        [TestInitialize]
        public void TestInitialize()
        {
            settings = new Settings
            {
                FocusTimeMinutes = 25,
                ShortBreakMinutes = 5,
                LongBreakMinutes = 15,
                IsNotificationOn = false
            };
        }

        [UITestMethod]
        public void StartTimer_ShouldSetIsRunningToTrue()
        {
            var timer = new PomodoroTimer(settings, TimerType.FocusTime);

            timer.StartTimer();

            Assert.IsTrue(timer.IsRunning);
        }

        [UITestMethod]
        public void PauseTimer_ShouldSetIsRunningToFalse()
        {
            var timer = new PomodoroTimer(settings, TimerType.FocusTime);

            timer.StartTimer();
            timer.PauseTimer();

            Assert.IsFalse(timer.IsRunning);
        }

        [UITestMethod]
        public void ResetTimer_ShouldSetTimerToFocusTime()
        {
            var timer = new PomodoroTimer(settings, TimerType.FocusTime);

            timer.ResetTimer();

            Assert.AreEqual(25, timer.Minutes);
            Assert.AreEqual(0, timer.Seconds);
            Assert.AreEqual(TimerType.FocusTime, timer.CurrentType);
        }

        [UITestMethod]
        public void SwitchToNextTimerType_ShouldSwitchToShortBreak()
        {
            var timer = new PomodoroTimer(settings, TimerType.FocusTime);

            timer.SwitchToNextTimerType();

            Assert.AreEqual(TimerType.ShortBreak, timer.CurrentType);
        }

        [UITestMethod]
        public void Timer_Tick_ShouldDecreaseSeconds()
        {
            var timer = new PomodoroTimer(settings, TimerType.FocusTime);

            timer.StartTimer();
            // Simulate one tick
            for (int i = 0; i < 1; i++)
            {
                timer.GetType().GetMethod("Timer_Tick", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(timer, new object[] { null, null });
            }

            Assert.AreEqual(59, timer.Seconds);
        }
    }
}
