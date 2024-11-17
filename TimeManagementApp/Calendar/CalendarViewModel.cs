using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TimeManagementApp.Dao;
using TimeManagementApp.ToDo;

namespace TimeManagementApp.Calendar
{
    public class CalendarViewModel : INotifyPropertyChanged
    {
        public DateTime Date { get; set; }
        public string DisplayDate { get; set; }

        public ObservableCollection<MyTask> Tasks;

        private IDao dao { get; set; }

        public CalendarViewModel()
        {
            Date = DateTime.Now;
            Tasks = new ObservableCollection<MyTask>
            {
                new MyTask { TaskName = "Task 1", Summarization = "Summary 1", StartDateTime = DateTime.Now, DueDateTime = DateTime.Now.AddHours(1) },
                new MyTask { TaskName = "Task 2", Summarization = "Summary 2", StartDateTime = DateTime.Now, DueDateTime = DateTime.Now.AddHours(1) },
                new MyTask { TaskName = "Task 3", Summarization = "Summary 3", StartDateTime = DateTime.Now, DueDateTime = DateTime.Now.AddHours(1) }
            };
            dao = new MockDao();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void GetTasksForDate(DateTime date)
        {
            Tasks = dao.GetTasksForDate(date);
        }
    }
}
