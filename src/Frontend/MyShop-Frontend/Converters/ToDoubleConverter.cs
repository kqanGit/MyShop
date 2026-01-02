using Microsoft.UI.Xaml.Data;
using System;
using System.Globalization;

namespace MyShop_Frontend.Converters;

public sealed class ToDoubleConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is null)
        {
            return 0d;
        }

        try
        {
            return System.Convert.ToDouble(value, CultureInfo.InvariantCulture);
        }
        catch
        {
            return 0d;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is null)
        {
            return 0d;
        }

        return System.Convert.ToDouble(value, CultureInfo.InvariantCulture);
    }
}