using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeManagementApp.Note;
using TimeManagementApp.ToDo;
using Windows.Storage;

namespace TimeManagementApp.Dao
{
    public class LocalSettingsDao : IDao
    {
        // Timer ---------------------------------------------------------------------------
        private ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        public TimeSpan LoadTimeSpan(string key)
        {
            if (localSettings.Values[key] is string timeSpanString)
            {
                return TimeSpan.Parse(timeSpanString);
            }
            return TimeSpan.Zero;
        }

        public void SaveTimeSpan(string key, TimeSpan timeSpan)
        {
            localSettings.Values[key] = timeSpan.ToString();
        }


        // Notes -------------------------------------------------------
        public ObservableCollection<MyNote> GetAllNote()
        {
            throw new NotImplementedException();
        }

        public void SaveNotes(ObservableCollection<MyNote> notes)
        {
            throw new NotImplementedException();
        }

        public void SaveNote(RichEditBox editor, MyNote note)
        {
            throw new NotImplementedException();
        }

        public Task OpenNote(RichEditBox editor, MyNote note)
        {
            throw new NotImplementedException();
        }

        public void DeleteNote(MyNote note)
        {
            throw new NotImplementedException();
        }

        public void RenameNote(MyNote note)
        {
            throw new NotImplementedException();
        }


        // Tasks -------------------------------------------------
        public ObservableCollection<MyTask> GetAllTasks()
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<MyTask> GetTodayTask()
        {
            throw new NotImplementedException();
        }
    }
}
