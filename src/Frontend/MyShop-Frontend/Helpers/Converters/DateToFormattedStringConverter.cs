using Microsoft.UI.Xaml.Data;
using System;
using System.Globalization;

namespace MyShop_Frontend.Helpers.Converters
{
    public class DateToFormattedStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTime dt)
            {
                var format = parameter as string;
                return string.IsNullOrWhiteSpace(format)
                    ? dt.ToString("MMM dd, yyyy", CultureInfo.InvariantCulture)
                    : dt.ToString(format, CultureInfo.InvariantCulture);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
