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
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using TimeManagementApp.Helper;
using TimeManagementApp.Services;

namespace TimeManagementApp.Dao
{
    public class MockDao : IDao
    {

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

        public void CreateUser(string username, string password, string email)
        {
            throw new NotImplementedException();
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
        
        public ObservableCollection<MyTask> GetAllTasks()
        {
            return new ObservableCollection<MyTask>()
            {
                new MyTask()
                {
                    TaskName = "Task 01",
                    TaskDescription = "Description 01",
                    StartDateTime = DateTime.Now,
                    DueDateTime = DateTime.Now.AddHours(1),
                },
                new MyTask()
                {
                    TaskName = "Task 02",
                    TaskDescription = "Description 02",
                    StartDateTime = DateTime.Now,
                    DueDateTime = DateTime.Now.AddHours(2),
                },
                new MyTask()
                {
                    TaskName = "Task 03",
                    TaskDescription = "Description 03",
                    StartDateTime = DateTime.Now,
                    DueDateTime = DateTime.Now.AddHours(3),
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

        public bool CheckUser(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}

