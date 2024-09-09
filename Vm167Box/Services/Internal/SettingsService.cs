﻿using Microsoft.Extensions.Logging;

namespace Vm167Box.Services.Internal;

internal class SettingsService : ISettingsService
{
    private readonly ILogger<SettingsService> _logger;
    private readonly IPreferences _settings = Preferences.Default;

    public SettingsService(ILogger<SettingsService> logger)
    {
        _logger = logger;
    }

    public AppTheme AppTheme
    {
        get => _settings.Get(nameof(AppTheme), Application.Current?.PlatformAppTheme ?? AppTheme.Unspecified);
        set => _settings.Set(nameof(AppTheme), (int)value);
    }

    public string? Analog1Name
    {
        get => _settings.Get(nameof(Analog1Name), string.Empty);
        set => _settings.Set(nameof(Analog1Name), value);
    }

    public string? Analog1Unit
    {
        get => _settings.Get(nameof(Analog1Unit), string.Empty);
        set => _settings.Set(nameof(Analog1Unit), value);
    }

    public double Analog1Point1
    {
        get => _settings.Get(nameof(Analog1Point1), double.NaN);
        set => _settings.Set(nameof(Analog1Point1), value);
    }

    public double Analog1Value1
    {
        get => _settings.Get(nameof(Analog1Value1), double.NaN);
        set => _settings.Set(nameof(Analog1Value1), value);
    }

    public double Analog1Point2
    {
        get => _settings.Get(nameof(Analog1Point2), double.NaN);
        set => _settings.Set(nameof(Analog1Point2), value);
    }

    public double Analog1Value2
    {
        get => _settings.Get(nameof(Analog1Value2), double.NaN);
        set => _settings.Set(nameof(Analog1Value2), value);
    }

    public string? Analog2Name
    {
        get => _settings.Get(nameof(Analog2Name), string.Empty);
        set => _settings.Set(nameof(Analog2Name), value);
    }

    public string? Analog2Unit
    {
        get => _settings.Get(nameof(Analog2Unit), string.Empty);
        set => _settings.Set(nameof(Analog2Unit), value);
    }

    public double Analog2Point1
    {
        get => _settings.Get(nameof(Analog2Point1), double.NaN);
        set => _settings.Set(nameof(Analog2Point1), value);
    }

    public double Analog2Value1
    {
        get => _settings.Get(nameof(Analog2Value1), double.NaN);
        set => _settings.Set(nameof(Analog2Value1), value);
    }

    public double Analog2Point2
    {
        get => _settings.Get(nameof(Analog2Point2), double.NaN);
        set => _settings.Set(nameof(Analog2Point2), value);
    }

    public double Analog2Value2
    {
        get => _settings.Get(nameof(Analog2Value2), double.NaN);
        set => _settings.Set(nameof(Analog2Value2), value);
    }

    public string? Analog3Name
    {
        get => _settings.Get(nameof(Analog3Name), string.Empty);
        set => _settings.Set(nameof(Analog3Name), value);
    }

    public string? Analog3Unit
    {
        get => _settings.Get(nameof(Analog3Unit), string.Empty);
        set => _settings.Set(nameof(Analog3Unit), value);
    }

    public double Analog3Point1
    {
        get => _settings.Get(nameof(Analog3Point1), double.NaN);
        set => _settings.Set(nameof(Analog3Point1), value);
    }

    public double Analog3Value1
    {
        get => _settings.Get(nameof(Analog3Value1), double.NaN);
        set => _settings.Set(nameof(Analog3Value1), value);
    }

    public double Analog3Point2
    {
        get => _settings.Get(nameof(Analog3Point2), double.NaN);
        set => _settings.Set(nameof(Analog3Point2), value);
    }

    public double Analog3Value2
    {
        get => _settings.Get(nameof(Analog3Value2), double.NaN);
        set => _settings.Set(nameof(Analog3Value2), value);
    }

    public string? Analog4Name
    {
        get => _settings.Get(nameof(Analog4Name), string.Empty);
        set => _settings.Set(nameof(Analog4Name), value);
    }

    public string? Analog4Unit
    {
        get => _settings.Get(nameof(Analog4Unit), string.Empty);
        set => _settings.Set(nameof(Analog4Unit), value);
    }

    public double Analog4Point1
    {
        get => _settings.Get(nameof(Analog4Point1), double.NaN);
        set => _settings.Set(nameof(Analog4Point1), value);
    }

    public double Analog4Value1
    {
        get => _settings.Get(nameof(Analog4Value1), double.NaN);
        set => _settings.Set(nameof(Analog4Value1), value);
    }

    public double Analog4Point2
    {
        get => _settings.Get(nameof(Analog4Point2), double.NaN);
        set => _settings.Set(nameof(Analog4Point2), value);
    }

    public double Analog4Value2
    {
        get => _settings.Get(nameof(Analog4Value2), double.NaN);
        set => _settings.Set(nameof(Analog4Value2), value);
    }

    public string? Analog5Name
    {
        get => _settings.Get(nameof(Analog5Name), string.Empty);
        set => _settings.Set(nameof(Analog5Name), value);
    }

    public string? Analog5Unit
    {
        get => _settings.Get(nameof(Analog5Unit), string.Empty);
        set => _settings.Set(nameof(Analog5Unit), value);
    }

    public double Analog5Point1
    {
        get => _settings.Get(nameof(Analog5Point1), double.NaN);
        set => _settings.Set(nameof(Analog5Point1), value);
    }

    public double Analog5Value1
    {
        get => _settings.Get(nameof(Analog5Value1), double.NaN);
        set => _settings.Set(nameof(Analog5Value1), value);
    }

    public double Analog5Point2
    {
        get => _settings.Get(nameof(Analog5Point2), double.NaN);
        set => _settings.Set(nameof(Analog5Point2), value);
    }

    public double Analog5Value2
    {
        get => _settings.Get(nameof(Analog5Value2), double.NaN);
        set => _settings.Set(nameof(Analog5Value2), value);
    }

    public string? Pwm1Name
    {
        get => _settings.Get(nameof(Pwm1Name), string.Empty);
        set => _settings.Set(nameof(Pwm1Name), value);
    }

    public string? Pwm1Unit
    {
        get => _settings.Get(nameof(Pwm1Unit), string.Empty);
        set => _settings.Set(nameof(Pwm1Unit), value);
    }

    public double Pwm1Point1
    {
        get => _settings.Get(nameof(Pwm1Point1), double.NaN);
        set => _settings.Set(nameof(Pwm1Point1), value);
    }

    public double Pwm1Value1
    {
        get => _settings.Get(nameof(Pwm1Value1), double.NaN);
        set => _settings.Set(nameof(Pwm1Value1), value);
    }

    public double Pwm1Point2
    {
        get => _settings.Get(nameof(Pwm1Point2), double.NaN);
        set => _settings.Set(nameof(Pwm1Point2), value);
    }

    public double Pwm1Value2
    {
        get => _settings.Get(nameof(Pwm1Value2), double.NaN);
        set => _settings.Set(nameof(Pwm1Value2), value);
    }

    public string? Pwm2Name
    {
        get => _settings.Get(nameof(Pwm2Name), string.Empty);
        set => _settings.Set(nameof(Pwm2Name), value);
    }

    public string? Pwm2Unit
    {
        get => _settings.Get(nameof(Pwm2Unit), string.Empty);
        set => _settings.Set(nameof(Pwm2Unit), value);
    }

    public double Pwm2Point1
    {
        get => _settings.Get(nameof(Pwm2Point1), double.NaN);
        set => _settings.Set(nameof(Pwm2Point1), value);
    }

    public double Pwm2Value1
    {
        get => _settings.Get(nameof(Pwm2Value1), double.NaN);
        set => _settings.Set(nameof(Pwm2Value1), value);
    }

    public double Pwm2Point2
    {
        get => _settings.Get(nameof(Pwm2Point2), double.NaN);
        set => _settings.Set(nameof(Pwm2Point2), value);
    }

    public double Pwm2Value2
    {
        get => _settings.Get(nameof(Pwm2Value2), double.NaN);
        set => _settings.Set(nameof(Pwm2Value2), value);
    }
}
