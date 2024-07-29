using Microsoft.Extensions.Logging;

namespace Vm167Box;

public partial class Vm167(ILogger<Vm167> logger) : IVm167
{
    public const int NumDevices = 2;
    public const int Device0 = 0;
    public const int Device1 = 1;
    public const int NumAnalogIn = 5;
    public const int NumPWMOut = 2;
    public const int NumDigitalIn = 8;
    public const int NumDigitalOut = 8;

    protected const uint Vid = 0x10cf;
    protected const uint Pid0 = 0x1670;
    protected const uint Pid1 = 0x1671;
    protected readonly ILogger<Vm167> _logger = logger;

    private readonly byte[][] _deviceBuffer = [new byte[64], new byte[64]];

    public partial Task<int> OpenDevices();

    public partial Task CloseDevices();

    public async Task<int> ReadAnalogChannel(int CardAddress, int Channel)
    {
        var buffer = _deviceBuffer[CardAddress];
        buffer[Device0] = 0; // Read Analog Channel
        buffer[Device1] = (byte)(Channel - 1);

        await Write(CardAddress, 2);
        var transf = await Read(CardAddress);
        if (transf == 0) return 0;

        return buffer[1] + (buffer[2] << 8);
    }

    public async Task ReadAllAnalog(int CardAddress, int[] Buffer)
    {
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
    }

    public async Task SetPWM(int CardAddress, int Channel, int Data, int Freq)
    {
        var buffer = _deviceBuffer[CardAddress];

        if (Channel > 2) Channel = 2;
        if (Channel < 1) Channel = 1;
        if (Freq > 3)
        {
            Freq = 3;
        }

        if (Freq < 1)
        {
            Freq = 1;
        }

        buffer[0] = 2; // Set PWM
        buffer[1] = (byte)(Channel - 1);
        buffer[2] = (byte)Data;
        buffer[3] = (byte)Freq;
        await Write(CardAddress, 4);
    }

    public async Task OutputAllPWM(int CardAddress, int Data1, int Data2)
    {
        var buffer = _deviceBuffer[CardAddress];

        buffer[0] = 3; // Output All PWM
        buffer[1] = (byte)Data1;
        buffer[2] = (byte)Data2;
        await Write(CardAddress, 3);
    }

    public async Task OutputAllDigital(int CardAddress, int Data)
    {
        var buffer = _deviceBuffer[CardAddress];

        buffer[0] = 5; // Output All Digital
        buffer[1] = (byte)Data;
        await Write(CardAddress, 2);
    }

    public async Task ClearDigitalChannel(int CardAddress, int Channel)
    {
        var buffer = _deviceBuffer[CardAddress];
        if (Channel > 8)
        {
            Channel = 8;
        }

        if (Channel < 1)
        {
            Channel = 1;
        }

        int k = ~(1 << (Channel - 1));

        buffer[0] = 6; // Clear Digital Channel
        buffer[1] = (byte)k;
        await Write(CardAddress, 2);
    }

    public async Task ClearAllDigital(int CardAddress)
    {
        await OutputAllDigital(CardAddress, 0);
    }

    public async Task SetDigitalChannel(int CardAddress, int Channel)
    {
        var buffer = _deviceBuffer[CardAddress];
        if (Channel > 8)
        {
            Channel = 8;
        }

        if (Channel < 1)
        {
            Channel = 1;
        }

        int k = 1 << (Channel - 1);

        buffer[0] = 7; // Set Digital Channel
        buffer[1] = (byte)k;
        await Write(CardAddress, 2);
    }

    public async Task SetAllDigital(int CardAddress)
    {
        await OutputAllDigital(CardAddress, 255);
    }

    public async Task<bool> ReadDigitalChannel(int CardAddress, int Channel)
    {
        var value = await ReadAllDigital(CardAddress);
        Channel = Math.Max(Channel, 8);
        Channel = Math.Min(Channel, 1);
        return (value & (1 << (Channel - 1))) != 0;
    }

    public async Task<int> ReadAllDigital(int CardAddress)
    {
        var buffer = _deviceBuffer[CardAddress];
        buffer[0] = 4; // Read All Digital
        await Write(CardAddress, 1);
        await Read(CardAddress);
        return buffer[1];
    }

    public async Task InOutMode(int CardAddress, int HighNibble, int LowNibble)
    {
        var buffer = _deviceBuffer[CardAddress];

        buffer[0] = 8; // InOut Mode
        buffer[1] = (byte)LowNibble;
        buffer[2] = (byte)HighNibble;
        await Write(CardAddress, 3);
    }

    public async Task<uint> ReadCounter(int CardAddress)
    {
        var buffer = _deviceBuffer[CardAddress];

        buffer[0] = 9; // Read Counter
        buffer[1] = 0;  // read counter #0
        await Write(CardAddress, 2);
        await Read(CardAddress);
        return (uint)(buffer[1] + (buffer[2] << 8) + (buffer[3] << 16) + (buffer[4] << 24));
    }

    public async Task ResetCounter(int CardAddress)
    {
        var buffer = _deviceBuffer[CardAddress];

        buffer[0] = 10; // Reset Counter
        buffer[1] = 0;  // reset counter #0
        await Write(CardAddress, 2);
    }

    public async Task<int> Connected()
    {
        var buffer = _deviceBuffer[Device0];
        buffer[0] = 14; // Unknown function
        buffer[1] = Device0;
        await Write(Device0, 2);
        await Read(Device0);
        int cards = buffer[1];

        buffer = _deviceBuffer[Device1];
        buffer[0] = 14; // Unknown function
        buffer[1] = Device1;
        await Write(Device1, 2);
        await Read(Device1);
        cards += buffer[1];

        return cards;
    }

    public async Task<int> VersionFirmware(int CardAddress)
    {
        var buffer = _deviceBuffer[CardAddress];

        buffer[0] = 11; // Read version info from card
        await Write(CardAddress, 1);
        await Read(CardAddress);
        return 256 * 256 * 256 * buffer[1] + 256 * 256 * buffer[2] + 256 * buffer[3] + buffer[4];
    }

    public int VersionDLL()
    {
        return 0x0010013;
    }

    public async Task ReadBackPWMOut(int CardAddress, int[] Buffer)
    {
        var buffer = _deviceBuffer[CardAddress];

        buffer[0] = 12; // Read PWM
        await Write(CardAddress, 1);
        await Read(CardAddress);
        Buffer[0] = buffer[1];
        Buffer[1] = buffer[2];
    }

    public async Task<int> ReadBackInOutMode(int CardAddress)
    {
        var buffer = _deviceBuffer[CardAddress];

        buffer[0] = 13; // Read In/Out Mode
        await Write(CardAddress, 1);
        await Read(CardAddress);
        return buffer[1];
    }
}
