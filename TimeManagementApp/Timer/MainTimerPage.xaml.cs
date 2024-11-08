using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using TimeManagementApp.Dao;
using Windows.Foundation;

namespace TimeManagementApp.Timer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainTimerPage : Page
    {
        public PomodoroTimer ViewModel { get; set; }

        public MainTimerPage()
        {
            this.InitializeComponent();
            ViewModel = PomodoroTimer.Instance;
        }


        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            if (SettingsPanel.Visibility == Visibility.Collapsed)
            {
                SettingsPanel.Visibility = Visibility.Visible;
                TimerPanel.Margin = new Thickness(-200, 0, 0, 0);
            }
        }

        private void StatisticsButton_Click(object sender, RoutedEventArgs e)
        {
            if (StatisticsPanel.Visibility == Visibility.Collapsed)
            {
                StatisticsTextBlock.Text = ReadStatisticsFromLocalSettings();
                StatisticsPanel.Visibility = Visibility.Visible;
            }
            else
            {
                StatisticsPanel.Visibility = Visibility.Collapsed;
            }
        }

        private string ReadStatisticsFromLocalSettings()
        {
            var tags = new List<string> { "Working", "Studying", "Reading" };
            var statsBuilder = new StringBuilder();

            IDao _dao = new LocalSettingsDao();

            foreach (var tag in tags)
            {
                var focusTime = _dao.LoadTimeSpan($"totalFocusedTime_{tag}");
                statsBuilder.AppendLine($"Total Focus Time for {tag}: {focusTime}");
            }

            return statsBuilder.ToString();
        }


        // currently save settings on hard code, will save to files later
        public void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.CurrentSettings.FocusTimeMinutes = (int)FocusTimeSlider.Value;
            ViewModel.CurrentSettings.ShortBreakMinutes = (int)ShortBreakSlider.Value;
            ViewModel.CurrentSettings.LongBreakMinutes = (int)LongBreakSlider.Value;

            ViewModel.ResetTimer();

            ViewModel.CurrentSettings.IsNotificationOn = NotificationToggleSwitch.IsOn;

            if (TagComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                ViewModel.CurrentSettings.Tag = selectedItem.Content.ToString();
            }
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
    }
}
