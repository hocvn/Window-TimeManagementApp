using Microsoft.UI.Xaml.Data;
using System;

namespace TimeManagementApp.Converters
{
    internal class DateToHourConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTime dateTime)
            {
                var ret = dateTime.ToString("hh:mm tt"); 
                return ret;
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
