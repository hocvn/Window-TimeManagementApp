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

namespace TimeManagementApp
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public static NavigationService NavigationService { get; set; } = new NavigationService();
        private bool _isFirstActivation = true;

        public MainWindow()
        {
            this.InitializeComponent();

            NavigationService.Initialize(mainFrame);
            WindowInitHelper.SetWindowSize(this);
            WindowInitHelper.SetTitle(this, "Time management");
        }


        // fix a bug when MainWindow lost focus, it navigates back to HomePage
        private void MainWindow_Activated(object sender, WindowActivatedEventArgs args)
        {
            if (_isFirstActivation)
            {
                _isFirstActivation = false;
                MainNavigationView.SelectedItem = NavItem_Home;
            }
        }


        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            FrameNavigationOptions navOptions = new(); 
            navOptions.TransitionInfoOverride = args.RecommendedNavigationTransitionInfo;

            Type pageType = typeof(BlankPage);
            var selectedItem = (NavigationViewItem)args.SelectedItem;

            if (selectedItem.Name == NavItem_Home.Name)
            {
                pageType = typeof(HomePage);
            }
            else if (selectedItem.Name == NavItem_ToDo.Name)
            {
                pageType = typeof(MainToDoPage);
            }
            else if (selectedItem.Name == NavItem_Timer.Name)
            {
                pageType = typeof(MainTimerPage);
            }
            else if (selectedItem.Name == NavItem_Note.Name)
            {
                pageType = typeof(NoteMainPage);
            }
            else
            {
                // other nav
            }
            NavigationService.Navigate(pageType);
        }
    }
}
