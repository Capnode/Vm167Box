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

        public int Analog1MinPoint
        {
            get => _settingsService.Analog1MinPoint;
            set => _settingsService.Analog1MinPoint = value;
        }

        public double Analog1MinValue
        {
            get => _settingsService.Analog1MinValue;
            set => _settingsService.Analog1MinValue = value;
        }

        public int Analog1MaxPoint
        {
            get => _settingsService.Analog1MaxPoint;
            set => _settingsService.Analog1MaxPoint = value;
        }

        public double Analog1MaxValue
        {
            get => _settingsService.Analog1MaxValue;
            set => _settingsService.Analog1MaxValue = value;
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

        public int Analog2MinPoint
        {
            get => _settingsService.Analog2MinPoint;
            set => _settingsService.Analog2MinPoint = value;
        }

        public double Analog2MinValue
        {
            get => _settingsService.Analog2MinValue;
            set => _settingsService.Analog2MinValue = value;
        }

        public int Analog2MaxPoint
        {
            get => _settingsService.Analog2MaxPoint;
            set => _settingsService.Analog2MaxPoint = value;
        }

        public double Analog2MaxValue
        {
            get => _settingsService.Analog2MaxValue;
            set => _settingsService.Analog2MaxValue = value;
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

        public int Analog3MinPoint
        {
            get => _settingsService.Analog3MinPoint;
            set => _settingsService.Analog3MinPoint = value;
        }

        public double Analog3MinValue
        {
            get => _settingsService.Analog3MinValue;
            set => _settingsService.Analog3MinValue = value;
        }

        public int Analog3MaxPoint
        {
            get => _settingsService.Analog3MaxPoint;
            set => _settingsService.Analog3MaxPoint = value;
        }

        public double Analog3MaxValue
        {
            get => _settingsService.Analog3MaxValue;
            set => _settingsService.Analog3MaxValue = value;
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

        public int Analog4MinPoint
        {
            get => _settingsService.Analog4MinPoint;
            set => _settingsService.Analog4MinPoint = value;
        }

        public double Analog4MinValue
        {
            get => _settingsService.Analog4MinValue;
            set => _settingsService.Analog4MinValue = value;
        }

        public int Analog4MaxPoint
        {
            get => _settingsService.Analog4MaxPoint;
            set => _settingsService.Analog4MaxPoint = value;
        }

        public double Analog4MaxValue
        {
            get => _settingsService.Analog4MaxValue;
            set => _settingsService.Analog4MaxValue = value;
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

        public int Analog5MinPoint
        {
            get => _settingsService.Analog5MinPoint;
            set => _settingsService.Analog5MinPoint = value;
        }

        public double Analog5MinValue
        {
            get => _settingsService.Analog5MinValue;
            set => _settingsService.Analog5MinValue = value;
        }

        public int Analog5MaxPoint
        {
            get => _settingsService.Analog5MaxPoint;
            set => _settingsService.Analog5MaxPoint = value;
        }

        public double Analog5MaxValue
        {
            get => _settingsService.Analog5MaxValue;
            set => _settingsService.Analog5MaxValue = value;
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

        public int Pwm1MinPoint
        {
            get => _settingsService.Pwm1MinPoint;
            set => _settingsService.Pwm1MinPoint = value;
        }

        public double Pwm1MinValue
        {
            get => _settingsService.Pwm1MinValue;
            set => _settingsService.Pwm1MinValue = value;
        }

        public int Pwm1MaxPoint
        {
            get => _settingsService.Pwm1MaxPoint;
            set => _settingsService.Pwm1MaxPoint = value;
        }

        public double Pwm1MaxValue
        {
            get => _settingsService.Pwm1MaxValue;
            set => _settingsService.Pwm1MaxValue = value;
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

        public int Pwm2MinPoint
        {
            get => _settingsService.Pwm2MinPoint;
            set => _settingsService.Pwm2MinPoint = value;
        }

        public double Pwm2MinValue
        {
            get => _settingsService.Pwm2MinValue;
            set => _settingsService.Pwm2MinValue = value;
        }

        public int Pwm2MaxPoint
        {
            get => _settingsService.Pwm2MaxPoint;
            set => _settingsService.Pwm2MaxPoint = value;
        }

        public double Pwm2MaxValue
        {
            get => _settingsService.Pwm2MaxValue;
            set => _settingsService.Pwm2MaxValue = value;
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
            OnPropertyChanged(nameof(Analog1MinPoint));
            OnPropertyChanged(nameof(Analog1MinValue));
            OnPropertyChanged(nameof(Analog1MaxPoint));
            OnPropertyChanged(nameof(Analog1MaxValue));
            OnPropertyChanged(nameof(Analog2Name));
            OnPropertyChanged(nameof(Analog2Unit));
            OnPropertyChanged(nameof(Analog2MinPoint));
            OnPropertyChanged(nameof(Analog2MinValue));
            OnPropertyChanged(nameof(Analog2MaxPoint));
            OnPropertyChanged(nameof(Analog2MaxValue));
            OnPropertyChanged(nameof(Analog3Name));
            OnPropertyChanged(nameof(Analog3Unit));
            OnPropertyChanged(nameof(Analog3MinPoint));
            OnPropertyChanged(nameof(Analog3MinValue));
            OnPropertyChanged(nameof(Analog3MaxPoint));
            OnPropertyChanged(nameof(Analog3MaxValue));
            OnPropertyChanged(nameof(Analog4Name));
            OnPropertyChanged(nameof(Analog4Unit));
            OnPropertyChanged(nameof(Analog4MinPoint));
            OnPropertyChanged(nameof(Analog4MinValue));
            OnPropertyChanged(nameof(Analog4MaxPoint));
            OnPropertyChanged(nameof(Analog4MaxValue));
            OnPropertyChanged(nameof(Analog5Name));
            OnPropertyChanged(nameof(Analog5Unit));
            OnPropertyChanged(nameof(Analog5MinPoint));
            OnPropertyChanged(nameof(Analog5MinValue));
            OnPropertyChanged(nameof(Analog5MaxPoint));
            OnPropertyChanged(nameof(Analog5MaxValue));
            OnPropertyChanged(nameof(Pwm1Name));
            OnPropertyChanged(nameof(Pwm1Unit));
            OnPropertyChanged(nameof(Pwm1MinPoint));
            OnPropertyChanged(nameof(Pwm1MinValue));
            OnPropertyChanged(nameof(Pwm1MaxPoint));
            OnPropertyChanged(nameof(Pwm1MaxValue));
            OnPropertyChanged(nameof(Pwm2Name));
            OnPropertyChanged(nameof(Pwm2Unit));
            OnPropertyChanged(nameof(Pwm2MinPoint));
            OnPropertyChanged(nameof(Pwm2MinValue));
            OnPropertyChanged(nameof(Pwm2MaxPoint));
            OnPropertyChanged(nameof(Pwm2MaxValue));
            return Task.CompletedTask;
        }
    }
}