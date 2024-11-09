using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeManagementApp.Dao;
using TimeManagementApp.Helper;


namespace TimeManagementApp.ToDo
{
    public class MyTaskViewModel : INotifyPropertyChanged
    {
        // use singleton for reading database one time only
        private static readonly MyTaskViewModel _instance = new MyTaskViewModel();
        public static MyTaskViewModel Instance => _instance;


        public ObservableCollection<MyTask> Tasks = new ObservableCollection<MyTask>();

        public event PropertyChangedEventHandler PropertyChanged;

        public MyTaskViewModel()
        {
            // checking if reading database multiple times
            Debug.WriteLine("Reading database ...");

            IDao dao = new MockDao();
            Tasks = dao.GetAllTasks();
        }


        public void InsertTask(MyTask newTask)
        {
            Tasks.Add(newTask);
        }

        public void DeleteTask(MyTask selectedTask)
        {
            Tasks.Remove(selectedTask);
        }

        public void UpdateTask(MyTask oldTask, MyTask newTask)
        {
            var index = Tasks.IndexOf(oldTask);
            Tasks[index] = newTask;
        }
    }
}