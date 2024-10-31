using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using TimeManagementApp.Dao;
using System.Collections.ObjectModel;
using TimeManagementApp.Helper;
using System.ComponentModel;

namespace TimeManagementApp.Note
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NoteMainPage : Page
    {
        public partial class NoteViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MyNote> Notes { get; set; }

            public int TotalItems { get; set; }

            public event PropertyChangedEventHandler PropertyChanged;

            public void Init()
            {
                IDao dao = new MockDao();
                Notes = dao.GetAllNote();

                TotalItems = Notes.Count;
            }

            public void AddNote(String newNoteName)
            {
                // Create a new note with id is the time when user create it
                String currentTime = TimeHelper.GetTime();
                // Remove all spaces of the current time
                var tokens = currentTime.Split(' ');
                currentTime = tokens[0] + tokens[1] + tokens[2] + tokens[3] + tokens[4] + tokens[5];

                MyNote newNote = new MyNote(currentTime, newNoteName);
                RichEditBox editor = new RichEditBox();

                IDao dao = new MockDao();
                dao.SaveNote(editor, newNote);
                // Update the note list
                Notes.Add(newNote);
                dao.SaveNotes(Notes);
                // Update ViewModel
                TotalItems++;
            }

            public void DeleteNote(MyNote note)
            {
                Notes.Remove(note);
                TotalItems--;
                IDao dao = new MockDao();
                dao.SaveNotes(Notes);
            }
        }

        public NoteViewModel ViewModel { get; set; } = new NoteViewModel();

        public NoteMainPage()
        {
            this.InitializeComponent();
            ViewModel.Init();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is MyNote note)
            {
                ViewModel.DeleteNote(note);
            }
        }

        private async void NewNoteButton_Click(object sender, RoutedEventArgs e)
        {
            string newNoteName = NewNoteNameTextBox.Text;
            // Check error when user add a new note
            if (newNoteName.Length == 0)
            {
                var dialog = new ContentDialog()
                {
                    Title = "Error",
                    Content = "Please enter a name for the new note",
                    CloseButtonText = "Ok",
                    XamlRoot = this.Content.XamlRoot
                };
                await dialog.ShowAsync();
                return;
            }

            ViewModel.AddNote(newNoteName);
        }

        private void Note_ItemClick(object sender, ItemClickEventArgs e)
        {
            MyNote clickedItem = e.ClickedItem as MyNote;
            if (clickedItem != null)
            {
                Frame.Navigate(typeof(NotePage), clickedItem);
            }
        }

        private async void DeleteNoteButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ContentDialog
            {
                Title = "Delete Note",
                Content = "Are you sure you want to delete this note?",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel",
                XamlRoot = this.XamlRoot
            };
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                var button = sender as Button;
                MyNote note = button.DataContext as MyNote;

                if (note != null)
                {
                    ViewModel.DeleteNote(note);
                }
            }
            return;
        }
    }
}
