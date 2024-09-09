namespace Vm167Box.Services;

public interface ISettingsService
{
    AppTheme AppTheme { get; set; }

    string? Analog1Name { get; set; }
    string? Analog1Unit { get; set; }
    double Analog1Point1 { get; set; }
    double Analog1Value1 { get; set; }
    double Analog1Point2 { get; set; }
    double Analog1Value2 { get; set; }

    string? Analog2Name { get; set; }
    string? Analog2Unit { get; set; }
    double Analog2Point1 { get; set; }
    double Analog2Value1 { get; set; }
    double Analog2Point2 { get; set; }
    double Analog2Value2 { get; set; }

    string? Analog3Name { get; set; }
    string? Analog3Unit { get; set; }
    double Analog3Point1 { get; set; }
    double Analog3Value1 { get; set; }
    double Analog3Point2 { get; set; }
    double Analog3Value2 { get; set; }

    string? Analog4Name { get; set; }
    string? Analog4Unit { get; set; }
    double Analog4Point1 { get; set; }
    double Analog4Value1 { get; set; }
    double Analog4Point2 { get; set; }
    double Analog4Value2 { get; set; }

    string? Analog5Name { get; set; }
    string? Analog5Unit { get; set; }
    double Analog5Point1 { get; set; }
    double Analog5Value1 { get; set; }
    double Analog5Point2 { get; set; }
    double Analog5Value2 { get; set; }

    string? Pwm1Name { get; set; }
    string? Pwm1Unit { get; set; }
    double Pwm1Point1 { get; set; }
    double Pwm1Value1 { get; set; }
    double Pwm1Point2 { get; set; }
    double Pwm1Value2 { get; set; }

    string? Pwm2Name { get; set; }
    string? Pwm2Unit { get; set; }
    double Pwm2Point1 { get; set; }
    double Pwm2Value1 { get; set; }
    double Pwm2Point2 { get; set; }
    double Pwm2Value2 { get; set; }
}
