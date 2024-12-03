using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using TimeManagementApp.Calendar;
using TimeManagementApp.Helper;
using TimeManagementApp.Home;

namespace TimeManagementApp.ToDo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditToDoPage : Page, INotifyPropertyChanged
    {
        public MyTask SelectedTask { get; set; } // selected task from MainPage
        public bool IsReminderOn { get; set; } = true;
        public bool IsPickingReminderTime { get; set; } = true;
        public int RepeatOptionSelectedIndex { get; set; } = 0;

        public event PropertyChangedEventHandler PropertyChanged;

        public EditToDoPage()
        {
            this.InitializeComponent();
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is MyTask task)
            {
                SelectedTask = task;

                IsReminderOn = task.ReminderTime == MainWindow.NullDateTime ? false : true;
                CustomReminderDatePicker.Date = task.ReminderTime
                    == MainWindow.NullDateTime 
                    ? DateTime.Now : task.ReminderTime;
                CustomReminderTimePicker.Time = task.ReminderTime == MainWindow.NullDateTime 
                    ? DateTime.Now.TimeOfDay : task.ReminderTime.TimeOfDay;

                switch (task.RepeatOption)
                {
                    case "Daily":
                        RepeatOptionSelectedIndex = 1;
                        break;
                    case "Weekly":
                        RepeatOptionSelectedIndex = 2;
                        break;
                    case "Monthly":
                        RepeatOptionSelectedIndex = 3;
                        break;
                    default:
                        RepeatOptionSelectedIndex = 0;
                        break;
                }
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
                UpdateTaskDueDate.Date.Year, UpdateTaskDueDate.Date.Month, UpdateTaskDueDate.Date.Day,
                UpdateTaskDueTime.Time.Hours, UpdateTaskDueTime.Time.Minutes, UpdateTaskDueTime.Time.Seconds
            );

            if (!IsReminderOn)
            {
                SelectedTask.ReminderTime = MainWindow.NullDateTime;
            }
            else if (IsPickingReminderTime)
            {
                var selectedDate = CustomReminderDatePicker.Date;
                var selectedTime = CustomReminderTimePicker.Time;

                SelectedTask.ReminderTime = new DateTime(
                    selectedDate.Year, selectedDate.Month, selectedDate.Day,
                    selectedTime.Hours, selectedTime.Minutes, selectedTime.Seconds
                );
            }

            switch (RepeatOptionSelectedIndex)
            {
                case 1:
                    SelectedTask.RepeatOption = "Daily";
                    break;
                case 2:
                    SelectedTask.RepeatOption = "Weekly";
                    break;
                case 3:
                    SelectedTask.RepeatOption = "Monthly";
                    break;
                default:
                    SelectedTask.RepeatOption = "";
                    break;
            }

            var task = new MyTask
            {
                TaskId = SelectedTask.TaskId,
                TaskName = UpdateTaskName.Text,
                DueDateTime = dueDateTime,
                Description = UpdateTaskDescription.Text,
                IsCompleted = SelectedTask.IsCompleted,
                IsImportant = SelectedTask.IsImportant,
                RepeatOption = SelectedTask.RepeatOption,
                ReminderTime = SelectedTask.ReminderTime,
                NoteId = SelectedTask.NoteId,
            };


            // send task to the MainPage for _dao to update, then send back to EditToDoPage
            MainWindow.NavigationService.Navigate(typeof(MainToDoPage), task.Clone());
            MainWindow.NavigationService.Navigate(typeof(EditToDoPage), task.Clone());

            await Dialog.ShowContent(this.XamlRoot, "Message", "Update Task successfully!", null, null, "OK");
        }


        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // cant use GoBack() this one because the update task feature need to navigate to MainToDoPage for _dao to update

            if (MainWindow.CurrentNavigationViewItem == "CalendarPage")
            {
                MainWindow.NavigationService.Navigate(typeof(CalendarPage));
            }
            else if (MainWindow.CurrentNavigationViewItem == "HomePage")
            {
                MainWindow.NavigationService.Navigate(typeof(HomePage));
            }
            else
            {
                MainWindow.NavigationService.Navigate(typeof(MainToDoPage));
            }
        }


        private void ReminderOption_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radioButton)
            {
                var selectedOption = radioButton.Content.ToString();
                var now = DateTime.Now;

                if (selectedOption == "End of Today")
                {
                    SelectedTask.ReminderTime = new DateTime(now.Year, now.Month, now.Day, 23, 59, 0);
                }
                else if (selectedOption == "Tomorrow Morning")
                {
                    SelectedTask.ReminderTime = now.AddDays(1).Date.AddHours(6);
                }
                else if (selectedOption == "Next week Morning")
                {
                    SelectedTask.ReminderTime = now.AddDays(7).Date.AddHours(6);
                }
                else
                {
                    // will handle it when user click save
                }
            }
        }



    }
}