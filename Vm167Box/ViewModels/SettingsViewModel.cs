using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Vm167Box.Services;

namespace Vm167Box.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        private readonly ISettingsService _settingsService;

        public SettingsViewModel(ISettingsService settingsService)
        {
            _settingsService = settingsService;
            _settingsService.Update += UpdateSettings;
        }

        public bool IsLightTheme
        {
            get => _settingsService.AppTheme is AppTheme.Light;
            set
            {
                var appTheme = value ? AppTheme.Light : AppTheme.Dark;
                _settingsService.AppTheme = appTheme;
                if (Application.Current is null) return;
                Application.Current.UserAppTheme = appTheme;
                OnPropertyChanged();
            }
        }

        public string Analog1Name
        {
            get => _settingsService.Analog1Name;
            set => _settingsService.Analog1Name = value;
        }

        public string Analog1Unit
        {
            get => _settingsService.Analog1Unit;
            set => _settingsService.Analog1Unit = value;
        }

        public double Analog1Point1
        {
            get => _settingsService.Analog1Point1;
            set => _settingsService.Analog1Point1 = value;
        }

        public double Analog1Value1
        {
            get => _settingsService.Analog1Value1;
            set => _settingsService.Analog1Value1 = value;
        }

        public double Analog1Point2
        {
            get => _settingsService.Analog1Point2;
            set => _settingsService.Analog1Point2 = value;
        }

        public double Analog1Value2
        {
            get => _settingsService.Analog1Value2;
            set => _settingsService.Analog1Value2 = value;
        }

        public string Analog2Name
        {
            get => _settingsService.Analog2Name;
            set => _settingsService.Analog2Name = value;
        }

        public string Analog2Unit
        {
            get => _settingsService.Analog2Unit;
            set => _settingsService.Analog2Unit = value;
        }

        public double Analog2Point1
        {
            get => _settingsService.Analog2Point1;
            set => _settingsService.Analog2Point1 = value;
        }

        public double Analog2Value1
        {
            get => _settingsService.Analog2Value1;
            set => _settingsService.Analog2Value1 = value;
        }

        public double Analog2Point2
        {
            get => _settingsService.Analog2Point2;
            set => _settingsService.Analog2Point2 = value;
        }

        public double Analog2Value2
        {
            get => _settingsService.Analog2Value2;
            set => _settingsService.Analog2Value2 = value;
        }

        public string Analog3Name
        {
            get => _settingsService.Analog3Name;
            set => _settingsService.Analog3Name = value;
        }

        public string Analog3Unit
        {
            get => _settingsService.Analog3Unit;
            set => _settingsService.Analog3Unit = value;
        }

        public double Analog3Point1
        {
            get => _settingsService.Analog3Point1;
            set => _settingsService.Analog3Point1 = value;
        }

        public double Analog3Value1
        {
            get => _settingsService.Analog3Value1;
            set => _settingsService.Analog3Value1 = value;
        }

        public double Analog3Point2
        {
            get => _settingsService.Analog3Point2;
            set => _settingsService.Analog3Point2 = value;
        }

        public double Analog3Value2
        {
            get => _settingsService.Analog3Value2;
            set => _settingsService.Analog3Value2 = value;
        }

        public string Analog4Name
        {
            get => _settingsService.Analog4Name;
            set =>_settingsService.Analog4Name = value;
        }

        public string Analog4Unit
        {
            get => _settingsService.Analog4Unit;
            set => _settingsService.Analog4Unit = value;
        }

        public double Analog4Point1
        {
            get => _settingsService.Analog4Point1;
            set => _settingsService.Analog4Point1 = value;
        }

        public double Analog4Value1
        {
            get => _settingsService.Analog4Value1;
            set => _settingsService.Analog4Value1 = value;
        }

        public double Analog4Point2
        {
            get => _settingsService.Analog4Point2;
            set => _settingsService.Analog4Point2 = value;
        }

        public double Analog4Value2
        {
            get => _settingsService.Analog4Value2;
            set => _settingsService.Analog4Value2 = value;
        }

        public string Analog5Name
        {
            get => _settingsService.Analog5Name;
            set => _settingsService.Analog5Name = value;
        }

        public string Analog5Unit
        {
            get => _settingsService.Analog5Unit;
            set => _settingsService.Analog5Unit = value;
        }

        public double Analog5Point1
        {
            get => _settingsService.Analog5Point1;
            set => _settingsService.Analog5Point1 = value;
        }

        public double Analog5Value1
        {
            get => _settingsService.Analog5Value1;
            set => _settingsService.Analog5Value1 = value;
        }

        public double Analog5Point2
        {
            get => _settingsService.Analog5Point2;
            set => _settingsService.Analog5Point2 = value;
        }

        public double Analog5Value2
        {
            get => _settingsService.Analog5Value2;
            set => _settingsService.Analog5Value2 = value;
        }

        public string Pwm1Name
        {
            get => _settingsService.Pwm1Name;
            set => _settingsService.Pwm1Name = value;
        }

        public string Pwm1Unit
        {
            get => _settingsService.Pwm1Unit;
            set => _settingsService.Pwm1Unit = value;
        }

        public double Pwm1Point1
        {
            get => _settingsService.Pwm1Point1;
            set => _settingsService.Pwm1Point1 = value;
        }

        public double Pwm1Value1
        {
            get => _settingsService.Pwm1Value1;
            set => _settingsService.Pwm1Value1 = value;
        }

        public double Pwm1Point2
        {
            get => _settingsService.Pwm1Point2;
            set => _settingsService.Pwm1Point2 = value;
        }

        public double Pwm1Value2
        {
            get => _settingsService.Pwm1Value2;
            set => _settingsService.Pwm1Value2 = value;
        }


        public string Pwm2Name
        {
            get => _settingsService.Pwm2Name;
            set => _settingsService.Pwm2Name = value;
        }

        public string Pwm2Unit
        {
            get => _settingsService.Pwm2Unit;
            set => _settingsService.Pwm2Unit = value;
        }

        public double Pwm2Point1
        {
            get => _settingsService.Pwm2Point1;
            set => _settingsService.Pwm2Point1 = value;
        }

        public double Pwm2Value1
        {
            get => _settingsService.Pwm2Value1;
            set => _settingsService.Pwm2Value1 = value;
        }

        public double Pwm2Point2
        {
            get => _settingsService.Pwm2Point2;
            set => _settingsService.Pwm2Point2 = value;
        }

        public double Pwm2Value2
        {
            get => _settingsService.Pwm2Value2;
            set => _settingsService.Pwm2Value2 = value;
        }

        [RelayCommand]
        public void Reset()
        {
            _settingsService.Reset();
        }

        private Task UpdateSettings()
        {
            OnPropertyChanged(nameof(IsLightTheme));
            OnPropertyChanged(nameof(Analog1Name));
            OnPropertyChanged(nameof(Analog1Unit));
            OnPropertyChanged(nameof(Analog1Point1));
            OnPropertyChanged(nameof(Analog1Value1));
            OnPropertyChanged(nameof(Analog1Point2));
            OnPropertyChanged(nameof(Analog1Value2));
            OnPropertyChanged(nameof(Analog2Name));
            OnPropertyChanged(nameof(Analog2Unit));
            OnPropertyChanged(nameof(Analog2Point1));
            OnPropertyChanged(nameof(Analog2Value1));
            OnPropertyChanged(nameof(Analog2Point2));
            OnPropertyChanged(nameof(Analog2Value2));
            OnPropertyChanged(nameof(Analog3Name));
            OnPropertyChanged(nameof(Analog3Unit));
            OnPropertyChanged(nameof(Analog3Point1));
            OnPropertyChanged(nameof(Analog3Value1));
            OnPropertyChanged(nameof(Analog3Point2));
            OnPropertyChanged(nameof(Analog3Value2));
            OnPropertyChanged(nameof(Analog4Name));
            OnPropertyChanged(nameof(Analog4Unit));
            OnPropertyChanged(nameof(Analog4Point1));
            OnPropertyChanged(nameof(Analog4Value1));
            OnPropertyChanged(nameof(Analog4Point2));
            OnPropertyChanged(nameof(Analog4Value2));
            OnPropertyChanged(nameof(Analog5Name));
            OnPropertyChanged(nameof(Analog5Unit));
            OnPropertyChanged(nameof(Analog5Point1));
            OnPropertyChanged(nameof(Analog5Value1));
            OnPropertyChanged(nameof(Analog5Point2));
            OnPropertyChanged(nameof(Analog5Value2));
            OnPropertyChanged(nameof(Pwm1Name));
            OnPropertyChanged(nameof(Pwm1Unit));
            OnPropertyChanged(nameof(Pwm1Point1));
            OnPropertyChanged(nameof(Pwm1Value1));
            OnPropertyChanged(nameof(Pwm1Point2));
            OnPropertyChanged(nameof(Pwm1Value2));
            OnPropertyChanged(nameof(Pwm2Name));
            OnPropertyChanged(nameof(Pwm2Unit));
            OnPropertyChanged(nameof(Pwm2Point1));
            OnPropertyChanged(nameof(Pwm2Value1));
            OnPropertyChanged(nameof(Pwm2Point2));
            OnPropertyChanged(nameof(Pwm2Value2));
            return Task.CompletedTask;
        }
    }
}