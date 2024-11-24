using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TimeManagementApp.Note;
using TimeManagementApp.Timer;
using TimeManagementApp.ToDo;

namespace TimeManagementApp.Dao
{
    public class ExcelDao : IDao
    {
        private readonly string _filePath;
        public ExcelDao(string filePath)
        {
            _filePath = filePath;

            // Ensure EPPlus license context is set
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
        }

        // Timer ---------------------------------------------------
        public void SaveSession(FocusSession session)
        {
            FileInfo fileInfo = new FileInfo(_filePath);

            using (var package = new ExcelPackage(fileInfo))
            {
                var worksheet = package.Workbook.Worksheets.Count == 0
                    ? package.Workbook.Worksheets.Add("FocusSessions")
                    : package.Workbook.Worksheets[0];

                int row = (worksheet.Dimension?.Rows ?? 0) + 1;

                // Assign a new unique ID to the session
                session.Id = row;

                worksheet.Cells[row, 1].Value = session.Id;
                worksheet.Cells[row, 2].Value = session.Duration.ToString();
                worksheet.Cells[row, 3].Value = session.Timestamp.ToString("o"); // ISO 8601 format
                worksheet.Cells[row, 4].Value = session.Tag;

                package.Save();
            }
        }

        public List<FocusSession> GetAllSessions()
        {
            var sessions = new List<FocusSession>();

            FileInfo fileInfo = new FileInfo(_filePath);
            if (!fileInfo.Exists)
            {
                return sessions; // No file exists yet, return empty list
            }

            using (var package = new ExcelPackage(fileInfo))
            {
                var worksheet = package.Workbook.Worksheets[0];
                if (worksheet.Dimension == null)
                {
                    return sessions;
                }

                for (int row = 1; row <= worksheet.Dimension.Rows; row++)
                {
                    var id = int.Parse(worksheet.Cells[row, 1].Value.ToString());
                    var duration = TimeSpan.Parse(worksheet.Cells[row, 2].Value.ToString());
                    var timestamp = DateTime.Parse(worksheet.Cells[row, 3].Value.ToString());
                    var tag = worksheet.Cells[row, 4].Value.ToString();

                    sessions.Add(new FocusSession { Id = id, Duration = duration, Timestamp = timestamp, Tag = tag });
                }
            }

            return sessions;
        }

        public List<FocusSession> GetAllSessionsWithTag(string tag)
        {
            var sessions = GetAllSessions();
            return sessions.Where(s => s.Tag == tag).ToList();
        }


        // Others ---------------------------------------------------
        public void DeleteNote(MyNote note)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<MyNote> GetAllNote()
        {
            throw new NotImplementedException();
        }

        public Task OpenNote(RichEditBox editor, MyNote note)
        {
            throw new NotImplementedException();
        }

        public void RenameNote(MyNote note)
        {
            throw new NotImplementedException();
        }

        public void SaveNote(RichEditBox editor, MyNote note)
        {
            throw new NotImplementedException();
        }

        public void SaveNotes(ObservableCollection<MyNote> notes)
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

        public void SaveSelectedBackground(LinearGradientBrush selectedBrush)
        {
            throw new NotImplementedException();
        }

        public LinearGradientBrush LoadSavedBackground(double offset1, double offset2)
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

        List<FocusSession> IDao.GetAllSessions()
        {
            throw new NotImplementedException();
        }

        List<FocusSession> IDao.GetAllSessionsWithTag(string tag)
        {
            throw new NotImplementedException();
        }

        LinearGradientBrush IDao.LoadSavedBackground(double offset1, double offset2)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<MyTask> GetTasksForDate(DateTime date)
        {
            throw new NotImplementedException();
        }
    }
}
