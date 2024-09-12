using Microsoft.Extensions.Logging;
using Vm167Box.Helpers;
using Vm167Lib;

namespace Vm167Box.Services.Internal;

internal sealed class Vm167Service : IVm167Service, IDisposable
{
    public event Func<Task>? Tick;

    private readonly ILogger<Vm167Service> _logger;
    private readonly IVm167 _vm167;
    private readonly Timer _timer;
    private readonly SemaphoreSlim _lock = new(1, 1);
    private int _device = -1;
    private bool _pending;

    public Vm167Service(ILogger<Vm167Service> logger, IVm167 vm167)
    {
        _logger = logger;
        _vm167 = vm167;
        _timer = new(async (obj) => await Loop(), null, Timeout.Infinite, Timeout.Infinite);
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

    public double AnalogIn1 { get; private set; }

    public double AnalogIn2 { get; private set; }

    public double AnalogIn3 { get; private set; }

    public double AnalogIn4 { get; private set; }

    public double AnalogIn5 { get; private set; }

    private double _pwmOut1;
    private bool _pwmOut1Request;
    public double PwmOut1
    {
        get => _pwmOut1;
        set
        {
            if (_pwmOut1 == value) return;
            _pwmOut1 = value;
            _pwmOut1Request = true;
        }
    }

    private double _pwmOut2;
    private bool _pwmOut2Request;
    public double PwmOut2
    {
        get => _pwmOut2;
        set
        {
            if (_pwmOut2 == value) return;
            _pwmOut2 = value;
            _pwmOut2Request = true;
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

        if (_pwmOut1Request ||
            _pwmOut2Request ||
            _pwmFrequencyRequest)
        {
            await WritePwmOut();
        }
    }

    private async Task WritePwmOut()
    {
        if (_pwmFrequencyRequest)
        {
            await _vm167.SetPWM(_device, 1, Convert.ToInt32(_pwmOut1), PwmFrequency);
            await _vm167.SetPWM(_device, 2, Convert.ToInt32(_pwmOut2), PwmFrequency);
        }
        else if (_pwmOut1Request && _pwmOut2Request)
        {
            await _vm167.OutputAllPWM(_device, Convert.ToInt32(_pwmOut1), Convert.ToInt32(_pwmOut2));
        }
        else if (_pwmOut1Request)
        {
            await _vm167.SetPWM(_device, 1, Convert.ToInt32(_pwmOut1), PwmFrequency);
        }
        else if (_pwmOut2Request)
        {
            await _vm167.SetPWM(_device, 2, Convert.ToInt32(_pwmOut2), PwmFrequency);
        }

        _pwmFrequencyRequest = false;
        _pwmOut1Request = false;
        _pwmOut2Request = false;
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
        _pwmOut1 = pwm[0];
        _pwmOut2 = pwm[1];
    }

    private async Task ReadAnalog()
    {
        int[] analog = new int[IVm167.NumAnalogIn];
        await _vm167.ReadAllAnalog(_device, analog);
        AnalogIn1 = analog[0];
        AnalogIn2 = analog[1];
        AnalogIn3 = analog[2];
        AnalogIn4 = analog[3];
        AnalogIn5 = analog[4];
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
}
