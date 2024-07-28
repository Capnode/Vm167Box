namespace Vm167Box;

public class Vm167 : IVm167
{
    public Task<int> OpenDevices()
    {
        throw new NotImplementedException();
    }

    public Task CloseDevices()
    {
        throw new NotImplementedException();
    }

    public Task<int> ReadAnalogChannel(int CardAddress, int Channel)
    {
        throw new NotImplementedException();
    }

    public Task ReadAllAnalog(int CardAddress, int[] Buffer)
    {
        throw new NotImplementedException();
    }

    public Task SetPWM(int CardAddress, int Channel, int Data, int Freq)
    {
        throw new NotImplementedException();
    }

    public Task OutputAllPWM(int CardAddress, int Data1, int Data2)
    {
        throw new NotImplementedException();
    }

    public Task OutputAllDigital(int CardAddress, int Data)
    {
        throw new NotImplementedException();
    }

    public Task ClearDigitalChannel(int CardAddress, int Channel)
    {
        throw new NotImplementedException();
    }

    public Task ClearAllDigital(int CardAddress)
    {
        throw new NotImplementedException();
    }

    public Task SetDigitalChannel(int CardAddress, int Channel)
    {
        throw new NotImplementedException();
    }

    public Task SetAllDigital(int CardAddress)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ReadDigitalChannel(int CardAddress, int Channel)
    {
        throw new NotImplementedException();
    }

    public Task<int> ReadAllDigital(int CardAddress)
    {
        throw new NotImplementedException();
    }

    public Task InOutMode(int CardAddress, int HighNibble, int LowNibble)
    {
        throw new NotImplementedException();
    }

    public Task<uint> ReadCounter(int CardAddress)
    {
        throw new NotImplementedException();
    }

    public Task ResetCounter(int CardAddress)
    {
        throw new NotImplementedException();
    }

    public Task<int> Connected()
    {
        throw new NotImplementedException();
    }

    public Task<int> VersionFirmware(int CardAddress)
    {
        throw new NotImplementedException();
    }

    public int VersionDLL()
    {
        throw new NotImplementedException();
    }

    public Task ReadBackPWMOut(int CardAddress, int[] Buffer)
    {
        throw new NotImplementedException();
    }

    public Task<int> ReadBackInOutMode(int CardAddress)
    {
        throw new NotImplementedException();
    }
}
