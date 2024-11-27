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
using Microsoft.UI.Xaml.Media.Imaging;
using System.ComponentModel;
using System.IO;

namespace TimeManagementApp.Home
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public partial class HomeViewModel : INotifyPropertyChanged
        {
            public String Greeting { get; set; }
            public ImageSource Icon { get; set; }

            public int TodayTasksTotal = 0;

            public int NotesTotal = 0;
            public ObservableCollection<MyTask> TodayTasks { get; set; }

            public ObservableCollection<MyNote> NoteList { get; set; }

            private IDao dao;

            public void Init()
            {
                var baseDirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory); // bin\x64\Debug\net9.0-windows10.0.22621.0\win-x64\AppX\
                var filePath = Path.Combine(baseDirectory.FullName, "..", "..", "..", "..", "..", "..", "Dao", "tasks.xlsx");
                dao = new MockDao(filePath);

                NoteList = dao.GetAllNote();
                TodayTasks = dao.GetTodayTask();

                TodayTasksTotal = TodayTasks.Count;
                NotesTotal = NoteList.Count;

                // Display maximum 6 tasks and notes
                if (TodayTasks.Count > 6)
                {
                    TodayTasks = new ObservableCollection<MyTask>(TodayTasks.Take(6));
                }

                if (NoteList.Count > 6)
                {
                    NoteList = new ObservableCollection<MyNote>(NoteList.Take(6));
                }

                string timeOfDay = TimeHelper.GetTimesOfDay();
                Greeting = $"Good {timeOfDay}";

                if (timeOfDay == "Morning")
                {
                    Icon = (BitmapImage)Application.Current.Resources["MorningImage"];
                }
                else if (timeOfDay == "Afternoon")
                {
                    Icon = (BitmapImage)Application.Current.Resources["AfternoonImage"];
                }
                else if (timeOfDay == "Evening")
                {
                    Icon = (BitmapImage)Application.Current.Resources["EveningImage"];
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
