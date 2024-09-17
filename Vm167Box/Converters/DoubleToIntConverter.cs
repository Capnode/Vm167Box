using System.Globalization;

namespace Vm167Box.Converters
{
    public class DoubleToIntConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 3 &&
                values[0] is double currentValue &&
                values[1] is double minValue &&
                values[2] is double maxValue)
            {
                var diff = maxValue - minValue;
                if (diff == 0) return 0;
                return (currentValue - minValue) / diff;
            }

            return 0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            if (value is double progressValue && targetTypes.Length == 3)
            {
                double minValue = System.Convert.ToDouble(parameter);
                double maxValue = System.Convert.ToDouble(parameter);
                double originalValue = progressValue * (maxValue - minValue) + minValue;
                return new object[] { originalValue, minValue, maxValue };
            }

            return new object[] { 0, 0, 0 };
        }
    }
}
