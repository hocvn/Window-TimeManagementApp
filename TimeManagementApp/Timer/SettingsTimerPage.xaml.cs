using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TimeManagementApp.ToDo;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace TimeManagementApp.Timer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsTimerPage : Page
    {
        public PomodoroTimer TimerViewModel { get; set; }

        public SettingsTimerPage()
        {
            this.InitializeComponent();
            TimerViewModel = PomodoroTimer.Instance;
        }


        // currently save settings on hard code, will save to files later
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


        // back to MainTimerPage
        public void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.NavigationService.Navigate(typeof(MainTimerPage));
        }
    }
}
