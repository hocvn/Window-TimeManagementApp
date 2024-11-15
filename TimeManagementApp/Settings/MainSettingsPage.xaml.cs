using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using TimeManagementApp.Dao;
using TimeManagementApp.Global;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace TimeManagementApp.Settings
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainSettingsPage : Page
    {
        public MainSettingsPage()
        {
            this.InitializeComponent();

            BrushesViewModel = new BrushesViewModel();
        }

        public BrushesViewModel BrushesViewModel { get; set; }


        private void BackgroundGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var gridView = sender as GridView;
            var selectedBrush = gridView.SelectedItem as LinearGradientBrush;

            // save to local settings
            IDao dao = new LocalSettingsDao();
            dao.SaveSelectedBackground(selectedBrush);

            // reload the background
            App.BackgroundViewModel.PageBackgroundBrush = dao.LoadSavedBackground(0.0, 8.0);
            App.BackgroundViewModel.NavigationViewBackgroundBrush = dao.LoadSavedBackground(0.0, 2.5);
        }
    }

}
