using Microsoft.UI.Xaml.Data;
using System;
using System.Globalization;

namespace MyShop_Frontend.Helpers.Converters
{
    public class PriceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is decimal price)
            {
                return price.ToString("C", new CultureInfo("en-US"));
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
