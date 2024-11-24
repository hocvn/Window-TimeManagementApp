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

namespace TimeManagementApp.Dao
{
    public class MockDao : IDao
    {
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
        public ObservableCollection<MyNote> GetAllNote()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            ObservableCollection<MyNote> notes = new ObservableCollection<MyNote>();
            if (localSettings.Values["mynotes"] is string notesJson)
            {
                //notes = JsonSerializer.Deserialize<ObservableCollection<MyNote>>(notesJson);
            }
            return notes;
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

        public int CreateNote(string noteName)
        {
            ObservableCollection<MyNote> notes = GetAllNote();
            int newId = notes.Count == 0 ? 0 : notes.Max(note => note.Id) + 1;
            MyNote newNote = new MyNote()
            {
                Id = newId,
                Name = noteName
            };
            notes.Insert(0, newNote);
            SaveNotes(notes);
            return newId;
        }


        // Tasks -------------------------------------------------------------
        public ObservableCollection<MyTask> GetAllTasks()
        {
            return new ObservableCollection<MyTask>()
            {
                new MyTask()
                {
                    TaskName = "Task 01",
                    Summarization = "Today, Repeat",
                    StartDateTime = DateTime.Now,
                    DueDateTime = DateTime.Now.AddHours(1),
                },
                new MyTask()
                {
                    TaskName = "Task 02",
                    Summarization = "Tomorrow, Reminder",
                    StartDateTime = DateTime.Now,
                    DueDateTime = DateTime.Now.AddHours(2),
                },
                new MyTask()
                {
                    TaskName = "Task 03",
                    Summarization = "0 of 2, Important",
                    StartDateTime = DateTime.Now,
                    DueDateTime = DateTime.Now.AddHours(3),
                },
                new MyTask()
                {
                    TaskName = "Task 04",
                    Summarization = "Today, Repeat",
                    StartDateTime = DateTime.Now,
                    DueDateTime = DateTime.Now.AddHours(1),
                },
                new MyTask()
                {
                    TaskName = "Task 05",
                    Summarization = "Tomorrow, Reminder",
                    StartDateTime = DateTime.Now,
                    DueDateTime = DateTime.Now.AddHours(2),
                },
                new MyTask()
                {
                    TaskName = "Task 06",
                    Summarization = "0 of 2, Important",
                    StartDateTime = DateTime.Now,
                    DueDateTime = DateTime.Now.AddHours(3),
                },
                new MyTask()
                {
                    TaskName = "Task 07",
                    Summarization = "Today, Repeat",
                    StartDateTime = DateTime.Now,
                    DueDateTime = DateTime.Now.AddHours(1),
                },
                new MyTask()
                {
                    TaskName = "Task 08",
                    Summarization = "Tomorrow, Reminder",
                    StartDateTime = DateTime.Now,
                    DueDateTime = DateTime.Now.AddHours(2),
                },
                new MyTask()
                {
                    TaskName = "Task 09",
                    Summarization = "0 of 2, Important",
                    StartDateTime = DateTime.Now,
                    DueDateTime = DateTime.Now.AddHours(3),
                },
                new MyTask()
                {
                    TaskName = "Task 10",
                    Summarization = "Today, Repeat",
                    StartDateTime = DateTime.Now,
                    DueDateTime = DateTime.Now.AddHours(1),
                },
                new MyTask()
                {
                    TaskName = "Task 11",
                    Summarization = "Tomorrow, Reminder",
                    StartDateTime = DateTime.Now,
                    DueDateTime = DateTime.Now.AddHours(2),
                },
                new MyTask()
                {
                    TaskName = "Task 12",
                    Summarization = "0 of 2, Important",
                    StartDateTime = DateTime.Now,
                    DueDateTime = DateTime.Now.AddHours(3),
                },
                new MyTask()
                {
                    TaskName = "Task 13",
                    Summarization = "Today, Repeat",
                    StartDateTime = DateTime.Now,
                    DueDateTime = DateTime.Now.AddDays(1),
                },
                new MyTask()
                {
                    TaskName = "Task 14",
                    Summarization = "Tomorrow, Reminder",
                    StartDateTime = DateTime.Now,
                    DueDateTime = DateTime.Now.AddDays(2),
                },
                new MyTask()
                {
                    TaskName = "Task 15",
                    Summarization = "0 of 2, Important",
                    StartDateTime = DateTime.Now,
                    DueDateTime = DateTime.Now.AddDays(3),
                },
            };
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

        public void SaveSelectedBackground(LinearGradientBrush selectedBrush)
        {
            throw new NotImplementedException();
        }

        public LinearGradientBrush LoadSavedBackground(double offset1, double offset2)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<MyTask> GetTasksForDate(DateTime date)
        {
            return new ObservableCollection<MyTask>
            {
                new MyTask { TaskName = "Task 1", Summarization = "Summary 1", StartDateTime = DateTime.Now, DueDateTime = DateTime.Now.AddDays(1) },
                new MyTask { TaskName = "Task 2", Summarization = "Summary 2", StartDateTime = DateTime.Now, DueDateTime = DateTime.Now.AddDays(2) },
                new MyTask { TaskName = "Task 3", Summarization = "Summary 3", StartDateTime = DateTime.Now, DueDateTime = DateTime.Now.AddDays(3) }
            };
        }
    }
}

