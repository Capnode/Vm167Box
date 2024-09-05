namespace Vm167Box.Services;

public interface IVm167Service
{
    int Device { get; set; }
    int PWMFrequency { get; set; }

    Task<int> OpenDevices();
    Task ClearAllDigital();
    Task ClearDigitalChannel(int channel);
    Task InOutMode(int high, int low);
    Task ReadAllAnalog(int[] analog);
    Task<bool> ReadDigitalChannel(int channel);
    Task<int> ReadAllDigital();
    Task<int> ReadBackInOutMode();
    Task ReadBackPWMOut(int[] pwm);
    Task<uint> ReadCounter();
    Task ResetCounter();
    Task SetAllDigital();
    Task SetDigitalChannel(int channel);
    Task SetPWM(int channel, int data, int freq);
    Task OutputAllPWM(int data1, int data2);
    Task OutputAllDigital(int data);
    int VersionDLL();
    Task<int> VersionFirmware();
}
