using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using TimeManagementApp.Note;
using TimeManagementApp.Timer;
using TimeManagementApp.ToDo;
using TimeManagementApp.Home;
using TimeManagementApp.Helper;
using TimeManagementApp.Services;
using TimeManagementApp.Settings;
using TimeManagementApp.Calendar;
using TimeManagementApp.Music;
using TimeManagementApp.Statistics;
using TimeManagementApp.Board;

namespace TimeManagementApp
{
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

        public void OpenNavPane()
        {
            MainNavigationView.IsPaneVisible = true;
        }

        public void HideNavPane()
        {
            MainNavigationView.IsPaneVisible = false;
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
            }
            else if (selectedItem.Name == NavItem_Statistics.Name)
            {
                CurrentNavigationViewItem = "StatisticsPage";
                pageType = typeof(StatisticsPage);
            }
            else if (selectedItem.Name == NavItem_Board.Name)
            {
                CurrentNavigationViewItem = "BoardPage";
                pageType = typeof(BoardPage);
            }

            NavigationService.Navigate(pageType);
        }
    }
}
