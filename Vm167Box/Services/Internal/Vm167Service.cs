using Microsoft.Extensions.Logging;
using Vm167Box.Helpers;
using Vm167Lib;

namespace Vm167Box.Services.Internal;

internal sealed class Vm167Service : IVm167Service, IDisposable
{
    public event Func<Task>? Tick;

    private readonly ILogger<Vm167Service> _logger;
    private readonly ISettingsService _settingsService;
    private readonly IVm167 _vm167;
    private readonly Timer _timer;
    private readonly Channel _analog1Channel = new();
    private readonly Channel _analog2Channel = new();
    private readonly Channel _analog3Channel = new();
    private readonly Channel _analog4Channel = new();
    private readonly Channel _analog5Channel = new();
    private readonly Channel _pwm1Channel = new();
    private readonly Channel _pwm2Channel = new();
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
        UpdateSettings().Wait();
    }

    public void Dispose()
    {
        _timer.Dispose();
    }

    private int _pwmFrequency = 1;
    private bool _pwmFrequencyRequest;
    public int PwmFrequency
    {
        get => _pwmFrequency;
        set
        {
            if (_pwmFrequency == value) return;
            _pwmFrequency = value;
            _pwmFrequencyRequest = true;
        }
    }

    private bool _isDigitalLowIn;
    private bool _isDigitalLowInRequest;
    public bool IsDigitalLowIn
    {
        get => _isDigitalLowIn;
        set
        {
            if (_isDigitalLowIn == value) return;
            _logger.LogTrace("IsDigitalLowIn = {}", value);
            _isDigitalLowIn = value;
            _isDigitalLowInRequest = true;
        }
    }

    private bool _isDigitalHighIn;
    private bool _isDigialHighInRequest;
    public bool IsDigitalHighIn
    {
        get => _isDigitalHighIn;
        set
        {
            if (_isDigitalHighIn == value) return;
            _logger.LogTrace("IsDigitalHighIn = {}", value);
            _isDigitalHighIn = value;
            _isDigialHighInRequest = true;
        }
    }

    private bool _digital1;
    private bool _digital1Request;
    public bool Digital1
    {
        get => _digital1;
        set
        {
            if (_digital1 == value) return;
            _digital1 = value;
            _digital1Request = true;
        }
    }

    private bool _digital2;
    private bool _digital2Request;
    public bool Digital2
    {
        get => _digital2;
        set
        {
            if (_digital2 == value) return;
            _digital2 = value;
            _digital2Request = true;
        }
    }

    private bool _digital3;
    private bool _digital3Request;
    public bool Digital3
    {
        get => _digital3;
        set
        {
            if (_digital3 == value) return;
            _digital3 = value;
            _digital3Request = true;
        }
    }

    private bool _digital4;
    private bool _digital4Request;
    public bool Digital4
    {
        get => _digital4;
        set
        {
            if (_digital4 == value) return;
            _digital4 = value;
            _digital4Request = true;
        }
    }

    private bool _digital5;
    private bool _digital5Request;
    public bool Digital5
    {
        get => _digital5;
        set
        {
            if (_digital5 == value) return;
            _digital5 = value;
            _digital5Request = true;
        }
    }

    private bool _digital6;
    private bool _digital6Request;
    public bool Digital6
    {
        get => _digital6;
        set
        {
            if (_digital6 == value) return;
            _digital6 = value;
            _digital6Request = true;
        }
    }

    private bool _digital7;
    private bool _digital7Request;
    public bool Digital7
    {
        get => _digital7;
        set
        {
            if (_digital7 == value) return;
            _digital7 = value;
            _digital7Request = true;
        }
    }

    private bool _digital8;
    private bool _digital8Request;
    public bool Digital8
    {
        get => _digital8;
        set
        {
            if (_digital8 == value) return;
            _digital8 = value;
            _digital8Request = true;
        }
    }

    private uint _counter;
    private bool _counterRequest;
    public uint Counter
    {
        get => _counter;
        set
        {
            if (_counter == value) return;
            _counter = value;
            _counterRequest = true;
        }
    }

    public double Analog1In { get; private set; }

    public double Analog2In { get; private set; }

    public double Analog3In { get; private set; }

    public double Analog4In { get; private set; }

    public double Analog5In { get; private set; }

    private double _pwm1Out;
    private bool _pwm1Request;
    public double Pwm1Out
    {
        get => _pwm1Out;
        set
        {
            if (_pwm1Out == value) return;
            _pwm1Out = value;
            _pwm1Request = true;
        }
    }

    private double _pwm2Out;
    private bool _pwm2Request;
    public double Pwm2Out
    {
        get => _pwm2Out;
        set
        {
            if (_pwm2Out == value) return;
            _pwm2Out = value;
            _pwm2Request = true;
        }
    }

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
        int mask = 1 << device;
        int opened;
        using (await _lock.UseWaitAsync())
        {
            opened = await _vm167.OpenDevices(mask);
        }

        _logger.LogDebug("Open card mask {Mask}", mask);
        if (mask != opened) throw new ApplicationException(Resources.AppResources.CardInUse);

        _timer.Change(IVm167Service.Period, IVm167Service.Period);
        _device = device;
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

    private async Task Loop()
    {
        if (_pending) return;
        _pending = true;
        _logger.LogTrace(">Loop");

        await WriteRequests();
        await ReadDigitalMode();
        await ReadDigital();
        await ReadCounter();
        await ReadAnalog();
        await ReadPwmOut();

        if (Tick == null) return;

        using (await _lock.UseWaitAsync())
        {
            await Tick.Invoke();
        }

        await WriteRequests();
        _logger.LogTrace("<Loop");
        _pending = false;
    }

    private async Task WriteRequests()
    {
        if (_isDigitalLowInRequest ||
            _isDigialHighInRequest)
        {
            await WriteDigitalMode();
        }

        if (_digital1Request ||
            _digital2Request ||
            _digital3Request ||
            _digital4Request ||
            _digital5Request ||
            _digital6Request ||
            _digital7Request ||
            _digital8Request)
        {
            await WriteDigital();
        }

        if (_counterRequest)
        {
            await ResetCounter();
        }

        if (_pwm1Request ||
            _pwm2Request ||
            _pwmFrequencyRequest)
        {
            await WritePwmOut();
        }
    }

    private async Task WritePwmOut()
    {
        if (_pwmFrequencyRequest)
        {
            await _vm167.SetPWM(_device, 1, _pwm1Channel.ToSignal(_pwm1Out), PwmFrequency);
            await _vm167.SetPWM(_device, 2, _pwm2Channel.ToSignal(_pwm2Out), PwmFrequency);
        }
        else if (_pwm1Request && _pwm1Request)
        {
            await _vm167.OutputAllPWM(_device, _pwm1Channel.ToSignal(_pwm1Out), _pwm2Channel.ToSignal(_pwm2Out));
        }
        else if (_pwm1Request)
        {
            await _vm167.SetPWM(_device, 1, _pwm1Channel.ToSignal(_pwm1Out), PwmFrequency);
        }
        else if (_pwm2Request)
        {
            await _vm167.SetPWM(_device, 2, _pwm2Channel.ToSignal(_pwm2Out), PwmFrequency);
        }

        _pwmFrequencyRequest = false;
        _pwm1Request = false;
        _pwm1Request = false;
    }

    private async Task ResetCounter()
    {
        await _vm167.ResetCounter(_device);
        _counterRequest = false;
    }

    private async Task ReadPwmOut()
    {
        int[] pwm = new int[IVm167.NumPwmOut];
        await _vm167.ReadBackPWMOut(_device, pwm);
        _pwm1Out = _pwm1Channel.ToValue(pwm[0]);
        _pwm2Out = _pwm2Channel.ToValue(pwm[1]);
    }

    private async Task ReadAnalog()
    {
        int[] analog = new int[IVm167.NumAnalogIn];
        await _vm167.ReadAllAnalog(_device, analog);
        Analog1In = _analog1Channel.ToValue(analog[0]);
        Analog2In = _analog2Channel.ToValue(analog[1]);
        Analog3In = _analog3Channel.ToValue(analog[2]);
        Analog4In = _analog4Channel.ToValue(analog[3]);
        Analog5In = _analog5Channel.ToValue(analog[4]);
    }

    private async Task ReadCounter()
    {
        var counter = await _vm167.ReadCounter(_device);
        _counter = counter;
    }

    private async Task ReadDigital()
    {
        var digital = await _vm167.ReadAllDigital(_device);
        _digital1 = (digital & 1) > 0;
        _digital2 = (digital & 2) > 0;
        _digital3 = (digital & 4) > 0;
        _digital4 = (digital & 8) > 0;
        _digital5 = (digital & 16) > 0;
        _digital6 = (digital & 32) > 0;
        _digital7 = (digital & 64) > 0;
        _digital8 = (digital & 128) > 0;
    }

    private async Task ReadDigitalMode()
    {
        var ioMode = await _vm167.ReadBackInOutMode(_device);
        _isDigitalLowIn = (ioMode & 1) > 0;
        _isDigitalHighIn = (ioMode & 2) > 0;
    }

    private async Task WriteDigitalMode()
    {
        if (_isDigitalLowInRequest ||
            _isDigialHighInRequest)
        {
            await _vm167.InOutMode(_device, IsDigitalHighIn ? 1 : 0, IsDigitalLowIn ? 1 : 0);
        }

        _isDigitalLowInRequest = false;
        _isDigialHighInRequest = false;
    }

    private async Task WriteDigital()
    {
        if (_digital1Request &&
            _digital2Request &&
            _digital3Request &&
            _digital4Request &&
            _digital5Request &&
            _digital6Request &&
            _digital7Request &&
            _digital8Request)
        {
            if (_digital1 &&
                _digital2 &&
                _digital3 &&
                _digital4 &&
                _digital5 &&
                _digital6 &&
                _digital7 &&
                _digital8)
            {
                await _vm167.SetAllDigital(_device);
            }
            else if (!_digital1 &&
                     !_digital2 &&
                     !_digital3 &&
                     !_digital4 &&
                     !_digital5 &&
                     !_digital6 &&
                     !_digital7 &&
                     !_digital8)
            {
                await _vm167.ClearAllDigital(_device);
            }
            else
            {
                await _vm167.OutputAllDigital(_device,
                    (_digital1 ? 1 : 0) |
                    (_digital2 ? 2 : 0) |
                    (_digital3 ? 4 : 0) |
                    (_digital4 ? 8 : 0) |
                    (_digital5 ? 16 : 0) |
                    (_digital6 ? 32 : 0) |
                    (_digital7 ? 64 : 0) |
                    (_digital8 ? 128 : 0));
            }

            _digital1Request = false;
            _digital2Request = false;
            _digital3Request = false;
            _digital4Request = false;
            _digital5Request = false;
            _digital6Request = false;
            _digital7Request = false;
            _digital8Request = false;
            return;
        }

        if (_digital1Request)
        {
            await DigitalOut(1, _digital1);
            _digital1Request = false;
        }

        if (_digital2Request)
        {
            await DigitalOut(2, _digital2);
            _digital2Request = false;
        }

        if (_digital3Request)
        {
            await DigitalOut(3, _digital3);
            _digital3Request = false;
        }

        if (_digital4Request)
        {
            await DigitalOut(4, _digital4);
            _digital4Request = false;
        }

        if (_digital5Request)
        {
            await DigitalOut(5, _digital5);
            _digital5Request = false;
        }

        if (_digital6Request)
        {
            await DigitalOut(6, _digital6);
            _digital6Request = false;
        }

        if (_digital7Request)
        {
            await DigitalOut(7, _digital7);
            _digital7Request = false;
        }

        if (_digital8Request)
        {
            await DigitalOut(8, _digital8);
            _digital8Request = false;
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

    private Task UpdateSettings()
    {
        _analog1Channel.Name = _settingsService.Analog1Name;
        _analog1Channel.Unit = _settingsService.Analog1Unit;
        _analog1Channel.MinSignal = _settingsService.Analog1MinSignal;
        _analog1Channel.MaxSignal = _settingsService.Analog1MaxSignal;
        _analog1Channel.MinValue = _settingsService.Analog1MinValue;
        _analog1Channel.MaxValue = _settingsService.Analog1MaxValue;

        _analog2Channel.Name = _settingsService.Analog2Name;
        _analog2Channel.Unit = _settingsService.Analog2Unit;
        _analog2Channel.MinSignal = _settingsService.Analog2MinSignal;
        _analog2Channel.MaxSignal = _settingsService.Analog2MaxSignal;
        _analog2Channel.MinValue = _settingsService.Analog2MinValue;
        _analog2Channel.MaxValue = _settingsService.Analog2MaxValue;

        _analog3Channel.Name = _settingsService.Analog3Name;
        _analog3Channel.Unit = _settingsService.Analog3Unit;
        _analog3Channel.MinSignal = _settingsService.Analog3MinSignal;
        _analog3Channel.MaxSignal = _settingsService.Analog3MaxSignal;
        _analog3Channel.MinValue = _settingsService.Analog3MinValue;
        _analog3Channel.MaxValue = _settingsService.Analog3MaxValue;

        _analog4Channel.Name = _settingsService.Analog4Name;
        _analog4Channel.Unit = _settingsService.Analog4Unit;
        _analog4Channel.MinSignal = _settingsService.Analog4MinSignal;
        _analog4Channel.MaxSignal = _settingsService.Analog4MaxSignal;
        _analog4Channel.MinValue = _settingsService.Analog4MinValue;
        _analog4Channel.MaxValue = _settingsService.Analog4MaxValue;

        _analog5Channel.Name = _settingsService.Analog5Name;
        _analog5Channel.Unit = _settingsService.Analog5Unit;
        _analog5Channel.MinSignal = _settingsService.Analog5MinSignal;
        _analog5Channel.MaxSignal = _settingsService.Analog5MaxSignal;
        _analog5Channel.MinValue = _settingsService.Analog5MinValue;
        _analog5Channel.MaxValue = _settingsService.Analog5MaxValue;

        _pwm1Channel.Name = _settingsService.Pwm1Name;
        _pwm1Channel.Unit = _settingsService.Pwm1Unit;
        _pwm1Channel.MinSignal = _settingsService.Pwm1MinSignal;
        _pwm1Channel.MaxSignal = _settingsService.Pwm1MaxSignal;
        _pwm1Channel.MinValue = _settingsService.Pwm1MinValue;
        _pwm1Channel.MaxValue = _settingsService.Pwm1MaxValue;

        _pwm2Channel.Name = _settingsService.Pwm2Name;
        _pwm2Channel.Unit = _settingsService.Pwm2Unit;
        _pwm2Channel.MinSignal = _settingsService.Pwm2MinSignal;
        _pwm2Channel.MaxSignal = _settingsService.Pwm2MaxSignal;
        _pwm2Channel.MinValue = _settingsService.Pwm2MinValue;
        _pwm2Channel.MaxValue = _settingsService.Pwm2MaxValue;

        return Task.CompletedTask;
    }
}
