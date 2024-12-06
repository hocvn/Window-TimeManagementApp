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

            public const int MAX_ITEM_DISPLAYED = 6;
            public ObservableCollection<MyTask> TodayTasks { get; set; }

            public ObservableCollection<MyNote> NoteList { get; set; }

            private IDao _dao = new SqlDao();

            public void Init()
            {
                NoteList = _dao.GetAllNote();
                TodayTasks = _dao.GetTodayTask();

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
            throw new NotImplementedException();
        }

        private void GridViewNoteItem_Click(object sender, ItemClickEventArgs e)
        {
            MyNote note = (MyNote)e.ClickedItem;
            MainWindow.NavigationService.Navigate(typeof(NotePage), note);
        }
    }
}
