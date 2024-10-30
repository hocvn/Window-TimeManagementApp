using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
        private ObservableCollection<MyNote> notes = new ObservableCollection<MyNote>
                {
                    new()
                    {
                        Name = "Note 01",
                    },
                    new()
                    {
                        Name = "Note 02",
                    },
                    new()
                    {
                        Name = "Note 03",
                    },
                };
        public ObservableCollection<MyNote> GetAllNote()
        {
            return notes; 
        }

        public MyNote GetNote()
        {
            return new MyNote()
            {
                Name = "Note 02",
            };
        }

        // Open a file in local folder and load its content to RichEditBox
        public async void OpenRtf(RichEditBox editor, MyNote note)
        {
            string fileName = "Document.rtf";

            // Kiểm tra file có tồn tại không
            StorageFile file = await ApplicationData.Current.LocalFolder.TryGetItemAsync(fileName) as StorageFile;
            if (file == null)
            {
                // Thông báo nếu file không tồn tại
                var dialog = new ContentDialog
                {
                    Title = "Failed.",
                    Content = "Failed to open note.",
                    CloseButtonText = "OK"
                };
                await dialog.ShowAsync();
                return;
            }

            // Mở file và tải nội dung vào RichEditBox
            using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read))
            {
                editor.Document.LoadFromStream(TextSetOptions.FormatRtf, stream);
            }
        }

        // Save the content of RichEditBox to a file in local folder
        public async void SaveRtf(RichEditBox editor, MyNote note)
        {
            string fileName = note.Id;
            StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

            // Lưu nội dung từ RichEditBox vào stream và ghi vào file
            using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                editor.Document.SaveToStream(TextGetOptions.FormatRtf, stream);
            }

            // Thông báo lưu thành công
            var dialog = new ContentDialog
            {
                Title = "Save successful.",
                Content = "Your note have been saved.",
                CloseButtonText = "OK"
            };
            await dialog.ShowAsync();
        }
    }
}
