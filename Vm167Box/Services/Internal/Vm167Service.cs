using Microsoft.Extensions.Logging;
using Vm167Box.Helpers;
using Vm167Box.Models;
using Vm167Lib;

namespace Vm167Box.Services.Internal;

internal sealed class Vm167Service : IVm167Service, IDisposable
{
    public event Func<Task>? Tick;

    private readonly ILogger<Vm167Service> _logger;
    private readonly ISettingsService _settingsService;
    private readonly IVm167 _vm167;
    private readonly Timer _timer;
    private readonly SemaphoreSlim _lock = new(1, 1);

    private int _device = -1;
    private bool _pending;

    public Vm167Service(ILogger<Vm167Service> logger, ISettingsService settingsService, IVm167 vm167)
    {
        _logger = logger;
        _settingsService = settingsService;
        _vm167 = vm167;

        _settingsService.Update += UpdateSettings;
        _timer = new(async (obj) => await Loop(), null, Timeout.Infinite, Timeout.Infinite);
    }

    public void Dispose()
    {
        _timer.Dispose();
    }

    private int _pwmFrequency = 1;
    private bool _pwmFrequencyChanged;
    public int PwmFrequency
    {
        get => _pwmFrequency;
        set
        {
            _pwmFrequency = value;
            _pwmFrequencyChanged = true;
        }
    }

    private uint _counter;
    private bool _counterChanged;
    public uint Counter
    {
        get => _counter;
        set
        {
            _counter = value;
            _counterChanged = true;
        }
    }

    private bool _allDigitalValue;
    private bool _allDigitalChanged;
    public bool AllDigital
    {
        get => _allDigitalValue;
        set
        {
            _allDigitalValue = value;
            _allDigitalChanged = true;
        }
    }

    public DigitalChannel DigitalLowIn { get; } = new();

    public DigitalChannel DigitalHighIn { get; } = new();

    public DigitalChannel Digital1 { get; } = new();

    public DigitalChannel Digital2 { get; } = new();

    public DigitalChannel Digital3 { get; } = new();

    public DigitalChannel Digital4 { get; } = new();

    public DigitalChannel Digital5 { get; } = new();

    public DigitalChannel Digital6 { get; } = new();

    public DigitalChannel Digital7 { get; } = new();

    public DigitalChannel Digital8 { get; } = new();

    public AnalogChannel Analog1 => _settingsService.Analog1;

    public AnalogChannel Analog2 => _settingsService.Analog2;

    public AnalogChannel Analog3 => _settingsService.Analog3;

    public AnalogChannel Analog4 => _settingsService.Analog4;

    public AnalogChannel Analog5 => _settingsService.Analog5;

    public AnalogChannel Pwm1 => _settingsService.Pwm1;

    public AnalogChannel Pwm2 => _settingsService.Pwm2;

    public async Task<int> ListDevices()
    {
        var mask = await _vm167.ListDevices();
        if (mask < 0) throw new ApplicationException(Resources.AppResources.NoCardFound);
        if (mask == 0) throw new ApplicationException(Resources.AppResources.DriverProblem);
        return mask;
    }

    public async Task OpenDevice(int device)
    {
        _logger.LogTrace("Open({})", device);
        _timer.Change(Timeout.Infinite, Timeout.Infinite);
        while (_pending)
        {
            await Task.Delay(IVm167Service.Period);
        }

        int mask = 1 << device;
        int opened;
        using (await _lock.UseWaitAsync())
        {
            opened = await _vm167.OpenDevices(mask);
        }

        _logger.LogDebug("Open card mask {Mask}", mask);
        if (mask != opened) throw new ApplicationException(Resources.AppResources.CardInUse);

        _allDigitalChanged = false;
        DigitalLowIn.Changed = true;
        DigitalHighIn.Changed = true;
        Digital1.Changed = true;
        Digital2.Changed = true;
        Digital3.Changed = true;
        Digital4.Changed = true;
        Digital5.Changed = true;
        Digital6.Changed = true;
        Digital7.Changed = true;
        Digital8.Changed = true;
        Analog1.Changed = true;
        Analog2.Changed = true;
        Analog3.Changed = true;
        Analog4.Changed = true;
        Analog5.Changed = true;
        Pwm1.Changed = true;
        Pwm2.Changed = true;

        _device = device;
        _settingsService.Initialize(device);
        _timer.Change(IVm167Service.Period, IVm167Service.Period);
    }

    public async Task CloseDevice()
    {
        _logger.LogTrace("Close");
        _timer.Change(Timeout.Infinite, Timeout.Infinite);
        using (await _lock.UseWaitAsync())
        {
            await _vm167.CloseDevices();
        }
    }

    public int VersionDLL()
    {
        return _vm167.VersionDLL();
    }

    public async Task<int> VersionFirmware()
    {
        return await _vm167.VersionFirmware(_device);
    }

    private void UpdateSettings()
    {
        Analog1.UpdateValue();
        Analog2.UpdateValue();
        Analog3.UpdateValue();
        Analog4.UpdateValue();
        Analog5.UpdateValue();
        Pwm1.UpdateValue();
        Pwm2.UpdateValue();
    }

    private async Task Loop()
    {
        if (_pending) return;
        _pending = true;
        _logger.LogTrace(">Loop");

        // Read from VM167
        await ReadDigitalMode();
        await ReadDigital();
        await ReadCounter();
        await ReadAnalog();
        await ReadPwmOut();

        // Notify subscribers
        if (Tick == null) return;
        using (await _lock.UseWaitAsync())
        {
            await Tick.Invoke();
        }

        // Write changes to VM167
        if (DigitalLowIn.Changed ||
            DigitalHighIn.Changed)
        {
            await WriteDigitalMode();
        }

        if (_allDigitalChanged ||
            Digital1.Changed ||
            Digital2.Changed ||
            Digital3.Changed ||
            Digital4.Changed ||
            Digital5.Changed ||
            Digital6.Changed ||
            Digital7.Changed ||
            Digital8.Changed)
        {
            await WriteDigital();
        }

        if (_counterChanged)
        {
            await ResetCounter();
        }

        if (_pwmFrequencyChanged || Pwm1.Changed || Pwm2.Changed)
        {
            await WritePwmOut();
        }

        DigitalLowIn.Changed = false;
        DigitalHighIn.Changed = false;
        Digital1.Changed = false;
        Digital2.Changed = false;
        Digital3.Changed = false;
        Digital4.Changed = false;
        Digital5.Changed = false;
        Digital6.Changed = false;
        Digital7.Changed = false;
        Digital8.Changed = false;
        Analog1.Changed = false;
        Analog2.Changed = false;
        Analog3.Changed = false;
        Analog4.Changed = false;
        Analog5.Changed = false;
        Pwm1.Changed = false;
        Pwm2.Changed = false;

        _logger.LogTrace("<Loop");
        _pending = false;
    }

    private async Task WritePwmOut()
    {
        _logger.LogTrace("WritePwmOut({}, {}, {})", _pwmFrequencyChanged, Pwm1.Changed, Pwm2.Changed);
        if (_pwmFrequencyChanged)
        {
            await _vm167.SetPWM(_device, 1, Pwm1.Signal, PwmFrequency);
            await _vm167.SetPWM(_device, 2, Pwm2.Signal, PwmFrequency);
            _pwmFrequencyChanged = false;
        }
        else if (Pwm1.Changed && Pwm2.Changed)
        {
            await _vm167.OutputAllPWM(_device, Pwm1.Signal, Pwm2.Signal);
            Pwm1.Changed = false;
            Pwm2.Changed = false;
        }
        else if (Pwm1.Changed)
        {
            await _vm167.SetPWM(_device, 1, Pwm1.Signal, PwmFrequency);
            Pwm1.Changed = false;
        }
        else if (Pwm2.Changed)
        {
            await _vm167.SetPWM(_device, 2, Pwm2.Signal, PwmFrequency);
            Pwm2.Changed = false;
        }
    }

    private async Task ResetCounter()
    {
        await _vm167.ResetCounter(_device);
        _counterChanged = false;
    }

    private async Task ReadPwmOut()
    {
        int[] pwm = new int[IVm167.NumPwmOut];
        await _vm167.ReadBackPWMOut(_device, pwm);
        Pwm1.Signal = pwm[0];
        Pwm2.Signal = pwm[1];
    }

    private async Task ReadAnalog()
    {
        int[] analog = new int[IVm167.NumAnalogIn];
        await _vm167.ReadAllAnalog(_device, analog);
        Analog1.Signal = analog[0];
        Analog2.Signal = analog[1];
        Analog3.Signal = analog[2];
        Analog4.Signal = analog[3];
        Analog5.Signal = analog[4];
    }

    private async Task ReadCounter()
    {
        var counter = await _vm167.ReadCounter(_device);
        _counter = counter;
    }

    private async Task ReadDigital()
    {
        var digital = await _vm167.ReadAllDigital(_device);
        Digital1.Value = (digital & 1) > 0;
        Digital2.Value = (digital & 2) > 0;
        Digital3.Value = (digital & 4) > 0;
        Digital4.Value = (digital & 8) > 0;
        Digital5.Value = (digital & 16) > 0;
        Digital6.Value = (digital & 32) > 0;
        Digital7.Value = (digital & 64) > 0;
        Digital8.Value = (digital & 128) > 0;
    }

    private async Task ReadDigitalMode()
    {
        var ioMode = await _vm167.ReadBackInOutMode(_device);
        DigitalLowIn.Value = (ioMode & 1) > 0;
        DigitalHighIn.Value = (ioMode & 2) > 0;
    }

    private async Task WriteDigitalMode()
    {
        if (DigitalLowIn.Changed ||
            DigitalHighIn.Changed)
        {
            await _vm167.InOutMode(_device, DigitalHighIn.Value ? 1 : 0, DigitalLowIn.Value ? 1 : 0);
        }
    }

    private async Task WriteDigital()
    {
        if (_allDigitalChanged)
        {
            if (_allDigitalValue)
            {
                await _vm167.SetAllDigital(_device);
            }
            else
            {
                await _vm167.ClearAllDigital(_device);
            }

            return;
        }

        if (Digital1.Changed &&
            Digital2.Changed &&
            Digital3.Changed &&
            Digital4.Changed &&
            Digital5.Changed &&
            Digital6.Changed &&
            Digital7.Changed &&
            Digital8.Changed)
        {
            await _vm167.OutputAllDigital(_device,
                (Digital1.Value ? 1 : 0) |
                (Digital2.Value ? 2 : 0) |
                (Digital3.Value ? 4 : 0) |
                (Digital4.Value ? 8 : 0) |
                (Digital5.Value ? 16 : 0) |
                (Digital6.Value ? 32 : 0) |
                (Digital7.Value ? 64 : 0) |
                (Digital8.Value ? 128 : 0));

            return;
        }

        if (Digital1.Changed)
        {
            await DigitalOut(1, Digital1.Value);
        }

        if (Digital2.Changed)
        {
            await DigitalOut(2, Digital2.Value);
        }

        if (Digital3.Changed)
        {
            await DigitalOut(3, Digital3.Value);
        }

        if (Digital4.Changed)
        {
            await DigitalOut(4, Digital4.Value);
        }

        if (Digital5.Changed)
        {
            await DigitalOut(5, Digital5.Value);
        }

        if (Digital6.Changed)
        {
            await DigitalOut(6, Digital6.Value);
        }

        if (Digital7.Changed)
        {
            await DigitalOut(7, Digital7.Value);
        }

        if (Digital8.Changed)
        {
            await DigitalOut(8, Digital8.Value);
        }
    }

    private async Task DigitalOut(int channel, bool value)
    {
        if (value)
        {
            await _vm167.SetDigitalChannel(_device, channel);
        }
        else
        {
            await _vm167.ClearDigitalChannel(_device, channel);
        }
    }
}
