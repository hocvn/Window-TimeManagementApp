using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using TimeManagementApp.Helper;

namespace TimeManagementApp.ToDo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditToDoPage : Page
    {
        public MyTask SelectedTask { get; set; } // selected task from MainPage

        public EditToDoPage()
        {
            this.InitializeComponent();
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is MyTask task)
            {
                SelectedTask = task;
            }

            base.OnNavigatedTo(e);
        }


        private async void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            // todo: save the reminders, steps, ... to summarization

            if (String.IsNullOrEmpty(UpdateTaskName.Text))
            {
                await Dialog.ShowContent(this.XamlRoot, "Error", "Task Name cannot be empty!", null, null, "OK");
                return;
            }

            var dueDateTime = new DateTime(
                UpdateTaskDueDate.Date.Year,
                UpdateTaskDueDate.Date.Month,
                UpdateTaskDueDate.Date.Day,
                UpdateTaskDueTime.Time.Hours,
                UpdateTaskDueTime.Time.Minutes,
                UpdateTaskDueTime.Time.Seconds
            );

            var task = new MyTask
            {
                TaskName = UpdateTaskName.Text,
                DueDateTime = dueDateTime,
                IsCompleted = false,
                IsImportant = false,
            };


            // send task to the MainPage for _dao to update, then send back to EditToDoPage
            MainWindow.NavigationService.Navigate(typeof(MainToDoPage), task.Clone());
            MainWindow.NavigationService.Navigate(typeof(EditToDoPage), task.Clone());

            await Dialog.ShowContent(this.XamlRoot, "Message", "Update Task successfully!", null, null, "OK");
        }


        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.NavigationService.GoBack();
        }


        private void ReminderOption_Checked(object sender, RoutedEventArgs e)
        {
            // todo
        }


    }
}
