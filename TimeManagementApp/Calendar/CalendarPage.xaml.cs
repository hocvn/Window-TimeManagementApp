using Microsoft.UI.Xaml.Controls;
using System;
using System.Linq;
using TimeManagementApp.ToDo;
using TimeManagementApp.Helper;

namespace TimeManagementApp.Calendar
{
    /// <summary>
    /// This page is responsible for displaying the calendar.
    /// </summary>
    public sealed partial class CalendarPage : Page
    {
        public CalendarViewModel ViewModel { get; set; }

        public CalendarPage()
        {
            this.InitializeComponent();
            ViewModel = new();
            ViewModel.Init();
        }

        private void ClosePane_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            SplitView.IsPaneOpen = false;
        }

        private void CalendarView_DateChanged(CalendarView sender, CalendarViewSelectedDatesChangedEventArgs args)
        {
            DateTime selectedDate = args.AddedDates.FirstOrDefault().DateTime;
            ViewModel.Date = selectedDate;
            ViewModel.DisplayDate = selectedDate.ToString("ddd, MMM dd");
            ViewModel.GetTasksForDate(selectedDate);
            SplitView.IsPaneOpen = true;
        }

        private void TaskItem_Click(object sender, ItemClickEventArgs e)
        {
            MainWindow.NavigationService.Navigate(typeof(EditToDoPage), e.ClickedItem);
        }

        private void CalendarView_CalendarViewDayItemChanging(CalendarView sender, CalendarViewDayItemChangingEventArgs args)
        {
            if (args.Phase == 0)
            {
                args.RegisterUpdateCallback(CalendarView_CalendarViewDayItemChanging);
            }
            else if (args.Phase == 1)
            {
                DateTime date = args.Item.Date.DateTime.Date;

                if (date != TimeHelper.GetToday())
                {
                    if (ViewModel.TaskCounts.TryGetValue(date, out int taskCount))
                    {
                        args.Item.SetDensityColors([Microsoft.UI.Colors.Green]);
                    }
                    args.RegisterUpdateCallback(CalendarView_CalendarViewDayItemChanging);
                }
            }
        }
    }
}
