using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TimeManagementApp.Note;
using TimeManagementApp.Timer;
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


        // Timer ----------------------------------------------
        void SaveSession(FocusSession session);
        List<FocusSession> GetAllSessions();
        List<FocusSession> GetAllSessionsWithTag(string tag);


        // Background -----------------------------------------
        public void SaveSelectedBackground(LinearGradientBrush selectedBrush);
        public LinearGradientBrush LoadSavedBackground(double offset1, double offset2);

    }
}

