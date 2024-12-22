using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace TimeManagementApp.Timer
{
    /// <summary>
    /// This page to display the pomodoro timer of the application, allow users to setting time and use the timer.
    /// </summary>
    public sealed partial class MainTimerPage : Page
    {
        public PomodoroTimer TimerViewModel { get; set; }

        public MainTimerPage()
        {
            this.InitializeComponent();
            TimerViewModel = PomodoroTimer.Instance;
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SplitView.IsPaneOpen = true;
            //MainWindow.NavigationService.Navigate(typeof(SettingsTimerPage));
        }


        public void StartButton_Click(object sender, RoutedEventArgs e)
        {
            TimerViewModel.StartTimer();
        }

        public void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            TimerViewModel.PauseTimer();
        }

        public void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            TimerViewModel.ResetTimer();
        }

        public void SkipButton_Click(object sender, RoutedEventArgs e)
        {
            TimerViewModel.SwitchToNextTimerType();
        }

        public void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            TimerViewModel.CurrentSettings.FocusTimeMinutes = (int)FocusTimeSlider.Value;
            TimerViewModel.CurrentSettings.ShortBreakMinutes = (int)ShortBreakSlider.Value;
            TimerViewModel.CurrentSettings.LongBreakMinutes = (int)LongBreakSlider.Value;

            TimerViewModel.ResetTimer();

            TimerViewModel.CurrentSettings.IsNotificationOn = NotificationToggleSwitch.IsOn;

            if (TagComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                TimerViewModel.CurrentSettings.Tag = selectedItem.Content.ToString();
            }
        }

        public void BackButton_Click(object sender, RoutedEventArgs e)
        {
            SplitView.IsPaneOpen = false;
        }
    }
}
