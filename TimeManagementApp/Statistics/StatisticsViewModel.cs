using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using TimeManagementApp.Timer;
using TimeManagementApp.Dao;
using Windows.UI;
using Microsoft.UI;

namespace TimeManagementApp.Statistics
{
    public class StatisticsViewModel : ObservableObject
    {
        private readonly SqlDao _sqlDao;

        public ObservableCollection<TaskStatistic> TaskStatistics { get; } = new ObservableCollection<TaskStatistic>();
        public ObservableCollection<TaskStatistic> TaskStatusStatistics { get; } = new ObservableCollection<TaskStatistic>();

        public List<Session> Sessions { get; } = new List<Session>();

        public StatisticsViewModel()
        {
            _sqlDao = new SqlDao();
            LoadTaskStatistics();
            LoadTaskStatusStatistics();
            LoadSessions();
        }

        private void LoadTaskStatistics()
        {
            var allTasks = _sqlDao.GetAllTasks();
            var now = DateTime.Now;

            var overdue = allTasks.Count(t => t.DueDateTime < now);
            var notOverdue = allTasks.Count(t => t.DueDateTime >= now);

            TaskStatistics.Add(new TaskStatistic("Overdue", overdue, Colors.Red));
            TaskStatistics.Add(new TaskStatistic("Not Overdue", notOverdue, Colors.Blue));
        }

        private void LoadSessions()
        {
            Sessions.AddRange(_sqlDao.GetAllSessions());
        }

        private void LoadTaskStatusStatistics()
        {
            var allTasks = _sqlDao.GetAllTasks();

            var notStarted = allTasks.Count(t => t.Status == "Not Started");
            var inProgress = allTasks.Count(t => t.Status == "In Progress");
            var onHold = allTasks.Count(t => t.Status == "On Hold");
            var completed = allTasks.Count(t => t.Status == "Completed");

            TaskStatusStatistics.Add(new TaskStatistic("Not Started", notStarted, Colors.Gray));
            TaskStatusStatistics.Add(new TaskStatistic("In Progress", inProgress, Colors.Orange));
            TaskStatusStatistics.Add(new TaskStatistic("On Hold", onHold, Colors.Yellow));
            TaskStatusStatistics.Add(new TaskStatistic("Completed", completed, Colors.Green));
        }
    }

    public class TaskStatistic
    {
        public string Name { get; }
        public int Value { get; }
        public Color Color { get; }

        public TaskStatistic(string name, int value, Color color)
        {
            Name = name;
            Value = value;
            Color = color;
        }
    }
}
