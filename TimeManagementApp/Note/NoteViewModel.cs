using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
using System.ComponentModel;
using TimeManagementApp.Dao;
using Microsoft.UI.Xaml.Controls;
using TimeManagementApp.Helper;
using Microsoft.UI.Xaml;
using Quartz.Util;
using System.Threading.Tasks;

namespace TimeManagementApp.Note
{
    public partial class NoteViewModel : INotifyPropertyChanged
    {
        public MyNote Note { get; set; }
        public Brush CurrentColor { get; set; }
        public bool HasUnsavedChanged { get; set; }

        // this is used to check if the back button is clicked, avoid the dialog to show up when the back button is clicked
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
            await _dao.OpenNote(Editor, Note);
        }

        internal void Save(RichEditBox Editor)
        {
            _dao.SaveNote(Editor, Note);
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
