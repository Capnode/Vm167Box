namespace Vm167Box.Services;

public enum Function
{
    Off = 0,
    SquareWave = 1,
    TriangleWave = 2,
    SawtoothWave = 3,
    SineWave = 4,
}

public interface IVm167Service
{
    int Device { get; set; }
    int PwmFrequency { get; set; }

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
    Task SetPwm(int channel, int data, int frequency);
    Task OutputAllPwm(int data1, int data2);
    Task OutputAllDigital(int data);
    int VersionDLL();
    Task<int> VersionFirmware();
    Task Generator(int channel, Function function, double frequency);
}
