﻿using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.Windows.AppNotifications;
using System;
using System.Collections.Generic;
using System.Globalization;
using TimeManagementApp.Global;
using TimeManagementApp.Helper;

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

        private static Window _window { get; set; } // Controll current window

        public static BackgroundViewModel BackgroundViewModel { get; private set; } = new BackgroundViewModel();

        /// <summary>
        /// Invoked when the Time Management application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            // Enable Prelaunch - this will allow the app to run in the background
            Windows.ApplicationModel.Core.CoreApplication.EnablePrelaunch(true);

            // Set the language
            string code = StorageHelper.GetSetting("language");
            code ??= "en-US";
            Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = code;
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(code);
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(code);

            // Set startup window
            _window = new LoginWindow();

            AppNotificationManager.Default.NotificationInvoked += NotificationManager_NotificationInvoked;
            AppNotificationManager.Default.Register();

            _window.Activate();
        }

        public static void SwitchLocalization(string code)
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(code);
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(code);
            Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = code;
            StorageHelper.SaveSetting("language", code);

            UpdateUiStrings();
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

        public static void UpdateUiStrings()
        {
            var rootFrame = _window.Content as FrameworkElement;

            if (rootFrame != null)
            {
                foreach (var element in EnumerateVisualTree(rootFrame))
                {
                    if (element is FrameworkElement frameworkElement)
                    {
                        frameworkElement.Language = Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride;
                        frameworkElement.UpdateLayout();
                    }
                }
            }
        }

        private static IEnumerable<DependencyObject> EnumerateVisualTree(DependencyObject root)
        {
            for (int i = 0, count = VisualTreeHelper.GetChildrenCount(root); i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(root, i);
                yield return child;

                foreach (var descendant in EnumerateVisualTree(child))
                {
                    yield return descendant;
                }
            }
        }


    }
}
