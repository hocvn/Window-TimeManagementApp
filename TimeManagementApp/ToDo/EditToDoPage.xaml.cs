using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TimeManagementApp.Helper;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

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


            var startDateTime = new DateTime(
                UpdateTaskStartDate.Date.Year,
                UpdateTaskStartDate.Date.Month,
                UpdateTaskStartDate.Date.Day,
                UpdateTaskStartTime.Time.Hours,
                UpdateTaskStartTime.Time.Minutes,
                UpdateTaskStartTime.Time.Seconds
            );

            var dueDateTime = new DateTime(
                UpdateTaskDueDate.Date.Year,
                UpdateTaskDueDate.Date.Month,
                UpdateTaskDueDate.Date.Day,
                UpdateTaskDueTime.Time.Hours,
                UpdateTaskDueTime.Time.Minutes,
                UpdateTaskDueTime.Time.Seconds
            );

            var newTask = new MyTask
            {
                TaskName = UpdateTaskName.Text,
                Summarization = "This summarization should bind all changed in EditPage",
                StartDateTime = startDateTime,
                DueDateTime = dueDateTime
            };


            // send newTask to the MainPage
            MainWindow.NavigationService.Navigate(typeof(MainToDoPage), newTask);

            await Dialog.ShowContent(this.XamlRoot, "Message", "Update Task successfully!", null, null, "OK");
        }


        private async void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var result = await Dialog.ShowContent(this.XamlRoot, "Warning", "Want to save all the changes?", "Yes", "No", "Cancel");

            if (result == ContentDialogResult.Primary)
            {
                UpdateButton_Click(null, null);
            }
            else if (result == ContentDialogResult.Secondary)
            {
                MainWindow.NavigationService.Navigate(typeof(MainToDoPage));
            }
            else
            {
                // do nothing
            }
        }


        private void ReminderOption_Checked(object sender, RoutedEventArgs e)
        {
            // todo
        }


    }

}
