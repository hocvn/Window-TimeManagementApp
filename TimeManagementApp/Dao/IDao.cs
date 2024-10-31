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
    interface IDao
    {
        ObservableCollection<MyNote> GetAllNote();

        void SaveNotes(ObservableCollection<MyNote> notes);

        void SaveNote(RichEditBox editor, MyNote note);

        void OpenNote(RichEditBox editor, MyNote note);

        void DeleteNote(MyNote note);

        void RenameNote(MyNote note);
      
        ObservableCollection<MyTask> GetAllTasks();
    }
}

