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

        public MainToDoPage()
        {
            this.InitializeComponent();
            ViewModel = MyTaskViewModel.Instance;
        }


        // navigate to the EditPage when an task is tapped
        private void TaskItem_Click(object sender, ItemClickEventArgs e)
        {
            var selectedTask = e.ClickedItem as MyTask;
            MainWindow.NavigationService.Navigate(typeof(EditToDoPage), selectedTask.Clone());
        }

        // navigate back from EditPage
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is MyTask task)
            {
                ViewModel.UpdateTask(task);
            }

            base.OnNavigatedTo(e);
        }


        // insert new task when press Enter on textbox
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
                };

                ViewModel.InsertTask(newTask);

                await Dialog.ShowContent(this.XamlRoot, "Message", "Insert Task seccessfully!", null, null, "OK");
            }
        }



        // delete a task
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


        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.CurrentPage > 1)
            {
                ViewModel.CurrentPage--;
                ViewModel.LoadCurrentPage();
            }
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            int maxPage = (int)Math.Ceiling((double)ViewModel.Tasks.Count / MyTaskViewModel.PageSize);

            if (ViewModel.CurrentPage < maxPage)
            {
                ViewModel.CurrentPage++;
                ViewModel.LoadCurrentPage();
            }
        }


        private void IsCompletedTask_Click(object sender, RoutedEventArgs e)
        {
            if (sender is AppBarButton button && button.CommandParameter is MyTask task)
            {
                task.IsCompleted = (task.IsCompleted == true) ? false : true;
                ViewModel.UpdateTask(task);
            }
        }
        private void IsImportantTask_Click(object sender, RoutedEventArgs e)
        {
            if (sender is AppBarButton button && button.CommandParameter is MyTask task)
            {
                task.IsImportant = (task.IsImportant == true) ? false : true;
                ViewModel.UpdateTask(task);
            }
        }

    }
}