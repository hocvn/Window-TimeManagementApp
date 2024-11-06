using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using TimeManagementApp.Dao;
using System.Collections.ObjectModel;
using TimeManagementApp.Helper;
using System.ComponentModel;
using System.Security.AccessControl;
using TimeManagementApp.Services;

namespace TimeManagementApp.Note
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NoteMainPage : Page
    {
        public partial class NoteListViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MyNote> Notes { get; set; }

            public int TotalItems { get; set; }

            private IDao Dao { get; set; }

            public event PropertyChangedEventHandler PropertyChanged;

            public void Init()
            {
                Dao = new MockDao();
                Notes = Dao.GetAllNote();
                TotalItems = Notes.Count;
            }

            public void AddNote(String newNoteName)
            {
                // Create a new note with id is the time when user create it
                String currentTime = TimeHelper.GetTimeString();
                // Remove all spaces of the current time
                var tokens = currentTime.Split(' ');
                currentTime = tokens[0] + tokens[1] + tokens[2] + tokens[3] + tokens[4] + tokens[5];

                MyNote newNote = new MyNote(currentTime, newNoteName);
                RichEditBox editor = new RichEditBox();

                IDao dao = new MockDao();
                dao.SaveNote(editor, newNote);
                // Update the note list
                Notes.Insert(0, newNote);
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

        public NoteListViewModel ViewModel { get; set; } = new NoteListViewModel();

        public NoteMainPage()
        {
            this.InitializeComponent();
            DataContext = App.TimerViewModel;
            ViewModel.Init();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is MyNote myNote)
            {
                ViewModel.DeleteNote(myNote);
            }
        }

        private async void NewNoteButton_Click(object sender, RoutedEventArgs e)
        {
            string newNoteName = NewNoteNameTextBox.Text;
            // Check error when user add a new note
            if (newNoteName.Length == 0)
            {
                await Dialog.ShowContent(this.XamlRoot, "Error", "Note Name cannot be empty!", null, null, "OK");
                return;
            }

            ViewModel.AddNote(newNoteName);
        }

        private void Note_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is MyNote clickedItem)
            {
                //Frame.Navigate(typeof(NotePage), clickedItem);
                MainWindow.NavigationService.Navigate(typeof(NotePage), clickedItem);
            }
        }

        private async void DeleteNoteButton_Click(object sender, RoutedEventArgs e)
        {
            var result = await Dialog.ShowContent(this.XamlRoot, "Remove Note", "Are you sure you want to remove this note?", "Yes", null, "No");
            if (result == ContentDialogResult.Primary)
            {
                var button = sender as Button;

                if (button.DataContext is MyNote note)
                {
                    ViewModel.DeleteNote(note);
                }
            }
            return;
        }
    }
}
