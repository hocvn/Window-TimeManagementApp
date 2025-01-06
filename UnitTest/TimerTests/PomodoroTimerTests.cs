using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeManagementApp.Timer;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using System.Threading;

namespace UnitTest.TimerTests
{
    [TestClass]
    public class PomodoroTimerTests
    {
        [UITestMethod]
        public void StartTimer_WhenCalled_ShouldSetIsRunningToTrue()
        {
            var timer = PomodoroTimer.Instance;
            timer.ResetTimer(); // Ensure timer is reset before starting
            timer.StartTimer();

            Assert.IsTrue(timer.IsRunning);
        }

        [UITestMethod]
        public void PauseTimer_WhenCalled_ShouldSetIsRunningToFalse()
        {
            var timer = PomodoroTimer.Instance;
            timer.ResetTimer(); // Ensure timer is reset before starting
            timer.StartTimer();
            timer.PauseTimer();

            Assert.IsFalse(timer.IsRunning);
        }

        [UITestMethod]
        public void ResetTimer_WhenCalled_ShouldSetRemainingTimeToFocusTimeMinutes()
        {
            var timer = PomodoroTimer.Instance;
            timer.ResetTimer();

            Assert.AreEqual(timer.CurrentSettings.FocusTimeMinutes, timer.Minutes);
            Assert.AreEqual(0, timer.Seconds);
            Assert.IsFalse(timer.IsRunning);
        }

        [UITestMethod]
        public void SwitchToNextTimerType_WhenCalled_ShouldChangeTimerType()
        {
            var timer = PomodoroTimer.Instance;
            timer.ResetTimer(); // Ensure timer is reset before starting
            var initialType = timer.CurrentType;

            timer.SwitchToNextTimerType();

            Assert.AreNotEqual(initialType, timer.CurrentType);
        }
    }
}