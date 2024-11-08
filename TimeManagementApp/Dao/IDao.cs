using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeManagementApp.Note;
using TimeManagementApp.ToDo;

namespace TimeManagementApp.Dao
{
    public interface IDao
    {
        // Notes -------------------------------------------------
        ObservableCollection<MyNote> GetAllNote();

        void SaveNotes(ObservableCollection<MyNote> notes);

        void SaveNote(RichEditBox editor, MyNote note);

        Task OpenNote(RichEditBox editor, MyNote note);

        void DeleteNote(MyNote note);

        void RenameNote(MyNote note);
      
        
        // Tasks ----------------------------------------------
        ObservableCollection<MyTask> GetAllTasks();
        ObservableCollection<MyTask> GetTodayTask();


        // TImer ----------------------------------------------
        //public void SaveValue(string key, string value);
        //public string LoadValue(string key);
        //public void RemoveValue(string key);
        public TimeSpan LoadTimeSpan(string key);
        public void SaveTimeSpan(string key, TimeSpan timeSpan);
    }
}

