using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Diagnostics;
using TimeManagementApp.Note;
using TimeManagementApp.Timer;
using TimeManagementApp.ToDo;
using TimeManagementApp.Home;
using TimeManagementApp.Helper;
using TimeManagementApp.Services;
using TimeManagementApp.Settings;
using TimeManagementApp.Calendar;
using TimeManagementApp.Music;

namespace TimeManagementApp
{
    public sealed partial class MainWindow : Window
    {
        public static readonly DateTime NullDateTime = new DateTime(1999, 1, 1, 1, 1, 1).ToUniversalTime();
        public static NavigationService NavigationService { get; set; } = new NavigationService();
        public static NavigationMenuHelper NavigationMenuHelper { get; set; } = new NavigationMenuHelper();

        private bool _isFirstActivation = true;

        public MainWindow()
        {
            this.InitializeComponent();
            NavigationService.Initialize(mainFrame);
            WindowInitHelper.SetWindowSize(this);
            WindowInitHelper.SetTitle(this, "Time management");

            // Listen for visibility changes
            NavigationMenuHelper.NavigationMenuVisibilityChanged += OnNavigationMenuVisibilityChanged;

            // Listen for IsPaneOpen changes
            MainNavigationView.PaneOpened += OnPaneOpened;
            MainNavigationView.PaneClosed += OnPaneClosed;
        }

        private void OnPaneOpened(NavigationView sender, object args)
        {
            Debug.WriteLine("PaneOpened event triggered");
            UpdateNavigationViewProperties();
        }

        private void OnPaneClosed(NavigationView sender, object args)
        {
            Debug.WriteLine("PaneClosed event triggered");
            UpdateNavigationViewProperties();
        }

        private void OnNavigationMenuVisibilityChanged(object sender, EventArgs e)
        {
            Debug.WriteLine($"NavigationMenuVisibility changed: {NavigationMenuHelper.IsNavigationMenuVisible}");
            UpdateNavigationViewProperties();
        }

        private void UpdateNavigationViewProperties()
        {
            if (NavigationMenuHelper.IsNavigationMenuVisible)
            {
                // Adjust width based on whether the pane is open or closed
                MainNavigationView.Width = MainNavigationView.IsPaneOpen ? 200 : 48;
            }
            else
            {
                MainNavigationView.Width = 0;
            }

            Debug.WriteLine($"NavigationView Width: {MainNavigationView.Width}");
        }

        private void MainWindow_Activated(object sender, WindowActivatedEventArgs args)
        {
            if (_isFirstActivation)
            {
                _isFirstActivation = false;
                MainNavigationView.SelectedItem = NavItem_Home;
            }
        }

        public static string CurrentNavigationViewItem { get; set; }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                NavigationService.Navigate(typeof(MainSettingsPage));
                return;
            }

            var selectedItem = (NavigationViewItem)args.SelectedItem;

            Type pageType = typeof(BlankPage);
            if (selectedItem.Name == NavItem_Home.Name)
            {
                CurrentNavigationViewItem = "HomePage";
                pageType = typeof(HomePage);
            }
            else if (selectedItem.Name == NavItem_ToDo.Name)
            {
                CurrentNavigationViewItem = "MainToDoPage";
                pageType = typeof(MainToDoPage);
            }
            else if (selectedItem.Name == NavItem_Timer.Name)
            {
                CurrentNavigationViewItem = "MainTimerPage";
                pageType = typeof(MainTimerPage);
            }
            else if (selectedItem.Name == NavItem_Note.Name)
            {
                CurrentNavigationViewItem = "NoteMainPage";
                pageType = typeof(NoteMainPage);
            }
            else if (selectedItem.Name == NavItem_Calendar.Name)
            {
                CurrentNavigationViewItem = "CalendarPage";
                pageType = typeof(CalendarPage);
            }
            else if (selectedItem.Name == NavItem_Music.Name)
            {
                CurrentNavigationViewItem = "MusicPage";
                pageType = typeof(MusicPage);
                NavigationMenuHelper.IsNavigationMenuVisible = false;
            }

            NavigationService.Navigate(pageType);
        }
    }
}
