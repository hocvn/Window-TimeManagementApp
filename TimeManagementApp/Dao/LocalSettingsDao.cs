using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TimeManagementApp.Note;
using TimeManagementApp.ToDo;
using Windows.Storage;
using Windows.UI.Shell;

namespace TimeManagementApp.Dao
{
    public class LocalSettingsDao : IDao
    {
        // Timer ---------------------------------------------------------------------------
        private ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        public void SaveSession(string key, TimeSpan sessionTime)
        {
            var sessions = LoadSessions(key);
            sessions.Add(new Timer.FocusSession { Duration = sessionTime, Timestamp = DateTime.UtcNow });
            
            localSettings.Values[key] = JsonSerializer.Serialize(sessions);
        }

        public List<Timer.FocusSession> LoadSessions(string key)
        {
            if (localSettings.Values.ContainsKey(key))
            {
                var sessionsJson = localSettings.Values[key] as string;
                return JsonSerializer.Deserialize<List<Timer.FocusSession>>(sessionsJson) ?? new List<Timer.FocusSession>();
            }

            return new List<Timer.FocusSession>();
        }


        //public TimeSpan LoadTimeSpan(string key)
        //{
        //    if (localSettings.Values[key] is string timeSpanString)
        //    {
        //        return TimeSpan.Parse(timeSpanString);
        //    }
        //    return TimeSpan.Zero;
        //}

        //public void SaveTimeSpan(string key, TimeSpan timeSpan)
        //{
        //    localSettings.Values[key] = timeSpan.ToString();
        //}


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
