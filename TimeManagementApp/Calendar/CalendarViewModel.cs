using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TimeManagementApp.Dao;
using TimeManagementApp.ToDo;

namespace TimeManagementApp.Calendar
{
    public class CalendarViewModel : INotifyPropertyChanged
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }

        public ObservableCollection<MyTask> Tasks;

        private IDao dao { get; set; }

        public CalendarViewModel()
        {
            Year = DateTime.Now.Year;
            Month = DateTime.Now.Month;
            Day = DateTime.Now.Day;
            dao = new MockDao();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void GetTasksForDate(DateTime date)
        {
            Tasks = dao.GetAllTasks(date);
        }
    }
}
