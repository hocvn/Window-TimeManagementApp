using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using TimeManagementApp.Dao;

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
            IDao dao = new MockDao();
            dao.SaveSelectedBackground(selectedBrush);

            // reload the background
            App.BackgroundViewModel.PageBackgroundBrush = dao.LoadSavedBackground(0.0, 8.0);
            App.BackgroundViewModel.NavigationViewBackgroundBrush = dao.LoadSavedBackground(0.0, 2.5);
        }
    }

}
