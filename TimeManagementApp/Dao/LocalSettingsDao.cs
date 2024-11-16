using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TimeManagementApp.Helper;
using TimeManagementApp.Note;
using TimeManagementApp.Timer;
using TimeManagementApp.ToDo;
using Windows.Foundation;
using Windows.Storage;

namespace TimeManagementApp.Dao
{
    public class LocalSettingsDao : IDao
    {
        // Background -----------------------------------------------------------
        private const string BackgroundBrushKey = "BackgroundBrush";

        public void SaveSelectedBackground(LinearGradientBrush selectedBrush)
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            var gradientStop1 = selectedBrush.GradientStops[0];
            var gradientStop2 = selectedBrush.GradientStops[1];

            localSettings.Values[BackgroundBrushKey] = $"{gradientStop1.Color}|{gradientStop2.Color}";
        }

        public LinearGradientBrush LoadSavedBackground(double offset1, double offset2)
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            if (localSettings.Values.ContainsKey(BackgroundBrushKey))
            {
                var savedBrush = localSettings.Values[BackgroundBrushKey].ToString().Split('|');
                var color1 = savedBrush[0];
                var color2 = savedBrush[1];

                return new LinearGradientBrush
                {
                    StartPoint = new Point(0, 0),
                    EndPoint = new Point(0, 1),
                    GradientStops = new GradientStopCollection
                    {
                        new GradientStop { Color = ColorHelper.FromArgb(color1), Offset = offset1 },
                        new GradientStop { Color = ColorHelper.FromArgb(color2), Offset = offset2 }
                    }
                };
            }
            return null;
        }


        // Others -------------------------------------------------------
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

        public ObservableCollection<MyTask> GetAllTasks()
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<MyTask> GetTodayTask()
        {
            throw new NotImplementedException();
        }

        public void SaveSession(FocusSession session)
        {
            throw new NotImplementedException();
        }

        public List<FocusSession> GetAllSessions()
        {
            throw new NotImplementedException();
        }

        public List<FocusSession> GetAllSessionsWithTag(string tag)
        {
            throw new NotImplementedException();
        }

        public void CreateUser(string username, string password, string email)
        {
            throw new NotImplementedException();
        }

        public bool CheckCredential(string username, string password)
        {
            throw new NotImplementedException();
        }

        public bool IsUsernameInUse(string username)
        {
            throw new NotImplementedException();
        }

        public bool IsEmailInUse(string username)
        {
            throw new NotImplementedException();
        }

        public string GetUsername(string email)
        {
            throw new NotImplementedException();
        }

        public string GetPassword(string username)
        {
            throw new NotImplementedException();
        }

        public void ResetPassword(string username, string password, string email)
        {
            throw new NotImplementedException();
        }
    }

}
