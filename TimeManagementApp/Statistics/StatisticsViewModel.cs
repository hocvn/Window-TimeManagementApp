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
        public List<Session> Sessions { get; } = new List<Session>();

        public StatisticsViewModel()
        {
            _sqlDao = new SqlDao();
            LoadTaskStatistics();
            LoadSessions();
        }

        private void LoadTaskStatistics()
        {
            var allTasks = _sqlDao.GetAllTasks();
            var now = DateTime.Now;

            var overdueNotCompleted = allTasks.Count(t => t.DueDateTime < now && !t.IsCompleted);
            var overdueCompleted = allTasks.Count(t => t.DueDateTime < now && t.IsCompleted);
            var notOverdue = allTasks.Count(t => t.DueDateTime >= now);

            TaskStatistics.Add(new TaskStatistic("Overdue & Uncompleted", overdueNotCompleted, Colors.Red));
            TaskStatistics.Add(new TaskStatistic("Overdue & Completed", overdueCompleted, Colors.Green));
            TaskStatistics.Add(new TaskStatistic("Not Overdue", notOverdue, Colors.Blue));
        }

        private void LoadSessions()
        {
            Sessions.AddRange(_sqlDao.GetAllSessions());
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
