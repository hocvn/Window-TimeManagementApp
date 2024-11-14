using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
                //TimerPanel.Margin = new Thickness(-200, 0, 0, 0);
            }
        }

        private void StatisticsButton_Click(object sender, RoutedEventArgs e)
        {
            if (StatisticsPanel.Visibility == Visibility.Collapsed)
            {
                StatisticsTextBlock.Text = ReadStatistics();
                StatisticsPanel.Visibility = Visibility.Visible;
            }
            else
            {
                StatisticsPanel.Visibility = Visibility.Collapsed;
            }
        }


        private string ReadStatistics()
        {
            var tags = new List<string> { "Working", "Studying", "Reading" };
            var statsBuilder = new StringBuilder();

            var directory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            var filePath = Path.Combine(directory.FullName, "sessions.xlsx");
            IDao dao = new ExcelDao(filePath);

            foreach (var tag in tags)
            {
                var focusTimeLastMonth = CalculateFocusTimeForTag(dao.GetAllSessionsWithTag(tag), TimeSpan.FromDays(30));
                var focusTimeLastWeek = CalculateFocusTimeForTag(dao.GetAllSessionsWithTag(tag), TimeSpan.FromDays(7));
                var focusTimeLastDay = CalculateFocusTimeForTag(dao.GetAllSessionsWithTag(tag), TimeSpan.FromDays(1));

                statsBuilder.AppendLine($"Total Focus Time for {tag} in the last month: {focusTimeLastMonth}");
                statsBuilder.AppendLine($"Total Focus Time for {tag} in the last week: {focusTimeLastWeek}");
                statsBuilder.AppendLine($"Total Focus Time for {tag} in the last day: {focusTimeLastDay}");
                statsBuilder.AppendLine();
            }

            return statsBuilder.ToString();
        }

        private TimeSpan CalculateFocusTimeForTag(List<FocusSession> sessions, TimeSpan timeFrame)
        {
            DateTime cutoffDate;

            try
            {
                cutoffDate = DateTime.UtcNow - timeFrame;
            }
            catch (ArgumentOutOfRangeException)
            {
                // If the timeFrame is too large, set cutoffDate to DateTime.MinValue
                cutoffDate = DateTime.MinValue;
            }

            var totalFocusTime = new TimeSpan();

            foreach (var session in sessions)
            {
                if (session.Timestamp >= cutoffDate)
                {
                    totalFocusTime += session.Duration;
                }
            }

            return totalFocusTime;
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
