using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

namespace TimeManagementApp.Converters
{
    public class TaskPropertyToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTime dateTime)
            {
                return dateTime != MainWindow.NullDateTime ? Visibility.Visible : Visibility.Collapsed;
            }
            if (value is string str && !string.IsNullOrWhiteSpace(str))
            {
                return Visibility.Visible;
            }
            if (value is int noteId) 
            { 
                return noteId != -1 ? Visibility.Visible : Visibility.Collapsed; 
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
