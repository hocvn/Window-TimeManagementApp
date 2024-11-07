using Microsoft.UI.Xaml;
using TimeManagementApp.Timer;

namespace TimeManagementApp
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        /// 

        // public static PomodoroTimer TimerViewModel { get; private set; } // TimerViewModel Singleton
        public App()
        {
            this.InitializeComponent();
            // TimerViewModel = new PomodoroTimer(new Settings(), TimerType.FocusTime);
        }

        public static Window LoginWindow { get; private set; }
        public static Window RegisterWindow { get; private set; }
        public static Window MainWindow { get; private set; }
        public static Window ForgotPasswordWindow { get; private set; }



        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            LoginWindow = new LoginWindow();
            LoginWindow.Activate();
        }
    }
}
