using Microsoft.Extensions.Logging;
using Vm167Box.Models;
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
        Load();
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

    private AnalogChannel _pwm1 = new();
    public AnalogChannel Pwm1 => _pwm1;

    private AnalogChannel _pwm2 = new();
    public AnalogChannel Pwm2 => _pwm2;

    public string Analog1Name
    {
        get => _settings.Get(nameof(Analog1Name), AppResources.Analog1);
        set
        {
            _settings.Set(nameof(Analog1Name), value);
            _analog1.Name = value;
            Update?.Invoke();
        }
    }

    public string Analog1Unit
    {
        get => _settings.Get(nameof(Analog1Unit), string.Empty);
        set
        {
            _settings.Set(nameof(Analog1Unit), value);
            _analog1.Unit = value;
            Update?.Invoke();
        }
    }

    public int Analog1MinSignal
    {
        get => _settings.Get(nameof(Analog1MinSignal), IVm167.AnalogMin);
        set
        {
            _settings.Set(nameof(Analog1MinSignal), value);
            _analog1.MinSignal = value;
            Update?.Invoke();
        }
    }

    public double Analog1MinValue
    {
        get => _settings.Get(nameof(Analog1MinValue), (double)IVm167.AnalogMin);
        set
        {
            _settings.Set(nameof(Analog1MinValue), value);
            _analog1.MinValue = value;
            Update?.Invoke();
        }
    }

    public int Analog1MaxSignal
    {
        get => _settings.Get(nameof(Analog1MaxSignal), IVm167.AnalogMax);
        set
        {
            _settings.Set(nameof(Analog1MaxSignal), value);
            _analog1.MaxSignal = value;
            Update?.Invoke();
        }
    }

    public double Analog1MaxValue
    {
        get => _settings.Get(nameof(Analog1MaxValue), (double)IVm167.AnalogMax);
        set
        {
            _settings.Set(nameof(Analog1MaxValue), value);
            _analog1.MaxValue = value;
            Update?.Invoke();
        }
    }

    public string Analog2Name
    {
        get => _settings.Get(nameof(Analog2Name), AppResources.Analog2);
        set
        {
            _settings.Set(nameof(Analog2Name), value);
            _analog2.Name = value;
            Update?.Invoke();
        }
    }

    public string Analog2Unit
    {
        get => _settings.Get(nameof(Analog2Unit), string.Empty);
        set
        {
            _settings.Set(nameof(Analog2Unit), value);
            _analog2.Unit = value;
            Update?.Invoke();
        }
    }

    public int Analog2MinSignal
    {
        get => _settings.Get(nameof(Analog2MinSignal), IVm167.AnalogMin);
        set
        {
            _settings.Set(nameof(Analog2MinSignal), value);
            _analog2.MinSignal = value;
            Update?.Invoke();
        }
    }

    public double Analog2MinValue
    {
        get => _settings.Get(nameof(Analog2MinValue), (double)IVm167.AnalogMin);
        set
        {
            _settings.Set(nameof(Analog2MinValue), value);
            _analog2.MinValue = value;
            Update?.Invoke();
        }
    }

    public int Analog2MaxSignal
    {
        get => _settings.Get(nameof(Analog2MaxSignal), IVm167.AnalogMax);
        set
        {
            _settings.Set(nameof(Analog2MaxSignal), value);
            _analog2.MaxSignal = value;
            Update?.Invoke();
        }
    }

    public double Analog2MaxValue
    {
        get => _settings.Get(nameof(Analog2MaxValue), (double)IVm167.AnalogMax);
        set
        {
            _settings.Set(nameof(Analog2MaxValue), value);
            _analog2.MaxValue = value;
            Update?.Invoke();
        }
    }

    public string Analog3Name
    {
        get => _settings.Get(nameof(Analog3Name), AppResources.Analog3);
        set
        {
            _settings.Set(nameof(Analog3Name), value);
            _analog3.Name = value;
            Update?.Invoke();
        }
    }

    public string Analog3Unit
    {
        get => _settings.Get(nameof(Analog3Unit), string.Empty);
        set
        {
            _settings.Set(nameof(Analog3Unit), value);
            _analog3.Unit = value;
            Update?.Invoke();
        }
    }

    public int Analog3MinSignal
    {
        get => _settings.Get(nameof(Analog3MinSignal), IVm167.AnalogMin);
        set
        {
            _settings.Set(nameof(Analog3MinSignal), value);
            _analog3.MinSignal = value;
            Update?.Invoke();
        }
    }

    public double Analog3MinValue
    {
        get => _settings.Get(nameof(Analog3MinValue), (double)IVm167.AnalogMin);
        set
        {
            _settings.Set(nameof(Analog3MinValue), value);
            _analog3.MinValue = value;
            Update?.Invoke();
        }
    }

    public int Analog3MaxSignal
    {
        get => _settings.Get(nameof(Analog3MaxSignal), IVm167.AnalogMax);
        set
        {
            _settings.Set(nameof(Analog3MaxSignal), value);
            _analog3.MaxSignal = value;
            Update?.Invoke();
        }
    }

    public double Analog3MaxValue
    {
        get => _settings.Get(nameof(Analog3MaxValue), (double)IVm167.AnalogMax);
        set
        {
            _settings.Set(nameof(Analog3MaxValue), value);
            _analog3.MaxValue = value;
            Update?.Invoke();
        }
    }

    public string Analog4Name
    {
        get => _settings.Get(nameof(Analog4Name), AppResources.Analog4);
        set
        {
            _settings.Set(nameof(Analog4Name), value);
            _analog4.Name = value;
            Update?.Invoke();
        }
    }

    public string Analog4Unit
    {
        get => _settings.Get(nameof(Analog4Unit), string.Empty);
        set
        {
            _settings.Set(nameof(Analog4Unit), value);
            _analog4.Unit = value;
            Update?.Invoke();
        }
    }

    public int Analog4MinSignal
    {
        get => _settings.Get(nameof(Analog4MinSignal), IVm167.AnalogMin);
        set
        {
            _settings.Set(nameof(Analog4MinSignal), value);
            _analog4.MinSignal = value;
            Update?.Invoke();
        }
    }

    public double Analog4MinValue
    {
        get => _settings.Get(nameof(Analog4MinValue), (double)IVm167.AnalogMin);
        set
        {
            _settings.Set(nameof(Analog4MinValue), value);
            _analog4.MinValue = value;
            Update?.Invoke();
        }
    }

    public int Analog4MaxSignal
    {
        get => _settings.Get(nameof(Analog4MaxSignal), IVm167.AnalogMax);
        set
        {
            _settings.Set(nameof(Analog4MaxSignal), value);
            _analog4.MaxSignal = value;
            Update?.Invoke();
        }
    }

    public double Analog4MaxValue
    {
        get => _settings.Get(nameof(Analog4MaxValue), (double)IVm167.AnalogMax);
        set
        {
            _settings.Set(nameof(Analog4MaxValue), value);
            _analog4.MaxValue = value;
            Update?.Invoke();
        }
    }

    public string Analog5Name
    {
        get => _settings.Get(nameof(Analog5Name), AppResources.Analog5);
        set
        {
            _settings.Set(nameof(Analog5Name), value);
            _analog5.Name = value;
            Update?.Invoke();
        }
    }

    public string Analog5Unit
    {
        get => _settings.Get(nameof(Analog5Unit), string.Empty);
        set
        {
            _settings.Set(nameof(Analog5Unit), value);
            _analog5.Unit = value;
            Update?.Invoke();
        }
    }

    public int Analog5MinSignal
    {
        get => _settings.Get(nameof(Analog5MinSignal), IVm167.AnalogMin);
        set
        {
            _settings.Set(nameof(Analog5MinSignal), value);
            _analog5.MinSignal = value;
            Update?.Invoke();
        }
    }

    public double Analog5MinValue
    {
        get => _settings.Get(nameof(Analog5MinValue), (double)IVm167.AnalogMin);
        set
        {
            _settings.Set(nameof(Analog5MinValue), value);
            _analog5.MinValue = value;
            Update?.Invoke();
        }
    }

    public int Analog5MaxSignal
    {
        get => _settings.Get(nameof(Analog5MaxSignal), IVm167.AnalogMax);
        set
        {
            _settings.Set(nameof(Analog5MaxSignal), value);
            _analog5.MaxSignal = value;
            Update?.Invoke();
        }
    }

    public double Analog5MaxValue
    {
        get => _settings.Get(nameof(Analog5MaxValue), (double)IVm167.AnalogMax);
        set
        {
            _settings.Set(nameof(Analog5MaxValue), value);
            _analog5.MaxValue = value;
            Update?.Invoke();
        }
    }

    public string Pwm1Name
    {
        get => _settings.Get(nameof(Pwm1Name), AppResources.Pwm1);
        set
        {
            _settings.Set(nameof(Pwm1Name), value);
            _pwm1.Name = value;
            Update?.Invoke();
        }
    }

    public string Pwm1Unit
    {
        get => _settings.Get(nameof(Pwm1Unit), string.Empty);
        set
        {
            _settings.Set(nameof(Pwm1Unit), value);
            _pwm1.Unit = value;
            Update?.Invoke();
        }
    }

    public int Pwm1MinSignal
    {
        get => _settings.Get(nameof(Pwm1MinSignal), IVm167.PwmMin);
        set
        {
            _settings.Set(nameof(Pwm1MinSignal), value);
            _pwm1.MinSignal = value;
            Update?.Invoke();
        }
    }

    public double Pwm1MinValue
    {
        get => _settings.Get(nameof(Pwm1MinValue), (double)IVm167.PwmMin);
        set
        {
            _settings.Set(nameof(Pwm1MinValue), value);
            _pwm1.MinValue = value;
            Update?.Invoke();
        }
    }

    public int Pwm1MaxSignal
    {
        get => _settings.Get(nameof(Pwm1MaxSignal), IVm167.PwmMax);
        set
        {
            _settings.Set(nameof(Pwm1MaxSignal), value);
            _pwm1.MaxSignal = value;
            Update?.Invoke();
        }
    }

    public double Pwm1MaxValue
    {
        get => _settings.Get(nameof(Pwm1MaxValue), (double)IVm167.PwmMax);
        set
        {
            _settings.Set(nameof(Pwm1MaxValue), value);
            _pwm1.MaxValue = value;
            Update?.Invoke();
        }
    }

    public string Pwm2Name
    {
        get => _settings.Get(nameof(Pwm2Name), AppResources.Pwm2);
        set
        {
            _settings.Set(nameof(Pwm2Name), value);
            _pwm2.Name = value;
            Update?.Invoke();
        }
    }

    public string Pwm2Unit
    {
        get => _settings.Get(nameof(Pwm2Unit), string.Empty);
        set
        {
            _settings.Set(nameof(Pwm2Unit), value);
            _pwm2.Unit = value;
            Update?.Invoke();
        }
    }

    public int Pwm2MinSignal
    {
        get => _settings.Get(nameof(Pwm2MinSignal), IVm167.PwmMin);
        set
        {
            _settings.Set(nameof(Pwm2MinSignal), value);
            _pwm2.MinSignal = value;
            Update?.Invoke();
        }
    }

    public double Pwm2MinValue
    {
        get => _settings.Get(nameof(Pwm2MinValue), (double)IVm167.PwmMin);
        set
        {
            _settings.Set(nameof(Pwm2MinValue), value);
            _pwm2.MinValue = value;
            Update?.Invoke();
        }
    }

    public int Pwm2MaxSignal
    {
        get => _settings.Get(nameof(Pwm2MaxSignal), IVm167.PwmMax);
        set
        {
            _settings.Set(nameof(Pwm2MaxSignal), value);
            _pwm2.MaxSignal = value;
            Update?.Invoke();
        }
    }

    public double Pwm2MaxValue
    {
        get => _settings.Get(nameof(Pwm2MaxValue), (double)IVm167.PwmMax);
        set
        {
            _settings.Set(nameof(Pwm2MaxValue), value);
            _pwm2.MaxValue = value;
            Update?.Invoke();
        }
    }

    public void Reset()
    {
        _settings.Clear();
        Load();
        Update?.Invoke();
    }

    private void Load()
    {
        _analog1.Name = Analog1Name;
        _analog1.Unit = Analog1Unit;
        _analog1.MinSignal = Analog1MinSignal;
        _analog1.MinValue = Analog1MinValue;
        _analog1.MaxSignal = Analog1MaxSignal;
        _analog1.MaxValue = Analog1MaxValue;
        _analog2.Name = Analog2Name;
        _analog2.Unit = Analog2Unit;
        _analog2.MinSignal = Analog2MinSignal;
        _analog2.MinValue = Analog2MinValue;
        _analog2.MaxSignal = Analog2MaxSignal;
        _analog2.MaxValue = Analog2MaxValue;
        _analog3.Name = Analog3Name;
        _analog3.Unit = Analog3Unit;
        _analog3.MinSignal = Analog3MinSignal;
        _analog3.MinValue = Analog3MinValue;
        _analog3.MaxSignal = Analog3MaxSignal;
        _analog3.MaxValue = Analog3MaxValue;
        _analog4.Name = Analog4Name;
        _analog4.Unit = Analog4Unit;
        _analog4.MinSignal = Analog4MinSignal;
        _analog4.MinValue = Analog4MinValue;
        _analog4.MaxSignal = Analog4MaxSignal;
        _analog4.MaxValue = Analog4MaxValue;
        _analog5.Name = Analog5Name;
        _analog5.Unit = Analog5Unit;
        _analog5.MinSignal = Analog5MinSignal;
        _analog5.MinValue = Analog5MinValue;
        _analog5.MaxSignal = Analog5MaxSignal;
        _analog5.MaxValue = Analog5MaxValue;
        _pwm1.Name = Pwm1Name;
        _pwm1.Unit = Pwm1Unit;
        _pwm1.MinSignal = Pwm1MinSignal;
        _pwm1.MinValue = Pwm1MinValue;
        _pwm1.MaxSignal = Pwm1MaxSignal;
        _pwm1.MaxValue = Pwm1MaxValue;
        _pwm2.Name = Pwm2Name;
        _pwm2.Unit = Pwm2Unit;
        _pwm2.MinSignal = Pwm2MinSignal;
        _pwm2.MinValue = Pwm2MinValue;
        _pwm2.MaxSignal = Pwm2MaxSignal;
        _pwm2.MaxValue = Pwm2MaxValue;
    }
}