using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using TimeManagementApp.Helper;


namespace TimeManagementApp.ToDo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainToDoPage : Page
    {
        public MyTaskViewModel ViewModel { get; set; }

        /// <summary>
        /// Initializes a new instance of the MainToDoPage class.
        /// </summary>
        public MainToDoPage()
        {
            this.InitializeComponent();
            ViewModel = MyTaskViewModel.Instance;
        }

        /// <summary>
        /// Navigate to the EditToDoPage when a task is tapped.
        /// </summary>
        private void TaskItem_Click(object sender, ItemClickEventArgs e)
        {
            var selectedTask = e.ClickedItem as MyTask;
            MainWindow.NavigationService.Navigate(typeof(EditToDoPage), selectedTask.Clone());
        }

        /// <summary>
        /// Update the task when navigating back from the EditToDoPage.
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is MyTask task)
            {
                ViewModel.UpdateTask(task);
            }

            base.OnNavigatedTo(e);
        }


        /// <summary>
        /// Insert a new task when the Enter key is pressed in the text box.
        /// </summary>
        private async void InsertTask_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                if (String.IsNullOrEmpty(InsertTaskName.Text))
                {
                    await Dialog.ShowContent(this.XamlRoot, "Error", "Task Name cannot be empty!", null, null, "OK");
                    return;
                }

                if (!InsertTaskDueDateTime.Date.HasValue)
                {
                    await Dialog.ShowContent(this.XamlRoot, "Error", "Date cannot be empty!", null, null, "OK");
                    return;
                }

                var startDateTime = DateTime.Now;

                // Set the due date to the end of the day
                var dueDateTime = new DateTime(
                    InsertTaskDueDateTime.Date.Value.Year, InsertTaskDueDateTime.Date.Value.Month, InsertTaskDueDateTime.Date.Value.Day,
                    23, 59, 00
                );

                var newTask = new MyTask
                {
                    TaskName = InsertTaskName.Text,
                    DueDateTime = dueDateTime,
                    IsCompleted = false,
                    IsImportant = false,
                    ReminderTime = MainWindow.NullDateTime,
                    Description = "",
                    NoteId = -1,
                    RepeatOption = "",
                };

                ViewModel.InsertTask(newTask);

                // Clear the input fields after insertion
                InsertTaskName.Text = null;
                InsertTaskDueDateTime.Date = null;

                await Dialog.ShowContent(this.XamlRoot, "Message", "Insert Task seccessfully!", null, null, "OK");
            }
        }

        /// <summary>
        /// Delete a task.
        /// </summary>
        private async void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (sender is AppBarButton button && button.CommandParameter is MyTask taskToDelete)
            {
                var result = await Dialog.ShowContent(this.XamlRoot, "Warning", "Are you sure to delete this task?", "Delete", null, "Cancel");

                if (result == ContentDialogResult.Primary)
                {
                    ViewModel.DeleteTask(taskToDelete);

                    await Dialog.ShowContent(this.XamlRoot, "Message", "Delete Task seccessfully!", null, null, "OK");
                }
                else
                {
                    // do nothing
                }
            }
        }

        /// <summary>
        /// Navigate to the previous page of tasks.
        /// </summary>
        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.CurrentPage > 1)
            {
                ViewModel.CurrentPage--;
                ViewModel.LoadCurrentPage();
            }
        }

        /// <summary>
        /// Navigate to the next page of tasks.
        /// </summary>
        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            int maxPage = (int)Math.Ceiling((double)ViewModel.Tasks.Count / MyTaskViewModel.PageSize);

            if (ViewModel.CurrentPage < maxPage)
            {
                ViewModel.CurrentPage++;
                ViewModel.LoadCurrentPage();
            }
        }

        /// <summary>
        /// Toggle the completion status of a task.
        /// </summary>
        private void IsCompletedTask_Click(object sender, RoutedEventArgs e)
        {
            if (sender is AppBarButton button && button.CommandParameter is MyTask task)
            {
                task.IsCompleted = (task.IsCompleted == true) ? false : true;
                ViewModel.UpdateTask(task);
            }
        }

        /// <summary>
        /// Toggle the importance status of a task.
        /// </summary>
        private void IsImportantTask_Click(object sender, RoutedEventArgs e)
        {
            if (sender is AppBarButton button && button.CommandParameter is MyTask task)
            {
                task.IsImportant = (task.IsImportant == true) ? false : true;
                ViewModel.UpdateTask(task);
            }
        }


        /// <summary>
        /// Press Enter for faster seraching.
        /// </summary>
        private void SearchTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                var textBox = sender as TextBox;
                ViewModel.SearchTerm = textBox.Text;
                ViewModel.LoadCurrentPage();
            }
        }

    }
}