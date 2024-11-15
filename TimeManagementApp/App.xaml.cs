using Microsoft.UI.Xaml;
using Microsoft.Windows.AppNotifications;
using System;
using TimeManagementApp.Global;
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

        public App()
        {
            this.InitializeComponent();
        }

        public static Window LoginWindow { get; private set; }
        public static Window RegisterWindow { get; private set; }
        public static Window MainWindow { get; private set; }
        public static Window ForgotPasswordWindow { get; private set; }

        public static BackgroundViewModel BackgroundViewModel { get; private set; } = new BackgroundViewModel();


        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            LoginWindow = new LoginWindow();

            AppNotificationManager.Default.NotificationInvoked += NotificationManager_NotificationInvoked;
            AppNotificationManager.Default.Register();

            LoginWindow.Activate();
        }

        private void NotificationManager_NotificationInvoked(AppNotificationManager sender, AppNotificationActivatedEventArgs args)
        {
            throw new NotImplementedException();
        }
    }
}
