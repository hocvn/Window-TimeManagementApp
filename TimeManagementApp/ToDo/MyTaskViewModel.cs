﻿using OfficeOpenXml.Drawing.Chart;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TimeManagementApp.Dao;
using Windows.Data.Xml.Dom;
using Windows.Networking.Vpn;
using Windows.Storage;
using Windows.System.Threading;
using Windows.UI.Notifications;

namespace TimeManagementApp.ToDo
{
    public class MyTaskViewModel : INotifyPropertyChanged
    {
        private static readonly MyTaskViewModel _instance = new MyTaskViewModel();
        public static MyTaskViewModel Instance => _instance;


        public ObservableCollection<MyTask> Tasks;

        public event PropertyChangedEventHandler PropertyChanged;


        // ViewModel setup
        public ObservableCollection<MyTask> ViewTasks;

        public const int PageSize = 7;
        public int CurrentPage { get; set; } = 1;
        public Func<MyTask, bool> Filter { get; set; } = null;
        public string SearchTerm { get; set; } = null;
        public string SortBy { get; set; } = null;
        public ListSortDirection SortDirection { get; set; } = ListSortDirection.Ascending;

        public void LoadCurrentPage()
        {
            IEnumerable<MyTask> query = Tasks;

            // Apply filtering
            if (Filter != null)
            {
                query = query.Where(Filter);
            }

            // Apply searching
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                query = query.Where(task =>
                    task.TaskName.Contains(SearchTerm)
                );
            }

            // Apply sorting
            if (!string.IsNullOrEmpty(SortBy))
            {
                query = SortDirection == ListSortDirection.Ascending
                    ? query.OrderBy(task => task.GetType().GetProperty(SortBy).GetValue(task, null))
                    : query.OrderByDescending(task => task.GetType().GetProperty(SortBy).GetValue(task, null));
            }

            // Apply paging
            var pagedTasks = query.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

            ViewTasks.Clear();
            foreach (var task in pagedTasks)
            {
                ViewTasks.Add(task);
            }
        }


        private IDao _dao;
        private ThreadPoolTimer _reminderTimer;

        public MyTaskViewModel()
        {
            //var baseDirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory); // bin\x64\Debug\net9.0-windows10.0.22621.0\win-x64\AppX\
            //var filePath = Path.Combine(baseDirectory.FullName, "..", "..", "..", "..", "..", "..", "Dao", "tasks.xlsx");
            //_dao = new MockDao(filePath);
            _dao = new SqlDao();
            Tasks = _dao.GetAllTasks();

            ViewTasks = new ObservableCollection<MyTask>();
            LoadCurrentPage();

            // using ThreadPoolTimer to check for reminders when the app is running
            _reminderTimer = ThreadPoolTimer.CreatePeriodicTimer((timer) =>
            {
                CheckReminders();
            }, TimeSpan.FromMinutes(1));

            CheckAndGenerateRepeatingTasks();
        }


        public void InsertTask(MyTask task)
        {
            Tasks.Add(task);
            _dao.InsertTask(task);

            LoadCurrentPage();
        }

        public void DeleteTask(MyTask task)
        {
            Tasks.Remove(task);
            _dao.DeleteTask(task);

            LoadCurrentPage();
        }

        public void UpdateTask(MyTask task)
        {
            var taskToUpdate = Tasks.FirstOrDefault(t => t.TaskId == task.TaskId);
            if (taskToUpdate != null)
            {
                var index = Tasks.IndexOf(taskToUpdate);
                Tasks[index] = task;
                _dao.UpdateTask(task);

                LoadCurrentPage();
                CheckReminders();
                CheckAndGenerateRepeatingTasks();
            }
        }


        // handle filter two way binding
        private int _filterSelectedIndex = 0;
        public int FilterSelectedIndex
        {
            get => _filterSelectedIndex;
            set
            {
                _filterSelectedIndex = value;
                UpdateFilter();
            }
        }

        public void UpdateFilter()
        {
            switch (FilterSelectedIndex)
            {
                case 1: // uncompleted tasks
                    Filter = task => !task.IsCompleted;
                    break;
                case 2: // completed tasks
                    Filter = task => task.IsCompleted;
                    break;
                case 3: // important tasks
                    Filter = task => task.IsImportant;
                    break;
                default: // all tasks
                    Filter = null;
                    break;
            }

            LoadCurrentPage();
        }


        public void CheckReminders()
        {
            foreach (var task in Tasks)
            {
                if (task.ReminderTime.ToString("yyyy-MM-dd HH:mm") == DateTime.Now.ToString("yyyy-MM-dd HH:mm") && !task.IsCompleted)
                {
                    ShowToastNotification(task);
                }
            }
        }

        private void ShowToastNotification(MyTask task)
        {
            string title = "Task Reminder";
            string message = $"It's time to: {task.TaskName}";

            string toastXml = $@"
                <toast>
                    <visual>
                        <binding template='ToastGeneric'>
                            <text>{title}</text>
                            <text>{message}</text>
                        </binding>
                    </visual>
                    <audio src='ms-appx:///Assets/notification.wav'/>
                </toast>";

            var toastDoc = new XmlDocument();
            toastDoc.LoadXml(toastXml);

            var toastNotification = new ToastNotification(toastDoc);
            ToastNotificationManager.CreateToastNotifier().Show(toastNotification);
        }


        public void CheckAndGenerateRepeatingTasks()
        {
            var tasksToAdd = new List<MyTask>(); // List to collect new tasks
            var taskDates = new HashSet<(string, DateTime)>(); // Set to track generated task names and dates

            foreach (var task in Tasks)
            {
                taskDates.Add((task.TaskName, task.DueDateTime)); // Track the generated task
            }

            foreach (var task in Tasks)
            {
                var nextDueDate = task.DueDateTime;
                var currentDate = DateTime.Now;

                // Only generate a new task if the original task's due date is less than or equal to the current date
                if (nextDueDate <= currentDate)
                {
                    // Generate the next due date based on RepeatOption
                    nextDueDate = task.RepeatOption switch
                    {
                        "Daily" => nextDueDate.AddDays(1),
                        "Weekly" => nextDueDate.AddDays(7),
                        "Monthly" => nextDueDate.AddMonths(1),
                        _ => nextDueDate
                    };

                    // Ensure the new task's due date is greater than or equal to the current date
                    if (nextDueDate >= currentDate && !taskDates.Contains((task.TaskName, nextDueDate)))
                    {
                        var newTask = new MyTask
                        {
                            TaskName = task.TaskName,
                            DueDateTime = nextDueDate,
                            Description = task.Description,
                            IsCompleted = false,
                            IsImportant = task.IsImportant,
                            RepeatOption = task.RepeatOption,
                            ReminderTime = task.ReminderTime,
                            NoteId = task.NoteId,
                        };

                        tasksToAdd.Add(newTask); // Collect the new task
                    }
                }
            }

            // Add the collected tasks to the original collection
            foreach (var newTask in tasksToAdd)
            {
                InsertTask(newTask);
            }
        }

    }
}
