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

        private void MyTasksListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentSelectTask = myTasksListView.SelectedItem as MyTask;
        }


        private void InsertTask_Click(object sender, RoutedEventArgs e)
        {
            // TODO: handle empty input

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
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentSelectTask != null)
            {
                ViewModel.DeleteTask(CurrentSelectTask);
                CurrentSelectTask = null;
            }
        }

        private void UpdateTask_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentSelectTask != null)
            {
                // TODO: handle empty input

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
            }
        }
    }
}