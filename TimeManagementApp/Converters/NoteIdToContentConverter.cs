using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Microsoft.UI.Xaml.Controls;
using TimeManagementApp.Dao;
using TimeManagementApp.Note;
using Microsoft.UI.Text;

namespace TimeManagementApp.Converters
{
    public class NoteIdToContentConverter : IValueConverter
    {
        public ObservableCollection<MyNote> AllNotes { get; set; }

        public NoteIdToContentConverter()
        {
            IDao dao = new SqlDao();
            AllNotes = dao.GetAllNote();
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is int noteId && noteId != -1)
            {
                var note = AllNotes.FirstOrDefault(n => n.Id == noteId);
                if (note != null)
                {
                    string plainText = ConvertRtfToPlainText(note.Content);
                    return plainText;
                }
            }
            return string.Empty;
        }

        private string ConvertRtfToPlainText(string rtf)
        {
            if (string.IsNullOrEmpty(rtf))
            {
                return string.Empty;
            }

            string plainText = string.Empty;
            RichEditBox richEditBox = new RichEditBox();
            richEditBox.Document.SetText(TextSetOptions.FormatRtf, rtf);
            richEditBox.Document.GetText(TextGetOptions.None, out plainText);

            return plainText.Trim();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
