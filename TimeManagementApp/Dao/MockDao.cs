using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeManagementApp.ToDo;

namespace TimeManagementApp.Dao
{
    public class MockDao : IDao
    {
        public ObservableCollection<MyTask> GetAllTasks()
        {
            return new ObservableCollection<MyTask>()
            {
                new MyTask()
                {
                    TaskName = "Task 01",
                    TaskDescription = "Description 01",
                    StartDateTime = DateTime.Now,
                    DueDateTime = DateTime.Now.AddHours(1),
                },
                new MyTask()
                {
                    TaskName = "Task 02",
                    TaskDescription = "Description 02",
                    StartDateTime = DateTime.Now,
                    DueDateTime = DateTime.Now.AddHours(2),
                },
                new MyTask()
                {
                    TaskName = "Task 03",
                    TaskDescription = "Description 03",
                    StartDateTime = DateTime.Now,
                    DueDateTime = DateTime.Now.AddHours(3),
                },
            };
        }
    }
}
