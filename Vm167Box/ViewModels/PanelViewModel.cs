using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Series;
using Vm167Box.Models;
using Vm167Box.Services;
using Vm167Lib;

namespace Vm167Box.ViewModels;

public class Frequency
{
    public required string Name { get; set; }
    public int Value { get; set; }
}

public class Range
{
    public double MinValue { get; set; }
    public double MaxValue { get; set; }
}

public partial class PanelViewModel : ObservableObject
{
    private const int MaxPoints = 1000;
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
    private TimeSpan _lastPlot;
    private bool _restartScope = true;
    private int _maxPoints = MaxPoints;
    private int _plotInterval = 1;

    public PanelViewModel(ILogger<PanelViewModel> logger, ISettingsService settingsService, IVm167Service vm167service)
    {
        _logger = logger;
        _settingsService = settingsService;
        _vm167Service = vm167service;
        _vm167Service.Tick += OnTick;

        ScopeModel = new PlotModel();
        ScopeModel.Legends.Add(new Legend { LegendPosition = LegendPosition.TopRight, LegendPlacement = LegendPlacement.Inside });
        UpdateSettings();

        _settingsService.Update += UpdateSettings;
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
    private DigitalChannel _digitalLowIn = new();

    [ObservableProperty]
    private DigitalChannel _digitalLowOut = new();

    [ObservableProperty]
    private DigitalChannel _digitalHighIn = new();

    [ObservableProperty]
    private DigitalChannel _digitalHighOut = new();

    [ObservableProperty]
    private DigitalChannel _digital1 = new();

    [ObservableProperty]
    private DigitalChannel _digital2 = new();

    [ObservableProperty]
    private DigitalChannel _digital3 = new();

    [ObservableProperty]
    private DigitalChannel _digital4 = new();

    [ObservableProperty]
    private DigitalChannel _digital5 = new();

    [ObservableProperty]
    private DigitalChannel _digital6 = new();

    [ObservableProperty]
    private DigitalChannel _digital7 = new();

    [ObservableProperty]
    private DigitalChannel _digital8 = new();

    public AnalogChannel Analog1 => _vm167Service.Analog1;

    public AnalogChannel Analog2 => _vm167Service.Analog2;

    public AnalogChannel Analog3 => _vm167Service.Analog3;

    public AnalogChannel Analog4 => _vm167Service.Analog4;

    public AnalogChannel Analog5 => _vm167Service.Analog5;

    public AnalogChannel Analog6 => _settingsService.Analog6;

    [ObservableProperty]
    private AnalogChannel _pwm1 = new();

    [ObservableProperty]
    private AnalogChannel _pwm2 = new();

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
        _vm167Service.AllDigital = true;
        _logger.LogTrace("<SetAllDigital()");
    }

    [RelayCommand]
    public void ClearAllDigital()
    {
        _logger.LogTrace(">ClearAllDigital()");
        _vm167Service.AllDigital = false;
        _logger.LogTrace("<ClearAllDigital()");
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

    [RelayCommand]
    public void History(int time)
    {
        _logger.LogTrace(">History({})", time);
        var plotIntervale = _plotInterval;
        if (time > 100)
        {
            _plotInterval = 10;
        }
        else
        {
            _plotInterval = 1;
        }

        if (plotIntervale != _plotInterval)
        {
            _restartScope = true;
        }

        _maxPoints = time * _plotInterval * 1000 / IVm167Service.Period;
        _logger.LogTrace("<History()");
    }

    [RelayCommand]
    public void Calibrate(string position)
    {
        _logger.LogTrace(">Calibrate({})", position);
        switch (position)
        {
            case "1L":
                _settingsService.Analog1MinSignal = Analog1.Signal;
                break;
            case "1H":
                _settingsService.Analog1MaxSignal = Analog1.Signal;
                break;
            case "2L":
                _settingsService.Analog2MinSignal = Analog2.Signal;
                break;
            case "2H":
                _settingsService.Analog2MaxSignal = Analog2.Signal;
                break;
            case "3L":
                _settingsService.Analog3MinSignal = Analog3.Signal;
                break;
            case "3H":
                _settingsService.Analog3MaxSignal = Analog3.Signal;
                break;
            case "4L":
                _settingsService.Analog4MinSignal = Analog4.Signal;
                break;
            case "4H":
                _settingsService.Analog4MaxSignal = Analog4.Signal;
                break;
            case "5L":
                _settingsService.Analog5MinSignal = Analog5.Signal;
                break;
            case "5H":
                _settingsService.Analog5MaxSignal = Analog5.Signal;
                break;
            case "6L":
                _settingsService.Analog6MinSignal = Analog6.Signal;
                break;
            case "6H":
                _settingsService.Analog6MaxSignal = Analog6.Signal;
                break;
        }

        _logger.LogTrace("<Calibrate()");
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
            IsOpen = false;
        }
    }

    private Task OnTick()
    {
        _logger.LogTrace(">Loop()");

        if (DigitalLowIn.Changed)
        {
            _vm167Service.DigitalLowIn.Value = DigitalLowIn.Value;
            DigitalLowOut.Value = !DigitalLowIn.Value;
            DigitalLowIn.Changed = false;
            OnPropertyChanged(nameof(DigitalLowIn));
            OnPropertyChanged(nameof(DigitalLowOut));
        }
        else if (_vm167Service.DigitalLowIn.Changed || DigitalLowIn.Value != _vm167Service.DigitalLowIn.Value)
        {
            DigitalLowIn.Copy(_vm167Service.DigitalLowIn);
            DigitalLowOut.Value = !DigitalLowIn.Value;
            OnPropertyChanged(nameof(DigitalLowIn));
            OnPropertyChanged(nameof(DigitalLowOut));
        }

        if (DigitalHighIn.Changed)
        {
            _vm167Service.DigitalHighIn.Value = DigitalHighIn.Value;
            DigitalHighOut.Value = !DigitalHighIn.Value;
            DigitalHighIn.Changed = false;
            OnPropertyChanged(nameof(DigitalHighIn));
            OnPropertyChanged(nameof(DigitalHighOut));
        }
        else if (_vm167Service.DigitalHighIn.Changed || DigitalHighIn.Value != _vm167Service.DigitalHighIn.Value)
        {
            DigitalHighIn.Copy(_vm167Service.DigitalHighIn);
            DigitalHighOut.Value = !DigitalHighIn.Value;
            OnPropertyChanged(nameof(DigitalHighIn));
            OnPropertyChanged(nameof(DigitalHighOut));
        }

        if (Digital1.Changed)
        {
            _vm167Service.Digital1.Value = Digital1.Value;
            Digital1.Changed = false;
            OnPropertyChanged(nameof(Digital1));
        }
        else if (_vm167Service.Digital1.Changed || Digital1.Value != _vm167Service.Digital1.Value)
        {
            Digital1.Copy(_vm167Service.Digital1);
            OnPropertyChanged(nameof(Digital1));
        }

        if (Digital2.Changed)
        {
            _vm167Service.Digital2.Value = Digital2.Value;
            Digital2.Changed = false;
            OnPropertyChanged(nameof(Digital2));
        }
        else if (_vm167Service.Digital2.Changed || Digital2.Value != _vm167Service.Digital2.Value)
        {
            Digital2.Copy(_vm167Service.Digital2);
            OnPropertyChanged(nameof(Digital2));
        }

        if (Digital3.Changed)
        {
            _vm167Service.Digital3.Value = Digital3.Value;
            Digital3.Changed = false;
            OnPropertyChanged(nameof(Digital3));
        }
        else if (_vm167Service.Digital3.Changed || Digital3.Value != _vm167Service.Digital3.Value)
        {
            Digital3.Copy(_vm167Service.Digital3);
            OnPropertyChanged(nameof(Digital3));
        }

        if (Digital4.Changed)
        {
            _vm167Service.Digital4.Value = Digital4.Value;
            Digital4.Changed = false;
            OnPropertyChanged(nameof(Digital4));
        }
        else if (_vm167Service.Digital4.Changed || Digital4.Value != _vm167Service.Digital4.Value)
        {
            Digital4.Copy(_vm167Service.Digital4);
            OnPropertyChanged(nameof(Digital4));
        }

        if (Digital5.Changed)
        {
            _vm167Service.Digital5.Value = Digital5.Value;
            Digital5.Changed = false;
            OnPropertyChanged(nameof(Digital5));
        }
        else if (_vm167Service.Digital5.Changed || Digital5.Value != _vm167Service.Digital5.Value)
        {
            Digital5.Copy(_vm167Service.Digital5);
            OnPropertyChanged(nameof(Digital5));
        }

        if (Digital6.Changed)
        {
            _vm167Service.Digital6.Value = Digital6.Value;
            Digital6.Changed = false;
            OnPropertyChanged(nameof(Digital6));
        }
        else if (_vm167Service.Digital6.Changed || Digital6.Value != _vm167Service.Digital6.Value)
        {
            Digital6.Copy(_vm167Service.Digital6);
            OnPropertyChanged(nameof(Digital6));
        }

        if (Digital7.Changed)
        {
            _vm167Service.Digital7.Value = Digital7.Value;
            Digital7.Changed = false;
            OnPropertyChanged(nameof(Digital7));
        }
        else if (_vm167Service.Digital7.Changed || Digital7.Value != _vm167Service.Digital7.Value)
        {
            Digital7.Copy(_vm167Service.Digital7);
            OnPropertyChanged(nameof(Digital7));
        }

        if (Digital8.Changed)
        {
            _vm167Service.Digital8.Value = Digital8.Value;
            Digital8.Changed = false;
            OnPropertyChanged(nameof(Digital8));
        }
        else if (_vm167Service.Digital8.Changed || Digital8.Value != _vm167Service.Digital8.Value)
        {
            Digital8.Copy(_vm167Service.Digital8);
            OnPropertyChanged(nameof(Digital8));
        }

        Counter = _vm167Service.Counter;

        if (Analog1.Changed) OnPropertyChanged(nameof(Analog1));
        if (Analog2.Changed) OnPropertyChanged(nameof(Analog2));
        if (Analog3.Changed) OnPropertyChanged(nameof(Analog3));
        if (Analog4.Changed) OnPropertyChanged(nameof(Analog4));
        if (Analog5.Changed) OnPropertyChanged(nameof(Analog5));
        if (Analog6.Changed) OnPropertyChanged(nameof(Analog6));

        if (Pwm1.Changed)
        {
            _vm167Service.Pwm1.Signal = Pwm1.Signal;
            Pwm1.Changed = false;
            OnPropertyChanged(nameof(Pwm1));
        }
        else if (_vm167Service.Pwm1.Changed || Pwm1.Value != _vm167Service.Pwm1.Value)
        {
            Pwm1.Copy(_vm167Service.Pwm1);
            OnPropertyChanged(nameof(Pwm1));
        }

        if (Pwm2.Changed)
        {
            _vm167Service.Pwm2.Signal = Pwm2.Signal;
            Pwm2.Changed = false;
            OnPropertyChanged(nameof(Pwm2));
        }
        else if (_vm167Service.Pwm2.Changed || Pwm2.Value != _vm167Service.Pwm2.Value)
        {
            Pwm2.Copy(_vm167Service.Pwm2);
            OnPropertyChanged(nameof(Pwm2));
        }

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
        if ((_plotInterval == 1) || (timestamp.Seconds != _lastPlot.Seconds))
        {
            var i = 0;
            AddPoint(i++, timestamp, Analog1.Value);
            AddPoint(i++, timestamp, Analog2.Value);
            AddPoint(i++, timestamp, Analog3.Value);
            AddPoint(i++, timestamp, Analog4.Value);
            AddPoint(i++, timestamp, Analog5.Value);
            AddPoint(i++, timestamp, Analog6.Value);
            AddPoint(i++, timestamp, Pwm1.Value);
            AddPoint(i++, timestamp, Pwm2.Value);
            ScopeModel.InvalidatePlot(true);
            _lastPlot = timestamp;
        }

        _restartScope = false;
        _resetScope = false;
        _logger.LogTrace("<Loop()");
        return Task.CompletedTask;
    }

    private void UpdateSettings()
    {
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

        AddSerie(Analog1.Name, Analog1.Unit, Resources.AppResources.AnalogIn, _settingsService.Analog1MinValue, _settingsService.Analog1MaxValue);
        AddSerie(Analog2.Name, Analog2.Unit, Resources.AppResources.AnalogIn, _settingsService.Analog2MinValue, _settingsService.Analog2MaxValue);
        AddSerie(Analog3.Name, Analog3.Unit, Resources.AppResources.AnalogIn, _settingsService.Analog3MinValue, _settingsService.Analog3MaxValue);
        AddSerie(Analog4.Name, Analog4.Unit, Resources.AppResources.AnalogIn, _settingsService.Analog4MinValue, _settingsService.Analog4MaxValue);
        AddSerie(Analog5.Name, Analog5.Unit, Resources.AppResources.AnalogIn, _settingsService.Analog5MinValue, _settingsService.Analog5MaxValue);
        AddSerie(Analog6.Name, Analog6.Unit, Resources.AppResources.AnalogIn, _settingsService.Analog6MinValue, _settingsService.Analog6MaxValue);
        AddSerie(Pwm1.Name, Pwm1.Unit, Resources.AppResources.PwmOut, _settingsService.Pwm1MinValue, _settingsService.Pwm1MaxValue);
        AddSerie(Pwm2.Name, Pwm2.Unit, Resources.AppResources.PwmOut, _settingsService.Pwm2MinValue, _settingsService.Pwm2MaxValue);

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

        var key = $"{unit}";
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

        while (points.Count > _maxPoints)
        {
            points.RemoveAt(0);
        }

        points.Add(new DataPoint(TimeSpanAxis.ToDouble(timestamp), value));
    }
}