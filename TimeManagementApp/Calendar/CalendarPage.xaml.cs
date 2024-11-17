using Microsoft.UI.Xaml.Controls;
using System;
using System.Linq;
using TimeManagementApp.Services;
using TimeManagementApp.ToDo;

namespace TimeManagementApp.Calendar
{
    /// <summary>
    /// This page is responsible for displaying the calendar.
    /// </summary>
    public sealed partial class CalendarPage : Page
    {
        public CalendarViewModel ViewModel { get; set; }
        private NavigationService NavigationService { get; set; }

        public CalendarPage()
        {
            this.InitializeComponent();
            ViewModel = new();
            NavigationService = new NavigationService();
        }

        private void ClosePane_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            SplitView.IsPaneOpen = false;
        }

        private void CalendarView_DateChanged(CalendarView sender, CalendarViewSelectedDatesChangedEventArgs args)
        {
            DateTime selectedDate = args.AddedDates.FirstOrDefault().DateTime;
            ViewModel.DisplayDate = selectedDate.ToString("ddd, MMM dd");
            ViewModel.GetTasksForDate(selectedDate);
            SplitView.IsPaneOpen = true;
        }

        private void TaskItem_Click(object sender, ItemClickEventArgs e)
        {
            NavigationService.Navigate(typeof(EditToDoPage), e.ClickedItem);
        }
    }
}
