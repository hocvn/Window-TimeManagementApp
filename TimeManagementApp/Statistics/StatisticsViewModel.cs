using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using OxyPlot;
using OxyPlot.Series;
using TimeManagementApp.ToDo;
using TimeManagementApp.Timer;
using Windows.UI;
using Microsoft.UI;
using OxyPlot.Axes;
using TimeManagementApp.Dao;

namespace TimeManagementApp.Statistics
{
    public class StatisticsViewModel : ObservableObject
    {
        private readonly SqlDao _sqlDao;

        public ObservableCollection<TaskStatistic> TaskStatistics { get; } = new ObservableCollection<TaskStatistic>();
        public ObservableCollection<TagTimeSeriesData> TagTimeSeriesData { get; } = new ObservableCollection<TagTimeSeriesData>();
        public ObservableCollection<TagTotalTime> TotalTimePerTag { get; } = new ObservableCollection<TagTotalTime>();

        public double TotalTimeLastDay { get; private set; }
        public double TotalTimeLastWeek { get; private set; }
        public double TotalTimeLastMonth { get; private set; }

        public StatisticsViewModel()
        {
            _sqlDao = new SqlDao();
            LoadTaskStatistics();
            LoadFocusSessionData();
        }

        private void LoadTaskStatistics()
        {
            var allTasks = _sqlDao.GetAllTasks();
            var now = DateTime.Now;

            var overdueNotCompleted = allTasks.Count(t => t.DueDateTime < now && !t.IsCompleted);
            var overdueCompleted = allTasks.Count(t => t.DueDateTime < now && t.IsCompleted);
            var notOverdue = allTasks.Count(t => t.DueDateTime >= now);

            TaskStatistics.Add(new TaskStatistic("Overdue and Not Completed", overdueNotCompleted, Colors.Red));
            TaskStatistics.Add(new TaskStatistic("Overdue and Completed", overdueCompleted, Colors.Green));
            TaskStatistics.Add(new TaskStatistic("Not Overdue", notOverdue, Colors.Blue));
        }

        private void LoadFocusSessionData()
        {
            var allSessions = _sqlDao.GetAllSessions();
            var tagGroups = allSessions.GroupBy(s => s.Tag);

            foreach (var tagGroup in tagGroups)
            {
                var groupedDataPoints = tagGroup
                    .GroupBy(s => s.Timestamp)
                    .Select(g => new DataPoint(DateTimeAxis.ToDouble(g.Key), g.Sum(s => s.Duration) / 60.0)) // Aggregate durations
                    .ToList();

                var tagData = new TagTimeSeriesData
                {
                    Tag = tagGroup.Key,
                    DataPoints = groupedDataPoints
                };
                TagTimeSeriesData.Add(tagData);

                var totalTime = tagGroup.Sum(s => s.Duration / 60.0); // Convert to minutes
                TotalTimePerTag.Add(new TagTotalTime { Tag = tagGroup.Key, TotalTime = totalTime });

                // Debug output
                Debug.WriteLine($"Tag: {tagGroup.Key}, Total Time (minutes): {totalTime}");
            }

            var now = DateTime.Now;
            TotalTimeLastDay = allSessions.Where(s => (now - s.Timestamp).TotalDays <= 1).Sum(s => s.Duration / 60.0); // Convert to minutes
            TotalTimeLastWeek = allSessions.Where(s => (now - s.Timestamp).TotalDays <= 7).Sum(s => s.Duration / 60.0); // Convert to minutes
            TotalTimeLastMonth = allSessions.Where(s => (now - s.Timestamp).TotalDays <= 30).Sum(s => s.Duration / 60.0); // Convert to minutes

            // Debug output
            Debug.WriteLine($"Total Time Last Day (minutes): {TotalTimeLastDay}");
            Debug.WriteLine($"Total Time Last Week (minutes): {TotalTimeLastWeek}");
            Debug.WriteLine($"Total Time Last Month (minutes): {TotalTimeLastMonth}");
        }
    }

    public class TagTimeSeriesData
    {
        public string Tag { get; set; }
        public List<DataPoint> DataPoints { get; set; }
    }

    public class TagTotalTime
    {
        public string Tag { get; set; }
        public double TotalTime { get; set; }
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
