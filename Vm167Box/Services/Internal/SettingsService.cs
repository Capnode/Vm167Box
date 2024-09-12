using Microsoft.Extensions.Logging;
using Vm167Box.Resources;
using Vm167Lib;

namespace Vm167Box.Services.Internal;

internal class SettingsService : ISettingsService
{
    public event Func<Task>? Update;
    
    private readonly ILogger<SettingsService> _logger;
    private readonly IPreferences _settings = Preferences.Default;

    public SettingsService(ILogger<SettingsService> logger)
    {
        _logger = logger;
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

    public string Analog1Name
    {
        get => _settings.Get(nameof(Analog1Name), AppResources.Analog1);
        set
        {
            _settings.Set(nameof(Analog1Name), value);
            Update?.Invoke();
        }
    }

    public string Analog1Unit
    {
        get => _settings.Get(nameof(Analog1Unit), string.Empty);
        set
        {
            _settings.Set(nameof(Analog1Unit), value);
            Update?.Invoke();
        }
    }

    public int Analog1MinSignal
    {
        get => _settings.Get(nameof(Analog1MinSignal), IVm167.AnalogMin);
        set
        {
            _settings.Set(nameof(Analog1MinSignal), value);
            Update?.Invoke();
        }
    }

    public double Analog1MinValue
    {
        get => _settings.Get(nameof(Analog1MinValue), (double)IVm167.AnalogMin);
        set
        {
            _settings.Set(nameof(Analog1MinValue), value);
            Update?.Invoke();
        }
    }

    public int Analog1MaxSignal
    {
        get => _settings.Get(nameof(Analog1MaxSignal), IVm167.AnalogMax);
        set
        {
            _settings.Set(nameof(Analog1MaxSignal), value);
            Update?.Invoke();
        }
    }

    public double Analog1MaxValue
    {
        get => _settings.Get(nameof(Analog1MaxValue), (double)IVm167.AnalogMax);
        set
        {
            _settings.Set(nameof(Analog1MaxValue), value);
            Update?.Invoke();
        }
    }

    public string Analog2Name
    {
        get => _settings.Get(nameof(Analog2Name), AppResources.Analog2);
        set
        {
            _settings.Set(nameof(Analog2Name), value);
            Update?.Invoke();
        }
    }

    public string Analog2Unit
    {
        get => _settings.Get(nameof(Analog2Unit), string.Empty);
        set
        {
            _settings.Set(nameof(Analog2Unit), value);
            Update?.Invoke();
        }
    }

    public int Analog2MinSignal
    {
        get => _settings.Get(nameof(Analog2MinSignal), IVm167.AnalogMin);
        set
        {
            _settings.Set(nameof(Analog2MinSignal), value);
            Update?.Invoke();
        }
    }

    public double Analog2MinValue
    {
        get => _settings.Get(nameof(Analog2MinValue), (double)IVm167.AnalogMin);
        set
        {
            _settings.Set(nameof(Analog2MinValue), value);
            Update?.Invoke();
        }
    }

    public int Analog2MaxSignal
    {
        get => _settings.Get(nameof(Analog2MaxSignal), IVm167.AnalogMax);
        set
        {
            _settings.Set(nameof(Analog2MaxSignal), value);
            Update?.Invoke();
        }
    }

    public double Analog2MaxValue
    {
        get => _settings.Get(nameof(Analog2MaxValue), (double)IVm167.AnalogMax);
        set
        {
            _settings.Set(nameof(Analog2MaxValue), value);
            Update?.Invoke();
        }
    }

    public string Analog3Name
    {
        get => _settings.Get(nameof(Analog3Name), AppResources.Analog3);
        set
        {
            _settings.Set(nameof(Analog3Name), value);
            Update?.Invoke();
        }
    }

    public string Analog3Unit
    {
        get => _settings.Get(nameof(Analog3Unit), string.Empty);
        set
        {
            _settings.Set(nameof(Analog3Unit), value);
            Update?.Invoke();
        }
    }

    public int Analog3MinSignal
    {
        get => _settings.Get(nameof(Analog3MinSignal), IVm167.AnalogMin);
        set
        {
            _settings.Set(nameof(Analog3MinSignal), value);
            Update?.Invoke();
        }
    }

    public double Analog3MinValue
    {
        get => _settings.Get(nameof(Analog3MinValue), (double)IVm167.AnalogMin);
        set
        {
            _settings.Set(nameof(Analog3MinValue), value);
            Update?.Invoke();
        }
    }

    public int Analog3MaxSignal
    {
        get => _settings.Get(nameof(Analog3MaxSignal), IVm167.AnalogMax);
        set
        {
            _settings.Set(nameof(Analog3MaxSignal), value);
            Update?.Invoke();
        }
    }

    public double Analog3MaxValue
    {
        get => _settings.Get(nameof(Analog3MaxValue), (double)IVm167.AnalogMax);
        set
        {
            _settings.Set(nameof(Analog3MaxValue), value);
            Update?.Invoke();
        }
    }

    public string Analog4Name
    {
        get => _settings.Get(nameof(Analog4Name), AppResources.Analog4);
        set
        {
            _settings.Set(nameof(Analog4Name), value);
            Update?.Invoke();
        }
    }

    public string Analog4Unit
    {
        get => _settings.Get(nameof(Analog4Unit), string.Empty);
        set
        {
            _settings.Set(nameof(Analog4Unit), value);
            Update?.Invoke();
        }
    }

    public int Analog4MinSignal
    {
        get => _settings.Get(nameof(Analog4MinSignal), IVm167.AnalogMin);
        set
        {
            _settings.Set(nameof(Analog4MinSignal), value);
            Update?.Invoke();
        }
    }

    public double Analog4MinValue
    {
        get => _settings.Get(nameof(Analog4MinValue), (double)IVm167.AnalogMin);
        set
        {
            _settings.Set(nameof(Analog4MinValue), value);
            Update?.Invoke();
        }
    }

    public int Analog4MaxSignal
    {
        get => _settings.Get(nameof(Analog4MaxSignal), IVm167.AnalogMax);
        set
        {
            _settings.Set(nameof(Analog4MaxSignal), value);
            Update?.Invoke();
        }
    }

    public double Analog4MaxValue
    {
        get => _settings.Get(nameof(Analog4MaxValue), (double)IVm167.AnalogMax);
        set
        {
            _settings.Set(nameof(Analog4MaxValue), value);
            Update?.Invoke();
        }
    }

    public string Analog5Name
    {
        get => _settings.Get(nameof(Analog5Name), AppResources.Analog5);
        set
        {
            _settings.Set(nameof(Analog5Name), value);
            Update?.Invoke();
        }
    }

    public string Analog5Unit
    {
        get => _settings.Get(nameof(Analog5Unit), string.Empty);
        set
        {
            _settings.Set(nameof(Analog5Unit), value);
            Update?.Invoke();
        }
    }

    public int Analog5MinSignal
    {
        get => _settings.Get(nameof(Analog5MinSignal), IVm167.AnalogMin);
        set
        {
            _settings.Set(nameof(Analog5MinSignal), value);
            Update?.Invoke();
        }
    }

    public double Analog5MinValue
    {
        get => _settings.Get(nameof(Analog5MinValue), (double)IVm167.AnalogMin);
        set
        {
            _settings.Set(nameof(Analog5MinValue), value);
            Update?.Invoke();
        }
    }

    public int Analog5MaxSignal
    {
        get => _settings.Get(nameof(Analog5MaxSignal), IVm167.AnalogMax);
        set
        {
            _settings.Set(nameof(Analog5MaxSignal), value);
            Update?.Invoke();
        }
    }

    public double Analog5MaxValue
    {
        get => _settings.Get(nameof(Analog5MaxValue), (double)IVm167.AnalogMax);
        set
        {
            _settings.Set(nameof(Analog5MaxValue), value);
            Update?.Invoke();
        }
    }

    public string Pwm1Name
    {
        get => _settings.Get(nameof(Pwm1Name), AppResources.Pwm1);
        set
        {
            _settings.Set(nameof(Pwm1Name), value);
            Update?.Invoke();
        }
    }

    public string Pwm1Unit
    {
        get => _settings.Get(nameof(Pwm1Unit), string.Empty);
        set
        {
            _settings.Set(nameof(Pwm1Unit), value);
            Update?.Invoke();
        }
    }

    public int Pwm1MinSignal
    {
        get => _settings.Get(nameof(Pwm1MinSignal), IVm167.PwmMin);
        set
        {
            _settings.Set(nameof(Pwm1MinSignal), value);
            Update?.Invoke();
        }
    }

    public double Pwm1MinValue
    {
        get => _settings.Get(nameof(Pwm1MinValue), (double)IVm167.PwmMin);
        set
        {
            _settings.Set(nameof(Pwm1MinValue), value);
            Update?.Invoke();
        }
    }

    public int Pwm1MaxSignal
    {
        get => _settings.Get(nameof(Pwm1MaxSignal), IVm167.PwmMax);
        set
        {
            _settings.Set(nameof(Pwm1MaxSignal), value);
            Update?.Invoke();
        }
    }

    public double Pwm1MaxValue
    {
        get => _settings.Get(nameof(Pwm1MaxValue), (double)IVm167.PwmMax);
        set
        {
            _settings.Set(nameof(Pwm1MaxValue), value);
            Update?.Invoke();
        }
    }

    public string Pwm2Name
    {
        get => _settings.Get(nameof(Pwm2Name), AppResources.Pwm2);
        set
        {
            _settings.Set(nameof(Pwm2Name), value);
            Update?.Invoke();
        }
    }

    public string Pwm2Unit
    {
        get => _settings.Get(nameof(Pwm2Unit), string.Empty);
        set
        {
            _settings.Set(nameof(Pwm2Unit), value);
            Update?.Invoke();
        }
    }

    public int Pwm2MinSignal
    {
        get => _settings.Get(nameof(Pwm2MinSignal), IVm167.PwmMin);
        set
        {
            _settings.Set(nameof(Pwm2MinSignal), value);
            Update?.Invoke();
        }
    }

    public double Pwm2MinValue
    {
        get => _settings.Get(nameof(Pwm2MinValue), (double)IVm167.PwmMin);
        set
        {
            _settings.Set(nameof(Pwm2MinValue), value);
            Update?.Invoke();
        }
    }

    public int Pwm2MaxSignal
    {
        get => _settings.Get(nameof(Pwm2MaxSignal), IVm167.PwmMax);
        set
        {
            _settings.Set(nameof(Pwm2MaxSignal), value);
            Update?.Invoke();
        }
    }

    public double Pwm2MaxValue
    {
        get => _settings.Get(nameof(Pwm2MaxValue), (double)IVm167.PwmMax);
        set
        {
            _settings.Set(nameof(Pwm2MaxValue), value);
            Update?.Invoke();
        }
    }

    public void Reset()
    {
        _settings.Clear();
        Update?.Invoke();
    }
}