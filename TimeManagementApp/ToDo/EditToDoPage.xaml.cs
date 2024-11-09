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


        private async void UpdateTask_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTask != null)
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


                // send newTask to the MainPage
                if (Frame.CanGoBack)
                {
                    MainWindow.NavigationService.Navigate(typeof(MainToDoPage), newTask);
                }

                await Dialog.ShowContent(this.XamlRoot, "Message", "Update Task successfully!", null, null, "OK");
            }
        }
    }

}
