using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

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
            ViewModel = App.TimerViewModel;
            DataContext = ViewModel;
        }

        // passing view model between navigations,
        // so that timer can still run & notify when we are working on other features
        //protected override void OnNavigatedTo(NavigationEventArgs e) 
        //{ 
        //    if (e.Parameter is PomodoroTimer viewModel) 
        //    { 
        //        ViewModel = viewModel; 
        //        DataContext = ViewModel; 
        //    }
            
        //    base.OnNavigatedTo(e); 
        //}

        // open settings panel
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
            ViewModel.CurrentSettings.FocusTimeMinutes = (int)FocusTimeSlider.Value;
            ViewModel.CurrentSettings.ShortBreakMinutes = (int)ShortBreakSlider.Value;
            ViewModel.CurrentSettings.LongBreakMinutes = (int)LongBreakSlider.Value;

            ViewModel.ResetTimer();

            ViewModel.CurrentSettings.IsNotificationOn = NotificationToggleSwitch.IsOn;
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
