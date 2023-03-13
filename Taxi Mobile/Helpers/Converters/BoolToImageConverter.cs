using System.Globalization;

namespace Taxi_mobile.Helpers.Converters
{
    class BoolToImageConverter : IValueConverter
    {
        public string PositiveImagePath { get; set; }
        public string NegativeImagePath { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isPositive = (bool)value;

            if (isPositive)
            {
                return ImageSource.FromFile(PositiveImagePath);
            }

            return ImageSource.FromFile(NegativeImagePath);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
