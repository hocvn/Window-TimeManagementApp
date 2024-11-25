using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
using System.ComponentModel;
using TimeManagementApp.Dao;
using Microsoft.UI.Xaml.Controls;
using Quartz.Util;
using System.Threading.Tasks;
using Microsoft.UI.Text;
using System;

namespace TimeManagementApp.Note
{
    public partial class NoteViewModel : INotifyPropertyChanged
    {
        public MyNote Note { get; set; }
        public Brush CurrentColor { get; set; }
        public bool HasUnsavedChanged { get; set; }

        // This is used to check if the back button is clicked, avoid the dialog to show up twice
        public bool BackButton_Clicked { get; set; } 

        private IDao _dao = new SqlDao();

        public event PropertyChangedEventHandler PropertyChanged;

        
        public void Init()
        {
            CurrentColor = new SolidColorBrush(Colors.Black);
            BackButton_Clicked = false;
        }

        internal async Task Load(RichEditBox Editor)
        {
            await _dao.OpenNote(Note);
            Editor.Document.SetText(TextSetOptions.FormatRtf, Note.Content.Trim());
            Editor.Document.GetText(TextGetOptions.FormatRtf, out string content);
            content = content.Replace(@"\highlight0", string.Empty);
            Note.Content = content;
        }

        internal void Save(RichEditBox Editor)
        {
            Editor.Document.GetText(TextGetOptions.FormatRtf, out string content);
            Note.Content = content;
            _dao.SaveNote(Note);
        }

        internal void Remove()
        {
            // Send note back to NoteMainPage to delete
            MainWindow.NavigationService.Navigate(typeof(NoteMainPage), Note);
        }

        internal void Rename(TextBox textBox)
        {
            string newName = textBox.Text;
            if (newName.IsNullOrWhiteSpace())
            {
                textBox.Text = Note.Name;
                return;
            }
            if (newName != Note.Name) { 
                Note.Name = newName;
                _dao.RenameNote(Note);
            }
        }
    }
}
