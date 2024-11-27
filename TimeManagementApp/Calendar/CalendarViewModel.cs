using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using TimeManagementApp.Dao;
using TimeManagementApp.ToDo;

namespace TimeManagementApp.Calendar
{
    public class CalendarViewModel : INotifyPropertyChanged
    {
        public DateTime Date { get; set; }

        public string DisplayDate { get; set; }

        public ObservableCollection<MyTask> Tasks { get; set; }

        public ObservableCollection<MyTask> TasksForDate { get; set; }

        public Dictionary<DateTime, int> TaskCounts { get; set; }

        private IDao dao { get; set; }

        public CalendarViewModel()
        {
            Date = DateTime.Now;
            Tasks = new ObservableCollection<MyTask>();
            TasksForDate = new ObservableCollection<MyTask>();
            TaskCounts = new Dictionary<DateTime, int>();

            //dao = new MockDao();
            var directory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            var filePath = Path.Combine(directory.FullName, "tasks.xlsx");
            dao = new MockDao(filePath);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Init()
        {
            Tasks = dao.GetAllTasks();

            foreach (MyTask task in Tasks)
            {
                if (TaskCounts.ContainsKey(task.DueDateTime.Date))
                    TaskCounts[task.DueDateTime.Date]++;
                else
                {
                    TaskCounts.Add(task.DueDateTime.Date, 1);
                }
            }
        }

        public void GetTasksForDate(DateTime date)
        {
            TasksForDate.Clear();

            foreach (MyTask task in Tasks)
            {
                if (task.DueDateTime.Date == date.Date)
                {
                    TasksForDate.Add(task);
                }
            }
        }
    }
}
