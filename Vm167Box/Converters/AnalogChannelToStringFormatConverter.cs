using System.Globalization;
using Vm167Box.Models;

namespace Vm167Box.Converters;

public class AnalogChannelToStringFormatConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is AnalogChannel channel && targetType == typeof(string))
        {
            var format = parameter as string;
            if (format == null)
            {
                format = "{0:F" + channel.Decimals + "} {1}";
            }

            return string.Format(culture, format ?? "{0}", channel.Value, channel.Unit);
        }

        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return double.TryParse(value as string, NumberStyles.Float, culture, out var result) ? result : null;
    }
}
