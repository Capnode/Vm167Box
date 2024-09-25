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

        public int Analog1Decimals
        {
            get => _settingsService.Analog1Decimals;
            set => _settingsService.Analog1Decimals = value;
        }

        public int Analog1MinSignal
        {
            get => _settingsService.Analog1MinSignal;
            set => _settingsService.Analog1MinSignal = value;
        }

        public double Analog1MinValue
        {
            get => _settingsService.Analog1MinValue;
            set => _settingsService.Analog1MinValue = value;
        }

        public int Analog1MaxSignal
        {
            get => _settingsService.Analog1MaxSignal;
            set => _settingsService.Analog1MaxSignal = value;
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

        public int Analog2Decimals
        {
            get => _settingsService.Analog2Decimals;
            set => _settingsService.Analog2Decimals = value;
        }

        public int Analog2MinSignal
        {
            get => _settingsService.Analog2MinSignal;
            set => _settingsService.Analog2MinSignal = value;
        }

        public double Analog2MinValue
        {
            get => _settingsService.Analog2MinValue;
            set => _settingsService.Analog2MinValue = value;
        }

        public int Analog2MaxSignal
        {
            get => _settingsService.Analog2MaxSignal;
            set => _settingsService.Analog2MaxSignal = value;
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

        public int Analog3Decimals
        {
            get => _settingsService.Analog3Decimals;
            set => _settingsService.Analog3Decimals = value;
        }

        public int Analog3MinSignal
        {
            get => _settingsService.Analog3MinSignal;
            set => _settingsService.Analog3MinSignal = value;
        }

        public double Analog3MinValue
        {
            get => _settingsService.Analog3MinValue;
            set => _settingsService.Analog3MinValue = value;
        }

        public int Analog3MaxSignal
        {
            get => _settingsService.Analog3MaxSignal;
            set => _settingsService.Analog3MaxSignal = value;
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

        public int Analog4Decimals
        {
            get => _settingsService.Analog4Decimals;
            set => _settingsService.Analog4Decimals = value;
        }

        public int Analog4MinSignal
        {
            get => _settingsService.Analog4MinSignal;
            set => _settingsService.Analog4MinSignal = value;
        }

        public double Analog4MinValue
        {
            get => _settingsService.Analog4MinValue;
            set => _settingsService.Analog4MinValue = value;
        }

        public int Analog4MaxSignal
        {
            get => _settingsService.Analog4MaxSignal;
            set => _settingsService.Analog4MaxSignal = value;
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

        public int Analog5Decimals
        {
            get => _settingsService.Analog5Decimals;
            set => _settingsService.Analog5Decimals = value;
        }

        public int Analog5MinSignal
        {
            get => _settingsService.Analog5MinSignal;
            set => _settingsService.Analog5MinSignal = value;
        }

        public double Analog5MinValue
        {
            get => _settingsService.Analog5MinValue;
            set => _settingsService.Analog5MinValue = value;
        }

        public int Analog5MaxSignal
        {
            get => _settingsService.Analog5MaxSignal;
            set => _settingsService.Analog5MaxSignal = value;
        }

        public double Analog5MaxValue
        {
            get => _settingsService.Analog5MaxValue;
            set => _settingsService.Analog5MaxValue = value;
        }

        public string Analog6Name
        {
            get => _settingsService.Analog6Name;
            set => _settingsService.Analog6Name = value;
        }

        public string Analog6Unit
        {
            get => _settingsService.Analog6Unit;
            set => _settingsService.Analog6Unit = value;
        }

        public int Analog6Decimals
        {
            get => _settingsService.Analog6Decimals;
            set => _settingsService.Analog6Decimals = value;
        }

        public int Analog6MinSignal
        {
            get => _settingsService.Analog6MinSignal;
            set => _settingsService.Analog6MinSignal = value;
        }

        public double Analog6MinValue
        {
            get => _settingsService.Analog6MinValue;
            set => _settingsService.Analog6MinValue = value;
        }

        public int Analog6MaxSignal
        {
            get => _settingsService.Analog6MaxSignal;
            set => _settingsService.Analog6MaxSignal = value;
        }

        public double Analog6MaxValue
        {
            get => _settingsService.Analog6MaxValue;
            set => _settingsService.Analog6MaxValue = value;
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

        public int Pwm1Decimals
        {
            get => _settingsService.Pwm1Decimals;
            set => _settingsService.Pwm1Decimals = value;
        }

        public int Pwm1MinSignal
        {
            get => _settingsService.Pwm1MinSignal;
            set => _settingsService.Pwm1MinSignal = value;
        }

        public double Pwm1MinValue
        {
            get => _settingsService.Pwm1MinValue;
            set => _settingsService.Pwm1MinValue = value;
        }

        public int Pwm1MaxSignal
        {
            get => _settingsService.Pwm1MaxSignal;
            set => _settingsService.Pwm1MaxSignal = value;
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

        public int Pwm2Decimals
        {
            get => _settingsService.Pwm2Decimals;
            set => _settingsService.Pwm2Decimals = value;
        }

        public int Pwm2MinSignal
        {
            get => _settingsService.Pwm2MinSignal;
            set => _settingsService.Pwm2MinSignal = value;
        }

        public double Pwm2MinValue
        {
            get => _settingsService.Pwm2MinValue;
            set => _settingsService.Pwm2MinValue = value;
        }

        public int Pwm2MaxSignal
        {
            get => _settingsService.Pwm2MaxSignal;
            set => _settingsService.Pwm2MaxSignal = value;
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

        private void UpdateSettings()
        {
            OnPropertyChanged(nameof(IsLightTheme));
            OnPropertyChanged(nameof(Analog1Name));
            OnPropertyChanged(nameof(Analog1Unit));
            OnPropertyChanged(nameof(Analog1Decimals));
            OnPropertyChanged(nameof(Analog1MinSignal));
            OnPropertyChanged(nameof(Analog1MinValue));
            OnPropertyChanged(nameof(Analog1MaxSignal));
            OnPropertyChanged(nameof(Analog1MaxValue));
            OnPropertyChanged(nameof(Analog2Name));
            OnPropertyChanged(nameof(Analog2Unit));
            OnPropertyChanged(nameof(Analog2Decimals));
            OnPropertyChanged(nameof(Analog2MinSignal));
            OnPropertyChanged(nameof(Analog2MinValue));
            OnPropertyChanged(nameof(Analog2MaxSignal));
            OnPropertyChanged(nameof(Analog2MaxValue));
            OnPropertyChanged(nameof(Analog3Name));
            OnPropertyChanged(nameof(Analog3Unit));
            OnPropertyChanged(nameof(Analog3Decimals));
            OnPropertyChanged(nameof(Analog3MinSignal));
            OnPropertyChanged(nameof(Analog3MinValue));
            OnPropertyChanged(nameof(Analog3MaxSignal));
            OnPropertyChanged(nameof(Analog3MaxValue));
            OnPropertyChanged(nameof(Analog4Name));
            OnPropertyChanged(nameof(Analog4Unit));
            OnPropertyChanged(nameof(Analog4Decimals));
            OnPropertyChanged(nameof(Analog4MinSignal));
            OnPropertyChanged(nameof(Analog4MinValue));
            OnPropertyChanged(nameof(Analog4MaxSignal));
            OnPropertyChanged(nameof(Analog4MaxValue));
            OnPropertyChanged(nameof(Analog5Name));
            OnPropertyChanged(nameof(Analog5Unit));
            OnPropertyChanged(nameof(Analog5Decimals));
            OnPropertyChanged(nameof(Analog5MinSignal));
            OnPropertyChanged(nameof(Analog5MinValue));
            OnPropertyChanged(nameof(Analog5MaxSignal));
            OnPropertyChanged(nameof(Analog5MaxValue));
            OnPropertyChanged(nameof(Analog6Name));
            OnPropertyChanged(nameof(Analog6Unit));
            OnPropertyChanged(nameof(Analog6Decimals));
            OnPropertyChanged(nameof(Analog6MinSignal));
            OnPropertyChanged(nameof(Analog6MinValue));
            OnPropertyChanged(nameof(Analog6MaxSignal));
            OnPropertyChanged(nameof(Analog6MaxValue));
            OnPropertyChanged(nameof(Pwm1Name));
            OnPropertyChanged(nameof(Pwm1Unit));
            OnPropertyChanged(nameof(Pwm1Decimals));
            OnPropertyChanged(nameof(Pwm1MinSignal));
            OnPropertyChanged(nameof(Pwm1MinValue));
            OnPropertyChanged(nameof(Pwm1MaxSignal));
            OnPropertyChanged(nameof(Pwm1MaxValue));
            OnPropertyChanged(nameof(Pwm2Name));
            OnPropertyChanged(nameof(Pwm2Unit));
            OnPropertyChanged(nameof(Pwm2Decimals));
            OnPropertyChanged(nameof(Pwm2MinSignal));
            OnPropertyChanged(nameof(Pwm2MinValue));
            OnPropertyChanged(nameof(Pwm2MaxSignal));
            OnPropertyChanged(nameof(Pwm2MaxValue));
        }
    }
}