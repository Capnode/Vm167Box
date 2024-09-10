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

public partial class PanelViewModel : ObservableObject
{
    private readonly ILogger<PanelViewModel> _logger;
    private readonly ISettingsService _settingsService;
    private readonly IVm167Service _vm167Service;
    private bool _pending;
    private DateTime _startTime;
    private bool _resetScope;
    private bool _restartScope = true;

    public PanelViewModel(ILogger<PanelViewModel> logger, ISettingsService settingsService, IVm167Service vm167service)
    {
        _logger = logger;
        _settingsService = settingsService;
        _settingsService.Update += UpdateSettings;
        _vm167Service = vm167service;
        _vm167Service.Tick += ReadDevice;

        ScopeModel = new PlotModel();
        ScopeModel.Legends.Add(new Legend { LegendPosition = LegendPosition.TopRight, LegendPlacement = LegendPlacement.Inside });
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
        ScopeModel.Axes.Add(new LinearAxis
        {
            Title = "AnalogIn",
            Key = "AnalogIn",
            Position = AxisPosition.Left,
            PositionTier = 1,
            Minimum = 0,
            Maximum = 1023
        });
        ScopeModel.Axes.Add(new LinearAxis
        {
            Title = "PwmOut",
            Key = "PwmOut",
            Position = AxisPosition.Left,
            PositionTier = 2,
            Minimum = 0,
            Maximum = 255
        });
    }

    public string Analog1Name => _settingsService.Analog1Name;

    public string Analog2Name => _settingsService.Analog2Name;

    public string Analog3Name => _settingsService.Analog3Name;

    public string Analog4Name => _settingsService.Analog4Name;

    public string Analog5Name => _settingsService.Analog5Name;

    public string Pwm1Name => _settingsService.Pwm1Name;

    public string Pwm2Name => _settingsService.Pwm2Name;

    public string Analog1Unit => _settingsService.Analog1Unit;

    public string Analog2Unit => _settingsService.Analog2Unit;

    public string Analog3Unit => _settingsService.Analog3Unit;

    public string Analog4Unit => _settingsService.Analog4Unit;

    public string Analog5Unit => _settingsService.Analog5Unit;

    public string Pwm1Unit => _settingsService.Pwm1Unit;

    public string Pwm2Unit => _settingsService.Pwm2Unit;

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

    [ObservableProperty]
    private bool _digitalLowIn;

    [ObservableProperty]
    private bool _digitalLowOut;

    [ObservableProperty]
    private bool _digitalHighIn;

    [ObservableProperty]
    private bool _digitalHighOut;

    [ObservableProperty]
    private bool _digital1;

    [ObservableProperty]
    private bool _digital2;

    [ObservableProperty]
    private bool _digital3;

    [ObservableProperty]
    private bool _digital4;

    [ObservableProperty]
    private bool _digital5;

    [ObservableProperty]
    private bool _digital6;

    [ObservableProperty]
    private bool _digital7;

    [ObservableProperty]
    private bool _digital8;

    [ObservableProperty]
    private double _analogIn1;

    [ObservableProperty]
    private double _analogIn2;

    [ObservableProperty]
    private double _analogIn3;

    [ObservableProperty]
    private double _analogIn4;

    [ObservableProperty]
    private double _analogIn5;

    [ObservableProperty]
    private uint _counter;

    [ObservableProperty]
    private int _pwmFrequency;

    [ObservableProperty]
    private int _pwmOut1;

    [ObservableProperty]
    private int _pwmOut2;

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
    public async Task SetAllDigital()
    {
        if (_pending) return;

        _logger.LogTrace(">SetAllDigital()");
        await _vm167Service.SetAllDigital();
        _logger.LogTrace("<SetAllDigital()");
    }

    [RelayCommand]
    public async Task ClearAllDigital()
    {
        if (_pending) return;

        _logger.LogTrace(">ClearAllDigital()");
        await _vm167Service.ClearAllDigital();
        _logger.LogTrace("<ClearAllDigital()");
    }

    [RelayCommand]
    public async Task Version()
    {
        if (_pending) return;

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
    public async Task DigitalInOutMode()
    {
        if (_pending) return;

        _logger.LogTrace(">DigitalInOutMode()");
        await _vm167Service.InOutMode(DigitalHighIn ? 1 : 0, DigitalLowIn ? 1 : 0);
        _logger.LogTrace("<DigitalInOutMode()");
    }

    [RelayCommand]
    public async Task DigitalOut(string channel)
    {
        if (_pending) return;

        _logger.LogTrace(">DigitalOut({channel})", channel);
        var state = channel switch
        {
            "1" => Digital1,
            "2" => Digital2,
            "3" => Digital3,
            "4" => Digital4,
            "5" => Digital5,
            "6" => Digital6,
            "7" => Digital7,
            "8" => Digital8,
            _ => false
        };

        if (state)
        {
            await _vm167Service.SetDigitalChannel(int.Parse(channel));
        }
        else
        {
            await _vm167Service.ClearDigitalChannel(int.Parse(channel));
        }

        _logger.LogTrace("<DigitalOut({channel})", channel);
    }

    [RelayCommand]
    public async Task ResetCounter()
    {
        if (_pending) return;

        _logger.LogTrace(">ResetCounter()");
        await _vm167Service.ResetCounter();
        _logger.LogTrace("<ResetCounter()");
    }

    [RelayCommand]
    public async Task PwmOut(string channel)
    {
        if (_pending) return;

        _logger.LogTrace(">PwmOut({channel})", channel);
        var value = channel switch
        {
            "1" => PwmOut1,
            "2" => PwmOut2,
            _ => 0
        };

        _vm167Service.PwmFrequency = Math.Min(PwmFrequency + 1, 3);
        await _vm167Service.SetPwm(int.Parse(channel), value);
        _logger.LogTrace("<PwmOut({channel})", channel);
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

    private async Task ReadDevice()
    {
        if (_pending) return;
        _pending = true;

        var ioMode = await _vm167Service.ReadBackInOutMode();
        DigitalLowOut = (ioMode & 1) == 0;
        DigitalLowIn = (ioMode & 1) > 0;
        DigitalHighOut = (ioMode & 2) == 0;
        DigitalHighIn = (ioMode & 2) > 0;

        var digital = await _vm167Service.ReadAllDigital();
        Digital1 = (digital & 1) > 0;
        Digital2 = (digital & 2) > 0;
        Digital3 = (digital & 4) > 0;
        Digital4 = (digital & 8) > 0;
        Digital5 = (digital & 16) > 0;
        Digital6 = (digital & 32) > 0;
        Digital7 = (digital & 64) > 0;
        Digital8 = (digital & 128) > 0;

        var counter = await _vm167Service.ReadCounter();
        Counter = counter;

        int[] analog = new int[IVm167.NumAnalogIn];
        await _vm167Service.ReadAllAnalog(analog);
        AnalogIn1 = analog[0];
        AnalogIn2 = analog[1];
        AnalogIn3 = analog[2];
        AnalogIn4 = analog[3];
        AnalogIn5 = analog[4];

        int[] pwm = new int[IVm167.NumPwmOut];
        await _vm167Service.ReadBackPWMOut(pwm);
        PwmOut1 = pwm[0];
        PwmOut2 = pwm[1];

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
        AddPoint(i++, timestamp, AnalogIn1);
        AddPoint(i++, timestamp, AnalogIn2);
        AddPoint(i++, timestamp, AnalogIn3);
        AddPoint(i++, timestamp, AnalogIn4);
        AddPoint(i++, timestamp, AnalogIn5);
        AddPoint(i++, timestamp, PwmOut1);
        AddPoint(i++, timestamp, PwmOut2);
        ScopeModel.InvalidatePlot(true);

        _restartScope = false;
        _resetScope = false;
        _pending = false;
    }

    private Task UpdateSettings()
    {
        OnPropertyChanged(nameof(Analog1Name));
        OnPropertyChanged(nameof(Analog2Name));
        OnPropertyChanged(nameof(Analog3Name));
        OnPropertyChanged(nameof(Analog4Name));
        OnPropertyChanged(nameof(Analog5Name));
        OnPropertyChanged(nameof(Pwm1Name));
        OnPropertyChanged(nameof(Pwm2Name));

        OnPropertyChanged(nameof(Analog1Unit));
        OnPropertyChanged(nameof(Analog2Unit));
        OnPropertyChanged(nameof(Analog3Unit));
        OnPropertyChanged(nameof(Analog4Unit));
        OnPropertyChanged(nameof(Analog5Unit));
        OnPropertyChanged(nameof(Pwm1Unit));
        OnPropertyChanged(nameof(Pwm2Unit));

        _restartScope = true;
        return Task.CompletedTask;
    }

    private void AddSeries()
    {
        ScopeModel.Series.Clear();
        AddSerie(Analog1Name, "AnalogIn");
        AddSerie(Analog2Name, "AnalogIn");
        AddSerie(Analog3Name, "AnalogIn");
        AddSerie(Analog4Name, "AnalogIn");
        AddSerie(Analog5Name, "AnalogIn");
        AddSerie(Pwm1Name, "PwmOut");
        AddSerie(Pwm2Name, "PwmOut");

        ScopeModel.DefaultColors = OxyPalettes.Hue(ScopeModel.Series.Count).Colors;
    }

    private void AddSerie(string title, string yAxisKey)
    {
        ScopeModel.Series.Add(new LineSeries
        {
            YAxisKey = yAxisKey,
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