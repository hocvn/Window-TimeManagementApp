using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.ComponentModel;

namespace TimeManagementApp.Timer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainTimerPage : Page, INotifyPropertyChanged
    {
        public PomodoroTimer ViewModel { get; set; }
        public string TotalFocusTime { get; set; }

        public MainTimerPage()
        {
            this.InitializeComponent();
            ViewModel = PomodoroTimer.Instance;

            ViewModel.Tag = "Studying";
            TagComboBox.SelectedIndex = 1;

            ViewModel.TimerEnded += OnTimerEnded;

            RefreshStatistics();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnTimerEnded(object sender, EventArgs e)
        {
            RefreshStatistics();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            if (SettingsPanel.Visibility == Visibility.Collapsed)
            {
                SettingsPanel.Visibility = Visibility.Visible;
                TimerPanel.Margin = new Thickness(-200, 0, 0, 0);
            }
        }


        // currently save settings on hard code, will save to files later
        public void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (TagComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                ViewModel.Tag = selectedItem.Content.ToString();
            }


            ViewModel.CurrentSettings.FocusTimeMinutes = (int)FocusTimeSlider.Value;
            ViewModel.CurrentSettings.ShortBreakMinutes = (int)ShortBreakSlider.Value;
            ViewModel.CurrentSettings.LongBreakMinutes = (int)LongBreakSlider.Value;

            ViewModel.ResetTimer();

            ViewModel.CurrentSettings.IsNotificationOn = NotificationToggleSwitch.IsOn;

            RefreshStatistics();
        }

        // close settings panel
        public void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsPanel.Visibility = Visibility.Collapsed;
            TimerPanel.Margin = new Thickness(0, 0, 0, 0);
        }

        // start, pause and reset timer

        public void StartButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.StartTimer();
        }

        public void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.PauseTimer();
        }

        public void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ResetTimer();
        }

        // skip session
        public void SkipButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SwitchToNextTimerType();
        }


        // refresh the statistics when saving settings or first navigating to timer page
        public void RefreshStatistics()
        {
            var totalFocusedTime = ViewModel.GetTotalFocusedTime(ViewModel.Tag);
            TotalFocusTime = $"Total Focus Time for {ViewModel.Tag}: {totalFocusedTime}";
        }
    }
}
