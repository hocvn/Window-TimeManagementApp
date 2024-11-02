using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Principal;
using Windows.Foundation;
using Windows.Foundation.Collections;
using TimeManagementApp.Helper;
using System.Diagnostics;
using CommunityToolkit.WinUI.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

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
            ViewModel = new MyTaskViewModel();
        }


        private void CloseDetailInformationButton_Click(object sender, RoutedEventArgs e)
        {
            DetailInformationArea.Visibility = Visibility.Collapsed;
        }

        private void TaskItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (MyTasksListBox.SelectedItem != null)
            {
                DetailInformationArea.Visibility = Visibility.Visible;
            }
        }

        private async void InsertTask_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                if (String.IsNullOrEmpty(InsertTaskName.Text))
                {
                    await Dialog.ShowContent(this.XamlRoot, "Error", "Task Name cannot be empty!", null, "OK");
                    return;
                }

                if (!InsertTaskDueDateTime.Date.HasValue)
                {
                    await Dialog.ShowContent(this.XamlRoot, "Error", "Date cannot be empty!", null, "OK");
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

                DetailInformationArea.Visibility = Visibility.Collapsed;
                await Dialog.ShowContent(this.XamlRoot, "Message", "Insert Task seccessfully!", null, "OK");
            }
        }

        private async void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            var selectedTask = MyTasksListBox.SelectedItem as MyTask;

            if (selectedTask != null)
            {
                var result = await Dialog.ShowContent(this.XamlRoot, "Warning", "Are you sure to delete this task?", "Delete", "Cancel");

                if (result == ContentDialogResult.Primary)
                {
                    ViewModel.DeleteTask(selectedTask);

                    DetailInformationArea.Visibility = Visibility.Collapsed;
                    await Dialog.ShowContent(this.XamlRoot, "Message", "Delete Task seccessfully!", null, "OK");
                }
                else
                {
                    // do nothing
                }
            }
        }

        private async void UpdateTask_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentSelectTask != null)
            {
                if (String.IsNullOrEmpty(UpdateTaskName.Text))
                {
                    await Dialog.ShowContent(this.XamlRoot, "Error", "Task Name cannot be empty!", null, "OK");
                    return;
                }

                if (!UpdateTaskStartDate.Date.HasValue || !UpdateTaskDueDate.Date.HasValue)
                {
                    await Dialog.ShowContent(this.XamlRoot, "Error", "Date cannot be empty!", null, "OK");
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

                DetailInformationArea.Visibility = Visibility.Collapsed;
                await Dialog.ShowContent(this.XamlRoot, "Message", "Update Task seccessfully!", null, "OK");
            }
        }

        private void TaskList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentSelectTask = MyTasksListBox.SelectedItem as MyTask;
        }
    }
}