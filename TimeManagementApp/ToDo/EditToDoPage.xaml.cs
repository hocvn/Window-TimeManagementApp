using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using TimeManagementApp.Calendar;
using TimeManagementApp.Dao;
using TimeManagementApp.Helper;
using TimeManagementApp.Home;
using TimeManagementApp.Note;

namespace TimeManagementApp.ToDo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditToDoPage : Page, INotifyPropertyChanged
    {
        public MyTask SelectedTask { get; set; } // selected task from MainPage
        public MyTask SavedTask { get; set; } // use to determine if user want to make a change
        public bool IsReminderOn { get; set; } = true;
        public bool IsPickingReminderTime { get; set; } = true;
        public int RepeatOptionSelectedIndex { get; set; } = 0;
        public ObservableCollection<MyNote> AllNotes { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// Initializes a new instance of the EditToDoPage class.
        /// </summary>
        public EditToDoPage()
        {
            this.InitializeComponent();

            IDao dao = new SqlDao();
            AllNotes = dao.GetAllNote();

            // Add "None" option
            AllNotes.Insert(0, new MyNote { Id = -1, Name = "None", Content = string.Empty });
        }


        /// <summary>
        /// Invoked when the page is navigated to within a Frame.
        /// </summary>
        /// <param name="e">Details about the navigation event.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is MyTask task)
            {
                SelectedTask = task;
                SavedTask = (MyTask)task.Clone();

                // Set reminder on/off based on the task's reminder time
                IsReminderOn = task.ReminderTime == MainWindow.NullDateTime ? false : true;
                CustomReminderDatePicker.Date = task.ReminderTime
                    == MainWindow.NullDateTime 
                    ? DateTime.Now : task.ReminderTime;
                CustomReminderTimePicker.Time = task.ReminderTime == MainWindow.NullDateTime 
                    ? DateTime.Now.TimeOfDay : task.ReminderTime.TimeOfDay;

                // Set repeat option index based on the task's repeat option
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
        

        /// <summary>
        /// Get the current task that user could have edited
        /// </summary>
        private MyTask GetCurrentTask()
        {
            // Combine date and time pickers to create a due date and time
            var dueDateTime = new DateTime(
                UpdateTaskDueDate.Date.Year, UpdateTaskDueDate.Date.Month, UpdateTaskDueDate.Date.Day,
                UpdateTaskDueTime.Time.Hours, UpdateTaskDueTime.Time.Minutes, UpdateTaskDueTime.Time.Seconds
            );

            // Get reminder time based on user selection
            DateTime reminderTime;
            if (!IsReminderOn)
            {
                reminderTime = MainWindow.NullDateTime;
            }
            else if (IsPickingReminderTime)
            {
                var selectedDate = CustomReminderDatePicker.Date;
                var selectedTime = CustomReminderTimePicker.Time;
                reminderTime = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, selectedTime.Hours, selectedTime.Minutes, selectedTime.Seconds);
            }
            else
            {
                reminderTime = SelectedTask.ReminderTime;
            }

            // Determine repeat option based on user selection
            string repeatOption = RepeatOptionSelectedIndex switch
            {
                1 => "Daily",
                2 => "Weekly",
                3 => "Monthly",
                _ => ""
            };


            // Return a task with updated values
            return new MyTask
            {
                TaskId = SelectedTask.TaskId,
                TaskName = UpdateTaskName.Text,
                DueDateTime = dueDateTime,
                Description = UpdateTaskDescription.Text,
                IsCompleted = SelectedTask.IsCompleted,
                IsImportant = SelectedTask.IsImportant,
                RepeatOption = repeatOption,
                ReminderTime = reminderTime,
                NoteId = SelectedTask.NoteId,
            };
        }

        /// <summary>
        /// Handles the click event for the Update button.
        /// </summary>
        private async void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate task name
            if (String.IsNullOrEmpty(UpdateTaskName.Text))
            {
                await Dialog.ShowContent(this.XamlRoot, "Error", "Task Name cannot be empty!", null, null, "OK");
                return;
            }

            var task = GetCurrentTask();

            // send task to the MainPage for _dao to update, then send back to EditToDoPage
            MainWindow.NavigationService.Navigate(typeof(MainToDoPage), task.Clone());
            MainWindow.NavigationService.Navigate(typeof(EditToDoPage), task.Clone());

            await Dialog.ShowContent(this.XamlRoot, "Message", "Update Task successfully!", null, null, "OK");
        }

        /// <summary>
        /// Handles the click event for the Back button.
        /// </summary>
        private async void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate task name
            if (String.IsNullOrEmpty(UpdateTaskName.Text))
            {
                await Dialog.ShowContent(this.XamlRoot, "Error", "Task Name cannot be empty!", null, null, "OK");
                return;
            }

            // Check if task need to be saved
            var task = GetCurrentTask();
            if (!MyTask.IsEqual(task, SavedTask))
            {
                var result = await Dialog.ShowContent(this.XamlRoot, "Warning", "Want to save all the changes?", "Yes", "No", "Cancel");

                if (result == ContentDialogResult.Primary)
                {
                    // send task to the MainPage for _dao to update, then send back to EditToDoPage
                    MainWindow.NavigationService.Navigate(typeof(MainToDoPage), task.Clone());
                    MainWindow.NavigationService.Navigate(typeof(EditToDoPage), task.Clone());

                    // back to the correspond page
                    GoBack();

                    await Dialog.ShowContent(this.XamlRoot, "Message", "Update Task successfully!", null, null, "OK");
                }
                else if (result == ContentDialogResult.Secondary)
                {
                    GoBack();
                }
                else
                {
                    // do nothing
                }

                return;
            }

            GoBack();
        }

        /// <summary>
        /// Go back to a correspond frame page
        /// </summary>
        private void GoBack()
        {
            // cant use normal GoBack() because the update task feature need to navigate to MainToDoPage for _dao to update

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


        /// <summary>
        /// Handles the selection of reminder options.
        /// </summary>
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