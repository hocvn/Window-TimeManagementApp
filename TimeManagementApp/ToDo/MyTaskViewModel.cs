using OfficeOpenXml.Drawing.Chart;
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
using Windows.Storage;

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
                    task.TaskName.Contains(SearchTerm) ||
                    task.Summarization.Contains(SearchTerm)
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

        public MyTaskViewModel()
        {
            var directory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            var filePath = Path.Combine(directory.FullName, "tasks.xlsx"); 
            _dao = new MockDao(filePath);
            Tasks = _dao.GetAllTasks();

            ViewTasks = new ObservableCollection<MyTask>();
            LoadCurrentPage();
        }


        public void InsertTask(MyTask newTask)
        {
            Tasks.Add(newTask);
            _dao.InsertTask(newTask);
            LoadCurrentPage();
        }

        public void DeleteTask(MyTask selectedTask)
        {
            Tasks.Remove(selectedTask);
            _dao.DeleteTask(selectedTask);
            LoadCurrentPage();
        }

        public void UpdateTask(MyTask oldTask, MyTask newTask)
        {
            var index = Tasks.IndexOf(oldTask);
            Tasks[index] = newTask;
            _dao.UpdateTask(oldTask, newTask);
            LoadCurrentPage();
        }
    }
}