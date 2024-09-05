using Microsoft.Extensions.Logging;
using Vm167Lib;

namespace Vm167Box.Services.Internal;

internal class Vm167Service : IVm167Service, IDisposable
{
    private readonly ILogger<Vm167Service> _logger;
    private readonly IVm167 _vm167;

    public Vm167Service(ILogger<Vm167Service> logger, IVm167 vm167)
    {
        _logger = logger;
        _vm167 = vm167;
    }

    public void Dispose()
    {
    }

    public int Device { get; set; }
    public int PwmFrequency { get; set; } = 1;

    public async Task<int> OpenDevices()
    {
        _logger.LogInformation("OpenDevices");
        await _vm167.CloseDevices();
        var mask = await _vm167.OpenDevices();
        _logger.LogInformation("Open card mask {Mask}", mask);
        if (mask < 0) throw new ApplicationException(Resources.AppResources.NoCardFound);
        if (mask == 0) throw new ApplicationException(Resources.AppResources.DriverProblem);
        return mask;
    }

    public async Task ClearAllDigital()
    {
        await _vm167.ClearAllDigital(Device);
    }

    public async Task ClearDigitalChannel(int channel)
    {
        await _vm167.ClearDigitalChannel(Device, channel);
    }

    public async Task InOutMode(int high, int low)
    {
        await _vm167.InOutMode(Device, high, low);
    }

    public async Task ReadAllAnalog(int[] analog)
    {
        await _vm167.ReadAllAnalog(Device, analog);
    }

    public async Task<bool> ReadDigitalChannel(int channel)
    {
        return await _vm167.ReadDigitalChannel(Device, channel);
    }

    public async Task<int> ReadAllDigital()
    {
        return await _vm167.ReadAllDigital(Device);
    }

    public async Task<int> ReadBackInOutMode()
    {
        return await _vm167.ReadBackInOutMode(Device);
    }

    public async Task ReadBackPWMOut(int[] pwm)
    {
        await _vm167.ReadBackPWMOut(Device, pwm);
    }

    public async Task<uint> ReadCounter()
    {
        return await _vm167.ReadCounter(Device);
    }

    public async Task ResetCounter()
    {
        await _vm167.ResetCounter(Device);
    }

    public async Task SetAllDigital()
    {
        await _vm167.SetAllDigital(Device);
    }

    public async Task SetDigitalChannel(int channel)
    {
        await _vm167.SetDigitalChannel(Device, channel);
    }

    public async Task SetPwm(int channel, int data, int frequency)
    {
        PwmFrequency = frequency;
        await _vm167.SetPWM(Device, channel, data, frequency);
    }

    public async Task OutputAllPwm(int data1, int data2)
    {
        await _vm167.OutputAllPWM(Device, data1, data2);
    }

    public async Task OutputAllDigital(int data)
    {
        await _vm167.OutputAllDigital(Device, data);
    }

    public int VersionDLL()
    {
        return _vm167.VersionDLL();
    }

    public async Task<int> VersionFirmware()
    {
        return await _vm167.VersionFirmware(Device);
    }

    public async Task Generator(int channel, Function function, double frequency)
    {
        _logger.LogInformation("Generator({Channel}, {Function}, {Frequency})", channel, function, frequency);
         await Task.CompletedTask;
    }
}
