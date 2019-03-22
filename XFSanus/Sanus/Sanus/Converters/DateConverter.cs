using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace Sanus.Converters
{
    public class DateTimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dateFormat = string.Empty;

            dateFormat = "dd/MM/yyyy";

            if (value is DateTimeOffset dateTimeOffset)
                return dateTimeOffset.ToLocalTime().ToString(dateFormat);
            if (value is DateTime datetime)
                return datetime.ToLocalTime().ToString(dateFormat);
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class DateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTimeOffset dateTimeOffset)
                return dateTimeOffset.ToString("D");
            if (value is DateTime datetime)
                return datetime.ToString("D");
            return value; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
