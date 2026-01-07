using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace MyShop_Frontend.Helpers.Converters
{
    public sealed class StatusToPayVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var status = value?.ToString()?.Trim().ToLowerInvariant();
            return status == "new" ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
            => throw new NotImplementedException();
    }
}
