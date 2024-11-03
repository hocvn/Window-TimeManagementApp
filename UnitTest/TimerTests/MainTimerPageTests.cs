using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using Microsoft.UI.Xaml.Controls;
using TimeManagementApp.Timer;
using Microsoft.UI.Xaml;

namespace UnitTest
{
    [TestClass]
    public class MainTimerPageTests
    {
        private PomodoroTimer viewModel;
        private MainTimerPage page;

        [TestInitialize]
        public void TestInitialize()
        {
            var settings = new Settings
            {
                FocusTimeMinutes = 25,
                ShortBreakMinutes = 5,
                LongBreakMinutes = 15,
                IsNotificationOn = false
            };
            viewModel = new PomodoroTimer(settings, TimerType.FocusTime);
            page = new MainTimerPage { ViewModel = viewModel };
            page.InitializeComponent();
        }

        [UITestMethod]
        public void StartButton_Click_ShouldStartTimer()
        {
            // Arrange
            var startButton = (AppBarButton)page.FindName("StartButton");

            // Act
            page.StartButton_Click(startButton, new RoutedEventArgs());

            // Assert
            Assert.IsTrue(viewModel.IsRunning);
        }

        [UITestMethod]
        public void PauseButton_Click_ShouldPauseTimer()
        {
            // Arrange
            var pauseButton = (AppBarButton)page.FindName("PauseButton");

            // Act
            page.PauseButton_Click(pauseButton, new RoutedEventArgs());

            // Assert
            Assert.IsFalse(viewModel.IsRunning);
        }

        [UITestMethod]
        public void ResetButton_Click_ShouldResetTimer()
        {
            // Arrange
            var resetButton = (AppBarButton)page.FindName("ResetButton");

            // Act
            page.ResetButton_Click(resetButton, new RoutedEventArgs());

            // Assert
            Assert.AreEqual(25, viewModel.CurrentSettings.FocusTimeMinutes); // Assuming default FocusTimeMinutes is 25
            Assert.AreEqual(0, viewModel.Seconds);
        }

        [UITestMethod]
        public void SkipButton_Click_ShouldSwitchTimerType()
        {
            // Arrange
            var skipButton = (AppBarButton)page.FindName("SkipButton");

            // Act
            page.SkipButton_Click(skipButton, new RoutedEventArgs());

            // Assert
            Assert.AreEqual(TimerType.ShortBreak, viewModel.CurrentType); // Assuming initial type is FocusTime
        }

        [UITestMethod]
        public void SaveButton_Click_ShouldUpdateSettings()
        {
            // Arrange
            var saveButton = (AppBarButton)page.FindName("SaveButton");
            var slider = (Slider)page.FindName("FocusTimeSlider");

            // Act
            slider.Value = 30;
            page.SaveButton_Click(saveButton, new RoutedEventArgs());

            // Assert
            Assert.AreEqual(30, viewModel.CurrentSettings.FocusTimeMinutes);
        }
    }
}
