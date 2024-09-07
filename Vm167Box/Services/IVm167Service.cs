namespace Vm167Box.Services;

public interface IVm167Service
{
    const int Period = 100;
    int PwmFrequency { get; set; }

    public event Func<Task> Tick;
    
    Task<int> ListDevices();
    Task OpenDevice(int device);
    Task CloseDevice();
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
    Task SetPwm(int channel, int data);
    Task OutputAllPwm(int data1, int data2);
    Task OutputAllDigital(int data);
    int VersionDLL();
    Task<int> VersionFirmware();
}
