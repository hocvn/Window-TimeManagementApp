using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using TimeManagementApp.Note;
using TimeManagementApp.Timer;
using TimeManagementApp.ToDo;
using TimeManagementApp.Home;
using TimeManagementApp.Helper;
using TimeManagementApp.Services;
using Windows.UI.ApplicationSettings;
using TimeManagementApp.Settings;
using TimeManagementApp.Calendar;
using TimeManagementApp.Global;

namespace TimeManagementApp
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public static readonly DateTime NullDateTime = new DateTime(1999, 1, 1, 1, 1, 1).ToUniversalTime();

        public static NavigationService NavigationService { get; set; } = new NavigationService();
        private bool _isFirstActivation = true;

        public MainWindow()
        {
            this.InitializeComponent();

            NavigationService.Initialize(mainFrame);
            WindowInitHelper.SetWindowSize(this);
            WindowInitHelper.SetTitle(this, "Time management");
        }

        private void MainWindow_Activated(object sender, WindowActivatedEventArgs args)
        {
            if (_isFirstActivation)
            {
                _isFirstActivation = false;
                MainNavigationView.SelectedItem = NavItem_Home;
            }
        }


        // decide which page to go back when the GoBack() not work as expected
        public static string CurrentNavigationViewItem { get; set; }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                NavigationService.Navigate(typeof(MainSettingsPage));
                return;
            }

            FrameNavigationOptions navOptions = new();
            navOptions.TransitionInfoOverride = args.RecommendedNavigationTransitionInfo;

            Type pageType = typeof(BlankPage);
            var selectedItem = (NavigationViewItem)args.SelectedItem;

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
            else
            {
                // other nav
            }

            NavigationService.Navigate(pageType);
        }

    }
}