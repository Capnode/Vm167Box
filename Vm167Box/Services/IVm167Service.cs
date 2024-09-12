namespace Vm167Box.Services;

public interface IVm167Service
{
    const int Period = 100;

    event Func<Task> Tick;

    bool IsDigitalLowIn { get; set; }
    bool IsDigitalHighIn { get; set; }
    bool Digital1 { get; set; }
    bool Digital2 { get; set; }
    bool Digital3 { get; set; }
    bool Digital4 { get; set; }
    bool Digital5 { get; set; }
    bool Digital6 { get; set; }
    bool Digital7 { get; set; }
    bool Digital8 { get; set; }
    uint Counter { get; set; }
    double AnalogIn1 { get; }
    double AnalogIn2 { get; }
    double AnalogIn3 { get; }
    double AnalogIn4 { get; }
    double AnalogIn5 { get; }
    double PwmOut1 { get; set; }
    double PwmOut2 { get; set; }
    int PwmFrequency { get; set; }

    Task<int> ListDevices();
    Task OpenDevice(int device);
    Task CloseDevice();
    int VersionDLL();
    Task<int> VersionFirmware();
}
