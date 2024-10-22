using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeManagementApp.Dao;

namespace TimeManagementApp.ToDo
{
    public class MyTaskViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<MyTask> Tasks = new ObservableCollection<MyTask>();

        public event PropertyChangedEventHandler PropertyChanged;

        public MyTaskViewModel()
        {
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