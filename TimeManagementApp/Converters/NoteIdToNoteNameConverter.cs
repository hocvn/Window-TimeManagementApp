using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using TimeManagementApp.Dao;
using TimeManagementApp.Note;

namespace TimeManagementApp.Converters
{
    public class NoteIdToNoteNameConverter : IValueConverter
    {
        public ObservableCollection<MyNote> AllNotes { get; set; }

        public NoteIdToNoteNameConverter()
        {
            IDao dao = new SqlDao();
            AllNotes = dao.GetAllNote();
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is int noteId && noteId != -1)
            {
                var note = AllNotes.FirstOrDefault(n => n.Id == noteId);
                return note?.Name ?? string.Empty;
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
