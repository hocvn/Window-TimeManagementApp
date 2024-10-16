using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using TimeManagementApp.ToDo;

namespace TimeManagementApp.Dao
{
    public interface IDao
    {
        ObservableCollection<MyTask> GetAllTasks();
    }
}