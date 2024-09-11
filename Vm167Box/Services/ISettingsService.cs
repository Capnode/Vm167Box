namespace Vm167Box.Services;

public interface ISettingsService
{
    public event Func<Task> Update;

    AppTheme AppTheme { get; set; }

    string Analog1Name { get; set; }
    string Analog1Unit { get; set; }
    double Analog1MinPoint { get; set; }
    double Analog1MinValue { get; set; }
    double Analog1MaxPoint { get; set; }
    double Analog1MaxValue { get; set; }

    string Analog2Name { get; set; }
    string Analog2Unit { get; set; }
    double Analog2MinPoint { get; set; }
    double Analog2MinValue { get; set; }
    double Analog2MaxPoint { get; set; }
    double Analog2MaxValue { get; set; }

    string Analog3Name { get; set; }
    string Analog3Unit { get; set; }
    double Analog3MinPoint { get; set; }
    double Analog3MinValue { get; set; }
    double Analog3MaxPoint { get; set; }
    double Analog3MaxValue { get; set; }

    string Analog4Name { get; set; }
    string Analog4Unit { get; set; }
    double Analog4MinPoint { get; set; }
    double Analog4MinValue { get; set; }
    double Analog4MaxPoint { get; set; }
    double Analog4MaxValue { get; set; }

    string Analog5Name { get; set; }
    string Analog5Unit { get; set; }
    double Analog5MinPoint { get; set; }
    double Analog5MinValue { get; set; }
    double Analog5MaxPoint { get; set; }
    double Analog5MaxValue { get; set; }

    string Pwm1Name { get; set; }
    string Pwm1Unit { get; set; }
    double Pwm1MinPoint { get; set; }
    double Pwm1MinValue { get; set; }
    double Pwm1MaxPoint { get; set; }
    double Pwm1MaxValue { get; set; }

    string Pwm2Name { get; set; }
    string Pwm2Unit { get; set; }
    double Pwm2MinPoint { get; set; }
    double Pwm2MinValue { get; set; }
    double Pwm2MaxPoint { get; set; }
    double Pwm2MaxValue { get; set; }

    /// <summary>
    /// Reset all settings to their default values.
    /// </summary>
    void Reset();
}