using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using System.ComponentModel;
using TimeManagementApp.Helper;


namespace TimeManagementApp.ToDo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainToDoPage : Page, INotifyPropertyChanged
    {
        public MyTaskViewModel ViewModel { get; set; }
        public MyTask CurrentSelectTask { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainToDoPage()
        {
            this.InitializeComponent();
            // DataContext = App.TimerViewModel
            ViewModel = new MyTaskViewModel();
        }


        private void TaskList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentSelectTask = MyTasksListBox.SelectedItem as MyTask;
        }


        // currently use visibility to control, will use frame navigation later
        private void TaskItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (MyTasksListBox.SelectedItem != null)
            {
                DeleteUpdateArea.Visibility = Visibility.Visible;
            }
        }
        private void CloseArea_Click(object sender, RoutedEventArgs e)
        {
            DeleteUpdateArea.Visibility = Visibility.Collapsed;
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
                    TaskDescription = "My Description",
                    StartDateTime = startDateTime,
                    DueDateTime = dueDateTime
                };

                ViewModel.InsertTask(newTask);

                DeleteUpdateArea.Visibility = Visibility.Collapsed;
                await Dialog.ShowContent(this.XamlRoot, "Message", "Insert Task seccessfully!", null, null, "OK");
            }
        }


        // delete a task
        private async void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            var selectedTask = MyTasksListBox.SelectedItem as MyTask;

            if (selectedTask != null)
            {
                var result = await Dialog.ShowContent(this.XamlRoot, "Warning", "Are you sure to delete this task?", "Delete", null, "Cancel");

                if (result == ContentDialogResult.Primary)
                {
                    ViewModel.DeleteTask(selectedTask);

                    DeleteUpdateArea.Visibility = Visibility.Collapsed;
                    await Dialog.ShowContent(this.XamlRoot, "Message", "Delete Task seccessfully!", null, null, "OK");
                }
                else
                {
                    // do nothing
                }
            }
        }


        // update a task
        private async void UpdateTask_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentSelectTask != null)
            {
                if (String.IsNullOrEmpty(UpdateTaskName.Text))
                {
                    await Dialog.ShowContent(this.XamlRoot, "Error", "Task Name cannot be empty!", null, null, "OK"); 
                    return;
                }

                if (!UpdateTaskStartDate.Date.HasValue || !UpdateTaskDueDate.Date.HasValue)
                {
                    await Dialog.ShowContent(this.XamlRoot, "Error", "Date cannot be empty!", null, null, "OK");
                    return;
                }

                var startDateTime = new DateTime(
                    UpdateTaskStartDate.Date.Value.Year,
                    UpdateTaskStartDate.Date.Value.Month,
                    UpdateTaskStartDate.Date.Value.Day,
                    UpdateTaskStartTime.Time.Hours,
                    UpdateTaskStartTime.Time.Minutes,
                    UpdateTaskStartTime.Time.Seconds
                );

                var dueDateTime = new DateTime(
                    UpdateTaskDueDate.Date.Value.Year,
                    UpdateTaskDueDate.Date.Value.Month,
                    UpdateTaskDueDate.Date.Value.Day,
                    UpdateTaskDueTime.Time.Hours,
                    UpdateTaskDueTime.Time.Minutes,
                    UpdateTaskDueTime.Time.Seconds
                );

                var newTask = new MyTask
                {
                    TaskName = UpdateTaskName.Text,
                    TaskDescription = UpdateTaskDescription.Text,
                    StartDateTime = startDateTime,
                    DueDateTime = dueDateTime
                };

                ViewModel.UpdateTask(CurrentSelectTask, newTask);

                DeleteUpdateArea.Visibility = Visibility.Collapsed;
                await Dialog.ShowContent(this.XamlRoot, "Message", "Update Task seccessfully!", null, null, "OK");
            }
        }
    }
}