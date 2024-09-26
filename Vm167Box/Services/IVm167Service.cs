using Vm167Box.Models;

namespace Vm167Box.Services;

public interface IVm167Service
{
    const int Period = 100;

    event Func<Task> Connected;
    event Func<Task> Tick;

    bool IsConnected { get; }
    uint Counter { get; set; }
    bool AllDigital { get; set; }
    int PwmFrequency { get; set; }
    DigitalChannel DigitalLowIn { get; }
    DigitalChannel DigitalHighIn { get; }
    DigitalChannel Digital1 { get; }
    DigitalChannel Digital2 { get; }
    DigitalChannel Digital3 { get; }
    DigitalChannel Digital4 { get; }
    DigitalChannel Digital5 { get; }
    DigitalChannel Digital6 { get; }
    DigitalChannel Digital7 { get; }
    DigitalChannel Digital8 { get; }
    AnalogChannel Analog1 { get; }
    AnalogChannel Analog2 { get; }
    AnalogChannel Analog3 { get; }
    AnalogChannel Analog4 { get; }
    AnalogChannel Analog5 { get; }
    AnalogChannel Pwm1 { get; }
    AnalogChannel Pwm2 { get; }

    Task<int> ListDevices();
    Task OpenDevice(int device);
    Task CloseDevice();
    int VersionDLL();
    Task<int> VersionFirmware();
}
