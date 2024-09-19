using System.Globalization;
using Vm167Box.Models;

namespace Vm167Box.Converters
{
    public class AnalogChannelToUnitIntervalConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is AnalogChannel channel && targetType == typeof(double))
            {
                var diff = channel.MaxValue - channel.MinValue;
                if (diff == 0) return 0;
                var result = (channel.Value - channel.MinValue) / diff;
                return result;
            }

            return 0;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
