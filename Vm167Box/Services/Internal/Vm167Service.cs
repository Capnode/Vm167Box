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

    public Vm167Service(ILogger<Vm167Service> logger, IVm167 vm167)
    {
        _logger = logger;
        _vm167 = vm167;
        _timer = new(async (obj) => await Update(), null, Timeout.Infinite, Timeout.Infinite);
    }

    public void Dispose()
    {
        _timer.Dispose();
    }

    public int PwmFrequency { get; set; }

    public async Task<int> ListDevices()
    {
        var mask = await _vm167.ListDevices();
        if (mask < 0) throw new ApplicationException(Resources.AppResources.NoCardFound);
        if (mask == 0) throw new ApplicationException(Resources.AppResources.DriverProblem);
        return mask;
    }

    public async Task OpenDevice(int device)
    {
        _logger.LogInformation("Open({})", device);
        int mask = 1 << device;
        int opened;
        using (await _lock.UseWaitAsync())
        {
            opened = await _vm167.OpenDevices(mask);
        }

        _logger.LogInformation("Open card mask {Mask}", mask);
        if (mask != opened) throw new ApplicationException(Resources.AppResources.CardInUse);

        _timer.Change(IVm167Service.Period, IVm167Service.Period);
        _device = device;
    }

    public async Task CloseDevice()
    {
        _logger.LogInformation("Close");
        _timer.Change(Timeout.Infinite, Timeout.Infinite);
        using (await _lock.UseWaitAsync())
        {
            await _vm167.CloseDevices();
        }
    }

    public async Task ClearAllDigital()
    {
        await _vm167.ClearAllDigital(_device);
    }

    public async Task ClearDigitalChannel(int channel)
    {
        await _vm167.ClearDigitalChannel(_device, channel);
    }

    public async Task InOutMode(int high, int low)
    {
        await _vm167.InOutMode(_device, high, low);
    }

    public async Task ReadAllAnalog(int[] analog)
    {
        await _vm167.ReadAllAnalog(_device, analog);
    }

    public async Task<bool> ReadDigitalChannel(int channel)
    {
        return await _vm167.ReadDigitalChannel(_device, channel);
    }

    public async Task<int> ReadAllDigital()
    {
        return await _vm167.ReadAllDigital(_device);
    }

    public async Task<int> ReadBackInOutMode()
    {
        return await _vm167.ReadBackInOutMode(_device);
    }

    public async Task ReadBackPWMOut(int[] pwm)
    {
        await _vm167.ReadBackPWMOut(_device, pwm);
    }

    public async Task<uint> ReadCounter()
    {
        return await _vm167.ReadCounter(_device);
    }

    public async Task ResetCounter()
    {
        await _vm167.ResetCounter(_device);
    }

    public async Task SetAllDigital()
    {
        await _vm167.SetAllDigital(_device);
    }

    public async Task SetDigitalChannel(int channel)
    {
        await _vm167.SetDigitalChannel(_device, channel);
    }

    public async Task SetPwm(int channel, int data)
    {
        await _vm167.SetPWM(_device, channel, data, PwmFrequency);
    }

    public async Task OutputAllPwm(int data1, int data2)
    {
        await _vm167.OutputAllPWM(_device, data1, data2);
    }

    public async Task OutputAllDigital(int data)
    {
        await _vm167.OutputAllDigital(_device, data);
    }

    public int VersionDLL()
    {
        return _vm167.VersionDLL();
    }

    public async Task<int> VersionFirmware()
    {
        return await _vm167.VersionFirmware(_device);
    }

    private async Task Update()
    {
        if (Tick == null) return;

        using (await _lock.UseWaitAsync())
        {
            await Tick.Invoke();
        }
    }
}
