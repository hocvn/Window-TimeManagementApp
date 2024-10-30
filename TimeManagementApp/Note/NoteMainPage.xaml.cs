using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using TimeManagementApp.Dao;
using System.Collections.ObjectModel;
using System.Diagnostics;
using TimeManagementApp.Helper;

namespace TimeManagementApp.Note
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NoteMainPage : Page
    {
        public class NoteViewModel
        {
            public ObservableCollection<MyNote> Notes { get; set; }

            public int TotalItems { get; set; }

            public int TotalPages { get; set; }

            public int RowsPerPage { get; set; }

            public int CurrentPage { get; set; }

            public void Init()
            {
                IDao dao = new MockDao();
                Notes = dao.GetAllNote();

                CurrentPage = 1;
                TotalItems = Notes.Count;
                RowsPerPage = 10;
                TotalPages = (TotalItems / RowsPerPage) + (TotalItems % RowsPerPage == 0 ? 0 : 1);
            }
        }

        public NoteViewModel ViewModel { get; set; }

        public NoteMainPage()
        {
            this.InitializeComponent();
            ViewModel = new NoteViewModel();
            ViewModel.Init();
        }

        private void NewNoteButton_Click(object sender, ItemClickEventArgs e)
        {
            string newNoteName = NewNoteNameTextBox.Text;
            if (newNoteName.Length == 0)
            {
                return;
            }
            //
            MyNote newNote = new MyNote();
            String currentTime = TimeHelper.GetTime();
            // Remove all spaces
            var tokens = currentTime.Split(' ');
            currentTime = tokens[0] + tokens[1] + tokens[2] + tokens[3] + tokens[4] + tokens[5];

            newNote.Id = currentTime;
            newNote.Name = newNoteName;
        }

        private void Note_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clickedItem = e.ClickedItem as MyNote;
            if (clickedItem != null)
            {
                Frame.Navigate(typeof(NotePage), clickedItem);
            }
        }
    }
}
