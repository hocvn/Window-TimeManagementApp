using Microsoft.UI.Text;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using TimeManagementApp.Note;
using Windows.Storage;
using Windows.Storage.Streams;
using TimeManagementApp.ToDo;
using System.Security.Cryptography;
using TimeManagementApp.Helper;
using static TimeManagementApp.UserCredential;
using TimeManagementApp.Timer;
using Microsoft.UI.Xaml.Media;
using Windows.Foundation;
using OfficeOpenXml;

namespace TimeManagementApp.Dao
{
    public class MockDao : IDao
    {
        public MockDao()
        {
            // do nothing, just for using parameterized constructor
        }

        private readonly string _filePath;

        public MockDao(string filePath)
        {
            _filePath = filePath;
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
        }


        // Users ------------------------------------------------------------------------------------
        private (string, string) EncryptPassword(string password)
        {
            // Encrypt the password
            var passwordInBytes = Encoding.UTF8.GetBytes(password);
            var entropyInBytes = new byte[20];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(entropyInBytes);
            }

            var encryptedPasswordInBytes = ProtectedData.Protect(
                   passwordInBytes,
                   entropyInBytes,
                   DataProtectionScope.CurrentUser
            );

            var encryptedPasswordBase64 = Convert.ToBase64String(encryptedPasswordInBytes);
            var entropyInBase64 = Convert.ToBase64String(entropyInBytes);

            return (encryptedPasswordBase64, entropyInBase64);
        }

        public void CreateUser(string username, string password, string email)
        {
            var usersDataJson = StorageHelper.GetSetting("usersData");
            Dictionary<string, UserInfo> usersData;

            if (String.IsNullOrEmpty(usersDataJson))
            {
                usersData = new Dictionary<string, UserInfo>();
            }
            else
            {
                usersData = JsonSerializer.Deserialize<Dictionary<string, UserInfo>>(usersDataJson);
            }

            (string encryptedPasswordBase64, string entropyInBase64) = EncryptPassword(password);
            UserInfo userInfo = new UserInfo(encryptedPasswordBase64, entropyInBase64, email);
            usersData[username] = userInfo;
            usersDataJson = System.Text.Json.JsonSerializer.Serialize(usersData);
            StorageHelper.SaveSetting("usersData", usersDataJson);
        }

        public bool CheckCredential(string username, string password)
        {
            var usersDataJson = StorageHelper.GetSetting("usersData");
            Dictionary<string, UserInfo> usersData;

            if (String.IsNullOrEmpty(usersDataJson))
            {
                return false; // There is no data stored for any user
            }

            usersData = JsonSerializer.Deserialize<Dictionary<string, UserInfo>>(usersDataJson);

            // Retrieve the encrypted password and entropy
            if (usersData.ContainsKey(username))
            {
                var UserInfo = usersData[username];
                var encryptedPasswordBase64 = UserInfo.Password;
                var entropyBase64 = UserInfo.Entropy;

                if (encryptedPasswordBase64 == null || entropyBase64 == null)
                {
                    return false;
                }
                var encryptedPasswordInBytes = Convert.FromBase64String(encryptedPasswordBase64);
                var entropyInBytes = Convert.FromBase64String(entropyBase64);

                // Decrypt the password
                var decryptedPasswordInBytes = ProtectedData.Unprotect(
                    encryptedPasswordInBytes,
                    entropyInBytes,
                    DataProtectionScope.CurrentUser
                );

                string decryptedPassword = Encoding.UTF8.GetString(decryptedPasswordInBytes);
                return decryptedPassword == password;
            }
            return false;
        }

        public bool IsUsernameInUse(string username)
        {
            var usersDataJson = StorageHelper.GetSetting("usersData");
            Dictionary<string, UserInfo> usersData;

            if (String.IsNullOrEmpty(usersDataJson))
            {
                return false; // There is no data stored for any user
            }

            usersData = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, UserInfo>>(usersDataJson);

            for (int i = 0; i < usersData.Count; i++)
            {
                if (usersData.ElementAt(i).Key == username)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsEmailInUse(string email)
        {
            var usersDataJson = StorageHelper.GetSetting("usersData");
            Dictionary<string, UserInfo> usersData;

            if (String.IsNullOrEmpty(usersDataJson))
            {
                return false; // There is no data stored for any user
            }

            usersData = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, UserInfo>>(usersDataJson);

            for (int i = 0; i < usersData.Count; i++)
            {
                if (usersData.ElementAt(i).Value.Email == email)
                {
                    return true;
                }
            }

            return false;
        }

        public string GetPassword(string username)
        {
            var usersDataJson = StorageHelper.GetSetting("usersData");
            Dictionary<string, UserInfo> usersData;

            if (String.IsNullOrEmpty(usersDataJson))
            {
                return null; // There is no data stored for any user
            }

            usersData = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, UserInfo>>(usersDataJson);

            // Retrieve the encrypted password and entropy
            if (usersData.ContainsKey(username))
            {
                var UserInfo = usersData[username];
                var encryptedPasswordBase64 = UserInfo.Password;
                var entropyBase64 = UserInfo.Entropy;

                if (encryptedPasswordBase64 == null || entropyBase64 == null)
                {
                    return null;
                }
                var encryptedPasswordInBytes = Convert.FromBase64String(encryptedPasswordBase64);
                var entropyInBytes = Convert.FromBase64String(entropyBase64);

                // Decrypt the password
                var decryptedPasswordInBytes = ProtectedData.Unprotect(
                    encryptedPasswordInBytes,
                    entropyInBytes,
                    DataProtectionScope.CurrentUser
                );

                return Encoding.UTF8.GetString(decryptedPasswordInBytes);
            }
            return null;
        }

        public string GetUsername(string email)
        {
            var usersDataJson = StorageHelper.GetSetting("usersData");
            Dictionary<string, UserInfo> usersData;

            if (String.IsNullOrEmpty(usersDataJson))
            {
                return null; // There is no data stored for any user
            }

            usersData = JsonSerializer.Deserialize<Dictionary<string, UserInfo>>(usersDataJson);

            for (int i = 0; i < usersData.Count; i++)
            {
                if (usersData.ElementAt(i).Value.Email == email)
                {
                    return usersData.ElementAt(i).Key;
                }
            }

            return null;
        }

        public void ResetPassword(string username, string password, string email)
        {
            CreateUser(username, password, email);
        }


        // Notes ------------------------------------------------------------------------------------
        public ObservableCollection<MyNote> GetAllNote()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            ObservableCollection<MyNote> notes = new ObservableCollection<MyNote>();
            if (localSettings.Values["mynotes"] is string notesJson)
            {
                notes = JsonSerializer.Deserialize<ObservableCollection<MyNote>>(notesJson);
            }
            return notes;
        }

        public void SaveNotes(ObservableCollection<MyNote> notes)
        {
            string notesJson = JsonSerializer.Serialize(notes);
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values["mynotes"] = notesJson;
        }

        // Open a file in local folder and load its content to RichEditBox
        public async Task OpenNote(RichEditBox editor, MyNote note)
        {
            try
            {
                string fileName = note.Id + ".rtf";

                // Check if file exists
                StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName) as StorageFile;

                // Open file and load its content to RichEditBox
                using IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read);
                editor.Document.LoadFromStream(TextSetOptions.FormatRtf, stream);
            }
            catch (FileNotFoundException)
            {
                Debug.WriteLine("File not found.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading file: {ex.Message}");

            }
        }

        // Save the content of RichEditBox to a file in local folder
        public async void SaveNote(RichEditBox editor, MyNote note)
        {
            string fileName = note.Id + ".rtf";
            StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

            // Save the content of RichEditBox to a stream and write to file
            using IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite);
            editor.Document.SaveToStream(TextGetOptions.FormatRtf, stream);
        }

        // Delete a file which store note in local folder
        public async void DeleteNote(MyNote note)
        {
            try
            {
                string fileName = note.Id + ".rtf";
                StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName) as StorageFile;

                // Delete the file
                await file.DeleteAsync();
            }
            catch (FileNotFoundException)
            {
                Debug.WriteLine("File not found.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deleting file: {ex.Message}");
            }
        }

        public void RenameNote(MyNote note)
        {
            ObservableCollection <MyNote> notes = GetAllNote();
            foreach (MyNote item in notes)
            {
                if (item.Id == note.Id)
                {
                    item.Name = note.Name;
                    break;
                }
            }
            SaveNotes(notes);
        }


        // Tasks ------------------------------------------------------------------------------------
        public ObservableCollection<MyTask> GetAllTasks()
        {
            return LoadTasksFromExcel(_filePath);
        }

        public void InsertTask(MyTask task)
        {
            var tasks = LoadTasksFromExcel(_filePath);
            task.TaskId = tasks.Any() ? tasks.Max(t => t.TaskId) + 1 : 1; // Assign new unique ID based on the max existing ID
            tasks.Add(task);
            SaveTasksToExcel(_filePath, tasks);
        }

        public void DeleteTask(MyTask task)
        {
            var tasks = LoadTasksFromExcel(_filePath);
            var taskToDelete = tasks.FirstOrDefault(t => t.TaskId == task.TaskId);
            if (taskToDelete != null)
            {
                tasks.Remove(taskToDelete);
                SaveTasksToExcel(_filePath, tasks);
            }
        }

        public void UpdateTask(MyTask task)
        {
            var tasks = LoadTasksFromExcel(_filePath);
            var taskToUpdate = tasks.FirstOrDefault(t => t.TaskId == task.TaskId);
            if (taskToUpdate != null)
            {
                var index = tasks.IndexOf(taskToUpdate);
                tasks[index] = task;
                SaveTasksToExcel(_filePath, tasks);
            }
        }

        private ObservableCollection<MyTask> LoadTasksFromExcel(string filePath)
        {
            var tasks = new ObservableCollection<MyTask>();

            if (!File.Exists(filePath))
            {
                return tasks;
            }

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                if (worksheet == null || worksheet.Dimension == null)
                {
                    return tasks;
                }

                for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                {
                    var task = new MyTask
                    {
                        TaskId = int.Parse(worksheet.Cells[row, 1].Text),
                        TaskName = worksheet.Cells[row, 2].Text,
                        DueDateTime = DateTime.Parse(worksheet.Cells[row, 3].Text),
                        Description = worksheet.Cells[row, 4].Text,
                        IsCompleted = bool.Parse(worksheet.Cells[row, 5].Text),
                        IsImportant = bool.Parse(worksheet.Cells[row, 6].Text),
                        RepeatOption = worksheet.Cells[row, 7].Text,
                        ReminderTime = DateTime.Parse(worksheet.Cells[row, 8].Text),
                        NoteId = int.Parse(worksheet.Cells[row, 9].Text)
                    };
                    tasks.Add(task);
                }
            }

            return tasks;
        }

        private void SaveTasksToExcel(string filePath, ObservableCollection<MyTask> tasks)
        {
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets.FirstOrDefault() ?? package.Workbook.Worksheets.Add("Tasks");

                worksheet.Cells[1, 1].Value = "TaskId";
                worksheet.Cells[1, 2].Value = "TaskName";
                worksheet.Cells[1, 3].Value = "DueDateTime";
                worksheet.Cells[1, 4].Value = "Description";
                worksheet.Cells[1, 5].Value = "IsCompleted";
                worksheet.Cells[1, 6].Value = "IsImportant";
                worksheet.Cells[1, 7].Value = "RepeatOption";
                worksheet.Cells[1, 8].Value = "ReminderTime";
                worksheet.Cells[1, 9].Value = "NoteId";

                int row = 2;
                foreach (var task in tasks)
                {
                    worksheet.Cells[row, 1].Value = task.TaskId;
                    worksheet.Cells[row, 2].Value = task.TaskName;
                    worksheet.Cells[row, 3].Value = task.DueDateTime.ToString();
                    worksheet.Cells[row, 4].Value = task.Description;
                    worksheet.Cells[row, 5].Value = task.IsCompleted.ToString();
                    worksheet.Cells[row, 6].Value = task.IsImportant.ToString();
                    worksheet.Cells[row, 7].Value = task.RepeatOption;
                    worksheet.Cells[row, 8].Value = task.ReminderTime.ToString();
                    worksheet.Cells[row, 9].Value = task.NoteId;
                    row++;
                }

                package.Save();
            }
        }


        public ObservableCollection<MyTask> GetTodayTask()
        {
            ObservableCollection<MyTask> todayTasks = new();
            foreach (MyTask task in GetAllTasks())
            {
                if (task.DueDateTime.Date == DateTime.Now.Date)
                {
                    todayTasks.Add(task);
                }
            }
            return todayTasks;
        }

        public ObservableCollection<MyTask> GetTasksForDate(DateTime date)
        {
            throw new NotImplementedException();
        }


        // Timer ------------------------------------------------------------------------------------
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


        // Background ------------------------------------------------------------------------------------
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
    }
}

