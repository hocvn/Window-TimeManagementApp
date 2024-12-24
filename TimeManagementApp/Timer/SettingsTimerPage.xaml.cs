using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;


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
            if (MainWindow.NavigationService.CanGoBack())
            {
                MainWindow.NavigationService.GoBack();
            }
            else
            {
                MainWindow.NavigationService.Navigate(typeof(MainTimerPage));
            }
        }
    }
}