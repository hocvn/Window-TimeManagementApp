using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using TimeManagementApp.Dao;
using TimeManagementApp.ToDo;
using Windows.Foundation;

namespace TimeManagementApp.Timer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
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
            MainWindow.NavigationService.Navigate(typeof(SettingsTimerPage));
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
    }
}
