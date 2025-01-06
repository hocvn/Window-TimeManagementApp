using System;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System.Collections.ObjectModel;
using TimeManagementApp.Dao;
using TimeManagementApp.ToDo;
using TimeManagementApp.Note;
using TimeManagementApp.Helper;
using System.ComponentModel;
using Microsoft.UI.Xaml.Media.Imaging;
using System.IO;

namespace TimeManagementApp.Home
{
    /// <summary>
    /// This page is responsible for displaying the home page, including some today tasks and notes.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public partial class HomeViewModel : INotifyPropertyChanged
        {
            public String Greeting { get; set; }
            public ImageSource Icon { get; set; }

            public int TodayTasksTotal = 0;

            public int NotesTotal = 0;

            public const int MAX_ITEM_DISPLAYED = 6;
            public ObservableCollection<MyTask> TodayTasks { get; set; }

            public ObservableCollection<MyNote> NoteList { get; set; }

            private IDao dao;

            public void Init()
            {
                //var baseDirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory); // bin\x64\Debug\net9.0-windows10.0.22621.0\win-x64\AppX\
                //var filePath = Path.Combine(baseDirectory.FullName, "..", "..", "..", "..", "..", "..", "Dao", "tasks.xlsx");
                //dao = new MockDao(filePath);
                dao = new SqlDao();

                NoteList = dao.GetAllNote();
                TodayTasks = dao.GetTodayTask();

                TodayTasksTotal = TodayTasks.Count;
                NotesTotal = NoteList.Count;

                // Display maximum 6 tasks and notes
                if (TodayTasks.Count > MAX_ITEM_DISPLAYED)
                {
                    TodayTasks = new ObservableCollection<MyTask>(TodayTasks.Take(MAX_ITEM_DISPLAYED));
                }

                if (NoteList.Count > MAX_ITEM_DISPLAYED)
                {
                    NoteList = new ObservableCollection<MyNote>(NoteList.Take(MAX_ITEM_DISPLAYED));
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
        }

        public HomeViewModel ViewModel { get; set; }

        public HomePage()
        {
            this.InitializeComponent();
            ViewModel = new HomeViewModel();
            ViewModel.Init();
            this.Loaded += HomePage_Loaded;
        }

        private void HomePage_Loaded(object sender, RoutedEventArgs e)
        {
            string timeOfDay = TimeHelper.GetTimesOfDay();

            if (timeOfDay == "Morning")
            {
                ViewModel.Greeting = "goodMorning".GetLocalized();
                ViewModel.Icon = (BitmapImage)Application.Current.Resources["MorningImage"];
            }
            else if (timeOfDay == "Afternoon")
            {
                ViewModel.Greeting = "goodAfternoon".GetLocalized();
                ViewModel.Icon = (BitmapImage)Application.Current.Resources["AfternoonImage"];
            }
            else if (timeOfDay == "Evening")
            {
                ViewModel.Greeting = "goodEvening".GetLocalized();
                ViewModel.Icon = (BitmapImage)Application.Current.Resources["EveningImage"];
            }
        }

        private void GridViewTaskItem_Click(object sender, ItemClickEventArgs e)
        {
            MyTask task = (MyTask)e.ClickedItem;
            MainWindow.NavigationService.Navigate(typeof(EditToDoPage), task);
        }

        private void GridViewNoteItem_Click(object sender, ItemClickEventArgs e)
        {
            MyNote note = (MyNote)e.ClickedItem;
            MainWindow.NavigationService.Navigate(typeof(NotePage), note);
        }
    }
}
