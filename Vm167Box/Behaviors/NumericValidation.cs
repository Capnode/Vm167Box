using CommunityToolkit.Maui.Behaviors;

namespace Vm167Box.Behaviors
{
    public static class NumericValidation
    {
        public static readonly BindableProperty ZZZActivateProperty =
            BindableProperty.CreateAttached(
                "ZZZActivate",
                typeof(bool),
                typeof(NumericValidation),
                false,
                propertyChanged: OnZZZActivateChanged);

        public static readonly BindableProperty MinimumValueProperty =
            BindableProperty.CreateAttached(
                "MinimumValue",
                typeof(double),
                typeof(NumericValidation),
                double.MinValue);

        public static readonly BindableProperty MaximumValueProperty =
            BindableProperty.CreateAttached(
                "MaximumValue",
                typeof(double),
                typeof(NumericValidation),
                double.MaxValue);

        public static bool GetZZZActivate(BindableObject view)
        {
            return (bool)view.GetValue(ZZZActivateProperty);
        }

        public static void SetZZZActivate(BindableObject view, bool value)
        {
            view.SetValue(ZZZActivateProperty, value);
        }

        public static double GetMinimumValue(BindableObject view)
        {
            return (double)view.GetValue(MinimumValueProperty);
        }

        public static void SetMinimumValue(BindableObject view, double value)
        {
            view.SetValue(MinimumValueProperty, value);
        }

        public static double GetMaximumValue(BindableObject view)
        {
            return (double)view.GetValue(MaximumValueProperty);
        }

        public static void SetMaximumValue(BindableObject view, double value)
        {
            view.SetValue(MaximumValueProperty, value);
        }

        private static void OnZZZActivateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is Entry entry && newValue is bool isEnabled && isEnabled)
            {
                var invalidStyle = new Style(typeof(Entry)) { Setters = { new Setter { Property = Entry.TextColorProperty, Value = Colors.Red } } };
                var validStyle = new Style(typeof(Entry)) { Setters = { new Setter { Property = Entry.TextColorProperty, Value = Colors.Green } } };
                var minimumValue = GetMinimumValue(entry);
                var maximumValue = GetMaximumValue(entry);

                entry.Behaviors.Add(new NumericValidationBehavior
                {
                    Flags = ValidationFlags.ValidateOnValueChanged,
                    InvalidStyle = invalidStyle,
                    MinimumValue = minimumValue,
                    MaximumValue = maximumValue,
                    ValidStyle = validStyle
                });
            }
        }
    }
}
