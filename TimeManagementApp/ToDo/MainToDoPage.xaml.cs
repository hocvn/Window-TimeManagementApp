using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
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
    public sealed partial class MainToDoPage : Page
    {
        public MyTaskViewModel ViewModel { get; set; }

        public MainToDoPage()
        {
            this.InitializeComponent();
            ViewModel = new MyTaskViewModel();
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
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

            ViewModel.AddTask(newTask);
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            var selectedTask = myTasksListView.SelectedItem as MyTask;
            if (selectedTask != null)
            {
                ViewModel.DeleteTask(selectedTask);
            }
        }

        private void MyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedComboBoxItem = MyComboBox.SelectedItem as ComboBoxItem;
            if (selectedComboBoxItem != null)
            {
                string selectedContent = selectedComboBoxItem.Content.ToString();
                string selectedTag = selectedComboBoxItem.Tag.ToString();

                if (selectedTag == "1")
                {
                    InsertStackPanel.Visibility = Visibility.Visible;
                    DeleteStackPanel.Visibility = Visibility.Collapsed;
                }
                else if (selectedTag == "2")
                {
                    InsertStackPanel.Visibility = Visibility.Collapsed;
                    DeleteStackPanel.Visibility = Visibility.Visible;
                }
                else
                {
                    // update
                }
            }
        }
    }
}
