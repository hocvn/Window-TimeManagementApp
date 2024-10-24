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

        public bool IsInsertExpanderExpanded { get; set; } = false;
        public bool IsDeleteExpanderExpanded { get; set; } = false;
        public bool IsUpdateExpanderExpanded { get; set; } = false;


        public event PropertyChangedEventHandler PropertyChanged;

        public MainToDoPage()
        {
            this.InitializeComponent();
            ViewModel = new MyTaskViewModel();
        }

        private void MyTasksListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentSelectTask = myTasksListView.SelectedItem as MyTask;
        }


        private async void InsertTask_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(InsertTaskName.Text))
            {
                await Dialog.ShowContent(this.XamlRoot, "Error", "Task Name cannot be empty!");
                return;
            }
            
            if (!InsertTaskStartDate.Date.HasValue || !InsertTaskEndDate.Date.HasValue)
            {
                await Dialog.ShowContent(this.XamlRoot, "Error", "Date cannot be empty!");
                return;
            }

            var startDateTime = new DateTime(
                InsertTaskStartDate.Date.Value.Year,
                InsertTaskStartDate.Date.Value.Month,
                InsertTaskStartDate.Date.Value.Day,
                InsertTaskStartTime.Time.Hours,
                InsertTaskStartTime.Time.Minutes,
                InsertTaskStartTime.Time.Seconds
            );

            var endDateTime = new DateTime(
                InsertTaskEndDate.Date.Value.Year,
                InsertTaskEndDate.Date.Value.Month,
                InsertTaskEndDate.Date.Value.Day,
                InsertTaskEndTime.Time.Hours,
                InsertTaskEndTime.Time.Minutes,
                InsertTaskEndTime.Time.Seconds
            );

            var newTask = new MyTask
            {
                TaskName = InsertTaskName.Text,
                TaskDescription = InsertTaskDescription.Text,
                StartTime = startDateTime,
                EndTime = endDateTime
            };

            ViewModel.InsertTask(newTask);

            IsInsertExpanderExpanded = false;
            await Dialog.ShowContent(this.XamlRoot, "Message", "Insert Task successfully!");
        }

        private async void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentSelectTask == null)
            {
                await Dialog.ShowContent(this.XamlRoot, "Error", "Please select a task!");
                return;
            }

            ViewModel.DeleteTask(CurrentSelectTask);
            CurrentSelectTask = null;

            IsDeleteExpanderExpanded = false;
            await Dialog.ShowContent(this.XamlRoot, "Message", "Delete Task successfully!");
        }

        private async void UpdateTask_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentSelectTask == null)
            {
                await Dialog.ShowContent(this.XamlRoot, "Error", "Please select a task!");
                return;
            }

            if (String.IsNullOrEmpty(UpdateTaskName.Text))
            {
                await Dialog.ShowContent(this.XamlRoot, "Error", "Task Name cannot be empty!");
                return;
            }
            
            if (!UpdateTaskStartDate.Date.HasValue || !UpdateTaskEndDate.Date.HasValue)
            {
                await Dialog.ShowContent(this.XamlRoot, "Error", "Date cannot be empty!");
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

            var endDateTime = new DateTime(
                UpdateTaskEndDate.Date.Value.Year,
                UpdateTaskEndDate.Date.Value.Month,
                UpdateTaskEndDate.Date.Value.Day,
                UpdateTaskEndTime.Time.Hours,
                UpdateTaskEndTime.Time.Minutes,
                UpdateTaskEndTime.Time.Seconds
            );

            var newTask = new MyTask
            {
                TaskName = UpdateTaskName.Text,
                TaskDescription = UpdateTaskDescription.Text,
                StartTime = startDateTime,
                EndTime = endDateTime
            };

            ViewModel.UpdateTask(CurrentSelectTask, newTask);
            CurrentSelectTask = null;

            IsUpdateExpanderExpanded = false;
            await Dialog.ShowContent(this.XamlRoot, "Message", "Update Task successfully!");
        }
    }
}