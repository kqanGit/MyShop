using Microsoft.UI.Xaml.Data;
using System;

namespace MyShop_Frontend.Helpers.Converters
{
    public class TierToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is int tierId)
            {
                return tierId switch
                {
                    1 => "VIP Customers",
                    2 => "New Leads",
                    3 => "Newsletter",
                    _ => "Unknown"
                };
            }
            return "Unknown";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
