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
using System.Collections.ObjectModel;
using TimeManagementApp.Dao;
using TimeManagementApp.ToDo;
using TimeManagementApp.Note;
using System.Collections.Specialized;


namespace TimeManagementApp.Home
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public class HomeViewModel : INotifyCollectionChanged
        {
            public ObservableCollection<MyNote> NoteList { get; set; } = [];
            public ObservableCollection<MyTask> TodayTask { get; set; } = [];

            IDao dao = new MockDao();

            public void Init()
            {
                NoteList = dao.GetAllNote();

                TodayTask = new ObservableCollection<MyTask>
                {
                    new MyTask { TaskName = "Task 1", TaskDescription = "Description 1", StartDateTime = DateTime.Now, DueDateTime = DateTime.Now },
                    new MyTask { TaskName = "Task 2", TaskDescription = "Description 2", StartDateTime = DateTime.Now, DueDateTime = DateTime.Now },
                    new MyTask { TaskName = "Task 3", TaskDescription = "Description 3", StartDateTime = DateTime.Now, DueDateTime = DateTime.Now },
                    new MyTask { TaskName = "Task 4", TaskDescription = "Description 4", StartDateTime = DateTime.Now, DueDateTime = DateTime.Now },
                    new MyTask { TaskName = "Task 5", TaskDescription = "Description 5", StartDateTime = DateTime.Now, DueDateTime = DateTime.Now }
                };
            }

            public event NotifyCollectionChangedEventHandler CollectionChanged;
        }

        HomeViewModel ViewModel = new HomeViewModel();

        public HomePage()
        {
            this.InitializeComponent();
            ViewModel.Init();
        }

        private void GridViewTaskItem_Click(object sender, ItemClickEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void GridViewNoteItem_Click(object sender, ItemClickEventArgs e)
        {
            MyNote note = (MyNote)e.ClickedItem;
            Frame.Navigate(typeof(NotePage), note);
        }
    }
}
