using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TimeManagementApp.Note;
using Windows.Data.Xml.Dom;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;


namespace TimeManagementApp.Dao
{
    class MockDao : IDao
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

        public void SaveNotes(ObservableCollection<MyNote> notes)
        {
            string notesJson = JsonSerializer.Serialize(notes);
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values["mynotes"] = notesJson;
        }

        // Open a file in local folder and load its content to RichEditBox
        public async void OpenNote(RichEditBox editor, MyNote note)
        {
            try
            {
                string fileName = note.Id + ".txt";

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
            string fileName = note.Id + ".txt";
            StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

            // Save the content of RichEditBox to a stream and write to file
            using IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite);
            editor.Document.SaveToStream(TextGetOptions.FormatRtf, stream);
        }

        // Delete a file which store note in local folder
        public async void DeleteNote(MyNote note)
        {
            string fileName = note.Id + ".txt";
            StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

            // Delete the file
            await file.DeleteAsync();

            // Update the note list
            ObservableCollection<MyNote> notes = GetAllNote();
            foreach (var item in notes)
            {
                if (item.Id == note.Id)
                {
                    notes.Remove(item);
                    break;
                }
            }
            SaveNotes(notes);
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
    }
}
