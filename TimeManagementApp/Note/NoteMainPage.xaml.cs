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
    /// This is the main page of the Note feature, responsible for displaying all tasks.
    /// </summary>
    public sealed partial class NoteMainPage : Page
    {
        public partial class NoteListViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MyNote> Notes { get; set; }

            public int TotalItems { get; set; }

            private IDao _dao { get; set; }

            public event PropertyChangedEventHandler PropertyChanged;

            public void Init()
            {
                _dao = new SqlDao();
                Notes = _dao.GetAllNote();
                TotalItems = Notes.Count;
            }

            public void AddNote(String newNoteName)
            {
                int id = _dao.CreateNote(newNoteName);
                MyNote newNote = new MyNote(id, newNoteName);
                // Update ViewModel
                Notes.Insert(0, newNote);
                TotalItems++;
            }

            public void DeleteNote(MyNote note)
            {
                _dao.DeleteNote(note);
                Notes.Remove(note);
                TotalItems--;
            }
        }

        public NoteListViewModel ViewModel { get; set; } = new NoteListViewModel();

        public NoteMainPage()
        {
            this.InitializeComponent();
            ViewModel.Init();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is MyNote myNote)
            {
                ViewModel.DeleteNote(myNote);
            }
            NumberOfNote.Text = "Total".GetLocalized() + ": " + ViewModel.TotalItems.ToString();
        }

        private async void NewNoteButton_Click(object sender, RoutedEventArgs e)
        {
            string newNoteName = NewNoteNameTextBox.Text;
            // Check error when user add a new note
            if (newNoteName.Length == 0)
            {
                await Dialog.ShowContent
                (
                    this.XamlRoot, 
                    "Error".GetLocalized(), 
                    "Note_Name_cannot_be_empty".GetLocalized(), 
                    null, 
                    null, 
                    "OK".GetLocalized()
                );
                return;
            }

            ViewModel.AddNote(newNoteName);
        }

        private void Note_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is MyNote clickedItem)
            { 
                MainWindow.NavigationService.Navigate(typeof(NotePage), clickedItem);
            }
        }

        private async void DeleteNoteButton_Click(object sender, RoutedEventArgs e)
        {
            var result = await Dialog.ShowContent
            (
                this.XamlRoot, 
                "Delete_note".GetLocalized(), 
                "Are_you_sure_you_want_to_remove_this_note".GetLocalized(), 
                "Yes".GetLocalized(), 
                null, 
                "No".GetLocalized()
            );
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
