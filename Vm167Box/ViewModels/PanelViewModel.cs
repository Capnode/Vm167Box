using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Series;
using Vm167Box.Services;
using Vm167Lib;

namespace Vm167Box.ViewModels;

public class Frequency
{
    public required string Name { get; set; }
    public int Value { get; set; }
}

public partial class PanelViewModel : ObservableObject
{
    private static readonly Frequency[] _frequencies =
    {
        new Frequency { Name = "2930 Hz", Value = IVm167.Freq2930 },
        new Frequency { Name = "11719 Hz", Value = IVm167.Freq11719 },
        new Frequency { Name = "46875 Hz", Value = IVm167.Freq46875 }
    };

    private readonly ILogger<PanelViewModel> _logger;
    private readonly ISettingsService _settingsService;
    private readonly IVm167Service _vm167Service;
    private DateTime _startTime;
    private bool _resetScope;
    private bool _restartScope = true;

    public PanelViewModel(ILogger<PanelViewModel> logger, ISettingsService settingsService, IVm167Service vm167service)
    {
        _logger = logger;
        _settingsService = settingsService;
        _vm167Service = vm167service;
        _vm167Service.Tick += Loop;

        ScopeModel = new PlotModel();
        ScopeModel.Legends.Add(new Legend { LegendPosition = LegendPosition.TopRight, LegendPlacement = LegendPlacement.Inside });
        UpdateSettings();

        _settingsService.Update += () =>
        {
            UpdateSettings();
            return Task.CompletedTask;
        };
    }


    public IEnumerable<Frequency> PwmFrequencies => _frequencies;

    [ObservableProperty]
    private bool _isOpen;

    [ObservableProperty]
    private bool _card0Exist;

    [ObservableProperty]
    private bool _card1Exist;

    [ObservableProperty]
    private bool _card0;

    [ObservableProperty]
    private bool _card1;

    [ObservableProperty]
    private string _firmwareVersion = string.Empty;

    [ObservableProperty]
    private string _dllVersion = string.Empty;

    private bool _digitalLowIn;
    public bool DigitalLowIn
    {
        get => _digitalLowIn;
        set
        {
            if (SetProperty(ref _digitalLowIn, value))
            {
                _logger.LogTrace("DigitalLowIn = {}", value);
                _vm167Service.IsDigitalLowIn = value;
            }
        }
    }

    private bool _digitalHighIn;
    public bool DigitalHighIn
    {
        get => _digitalHighIn;
        set
        {
            if (SetProperty(ref _digitalHighIn, value))
            {
                _logger.LogTrace("DigitalHighIn = {}", value);
                _vm167Service.IsDigitalHighIn = value;
            }
        }
    }

    private bool _digitalLowOut;
    public bool DigitalLowOut
    {
        get => _digitalLowOut;
        set
        {
            if (SetProperty(ref _digitalLowOut, value))
            {
                _logger.LogTrace("DigitalLowOut = {}", value);
                _vm167Service.IsDigitalLowIn = !value;
            }
        }
    }

    private bool _digitalHighOut;
    public bool DigitalHighOut
    {
        get => _digitalHighOut;
        set
        {
            if (SetProperty(ref _digitalHighOut, value))
            {
                _logger.LogTrace("DigitalHighOut = {}", value);
                _vm167Service.IsDigitalHighIn = !value;
            }
        }
    }

    private bool _digital1;
    public bool Digital1
    {
        get => _digital1;
        set
        {
            if (SetProperty(ref _digital1, value))
            {
                _vm167Service.Digital1 = value;
            }
        }
    }

    private bool _digital2;
    public bool Digital2
    {
        get => _digital2;
        set
        {
            if (SetProperty(ref _digital2, value))
            {
                _vm167Service.Digital2 = value;
            }
        }
    }

    private bool _digital3;
    public bool Digital3
    {
        get => _digital3;
        set
        {
            if (SetProperty(ref _digital3, value))
            {
                _vm167Service.Digital3 = value;
            }
        }
    }

    private bool _digital4;
    public bool Digital4
    {
        get => _digital4;
        set
        {
            if (SetProperty(ref _digital4, value))
            {
                _vm167Service.Digital4 = value;
            }
        }
    }

    private bool _digital5;
    public bool Digital5
    {
        get => _digital5;
        set
        {
            if (SetProperty(ref _digital5, value))
            {
                _vm167Service.Digital5 = value;
            }
        }
    }

    private bool _digital6;
    public bool Digital6
    {
        get => _digital6;
        set
        {
            if (SetProperty(ref _digital6, value))
            {
                _vm167Service.Digital6 = value;
            }
        }
    }

    private bool _digital7;
    public bool Digital7
    {
        get => _digital7;
        set
        {
            if (SetProperty(ref _digital7, value))
            {
                _vm167Service.Digital7 = value;
            }
        }
    }

    private bool _digital8;
    public bool Digital8
    {
        get => _digital8;
        set
        {
            if (SetProperty(ref _digital8, value))
            {
                _vm167Service.Digital8 = value;
            }
        }
    }

    [ObservableProperty]
    private double _analog1In;

    [ObservableProperty]
    private string _analog1Name = string.Empty;

    [ObservableProperty]
    private string _analog1Unit = string.Empty;

    [ObservableProperty]
    private double _analog1MinValue;

    [ObservableProperty]
    private double _analog1MaxValue;

    [ObservableProperty]
    private double _analog2In;

    [ObservableProperty]
    private string _analog2Name = string.Empty;

    [ObservableProperty]
    private string _analog2Unit = string.Empty;

    [ObservableProperty]
    private double _analog2MinValue;

    [ObservableProperty]
    private double _analog2MaxValue;

    [ObservableProperty]
    private double _analog3In;

    [ObservableProperty]
    private string _analog3Name = string.Empty;

    [ObservableProperty]
    private string _analog3Unit = string.Empty;

    [ObservableProperty]
    private double _analog3MinValue;

    [ObservableProperty]
    private double _analog3MaxValue;

    [ObservableProperty]
    private double _analog4In;

    [ObservableProperty]
    private string _analog4Name = string.Empty;

    [ObservableProperty]
    private string _analog4Unit = string.Empty;

    [ObservableProperty]
    private double _analog4MinValue;

    [ObservableProperty]
    private double _analog4MaxValue;

    [ObservableProperty]
    private double _analog5In;

    [ObservableProperty]
    private string _analog5Name = string.Empty;

    [ObservableProperty]
    private string _analog5Unit = string.Empty;

    [ObservableProperty]
    private double _analog5MinValue;

    [ObservableProperty]
    private double _analog5MaxValue;

    [ObservableProperty]
    private uint _counter;

    private Frequency _pwmFrequency = _frequencies[0];
    public Frequency PwmFrequency
    {
        get => _pwmFrequency;
        set
        {
            SetProperty(ref _pwmFrequency, value);
            _vm167Service.PwmFrequency = value.Value;
        }
    }

    private double _pwm1Out;
    public double Pwm1Out
    {
        get => _pwm1Out;
        set
        {
            if (SetProperty(ref _pwm1Out, value))
            { 
                _vm167Service.Pwm1Out = value;
            }
        }
    }

    [ObservableProperty]
    private string _pwm1Name = string.Empty;

    [ObservableProperty]
    private string _pwm1Unit = string.Empty;

    [ObservableProperty]
    private double _pwm1MinValue;

    [ObservableProperty]
    private double _pwm1MaxValue;

    private double _pwm2Out;
    public double Pwm2Out
    {
        get => _pwm2Out;
        set
        {
            if (SetProperty(ref _pwm2Out, value))
            {
                _vm167Service.Pwm2Out = value;
            }
        }
    }

    [ObservableProperty]
    private string _pwm2Name = string.Empty;

    [ObservableProperty]
    private string _pwm2Unit = string.Empty;

    [ObservableProperty]
    private double _pwm2MinValue;

    [ObservableProperty]
    private double _pwm2MaxValue;

    [ObservableProperty]
    private PlotModel _scopeModel;

    [RelayCommand]
    public async Task Open()
    {
        _logger.LogTrace(">Open()");
        try
        {
            var mask = await _vm167Service.ListDevices();
            Card0Exist = (mask & 1) > 0;
            Card1Exist = (mask & 2) > 0;
        }
        catch (Exception)
        {
            Card0Exist = false;
            Card1Exist = false;
            throw;
        }
        finally
        {
            _logger.LogTrace("<Open()");
        }
    }

    [RelayCommand]
    public async Task SelectCard0(CheckedChangedEventArgs args)
    {
        _logger.LogTrace(">SelectCard0({})", args.Value);
        await SelectCard(args.Value, IVm167.Device0);
        _logger.LogTrace("<SelectCard0()");
    }

    [RelayCommand]
    public async Task SelectCard1(CheckedChangedEventArgs args)
    {
        _logger.LogTrace(">SelectCard1({})", args.Value);
        await SelectCard(args.Value, IVm167.Device1);
        _logger.LogTrace("<SelectCard1()");
    }

    [RelayCommand]
    public void SetAllDigital()
    {
        _logger.LogTrace(">SetAllDigital()");
        _vm167Service.Digital1 = true;
        _vm167Service.Digital2 = true;
        _vm167Service.Digital3 = true;
        _vm167Service.Digital4 = true;
        _vm167Service.Digital5 = true;
        _vm167Service.Digital6 = true;
        _vm167Service.Digital7 = true;
        _vm167Service.Digital8 = true;
        _logger.LogTrace("<SetAllDigital()");
    }

    [RelayCommand]
    public void ClearAllDigital()
    {
        _logger.LogTrace(">ClearAllDigital()");
        _vm167Service.Digital1 = false;
        _vm167Service.Digital2 = false;
        _vm167Service.Digital3 = false;
        _vm167Service.Digital4 = false;
        _vm167Service.Digital5 = false;
        _vm167Service.Digital6 = false;
        _vm167Service.Digital7 = false;
        _vm167Service.Digital8 = false;
        _logger.LogTrace("<ClearAllDigital()");
    }

    [RelayCommand]
    public async Task Version()
    {
        _logger.LogTrace(">Version()");
        var value = await _vm167Service.VersionFirmware();
        var firmware = new Version(value >> 24, (value >> 16) & 0xFF, (value >> 8) & 0xFF, value & 0xFF);
        FirmwareVersion = $"Firmware: v{firmware}";

        value = _vm167Service.VersionDLL();
        var dll = new Version(value >> 24, (value >> 16) & 0xFF, (value >> 8) & 0xFF, value & 0xFF);
        DllVersion = $"DLL: v{dll}";
        _logger.LogTrace("<Version()");
    }

    [RelayCommand]
    public void ResetCounter()
    {
        _logger.LogTrace(">ResetCounter()");
        _vm167Service.Counter = 0;
        _logger.LogTrace("<ResetCounter()");
    }

    [RelayCommand]
    public void ResetScope()
    {
        _resetScope = true;
    }

    [RelayCommand]
    public void RestartScope()
    {
        _restartScope = true;
    }

    private async Task SelectCard(bool selected, int card)
    {
        if (selected)
        {
            try
            {
                await _vm167Service.OpenDevice(card);
                _restartScope = true;
                IsOpen = true;
            }
            catch (Exception)
            {
                IsOpen = false;
                Card0 = false;
                Card1 = false;
                throw;
            }
        }
        else
        {
            await _vm167Service.CloseDevice();
            FirmwareVersion = string.Empty;
            DllVersion = string.Empty;
            IsOpen = false;
        }
    }

    private Task Loop()
    {
        _logger.LogTrace(">Loop()");
        DigitalLowOut = !_vm167Service.IsDigitalLowIn;
        DigitalLowIn = _vm167Service.IsDigitalLowIn;
        DigitalHighOut = !_vm167Service.IsDigitalHighIn;
        DigitalHighIn = _vm167Service.IsDigitalHighIn;

        Digital1 = _vm167Service.Digital1;
        Digital2 = _vm167Service.Digital2;
        Digital3 = _vm167Service.Digital3;
        Digital4 = _vm167Service.Digital4;
        Digital5 = _vm167Service.Digital5;
        Digital6 = _vm167Service.Digital6;
        Digital7 = _vm167Service.Digital7;
        Digital8 = _vm167Service.Digital8;

        Counter = _vm167Service.Counter;

        Analog1In = _vm167Service.Analog1In;
        Analog2In = _vm167Service.Analog2In;
        Analog3In = _vm167Service.Analog3In;
        Analog4In = _vm167Service.Analog4In;
        Analog5In = _vm167Service.Analog5In;

        Pwm1Out = _vm167Service.Pwm1Out;
        Pwm2Out = _vm167Service.Pwm2Out;

        var timestamp = DateTime.Now - _startTime;
        if (_restartScope)
        {
            _startTime = DateTime.Now;
            timestamp = TimeSpan.Zero;
            ScopeModel.ResetAllAxes();
            AddSeries();
        }
        else if (_resetScope)
        {
            ScopeModel.ResetAllAxes();
        }

        // Update points
        var i = 0;
        AddPoint(i++, timestamp, Analog1In);
        AddPoint(i++, timestamp, Analog2In);
        AddPoint(i++, timestamp, Analog3In);
        AddPoint(i++, timestamp, Analog4In);
        AddPoint(i++, timestamp, Analog5In);
        AddPoint(i++, timestamp, Pwm1Out);
        AddPoint(i++, timestamp, Pwm2Out);
        ScopeModel.InvalidatePlot(true);

        _restartScope = false;
        _resetScope = false;

        _logger.LogTrace("<Loop()");
        return Task.CompletedTask;
    }

    private void UpdateSettings()
    {
        Analog1Name = _settingsService.Analog1Name;
        Analog1Unit = _settingsService.Analog1Unit;
        Analog1MinValue = _settingsService.Analog1MinValue;
        Analog1MaxValue = _settingsService.Analog1MaxValue;

        Analog2Name = _settingsService.Analog2Name;
        Analog2Unit = _settingsService.Analog2Unit;
        Analog2MinValue = _settingsService.Analog2MinValue;
        Analog2MaxValue = _settingsService.Analog2MaxValue;

        Analog3Name = _settingsService.Analog3Name;
        Analog3Unit = _settingsService.Analog3Unit;
        Analog3MinValue = _settingsService.Analog3MinValue;
        Analog3MaxValue = _settingsService.Analog3MaxValue;

        Analog4Name = _settingsService.Analog4Name;
        Analog4Unit = _settingsService.Analog4Unit;
        Analog4MinValue = _settingsService.Analog4MinValue;
        Analog4MaxValue = _settingsService.Analog4MaxValue;

        Analog5Name = _settingsService.Analog5Name;
        Analog5Unit = _settingsService.Analog5Unit;
        Analog5MinValue = _settingsService.Analog5MinValue;
        Analog5MaxValue = _settingsService.Analog5MaxValue;

        Pwm1Name = _settingsService.Pwm1Name;
        Pwm1Unit = _settingsService.Pwm1Unit;
        Pwm1MinValue = _settingsService.Pwm1MinValue;
        Pwm1MaxValue = _settingsService.Pwm1MaxValue;

        Pwm2Name = _settingsService.Pwm2Name;
        Pwm2Unit = _settingsService.Pwm2Unit;
        Pwm2MinValue = _settingsService.Pwm2MinValue;
        Pwm2MaxValue = _settingsService.Pwm2MaxValue;

        _restartScope = true;
    }

    private void AddSeries()
    {
        ScopeModel.Axes.Clear();
        ScopeModel.Series.Clear();

        ScopeModel.Axes.Add(new LinearAxis
        {
            Title = "Time (s)",
            Position = AxisPosition.Bottom,
            IntervalLength = 60,
            IsPanEnabled = true,
            IsZoomEnabled = true,
            MajorGridlineStyle = LineStyle.Solid,
            MinorGridlineStyle = LineStyle.Solid,
        });

        AddSerie(Analog1Name, Analog1Unit, Resources.AppResources.AnalogIn, _settingsService.Analog1MinValue, _settingsService.Analog1MaxValue);
        AddSerie(Analog2Name, Analog2Unit, Resources.AppResources.AnalogIn, _settingsService.Analog2MinValue, _settingsService.Analog2MaxValue);
        AddSerie(Analog3Name, Analog3Unit, Resources.AppResources.AnalogIn, _settingsService.Analog3MinValue, _settingsService.Analog3MaxValue);
        AddSerie(Analog4Name, Analog4Unit, Resources.AppResources.AnalogIn, _settingsService.Analog4MinValue, _settingsService.Analog4MaxValue);
        AddSerie(Analog5Name, Analog5Unit, Resources.AppResources.AnalogIn, _settingsService.Analog5MinValue, _settingsService.Analog5MaxValue);
        AddSerie(Pwm1Name, Pwm1Unit, Resources.AppResources.PwmOut, _settingsService.Pwm1MinValue, _settingsService.Pwm1MaxValue);
        AddSerie(Pwm2Name, Pwm2Unit, Resources.AppResources.PwmOut, _settingsService.Pwm2MinValue, _settingsService.Pwm2MaxValue);

        ScopeModel.DefaultColors =  OxyPalette.Interpolate(
            ScopeModel.Series.Count,
            OxyColors.DarkBlue,
            OxyColors.DarkRed,
            OxyColors.Violet,
            OxyColors.Indigo,
            OxyColors.Blue,
            OxyColors.Green,
            OxyColors.Orange,
            OxyColors.Cyan,
            OxyColors.Magenta,
            OxyColors.Red).Colors;
    }

    private void AddSerie(string title, string unit, string fallback, double minValue, double maxValue)
    {
        if (string.IsNullOrWhiteSpace(unit))
        {
            unit = fallback;
        }

        var key = $"{unit}_{minValue}_{maxValue}";
        var count = ScopeModel.Axes.Count;
        var axes = ScopeModel.Axes.FirstOrDefault(a => a.Key == key);
        if (axes == default)
        {
            ScopeModel.Axes.Add(new LinearAxis
            {
                Title = unit,
                Key = key,
                Position = AxisPosition.Left,
                PositionTier = count + 1,
                Minimum = minValue,
                Maximum = maxValue
            });
        }

        ScopeModel.Series.Add(new LineSeries
        {
            YAxisKey = key,
            Title = title,
            LineStyle = LineStyle.Solid,
            TrackerFormatString = "{0}\n{1}: {2:0.###}\n{3}: {4:0.###}"
        });
    }

    private void AddPoint(int i, TimeSpan timestamp, double value)
    {
        var serie = (LineSeries)ScopeModel.Series[i];
        var points = serie.Points;
        if (_restartScope)
        {
            points.Clear();
        }

        points.Add(new DataPoint(TimeSpanAxis.ToDouble(timestamp), value));
    }
}