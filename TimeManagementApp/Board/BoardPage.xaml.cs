using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Syncfusion.UI.Xaml.Kanban;

namespace TimeManagementApp.Board
{
    public sealed partial class BoardPage : Page
    {
        public BoardPage()
        {
            this.InitializeComponent();
        }

        private void MainGrid_Loaded(object sender, RoutedEventArgs e)
        {
            SetColumnWidths();
        }

        private void MainGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetColumnWidths();
        }

        private void SetColumnWidths()
        {
            // Get the width of the grid
            double gridWidth = MainGrid.ActualWidth;

            // Calculate the total gap width
            double totalGapWidth = 6 * (kanban.Columns.Count - 1);

            // Calculate the width of each column
            double columnWidth = (gridWidth - totalGapWidth) / kanban.Columns.Count;

            // Set the width of each column
            kanban.ColumnWidth = columnWidth;
        }
    }
}