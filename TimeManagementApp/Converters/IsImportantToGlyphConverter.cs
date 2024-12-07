using Microsoft.UI.Xaml.Data;
using System;

namespace TimeManagementApp.Converters
{
    public class IsImportantToGlyphConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool isImportant)
            {
                return isImportant ? "\uE735" : "\uE734";
            }
            return "\uE734";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

