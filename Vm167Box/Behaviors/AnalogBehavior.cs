using CommunityToolkit.Maui.Behaviors;
using Vm167Lib;

namespace Vm167Box.Behaviors
{
    public static class AnalogBehavior
    {
        public static readonly BindableProperty NumericValidationBehaviorProperty =
            BindableProperty.CreateAttached(
                "NumericValidationBehavior",
                typeof(bool),
                typeof(AnalogBehavior),
                false,
                propertyChanged: OnNumericValidationBehaviorChanged);

        public static bool GetNumericValidationBehavior(BindableObject view)
        {
            return (bool)view.GetValue(NumericValidationBehaviorProperty);
        }

        public static void SetNumericValidationBehavior(BindableObject view, bool value)
        {
            view.SetValue(NumericValidationBehaviorProperty, value);
        }

        private static void OnNumericValidationBehaviorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is Entry entry && newValue is bool isEnabled && isEnabled)
            {
                var invalidStyle = new Style(typeof(Entry))
                {
                    Setters = { new Setter { Property = Entry.TextColorProperty, Value = Colors.Red } }
                };

                var validStyle = new Style(typeof(Entry))
                {
                    Setters = { new Setter { Property = Entry.TextColorProperty, Value = Colors.Green } }
                };

                entry.Behaviors.Add(new NumericValidationBehavior
                {
                    Flags = ValidationFlags.ValidateOnValueChanged,
                    InvalidStyle = invalidStyle,
                    MinimumValue = IVm167.AnalogMin,
                    MaximumValue = IVm167.AnalogMax,
                    ValidStyle = validStyle
                });
            }
        }
    }
}
