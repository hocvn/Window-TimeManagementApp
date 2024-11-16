using Microsoft.UI.Xaml.Controls;
using System;
using System.Linq;

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
        }

        private void ClosePane_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            SplitView.IsPaneOpen = false;
        }

        private void CalendarView_DateChanged(CalendarView sender, CalendarViewSelectedDatesChangedEventArgs args)
        {
            DateTime selectedDate = args.AddedDates.FirstOrDefault().DateTime;
            ViewModel.GetTasksForDate(selectedDate);
            SplitView.IsPaneOpen = true;
        }

        private void TaskItem_Click(object sender, ItemClickEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
