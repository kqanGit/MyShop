using Microsoft.UI.Xaml.Data;
using System;

namespace MyShop_Frontend.Helpers.Converters
{
    public class DateToFormattedStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTime dt)
            {
                return dt.ToString("MMM dd, yyyy");
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
