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

        private void InsertTask_Click(object sender, RoutedEventArgs e)
        {
            var startDateTime = new DateTime(
                NewTaskStartTime.Date.Year,
                NewTaskStartTime.Date.Month,
                NewTaskStartTime.Date.Day,
                NewTaskStartTimeTime.Time.Hours,
                NewTaskStartTimeTime.Time.Minutes,
                NewTaskStartTimeTime.Time.Seconds
            );

            var endDateTime = new DateTime(
                NewTaskEndTime.Date.Year,
                NewTaskEndTime.Date.Month,
                NewTaskEndTime.Date.Day,
                NewTaskEndTimeTime.Time.Hours,
                NewTaskEndTimeTime.Time.Minutes,
                NewTaskEndTimeTime.Time.Seconds
            );

            var newTask = new MyTask
            {
                TaskName = NewTaskName.Text,
                TaskDescription = NewTaskDescription.Text,
                StartTime = startDateTime,
                EndTime = endDateTime
            };

            ViewModel.InsertTask(newTask);

            MyComboBox.SelectedItem = null;
            InsertStackPanel.Visibility = Visibility.Collapsed;
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentSelectTask != null)
            {
                ViewModel.DeleteTask(CurrentSelectTask);
                CurrentSelectTask = null;

                MyComboBox.SelectedItem = null;
                DeleteStackPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void UpdateTask_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentSelectTask != null)
            {
                var startDateTime = new DateTime(
                    UpdateTaskStartTime.Date.Year,
                    UpdateTaskStartTime.Date.Month,
                    UpdateTaskStartTime.Date.Day,
                    UpdateTaskStartTimeTime.Time.Hours,
                    UpdateTaskStartTimeTime.Time.Minutes,
                    UpdateTaskStartTimeTime.Time.Seconds
                );

                var endDateTime = new DateTime(
                    UpdateTaskEndTime.Date.Year,
                    UpdateTaskEndTime.Date.Month,
                    UpdateTaskEndTime.Date.Day,
                    UpdateTaskEndTimeTime.Time.Hours,
                    UpdateTaskEndTimeTime.Time.Minutes,
                    UpdateTaskEndTimeTime.Time.Seconds
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

                MyComboBox.SelectedItem = null;
                UpdateStackPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void MyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedComboBoxItem = MyComboBox.SelectedItem as ComboBoxItem;
            if (selectedComboBoxItem != null)
            {
                var selectedTag = selectedComboBoxItem.Tag.ToString();
                InsertStackPanel.Visibility = selectedTag == "InsertTag" ? Visibility.Visible : Visibility.Collapsed;
                DeleteStackPanel.Visibility = selectedTag == "DeleteTag" ? Visibility.Visible : Visibility.Collapsed;
                UpdateStackPanel.Visibility = selectedTag == "UpdateTag" ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void MyTasksListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentSelectTask = myTasksListView.SelectedItem as MyTask;
        }
    }
}