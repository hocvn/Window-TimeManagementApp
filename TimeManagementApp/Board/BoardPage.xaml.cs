using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Syncfusion.UI.Xaml.Kanban;
using System.Diagnostics;
using System.Threading.Tasks;
using TimeManagementApp.Dao;
using TimeManagementApp.ToDo;

namespace TimeManagementApp.Board
{
    public sealed partial class BoardPage : Page
    {
        public MyTask CurrentDraggingTask { get; set; }

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

        private void OnKanbanCardTapped(object sender, KanbanCardTappedEventArgs e)
        {
            var selectedCard = e.SelectedCard.Content as KanbanModel;
            
            if (selectedCard != null)
            {
                var selectedTaskId = (int)selectedCard.Id;

                IDao dao = new SqlDao();
                var selectedTask = dao.GetTaskById(selectedTaskId);

                MainWindow.NavigationService.Navigate(typeof(EditToDoPage), selectedTask);
            }
        }

        private void OnKanbanCardDragStarting(object sender, KanbanCardDragStartingEventArgs e)
        {
            var draggingCard = e.Card.Content as KanbanModel;

            if (draggingCard != null)
            {
                var draggingTaskId = (int)draggingCard.Id;

                IDao dao = new SqlDao();
                var draggingTask = dao.GetTaskById(draggingTaskId);

                CurrentDraggingTask = draggingTask;
            }
        }

        private void OnKanbanCardDrop(object sender, KanbanCardDropEventArgs e)
        {
            var targetStatus = e.TargetColumn.Categories.ToString();
            var targetSwimlane = e.TargetSwimlane.ToString();

            CurrentDraggingTask.Status = targetStatus;
            CurrentDraggingTask.IsCompleted = (targetStatus == "Completed");
            CurrentDraggingTask.IsImportant = (targetSwimlane == "Important");

            MyTaskViewModel.Instance.UpdateTask(CurrentDraggingTask);
        }
    }
}