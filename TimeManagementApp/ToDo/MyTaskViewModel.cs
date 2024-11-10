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


        public ObservableCollection<MyTask> Tasks;
        
        public event PropertyChangedEventHandler PropertyChanged;


        // paging setup
        public const int PageSize = 7;
        public int CurrentPage { get; set; } = 1;

        public ObservableCollection<MyTask> PagedTasks { get; private set; }

        public void LoadCurrentPage()
        {
            PagedTasks.Clear();
            var pagedTasks = Tasks.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

            foreach (var task in pagedTasks)
            {
                PagedTasks.Add(task);
            }
        }


        public MyTaskViewModel()
        {
            // checking if reading database multiple times
            Debug.WriteLine("Reading database ...");

            IDao dao = new MockDao();
            Tasks = dao.GetAllTasks();

            PagedTasks = new ObservableCollection<MyTask>();
            LoadCurrentPage();
        }


        // todo: are there any solution for insert, delete, update task
        // that dont need to reread all tasks (no LoadCurrentPage) ?
        public void InsertTask(MyTask newTask)
        {
            Tasks.Add(newTask);
            LoadCurrentPage();
        }

        public void DeleteTask(MyTask selectedTask)
        {
            Tasks.Remove(selectedTask);
            LoadCurrentPage();
        }

        public void UpdateTask(MyTask oldTask, MyTask newTask)
        {
            var index = Tasks.IndexOf(oldTask);
            Tasks[index] = newTask;
            LoadCurrentPage();
        }
    }
}