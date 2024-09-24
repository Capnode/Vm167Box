using Microsoft.Extensions.Logging;
using Vm167Lib.Helpers;

namespace Vm167Lib;

public partial class Vm167(ILogger<Vm167> logger) : IVm167, IDisposable
{
    protected const uint Vid = 0x10cf;
    protected const uint Pid0 = 0x1670;
    protected const uint Pid1 = 0x1671;
    protected const int Pwm1 = 1;
    protected const int Pwm2 = 2;
    protected const int Channel1 = 1;
    protected const int Channel8 = 8;

    protected readonly ILogger<Vm167> _logger = logger;
    private readonly SemaphoreSlim _lock = new(1, 1);
    private readonly byte[][] _deviceBuffer = [new byte[64], new byte[64]];

    public void Dispose()
    {
        Close();
    }

    public async Task<int> ListDevices()
    {
        using (await _lock.UseWaitAsync())
        {
            _logger.LogTrace(">ListDevices()");
            var mask = await Scan();
            _logger.LogTrace("<ListDevices() => {}", mask);
            return mask;
        }
    }

    public async Task<int> OpenDevices(int mask)
    {
        using (await _lock.UseWaitAsync())
        {
            _logger.LogTrace(">OpenDevices({})", mask);
            var result = await Open(mask);
            _logger.LogTrace("<OpenDevices() => {}", result);
            return result;
        }
    }

    public async Task CloseDevices()
    {
        using (await _lock.UseWaitAsync())
        {
            _logger.LogTrace(">CloseDevices()");
            Close();
            _logger.LogTrace("<CloseDevices()");
        }
    }

    public async Task<int> ReadAnalogChannel(int CardAddress, int Channel)
    {
        using (await _lock.UseWaitAsync())
        {
            _logger.LogTrace(">ReadAnalogChannel({},{})", CardAddress, Channel);
            var buffer = _deviceBuffer[CardAddress];
            buffer[IVm167.Device0] = 0; // Read Analog Channel
            buffer[IVm167.Device1] = (byte)(Channel - 1);

            await Write(CardAddress, 2);
            var transf = await Read(CardAddress);
            if (transf == 0) return 0;
            var value = buffer[1] + (buffer[2] << 8);
            _logger.LogTrace("<ReadAnalogChannel({},{}) => {}", CardAddress, Channel, value);
            return value;
        }
    }

    public async Task ReadAllAnalog(int CardAddress, int[] Buffer)
    {
        using (await _lock.UseWaitAsync())
        {
            _logger.LogTrace(">ReadAllAnalog({})", CardAddress);
            var buffer = _deviceBuffer[CardAddress];
            buffer[0] = 1; // Read All Analog Channels

            await Write(CardAddress, 1);
            var transf = await Read(CardAddress);
            if (transf == 0) return;
            for (int i = 0; i < Buffer.Length; i++)
            {
                var value = buffer[2 * i + 1] + (buffer[2 * i + 2] << 8);
                Buffer[i] = value;
            }

            _logger.LogTrace("<ReadAllAnalog({}) => {}", CardAddress, Buffer);
        }
    }

    public async Task SetPWM(int CardAddress, int Channel, int Data, int Freq)
    {
        using (await _lock.UseWaitAsync())
        {
            _logger.LogTrace(">SetPWM({},{},{},{})", CardAddress, Channel, Data, Freq);
            var buffer = _deviceBuffer[CardAddress];
            Channel = Validate(Channel, Pwm1, Pwm2);
            Freq = Validate(Freq, IVm167.Freq2930, IVm167.Freq46875);
            Data = Validate(Data, IVm167.PwmMin, IVm167.PwmMax);

            buffer[0] = 2; // Set PWM
            buffer[1] = (byte)(Channel - 1);
            buffer[2] = (byte)Data;
            buffer[3] = (byte)Freq;
            await Write(CardAddress, 4);
            _logger.LogTrace("<SetPWM()");
        }
    }

    public async Task OutputAllPWM(int CardAddress, int Data1, int Data2)
    {
        using (await _lock.UseWaitAsync())
        {
            _logger.LogTrace(">OutputAllPWM({},{},{})", CardAddress, Data1, Data2);
            var buffer = _deviceBuffer[CardAddress];
            Data1 = Validate(Data1, IVm167.PwmMin, IVm167.PwmMax);
            Data2 = Validate(Data2, IVm167.PwmMin, IVm167.PwmMax);

            buffer[0] = 3; // Output All PWM
            buffer[1] = (byte)Data1;
            buffer[2] = (byte)Data2;
            await Write(CardAddress, 3);
            _logger.LogTrace("<OutputAllPWM()");
        }
    }

    public async Task OutputAllDigital(int CardAddress, int Data)
    {
        using (await _lock.UseWaitAsync())
        {
            _logger.LogTrace(">OutputAllDigital({},{})", CardAddress, Data);
            var buffer = _deviceBuffer[CardAddress];
            buffer[0] = 5; // Output All Digital
            buffer[1] = (byte)Validate(Data, 0, 255);
            await Write(CardAddress, 2);
            _logger.LogTrace("<OutputAllDigital()");
        }
    }

    public async Task ClearDigitalChannel(int CardAddress, int Channel)
    {
        using (await _lock.UseWaitAsync())
        {
            _logger.LogTrace(">ClearDigitalChannel({},{})", CardAddress, Channel);
            var buffer = _deviceBuffer[CardAddress];
            Channel = Validate(Channel, Channel, Channel8);
            int k = ~(1 << (Channel - 1));

            buffer[0] = 6; // Clear Digital Channel
            buffer[1] = (byte)k;
            await Write(CardAddress, 2);
            _logger.LogTrace("<ClearDigitalChannel()");
        }
    }

    public async Task ClearAllDigital(int CardAddress)
    {
        _logger.LogTrace(">ClearAllDigital({})", CardAddress);
        await OutputAllDigital(CardAddress, 0);
        _logger.LogTrace("<ClearAllDigital()");
    }

    public async Task SetDigitalChannel(int CardAddress, int Channel)
    {
        using (await _lock.UseWaitAsync())
        {
            _logger.LogTrace(">SetDigitalChannel({},{})", CardAddress, Channel);
            var buffer = _deviceBuffer[CardAddress];
            Channel = Validate(Channel, Channel1, Channel8);
            int k = 1 << (Channel - 1);

            buffer[0] = 7; // Set Digital Channel
            buffer[1] = (byte)k;
            await Write(CardAddress, 2);
            _logger.LogTrace("<SetDigitalChannel()");
        }
    }

    public async Task SetAllDigital(int CardAddress)
    {
        _logger.LogTrace(">SetAllDigital({})", CardAddress);
        await OutputAllDigital(CardAddress, 255);
        _logger.LogTrace("<SetAllDigital()");
    }

    public async Task<bool> ReadDigitalChannel(int CardAddress, int Channel)
    {
        _logger.LogTrace(">ReadDigitalChannel({},{})", CardAddress, Channel);
        var value = await ReadAllDigital(CardAddress);
        Channel = Validate(Channel, Channel1, Channel8);
        var val = (value & (1 << (Channel - 1))) != 0;
        _logger.LogTrace("<ReadDigitalChannel() => {}", val);
        return val;
    }

    public async Task<int> ReadAllDigital(int CardAddress)
    {
        using (await _lock.UseWaitAsync())
        {
            _logger.LogTrace(">ReadAllDigital({})", CardAddress);
            var buffer = _deviceBuffer[CardAddress];
            buffer[0] = 4; // Read All Digital
            await Write(CardAddress, 1);
            await Read(CardAddress);
            var value = buffer[1];
            _logger.LogTrace("<ReadAllDigital() => {}", value);
            return value;
        }
    }

    public async Task InOutMode(int CardAddress, int HighNibble, int LowNibble)
    {
        using (await _lock.UseWaitAsync())
        {
            _logger.LogTrace(">InOutMode({},{},{})", CardAddress, HighNibble, LowNibble);
            var buffer = _deviceBuffer[CardAddress];
            buffer[0] = 8; // InOut Mode
            buffer[1] = (byte)LowNibble;
            buffer[2] = (byte)HighNibble;
            await Write(CardAddress, 3);
            _logger.LogTrace("<InOutMode()");
        }
    }

    public async Task<uint> ReadCounter(int CardAddress)
    {
        using (await _lock.UseWaitAsync())
        {
            _logger.LogTrace(">ReadCounter({})", CardAddress);
            var buffer = _deviceBuffer[CardAddress];
            buffer[0] = 9; // Read Counter
            buffer[1] = 0;  // read counter #0
            await Write(CardAddress, 2);
            await Read(CardAddress);
            var value = (uint)(buffer[1] + (buffer[2] << 8) + (buffer[3] << 16) + (buffer[4] << 24));
            _logger.LogTrace("<ReadCounter() => {}", value);
            return value;
        }
    }

    public async Task ResetCounter(int CardAddress)
    {
        using (await _lock.UseWaitAsync())
        {
            _logger.LogTrace(">ResetCounter({})", CardAddress);
            var buffer = _deviceBuffer[CardAddress];
            buffer[0] = 10; // Reset Counter
            buffer[1] = 0;  // reset counter #0
            await Write(CardAddress, 2);
            _logger.LogTrace("<ResetCounter()");
        }
    }

    public async Task<int> Connected()
    {
        using (await _lock.UseWaitAsync())
        {
            _logger.LogTrace(">Connected()");
            var buffer = _deviceBuffer[IVm167.Device0];
            buffer[0] = 14; // Unknown function
            buffer[1] = IVm167.Device0;
            await Write(IVm167.Device0, 2);
            await Read(IVm167.Device0);
            int cards = buffer[1];

            buffer = _deviceBuffer[IVm167.Device1];
            buffer[0] = 14; // Unknown function
            buffer[1] = IVm167.Device1;
            await Write(IVm167.Device1, 2);
            await Read(IVm167.Device1);
            cards += buffer[1];
            _logger.LogTrace("<Connected() => {}", cards);
            return cards;
        }
    }

    public async Task<int> VersionFirmware(int CardAddress)
    {
        using (await _lock.UseWaitAsync())
        {
            _logger.LogTrace(">VersionFirmware({})", CardAddress);
            var buffer = _deviceBuffer[CardAddress];
            buffer[0] = 11; // Read version info from card
            await Write(CardAddress, 1);
            await Read(CardAddress);
            var value = 256 * 256 * 256 * buffer[1] + 256 * 256 * buffer[2] + 256 * buffer[3] + buffer[4];
            _logger.LogTrace("<VersionFirmware() => {}", value);
            return value;
        }
    }

    public int VersionDLL()
    {
        _logger.LogTrace(">VersionDLL()");
        var value = 0x0010013;
        _logger.LogTrace("<VersionDLL() => {}", value);
        return value;
    }

    public async Task ReadBackPWMOut(int CardAddress, int[] Buffer)
    {
        using (await _lock.UseWaitAsync())
        {
            _logger.LogTrace(">ReadBackPWMOut({})", CardAddress);
            var buffer = _deviceBuffer[CardAddress];
            buffer[0] = 12; // Read PWM
            await Write(CardAddress, 1);
            await Read(CardAddress);
            Buffer[0] = buffer[1];
            Buffer[1] = buffer[2];
            _logger.LogTrace("<ReadBackPWMOut({}) => {}", CardAddress, Buffer);
        }
    }

    public async Task<int> ReadBackInOutMode(int CardAddress)
    {
        using (await _lock.UseWaitAsync())
        {
            _logger.LogTrace(">ReadBackInOutMode({})", CardAddress);
            var buffer = _deviceBuffer[CardAddress];
            buffer[0] = 13; // Read In/Out Mode
            await Write(CardAddress, 1);
            await Read(CardAddress);
            var value = buffer[1];
            _logger.LogTrace("<ReadBackInOutMode() => {}", value);
            return value;
        }
    }

    private int Validate(int value, int low, int high)
    {
        return value < low ? low : value > high ? high : value;
    }
}
