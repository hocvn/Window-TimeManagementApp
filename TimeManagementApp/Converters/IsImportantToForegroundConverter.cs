using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;

namespace TimeManagementApp.Converters
{
    public class IsImportantToForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool isImportant)
            {
                return isImportant ? new SolidColorBrush(Colors.Red) : new SolidColorBrush(Colors.Black);
            }
            return new SolidColorBrush(Colors.Black); 
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
