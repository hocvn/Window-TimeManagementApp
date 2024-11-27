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
                return dateTime != DateTime.MinValue ? Visibility.Visible : Visibility.Collapsed;
            }
            if (value is string str && !string.IsNullOrWhiteSpace(str))
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
