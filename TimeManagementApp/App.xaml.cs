using Microsoft.UI.Xaml;
using Microsoft.Windows.AppNotifications;
using System;

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

        private static Window _window { get; set; }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            _window = new LoginWindow();

            AppNotificationManager.Default.NotificationInvoked += NotificationManager_NotificationInvoked;
            AppNotificationManager.Default.Register();

            _window.Activate();
        }

        public static void NavigateWindow(Window window)
        {
            window.Activate();
            _window.Close();
            _window = window;
        }
        public static void CloseWindow()
        {
            _window.Close();
        }

        private void NotificationManager_NotificationInvoked(AppNotificationManager sender, AppNotificationActivatedEventArgs args)
        {
            throw new NotImplementedException();
        }
    }
}
