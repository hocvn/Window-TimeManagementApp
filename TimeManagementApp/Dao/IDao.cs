using Microsoft.Data.SqlClient;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TimeManagementApp.Helper;
using TimeManagementApp.Note;
using TimeManagementApp.Services;
using TimeManagementApp.ToDo;
using Windows.System;

namespace TimeManagementApp.Dao
{
    public interface IDao
    {
        public void CreateUser(string username, string password, string email);

        public bool CheckUser(string username, string password);
        ObservableCollection<MyNote> GetAllNote();

        void SaveNotes(ObservableCollection<MyNote> notes);

        void SaveNote(RichEditBox editor, MyNote note);

        Task OpenNote(RichEditBox editor, MyNote note);

        void DeleteNote(MyNote note);

        void RenameNote(MyNote note);
      
        ObservableCollection<MyTask> GetAllTasks();
        ObservableCollection<MyTask> GetTodayTask();
    }
}

