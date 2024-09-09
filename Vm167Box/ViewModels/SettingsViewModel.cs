using CommunityToolkit.Mvvm.ComponentModel;
using System.Globalization;
using Vm167Box.Services;

namespace Vm167Box.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        private readonly ISettingsService _settingsService;

        public SettingsViewModel(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public bool IsLightTheme
        {
            get => Application.Current?.RequestedTheme is AppTheme.Light;
            set
            {
                var appTheme = value ? AppTheme.Light : AppTheme.Dark;
                _settingsService.AppTheme = appTheme;
                if (Application.Current is null) return;
                Application.Current.UserAppTheme = appTheme;
                OnPropertyChanged();
            }
        }

        public string? Analog1Name
        {
            get => _settingsService.Analog1Name;
            set => _settingsService.Analog1Name = value;
        }

        public string? Analog1Unit
        {
            get => _settingsService.Analog1Unit;
            set => _settingsService.Analog1Unit = value;
        }

        public string? Analog1Point1
        {
            get => ToString(_settingsService.Analog1Point1);
            set => _settingsService.Analog1Point1 = ToDouble(value);
        }

        public string? Analog1Value1
        {
            get => ToString(_settingsService.Analog1Value1);
            set => _settingsService.Analog1Value1 = ToDouble(value);
        }

        public string? Analog1Point2
        {
            get => ToString(_settingsService.Analog1Point2);
            set => _settingsService.Analog1Point2 = ToDouble(value);
        }

        public string? Analog1Value2
        {
            get => ToString(_settingsService.Analog1Value2);
            set => _settingsService.Analog1Value2 = ToDouble(value);
        }

        public string? Analog2Name
        {
            get => _settingsService.Analog2Name;
            set => _settingsService.Analog2Name = value;
        }

        public string? Analog2Unit
        {
            get => _settingsService.Analog2Unit;
            set => _settingsService.Analog2Unit = value;
        }

        public string? Analog2Point1
        {
            get => ToString(_settingsService.Analog2Point1);
            set => _settingsService.Analog2Point1 = ToDouble(value);
        }

        public string? Analog2Value1
        {
            get => ToString(_settingsService.Analog2Value1);
            set => _settingsService.Analog2Value1 = ToDouble(value);
        }

        public string? Analog2Point2
        {
            get => ToString(_settingsService.Analog2Point2);
            set => _settingsService.Analog2Point2 = ToDouble(value);
        }

        public string? Analog2Value2
        {
            get => ToString(_settingsService.Analog2Value2);
            set => _settingsService.Analog2Value2 = ToDouble(value);
        }

        public string? Analog3Name
        {
            get => _settingsService.Analog3Name;
            set => _settingsService.Analog3Name = value;
        }

        public string? Analog3Unit
        {
            get => _settingsService.Analog3Unit;
            set => _settingsService.Analog3Unit = value;
        }

        public string? Analog3Point1
        {
            get => ToString(_settingsService.Analog3Point1);
            set => _settingsService.Analog3Point1 = ToDouble(value);
        }

        public string? Analog3Value1
        {
            get => ToString(_settingsService.Analog3Value1);
            set => _settingsService.Analog3Value1 = ToDouble(value);
        }

        public string? Analog3Point2
        {
            get => ToString(_settingsService.Analog3Point2);
            set => _settingsService.Analog3Point2 = ToDouble(value);
        }

        public string? Analog3Value2
        {
            get => ToString(_settingsService.Analog3Value2);
            set => _settingsService.Analog3Value2 = ToDouble(value);
        }

        public string? Analog4Name
        {
            get => _settingsService.Analog4Name;
            set =>_settingsService.Analog4Name = value;
        }

        public string? Analog4Unit
        {
            get => _settingsService.Analog4Unit;
            set => _settingsService.Analog4Unit = value;
        }

        public string? Analog4Point1
        {
            get => ToString(_settingsService.Analog4Point1);
            set => _settingsService.Analog4Point1 = ToDouble(value);
        }

        public string? Analog4Value1
        {
            get => ToString(_settingsService.Analog4Value1);
            set => _settingsService.Analog4Value1 = ToDouble(value);
        }

        public string? Analog4Point2
        {
            get => ToString(_settingsService.Analog4Point2);
            set => _settingsService.Analog4Point2 = ToDouble(value);
        }

        public string? Analog4Value2
        {
            get => ToString(_settingsService.Analog4Value2);
            set => _settingsService.Analog4Value2 = ToDouble(value);
        }

        public string? Analog5Name
        {
            get => _settingsService.Analog5Name;
            set => _settingsService.Analog5Name = value;
        }

        public string? Analog5Unit
        {
            get => _settingsService.Analog5Unit;
            set => _settingsService.Analog5Unit = value;
        }

        public string? Analog5Point1
        {
            get => ToString(_settingsService.Analog5Point1);
            set => _settingsService.Analog5Point1 = ToDouble(value);
        }

        public string? Analog5Value1
        {
            get => ToString(_settingsService.Analog5Value1);
            set => _settingsService.Analog5Value1 = ToDouble(value);
        }

        public string? Analog5Point2
        {
            get => ToString(_settingsService.Analog5Point2);
            set => _settingsService.Analog5Point2 = ToDouble(value);
        }

        public string? Analog5Value2
        {
            get => ToString(_settingsService.Analog5Value2);
            set => _settingsService.Analog5Value2 = ToDouble(value);
        }

        public string? Pwm1Name
        {
            get => _settingsService.Pwm1Name;
            set => _settingsService.Pwm1Name = value;
        }

        public string? Pwm1Unit
        {
            get => _settingsService.Pwm1Unit;
            set => _settingsService.Pwm1Unit = value;
        }

        public string? Pwm1Point1
        {
            get => ToString(_settingsService.Pwm1Point1);
            set => _settingsService.Pwm1Point1 = ToDouble(value);
        }

        public string? Pwm1Value1
        {
            get => ToString(_settingsService.Pwm1Value1);
            set => _settingsService.Pwm1Value1 = ToDouble(value);
        }

        public string? Pwm1Point2
        {
            get => ToString(_settingsService.Pwm1Point2);
            set => _settingsService.Pwm1Point2 = ToDouble(value);
        }

        public string? Pwm1Value2
        {
            get => ToString(_settingsService.Pwm1Value2);
            set => _settingsService.Pwm1Value2 = ToDouble(value);
        }


        public string? Pwm2Name
        {
            get => _settingsService.Pwm2Name;
            set => _settingsService.Pwm2Name = value;
        }

        public string? Pwm2Unit
        {
            get => _settingsService.Pwm2Unit;
            set => _settingsService.Pwm2Unit = value;
        }

        public string? Pwm2Point1
        {
            get => ToString(_settingsService.Pwm2Point1);
            set => _settingsService.Pwm2Point1 = ToDouble(value);
        }

        public string? Pwm2Value1
        {
            get => ToString(_settingsService.Pwm2Value1);
            set => _settingsService.Pwm2Value1 = ToDouble(value);
        }

        public string? Pwm2Point2
        {
            get => ToString(_settingsService.Pwm2Point2);
            set => _settingsService.Pwm2Point2 = ToDouble(value);
        }

        public string? Pwm2Value2
        {
            get => ToString(_settingsService.Pwm2Value2);
            set => _settingsService.Pwm2Value2 = ToDouble(value);
        }

        private string? ToString(double value)
        {
            if (Double.IsNaN(value)) return null;
            return value.ToString();
        }

        private double ToDouble(string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return double.NaN;
            if (double.TryParse(value, CultureInfo.CurrentCulture.NumberFormat, out double result)) return result;
            return double.NaN;
        }
    }
}