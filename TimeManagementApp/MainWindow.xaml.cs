using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Printing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TimeManagementApp.Note;
using TimeManagementApp.Timer;
using TimeManagementApp.ToDo;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace TimeManagementApp
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public PomodoroTimer TimerViewModel { get; set; } // use for passing timer between navigations

        public MainWindow()
        {
            this.InitializeComponent();
            SetWindowSize();
            this.Title = "Time management"; // app
            TimerViewModel = new PomodoroTimer(new Settings(), TimerType.FocusTime);
        }

        private void SetWindowSize()
        {
            var displayArea = DisplayArea.GetFromWindowId(AppWindow.Id, DisplayAreaFallback.Primary);
            var screenWidth = displayArea.WorkArea.Width;
            var screenHeight = displayArea.WorkArea.Height;

            int width = (int)(screenWidth * 0.8);
            int height = (int)(screenHeight * 0.8);

            // Center the window
            int middleX = (int)(screenWidth - width) / 2;
            int middleY = (int)(screenHeight - height) / 2;

            this.AppWindow.MoveAndResize(new Windows.Graphics.RectInt32(middleX, Math.Max(middleY - 100, 0), width, height));
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            FrameNavigationOptions navOptions = new FrameNavigationOptions();
            navOptions.TransitionInfoOverride = args.RecommendedNavigationTransitionInfo;

            if (sender.PaneDisplayMode == NavigationViewPaneDisplayMode.Top)
            {
                navOptions.IsNavigationStackEnabled = false;
            }

            Type pageType = typeof(BlankPage);
            var selectedItem = (NavigationViewItem)args.SelectedItem;


            if (selectedItem.Name == NavItem_ToDo.Name)
            {
                pageType = typeof(MainToDoPage);
            }
            if (selectedItem.Name == NavItem_Timer.Name)
            {
                pageType = typeof(MainTimerPage);
            }
            else if (selectedItem.Name == NavItem_Note.Name)
            {
                pageType = typeof(NoteMainPage);
                _ = mainFrame.Navigate(pageType);
            }
            else 
            {
                // other nav
            }

            mainFrame.NavigateToType(pageType, TimerViewModel, navOptions);
        }
    }
}
