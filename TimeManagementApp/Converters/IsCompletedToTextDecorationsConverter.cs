using Microsoft.UI.Text;
using Microsoft.UI.Xaml.Data;
using System;
using Windows.UI.Text;

namespace TimeManagementApp.Converters
{
    public class IsCompletedToTextDecorationsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool isCompleted)
            {
                return isCompleted ? TextDecorations.Strikethrough : TextDecorations.None;
            }
            return TextDecorations.None;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

