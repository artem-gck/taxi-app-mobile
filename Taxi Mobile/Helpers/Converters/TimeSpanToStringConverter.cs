using System.Globalization;
using Taxi_mobile.Resources.Dictionaries;

namespace Taxi_mobile.Helpers.Converters
{
    public class TimeSpanToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = $"0 years";

            var timeSpan = (TimeSpan)value;
            
            var countOfDays = timeSpan.Days;
            
            if (countOfDays / 365 != 0)
            {
                var countOfYears = countOfDays / 365;
                var isMoreThenYear = countOfDays % 365 == 0;

                return isMoreThenYear ? $"{countOfYears} {ResourcesViewModel.Years}" : $"{countOfYears} {ResourcesViewModel.YearsAndMore}";
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
