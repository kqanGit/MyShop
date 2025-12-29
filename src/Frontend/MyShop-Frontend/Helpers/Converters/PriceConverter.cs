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
                return price.ToString("C0", new CultureInfo("vi-VN"));
            }
            if (value is double dPrice)
            {
                return dPrice.ToString("C0", new CultureInfo("vi-VN"));
            } 
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
