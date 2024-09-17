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
    double Analog1In { get; }
    double Analog2In { get; }
    double Analog3In { get; }
    double Analog4In { get; }
    double Analog5In { get; }
    double Pwm1Out { get; set; }
    double Pwm2Out { get; set; }
    int PwmFrequency { get; set; }

    Task<int> ListDevices();
    Task OpenDevice(int device);
    Task CloseDevice();
    int VersionDLL();
    Task<int> VersionFirmware();
}
