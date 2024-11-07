using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
using System.ComponentModel;
using TimeManagementApp.Dao;
using Microsoft.UI.Xaml.Controls;
using TimeManagementApp.Helper;
using Microsoft.UI.Xaml;
using Quartz.Util;

namespace TimeManagementApp.Note
{
    public partial class NoteViewModel : INotifyPropertyChanged
    {
        public MyNote Note { get; set; }
        public Brush CurrentColor { get; set; }


        public bool BackButton_Clicked { get; set; }

        private IDao _dao = new MockDao();

        public event PropertyChangedEventHandler PropertyChanged;

        
        public void Init()
        {
            CurrentColor = new SolidColorBrush(Colors.Black);
            BackButton_Clicked = false;
        }

        internal void Load(RichEditBox Editor)
        {
            _dao.OpenNote(Editor, Note);
        }

        internal void Save(RichEditBox Editor)
        {
            _dao.SaveNote(Editor, Note);
        }

        internal async void Remove(XamlRoot xamlRoot)
        {
            var result = await Dialog.ShowContent(xamlRoot, "Remove Note", "Are you sure you want to remove this note?", "Yes", null, "No");
            if (result == ContentDialogResult.Primary)
            {
                // Send note back to NoteMainPage to delete
                MainWindow.NavigationService.Navigate(typeof(NoteMainPage), Note);
            }
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
