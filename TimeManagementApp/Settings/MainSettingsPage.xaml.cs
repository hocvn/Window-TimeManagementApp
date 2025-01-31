using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using TimeManagementApp.Account;
using TimeManagementApp.Dao;
using TimeManagementApp.Helper;

namespace TimeManagementApp.Settings
{
    /// <summary>
    /// This page is used to display and change the settings of the application.
    /// </summary>
    public sealed partial class MainSettingsPage : Page
    {
        public MainSettingsPage()
        {
            this.InitializeComponent();

            BrushesViewModel = new BrushesViewModel();

            if (ResourcesHelper.GetLanguage() == "en")
            {
                CountryComboBox.SelectedIndex = 0;
            }
            else
            {
                CountryComboBox.SelectedIndex = 1;
            }
        }

        public BrushesViewModel BrushesViewModel { get; set; }

        private void BackgroundGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var gridView = sender as GridView;
            var selectedBrush = gridView.SelectedItem as LinearGradientBrush;

            // save to local settings
            IDao dao = new MockDao();
            dao.SaveSelectedBackground(selectedBrush);

            // reload the background
            App.BackgroundViewModel.PageBackgroundBrush = dao.LoadSavedBackground(0.0, 8.0);
            App.BackgroundViewModel.NavigationViewBackgroundBrush = dao.LoadSavedBackground(0.0, 2.5);
        }

        private void Country_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox CountryComboBox = sender as ComboBox;
            if (CountryComboBox.SelectedItem != null)
            {
                var selectedCountry = CountryComboBox.SelectedItem as ComboBoxItem;
                if (selectedCountry.Content.ToString() == "Vietnam")
                {
                    App.SwitchLocalization("vi-VN");
                }
                else
                {
                    App.SwitchLocalization("en-US");
                }
            }
        }

        private void OpenLinkButton_Click(object sender, RoutedEventArgs e)
        {
            var uri = new Uri("https://sites.google.com/view/time-management-app");
            _ = Windows.System.Launcher.LaunchUriAsync(uri);
        }

        private void Account_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.NavigationService.Navigate(typeof(AccountPage), null);
        }
    }
}
