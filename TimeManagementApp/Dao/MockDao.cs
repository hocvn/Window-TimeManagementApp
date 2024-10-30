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
        public async void OpenRtf(RichEditBox editor, MyNote note)
        {
            try
            {
                string fileName = note.Id + ".txt";

                // Kiểm tra file có tồn tại không
                StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName) as StorageFile;

                // Mở file và tải nội dung vào RichEditBox
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
        public async void SaveRtf(RichEditBox editor, MyNote note)
        {
            string fileName = note.Id + ".txt";
            StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

            // Lưu nội dung từ RichEditBox vào stream và ghi vào file
            using IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite);
            editor.Document.SaveToStream(TextGetOptions.FormatRtf, stream);
        }
    }
}
