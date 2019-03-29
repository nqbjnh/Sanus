using System;
using System.Globalization;
using Xamarin.Forms;

namespace Sanus.Converters
{
    public class TimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is DateTimeOffset))
                return value;
            return ((DateTimeOffset)value).ToString("HH:mm");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}