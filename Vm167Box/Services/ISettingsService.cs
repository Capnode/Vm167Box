namespace Vm167Box.Services;

public interface ISettingsService
{
    public event Func<Task> Update;

    AppTheme AppTheme { get; set; }

    string Analog1Name { get; set; }
    string Analog1Unit { get; set; }
    int Analog1MinSignal { get; set; }
    double Analog1MinValue { get; set; }
    int Analog1MaxSignal { get; set; }
    double Analog1MaxValue { get; set; }

    string Analog2Name { get; set; }
    string Analog2Unit { get; set; }
    int Analog2MinSignal { get; set; }
    double Analog2MinValue { get; set; }
    int Analog2MaxSignal { get; set; }
    double Analog2MaxValue { get; set; }

    string Analog3Name { get; set; }
    string Analog3Unit { get; set; }
    int Analog3MinSignal { get; set; }
    double Analog3MinValue { get; set; }
    int Analog3MaxSignal { get; set; }
    double Analog3MaxValue { get; set; }

    string Analog4Name { get; set; }
    string Analog4Unit { get; set; }
    int Analog4MinSignal { get; set; }
    double Analog4MinValue { get; set; }
    int Analog4MaxSignal { get; set; }
    double Analog4MaxValue { get; set; }

    string Analog5Name { get; set; }
    string Analog5Unit { get; set; }
    int Analog5MinSignal { get; set; }
    double Analog5MinValue { get; set; }
    int Analog5MaxSignal { get; set; }
    double Analog5MaxValue { get; set; }

    string Pwm1Name { get; set; }
    string Pwm1Unit { get; set; }
    int Pwm1MinSignal { get; set; }
    double Pwm1MinValue { get; set; }
    int Pwm1MaxSignal { get; set; }
    double Pwm1MaxValue { get; set; }

    string Pwm2Name { get; set; }
    string Pwm2Unit { get; set; }
    int Pwm2MinSignal { get; set; }
    double Pwm2MinValue { get; set; }
    int Pwm2MaxSignal { get; set; }
    double Pwm2MaxValue { get; set; }

    /// <summary>
    /// Reset all settings to their default values.
    /// </summary>
    void Reset();
}