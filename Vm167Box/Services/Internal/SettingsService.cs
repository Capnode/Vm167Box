using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
using Vm167Box.Models;
using Vm167Box.Resources;
using Vm167Lib;

namespace Vm167Box.Services.Internal;

internal class SettingsService : ISettingsService
{
    public const int Decimals = 2;

    public event Action? Update;

    private readonly ILogger<SettingsService> _logger;
    private readonly IPreferences _settings = Preferences.Default;

    private int _deviceId = 0;

    public SettingsService(ILogger<SettingsService> logger)
    {
        _logger = logger;
        LoadChannels();
    }

    public AppTheme AppTheme
    {
        get => _settings.Get(nameof(AppTheme), Application.Current?.PlatformAppTheme ?? AppTheme.Unspecified);
        set
        {
            _settings.Set(nameof(AppTheme), (int)value);
            Update?.Invoke();
        }
    }

    private AnalogChannel _analog1 = new();
    public AnalogChannel Analog1 => _analog1;

    private AnalogChannel _analog2 = new();
    public AnalogChannel Analog2 => _analog2;

    private AnalogChannel _analog3 = new();
    public AnalogChannel Analog3 => _analog3;

    private AnalogChannel _analog4 = new();
    public AnalogChannel Analog4 => _analog4;

    private AnalogChannel _analog5 = new();
    public AnalogChannel Analog5 => _analog5;

    private AnalogChannel _analog6 = new();
    public AnalogChannel Analog6 => _analog6;

    private AnalogChannel _pwm1 = new();
    public AnalogChannel Pwm1 => _pwm1;

    private AnalogChannel _pwm2 = new();
    public AnalogChannel Pwm2 => _pwm2;

    public string Analog1Name
    {
        get => _settings.Get(Key(), AppResources.Analog1);
        set
        {
            if (Analog1Name == value) return;
            _settings.Set(Key(), value);
            _analog1.Name = value;
            Update?.Invoke();
        }
    }

    public string Analog1Unit
    {
        get => _settings.Get(Key(), string.Empty);
        set
        {
            if (Analog1Unit == value) return;
            _settings.Set(Key(), value);
            _analog1.Unit = value;
            Update?.Invoke();
        }
    }

    public int Analog1Decimals
    {
        get => _settings.Get(Key(), Decimals);
        set
        {
            if (Analog1Decimals == value) return;
            _settings.Set(Key(), value);
            _analog1.Decimals = value;
            Update?.Invoke();
        }
    }

    public int Analog1MinSignal
    {
        get => _settings.Get(Key(), IVm167.AnalogMin);
        set
        {
            if (Analog1MinSignal == value) return;
            _settings.Set(Key(), value);
            _analog1.MinSignal = value;
            Update?.Invoke();
        }
    }

    public double Analog1MinValue
    {
        get => _settings.Get(Key(), (double)IVm167.AnalogMin);
        set
        {
            if (Analog1MinValue == value) return;
            _settings.Set(Key(), value);
            _analog1.MinValue = value;
            Update?.Invoke();
        }
    }

    public int Analog1MaxSignal
    {
        get => _settings.Get(Key(), IVm167.AnalogMax);
        set
        {
            if (Analog1MaxSignal == value) return;
            _settings.Set(Key(), value);
            _analog1.MaxSignal = value;
            Update?.Invoke();
        }
    }

    public double Analog1MaxValue
    {
        get => _settings.Get(Key(), (double)IVm167.AnalogMax);
        set
        {
            if (Analog1MaxValue == value) return;
            _settings.Set(Key(), value);
            _analog1.MaxValue = value;
            Update?.Invoke();
        }
    }

    public string Analog2Name
    {
        get => _settings.Get(Key(), AppResources.Analog2);
        set
        {
            if (Analog2Name == value) return;
            _settings.Set(Key(), value);
            _analog2.Name = value;
            Update?.Invoke();
        }
    }

    public string Analog2Unit
    {
        get => _settings.Get(Key(), string.Empty);
        set
        {
            if (Analog2Unit == value) return;
            _settings.Set(Key(), value);
            _analog2.Unit = value;
            Update?.Invoke();
        }
    }

    public int Analog2Decimals
    {
        get => _settings.Get(Key(), Decimals);
        set
        {
            if (Analog2Decimals == value) return;
            _settings.Set(Key(), value);
            _analog2.Decimals = value;
            Update?.Invoke();
        }
    }

    public int Analog2MinSignal
    {
        get => _settings.Get(Key(), IVm167.AnalogMin);
        set
        {
            if (Analog2MinSignal == value) return;
            _settings.Set(Key(), value);
            _analog2.MinSignal = value;
            Update?.Invoke();
        }
    }

    public double Analog2MinValue
    {
        get => _settings.Get(Key(), (double)IVm167.AnalogMin);
        set
        {
            if (Analog2MinValue == value) return;
            _settings.Set(Key(), value);
            _analog2.MinValue = value;
            Update?.Invoke();
        }
    }

    public int Analog2MaxSignal
    {
        get => _settings.Get(Key(), IVm167.AnalogMax);
        set
        {
            if (Analog2MaxSignal == value) return;
            _settings.Set(Key(), value);
            _analog2.MaxSignal = value;
            Update?.Invoke();
        }
    }

    public double Analog2MaxValue
    {
        get => _settings.Get(Key(), (double)IVm167.AnalogMax);
        set
        {
            if (Analog2MaxValue == value) return;
            _settings.Set(Key(), value);
            _analog2.MaxValue = value;
            Update?.Invoke();
        }
    }

    public string Analog3Name
    {
        get => _settings.Get(Key(), AppResources.Analog3);
        set
        {
            if (Analog3Name == value) return;
            _settings.Set(Key(), value);
            _analog3.Name = value;
            Update?.Invoke();
        }
    }

    public string Analog3Unit
    {
        get => _settings.Get(Key(), string.Empty);
        set
        {
            if (Analog3Unit == value) return;
            _settings.Set(Key(), value);
            _analog3.Unit = value;
            Update?.Invoke();
        }
    }

    public int Analog3Decimals
    {
        get => _settings.Get(Key(), Decimals);
        set
        {
            if (Analog3Decimals == value) return;
            _settings.Set(Key(), value);
            _analog3.Decimals = value;
            Update?.Invoke();
        }
    }

    public int Analog3MinSignal
    {
        get => _settings.Get(Key(), IVm167.AnalogMin);
        set
        {
            if (Analog3MinSignal == value) return;
            _settings.Set(Key(), value);
            _analog3.MinSignal = value;
            Update?.Invoke();
        }
    }

    public double Analog3MinValue
    {
        get => _settings.Get(Key(), (double)IVm167.AnalogMin);
        set
        {
            if (Analog3MinValue == value) return;
            _settings.Set(Key(), value);
            _analog3.MinValue = value;
            Update?.Invoke();
        }
    }

    public int Analog3MaxSignal
    {
        get => _settings.Get(Key(), IVm167.AnalogMax);
        set
        {
            if (Analog3MaxSignal == value) return;
            _settings.Set(Key(), value);
            _analog3.MaxSignal = value;
            Update?.Invoke();
        }
    }

    public double Analog3MaxValue
    {
        get => _settings.Get(Key(), (double)IVm167.AnalogMax);
        set
        {
            if (Analog3MaxValue == value) return;
            _settings.Set(Key(), value);
            _analog3.MaxValue = value;
            Update?.Invoke();
        }
    }

    public string Analog4Name
    {
        get => _settings.Get(Key(), AppResources.Analog4);
        set
        {
            if (Analog4Name == value) return;
            _settings.Set(Key(), value);
            _analog4.Name = value;
            Update?.Invoke();
        }
    }

    public string Analog4Unit
    {
        get => _settings.Get(Key(), string.Empty);
        set
        {
            if (Analog4Unit == value) return;
            _settings.Set(Key(), value);
            _analog4.Unit = value;
            Update?.Invoke();
        }
    }

    public int Analog4Decimals
    {
        get => _settings.Get(Key(), Decimals);
        set
        {
            if (Analog4Decimals == value) return;
            _settings.Set(Key(), value);
            _analog4.Decimals = value;
            Update?.Invoke();
        }
    }

    public int Analog4MinSignal
    {
        get => _settings.Get(Key(), IVm167.AnalogMin);
        set
        {
            if (Analog4MinSignal == value) return;
            _settings.Set(Key(), value);
            _analog4.MinSignal = value;
            Update?.Invoke();
        }
    }

    public double Analog4MinValue
    {
        get => _settings.Get(Key(), (double)IVm167.AnalogMin);
        set
        {
            if (Analog4MinValue == value) return;
            _settings.Set(Key(), value);
            _analog4.MinValue = value;
            Update?.Invoke();
        }
    }

    public int Analog4MaxSignal
    {
        get => _settings.Get(Key(), IVm167.AnalogMax);
        set
        {
            if (Analog4MaxSignal == value) return;
            _settings.Set(Key(), value);
            _analog4.MaxSignal = value;
            Update?.Invoke();
        }
    }

    public double Analog4MaxValue
    {
        get => _settings.Get(Key(), (double)IVm167.AnalogMax);
        set
        {
            if (Analog4MaxValue == value) return;
            _settings.Set(Key(), value);
            _analog4.MaxValue = value;
            Update?.Invoke();
        }
    }

    public string Analog5Name
    {
        get => _settings.Get(Key(), AppResources.Analog5);
        set
        {
            if (Analog5Name == value) return;
            _settings.Set(Key(), value);
            _analog5.Name = value;
            Update?.Invoke();
        }
    }

    public string Analog5Unit
    {
        get => _settings.Get(Key(), string.Empty);
        set
        {
            if (Analog5Unit == value) return;
            _settings.Set(Key(), value);
            _analog5.Unit = value;
            Update?.Invoke();
        }
    }

    public int Analog5Decimals
    {
        get => _settings.Get(Key(), Decimals);
        set
        {
            if (Analog5Decimals == value) return;
            _settings.Set(Key(), value);
            _analog5.Decimals = value;
            Update?.Invoke();
        }
    }

    public int Analog5MinSignal
    {
        get => _settings.Get(Key(), IVm167.AnalogMin);
        set
        {
            if (Analog5MinSignal == value) return;
            _settings.Set(Key(), value);
            _analog5.MinSignal = value;
            Update?.Invoke();
        }
    }

    public double Analog5MinValue
    {
        get => _settings.Get(Key(), (double)IVm167.AnalogMin);
        set
        {
            if (Analog5MinValue == value) return;
            _settings.Set(Key(), value);
            _analog5.MinValue = value;
            Update?.Invoke();
        }
    }

    public int Analog5MaxSignal
    {
        get => _settings.Get(Key(), IVm167.AnalogMax);
        set
        {
            if (Analog5MaxSignal == value) return;
            _settings.Set(Key(), value);
            _analog5.MaxSignal = value;
            Update?.Invoke();
        }
    }

    public double Analog5MaxValue
    {
        get => _settings.Get(Key(), (double)IVm167.AnalogMax);
        set
        {
            if (Analog5MaxValue == value) return;
            _settings.Set(Key(), value);
            _analog5.MaxValue = value;
            Update?.Invoke();
        }
    }

    public string Analog6Name
    {
        get => _settings.Get(Key(), AppResources.Analog6);
        set
        {
            if (Analog6Name == value) return;
            _settings.Set(Key(), value);
            _analog6.Name = value;
            Update?.Invoke();
        }
    }

    public string Analog6Unit
    {
        get => _settings.Get(Key(), string.Empty);
        set
        {
            if (Analog6Unit == value) return;
            _settings.Set(Key(), value);
            _analog6.Unit = value;
            Update?.Invoke();
        }
    }

    public int Analog6Decimals
    {
        get => _settings.Get(Key(), Decimals);
        set
        {
            if (Analog6Decimals == value) return;
            _settings.Set(Key(), value);
            _analog6.Decimals = value;
            Update?.Invoke();
        }
    }

    public int Analog6MinSignal
    {
        get => _settings.Get(Key(), IVm167.AnalogMin);
        set
        {
            if (Analog6MinSignal == value) return;
            _settings.Set(Key(), value);
            _analog6.MinSignal = value;
            Update?.Invoke();
        }
    }

    public double Analog6MinValue
    {
        get => _settings.Get(Key(), (double)IVm167.AnalogMin);
        set
        {
            if (Analog6MinValue == value) return;
            _settings.Set(Key(), value);
            _analog6.MinValue = value;
            Update?.Invoke();
        }
    }

    public int Analog6MaxSignal
    {
        get => _settings.Get(Key(), IVm167.AnalogMax);
        set
        {
            if (Analog6MaxSignal == value) return;
            _settings.Set(Key(), value);
            _analog6.MaxSignal = value;
            Update?.Invoke();
        }
    }

    public double Analog6MaxValue
    {
        get => _settings.Get(Key(), (double)IVm167.AnalogMax);
        set
        {
            if (Analog6MaxValue == value) return;
            _settings.Set(Key(), value);
            _analog6.MaxValue = value;
            Update?.Invoke();
        }
    }

    public string Pwm1Name
    {
        get => _settings.Get(Key(), AppResources.Pwm1);
        set
        {
            if (Pwm1Name == value) return;
            _settings.Set(Key(), value);
            _pwm1.Name = value;
            Update?.Invoke();
        }
    }

    public string Pwm1Unit
    {
        get => _settings.Get(Key(), string.Empty);
        set
        {
            if (Pwm1Unit == value) return;
            _settings.Set(Key(), value);
            _pwm1.Unit = value;
            Update?.Invoke();
        }
    }

    public int Pwm1Decimals
    {
        get => _settings.Get(Key(), Decimals);
        set
        {
            if (Pwm1Decimals == value) return;
            _settings.Set(Key(), value);
            _pwm1.Decimals = value;
            Update?.Invoke();
        }
    }

    public int Pwm1MinSignal
    {
        get => _settings.Get(Key(), IVm167.PwmMin);
        set
        {
            if (Pwm1MinSignal == value) return;
            _settings.Set(Key(), value);
            _pwm1.MinSignal = value;
            Update?.Invoke();
        }
    }

    public double Pwm1MinValue
    {
        get => _settings.Get(Key(), (double)IVm167.PwmMin);
        set
        {
            if (Pwm1MinValue == value) return;
            _settings.Set(Key(), value);
            _pwm1.MinValue = value;
            Update?.Invoke();
        }
    }

    public int Pwm1MaxSignal
    {
        get => _settings.Get(Key(), IVm167.PwmMax);
        set
        {
            if (Pwm1MaxSignal == value) return;
            _settings.Set(Key(), value);
            _pwm1.MaxSignal = value;
            Update?.Invoke();
        }
    }

    public double Pwm1MaxValue
    {
        get => _settings.Get(Key(), (double)IVm167.PwmMax);
        set
        {
            if (Pwm1MaxValue == value) return;
            _settings.Set(Key(), value);
            _pwm1.MaxValue = value;
            Update?.Invoke();
        }
    }

    public string Pwm2Name
    {
        get => _settings.Get(Key(), AppResources.Pwm2);
        set
        {
            if (Pwm2Name == value) return;
            _settings.Set(Key(), value);
            _pwm2.Name = value;
            Update?.Invoke();
        }
    }

    public string Pwm2Unit
    {
        get => _settings.Get(Key(), string.Empty);
        set
        {
            if (Pwm2Unit == value) return;
            _settings.Set(Key(), value);
            _pwm2.Unit = value;
            Update?.Invoke();
        }
    }

    public int Pwm2Decimals
    {
        get => _settings.Get(Key(), Decimals);
        set
        {
            if (Pwm2Decimals == value) return;
            _settings.Set(Key(), value);
            _pwm2.Decimals = value;
            Update?.Invoke();
        }
    }

    public int Pwm2MinSignal
    {
        get => _settings.Get(Key(), IVm167.PwmMin);
        set
        {
            if (Pwm2MinSignal == value) return;
            _settings.Set(Key(), value);
            _pwm2.MinSignal = value;
            Update?.Invoke();
        }
    }

    public double Pwm2MinValue
    {
        get => _settings.Get(Key(), (double)IVm167.PwmMin);
        set
        {
            if (Pwm2MinValue == value) return;
            _settings.Set(Key(), value);
            _pwm2.MinValue = value;
            Update?.Invoke();
        }
    }

    public int Pwm2MaxSignal
    {
        get => _settings.Get(Key(), IVm167.PwmMax);
        set
        {
            if (Pwm2MaxSignal == value) return;
            _settings.Set(Key(), value);
            _pwm2.MaxSignal = value;
            Update?.Invoke();
        }
    }

    public double Pwm2MaxValue
    {
        get => _settings.Get(Key(), (double)IVm167.PwmMax);
        set
        {
            if (Pwm2MaxValue == value) return;
            _settings.Set(Key(), value);
            _pwm2.MaxValue = value;
            Update?.Invoke();
        }
    }

    public int ReferenceIndex
    {
        get => _settings.Get(Key(), 0);
        set
        {
            if (ReferenceIndex == value) return;
            _settings.Set(Key(), value);
            Update?.Invoke();
        }
    }

    public int FeedbackIndex
    {
        get => _settings.Get(Key(), 0);
        set
        {
            if (FeedbackIndex == value) return;
            _settings.Set(Key(), value);
            Update?.Invoke();
        }
    }

    public int ControlIndex
    {
        get => _settings.Get(Key(), 0);
        set
        {
            if (ControlIndex == value) return;
            _settings.Set(Key(), value);
            Update?.Invoke();
        }
    }

    public double RegulatorKp
    {
        get => _settings.Get(Key(), 1d);
        set
        {
            _settings.Set(Key(), value);
        }
    }

    public double RegulatorKi
    {
        get => _settings.Get(Key(), 0d);
        set
        {
            _settings.Set(Key(), value);
        }
    }

    public double RegulatorKd
    {
        get => _settings.Get(Key(), 0d);
        set
        {
            _settings.Set(Key(), value);
        }
    }

    public string Safety
    {
        get => _settings.Get(Key(), string.Empty);
        set
        {
            _settings.Set(Key(), value);
        }
    }

    public void Initialize(int deviceId)
    {
        _deviceId = deviceId;
        LoadChannels();
        Update?.Invoke();
    }

    public void Reset()
    {
        _settings.Clear();
        LoadChannels();
        Update?.Invoke();
    }

    private string Key([CallerMemberName] string? propertyName = null)
    {
        return $"{propertyName}[{_deviceId}]";
    }

    private void LoadChannels()
    {
        _analog1.Name = Analog1Name;
        _analog1.Unit = Analog1Unit;
        _analog1.Decimals = Analog1Decimals;
        _analog1.MinSignal = Analog1MinSignal;
        _analog1.MinValue = Analog1MinValue;
        _analog1.MaxSignal = Analog1MaxSignal;
        _analog1.MaxValue = Analog1MaxValue;
        _analog2.Name = Analog2Name;
        _analog2.Unit = Analog2Unit;
        _analog2.Decimals = Analog2Decimals;
        _analog2.MinSignal = Analog2MinSignal;
        _analog2.MinValue = Analog2MinValue;
        _analog2.MaxSignal = Analog2MaxSignal;
        _analog2.MaxValue = Analog2MaxValue;
        _analog3.Name = Analog3Name;
        _analog3.Unit = Analog3Unit;
        _analog3.Decimals = Analog3Decimals;
        _analog3.MinSignal = Analog3MinSignal;
        _analog3.MinValue = Analog3MinValue;
        _analog3.MaxSignal = Analog3MaxSignal;
        _analog3.MaxValue = Analog3MaxValue;
        _analog4.Name = Analog4Name;
        _analog4.Unit = Analog4Unit;
        _analog4.Decimals = Analog4Decimals;
        _analog4.MinSignal = Analog4MinSignal;
        _analog4.MinValue = Analog4MinValue;
        _analog4.MaxSignal = Analog4MaxSignal;
        _analog4.MaxValue = Analog4MaxValue;
        _analog5.Name = Analog5Name;
        _analog5.Unit = Analog5Unit;
        _analog5.Decimals = Analog5Decimals;
        _analog5.MinSignal = Analog5MinSignal;
        _analog5.MinValue = Analog5MinValue;
        _analog5.MaxSignal = Analog5MaxSignal;
        _analog5.MaxValue = Analog5MaxValue;
        _analog6.Name = Analog6Name;
        _analog6.Unit = Analog6Unit;
        _analog6.Decimals = Analog6Decimals;
        _analog6.MinSignal = Analog6MinSignal;
        _analog6.MinValue = Analog6MinValue;
        _analog6.MaxSignal = Analog6MaxSignal;
        _analog6.MaxValue = Analog6MaxValue;
        _pwm1.Name = Pwm1Name;
        _pwm1.Unit = Pwm1Unit;
        _pwm1.Decimals = Pwm1Decimals;
        _pwm1.MinSignal = Pwm1MinSignal;
        _pwm1.MinValue = Pwm1MinValue;
        _pwm1.MaxSignal = Pwm1MaxSignal;
        _pwm1.MaxValue = Pwm1MaxValue;
        _pwm2.Name = Pwm2Name;
        _pwm2.Unit = Pwm2Unit;
        _pwm2.Decimals = Pwm2Decimals;
        _pwm2.MinSignal = Pwm2MinSignal;
        _pwm2.MinValue = Pwm2MinValue;
        _pwm2.MaxSignal = Pwm2MaxSignal;
        _pwm2.MaxValue = Pwm2MaxValue;
    }
}