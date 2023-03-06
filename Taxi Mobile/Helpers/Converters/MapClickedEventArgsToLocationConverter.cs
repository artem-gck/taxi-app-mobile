using Microsoft.Maui.Controls.Maps;
using System.Globalization;

namespace Taxi_mobile.Helpers.Converters
{
    public class MapClickedEventArgsToLocationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var eventArgs = value as MapClickedEventArgs;
            return eventArgs.Location;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
