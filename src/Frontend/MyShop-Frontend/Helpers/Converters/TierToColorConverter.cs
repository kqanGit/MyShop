using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
using System;
using Windows.UI;

namespace MyShop_Frontend.Helpers.Converters
{
    public class TierToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is int tierId)
            {
                // Simple color logic based on TierId
                return tierId switch
                {
                    1 => new SolidColorBrush(Color.FromArgb(255, 255, 228, 230)), // VIP - Light Red
                    2 => new SolidColorBrush(Color.FromArgb(255, 209, 250, 229)), // New - Light Green
                    3 => new SolidColorBrush(Color.FromArgb(255, 219, 234, 254)), // Newsletter - Light Blue
                    _ => new SolidColorBrush(Colors.Transparent)
                };
            }
            return new SolidColorBrush(Colors.Transparent);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
