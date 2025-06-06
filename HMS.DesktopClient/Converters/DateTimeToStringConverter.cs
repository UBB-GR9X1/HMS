using Microsoft.UI.Xaml.Data;
using System;

namespace HMS.DesktopClient.Converters
{
    public class DateTimeToStringConverter : IValueConverter
    {
        public static readonly DateTimeToStringConverter Instance = new DateTimeToStringConverter();

        private DateTimeToStringConverter() { }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTime dateTime)
            {
                return dateTime.ToString("d"); // Short date format
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
} 