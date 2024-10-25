using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

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

            ViewModel = new PomodoroTimer(new Settings(), TimerType.FocusTime);
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            if (SettingsPanel.Visibility == Visibility.Collapsed)
            {
                SettingsPanel.Visibility = Visibility.Visible;
                TimerPanel.Margin = new Thickness(-200, 0, 0, 0);
            }
            else
            {
                SettingsPanel.Visibility = Visibility.Collapsed;
                TimerPanel.Margin = new Thickness(0, 0, 0, 0);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.CurrentSettings.FocusTimeMinutes = (int)FocusTimeSlider.Value;
            ViewModel.CurrentSettings.ShortBreakMinutes = (int)ShortBreakSlider.Value;
            ViewModel.CurrentSettings.LongBreakMinutes = (int)LongBreakSlider.Value;

            ViewModel.ResetTimer();

            // TODO: Dark Mode, Sound, other settings ...
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.StartTimer();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.PauseTimer();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ResetTimer();
        }

        private void SkipButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SwitchToNextTimerType();
        }
    }
}
