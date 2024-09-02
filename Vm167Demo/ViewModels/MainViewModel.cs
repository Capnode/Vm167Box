using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Series;
using Vm167Box;

namespace Vm167Demo.ViewModels;

public partial class MainViewModel : BaseViewModel, IDisposable
{
    private readonly ILogger<MainViewModel> _logger;
    private readonly IVm167 _vm167;
    private Timer? _timer;
    private bool _pending;
    private DateTime _startTime;
    private bool _resetScope;
    private bool _restartScope = true;

    private int Device => Card0 ? Vm167.Device0 : Card1 ? Vm167.Device1 : -1;

    public MainViewModel(ILogger<MainViewModel> logger, IVm167 vm167)
    {
        _logger = logger;
        _vm167 = vm167;

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

        foreach (var i in Enumerable.Range(0, Vm167.NumAnalogIn))
        {
            ScopeModel.Series.Add(new LineSeries
            {
                YAxisKey = "AnalogIn",
                Title = $"AnalogIn{i + 1}",
                LineStyle = LineStyle.Solid
            });
        }

        foreach (var i in Enumerable.Range(0, Vm167.NumPwmOut))
        {
            ScopeModel.Series.Add(new LineSeries
            {
                YAxisKey = "PwmOut",
                Title = $"PwmOut{i + 1}",
                LineStyle = LineStyle.Solid
            });
        }
    }

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
    private int _pwmOut1;

    [ObservableProperty]
    private int _pwmOut2;

    [ObservableProperty]
    private int _pwmFreq;

    [ObservableProperty]
    private PlotModel _scopeModel;

    public void Dispose()
    {
        _timer?.Dispose();
        while (_pending)
        {
            Thread.Sleep(100);
        }

        _vm167.Dispose();
        _timer = null;
    }

    [RelayCommand]
    public async Task Open()
    {
        _logger.LogTrace(">Open()");
        try
        {
            await _vm167.CloseDevices();
            var mask = await _vm167.OpenDevices();
            _logger.LogInformation("Open card mask {Mask}", mask);
            if (mask < 0) throw new ApplicationException(Resources.AppResources.NoCardFound);
            if (mask == 0) throw new ApplicationException(Resources.AppResources.DriverProblem);
            IsOpen = true;
            Card0Exist = (mask & 1) > 0;
            Card1Exist = (mask & 2) > 0;
            Card0 = Card0Exist;
            Card1 = Card1Exist && !Card0Exist;
            if (_timer != null) return;

            _timer = new(
                async(obj) => await ReadDevice(),
                null,
                TimeSpan.FromMilliseconds(100),
                TimeSpan.FromMilliseconds(100));
        }
        catch (Exception)
        {
            IsOpen = false;
            Card0Exist = false;
            Card1Exist = false;
            Card0 = false;
            Card1 = false;
            throw;
        }
        finally
        {
            _logger.LogTrace("<Open()");
        }
    }

    [RelayCommand]
    public async Task SetAllDigital()
    {
        if (_pending) return;

        _logger.LogTrace(">SetAllDigital()");
        await _vm167.SetAllDigital(Device);
        _logger.LogTrace("<SetAllDigital()");
    }

    [RelayCommand]
    public async Task ClearAllDigital()
    {
        if (_pending) return;

        _logger.LogTrace(">ClearAllDigital()");
        await _vm167.ClearAllDigital(Device);
        _logger.LogTrace("<ClearAllDigital()");
    }

    [RelayCommand]
    public async Task Version()
    {
        if (_pending) return;

        _logger.LogTrace(">Version()");
        var value = await _vm167.VersionFirmware(Device);
        var firmware = new Version(value >> 24, (value >> 16) & 0xFF, (value >> 8) & 0xFF, value & 0xFF);
        FirmwareVersion = $"Firmware: v{firmware}";

        value = _vm167.VersionDLL();
        var dll = new Version(value >> 24, (value >> 16) & 0xFF, (value >> 8) & 0xFF, value & 0xFF);
        DllVersion = $"DLL: v{dll}";
        _logger.LogTrace("<Version()");
    }

    [RelayCommand]
    public async Task DigitalInOutMode()
    {
        if (_pending) return;

        _logger.LogTrace(">DigitalInOutMode()");
        await _vm167.InOutMode(Device, DigitalHighIn ? 1 : 0, DigitalLowIn ? 1 : 0);
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
            await _vm167.SetDigitalChannel(Device, int.Parse(channel));
        }
        else
        {
            await _vm167.ClearDigitalChannel(Device, int.Parse(channel));
        }

        _logger.LogTrace("<DigitalOut({channel})", channel);
    }

    [RelayCommand]
    public async Task ResetCounter()
    {
        if (_pending) return;

        _logger.LogTrace(">ResetCounter()");
        await _vm167.ResetCounter(Device);
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

        await _vm167.SetPWM(Device, int.Parse(channel), value, Math.Min(PwmFreq + 1, 3));
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

    private async Task ReadDevice()
    {
        if (_pending || Device < 0) return;
        _pending = true;

        var ioMode = await _vm167.ReadBackInOutMode(Device);
        DigitalLowOut = (ioMode & 1) == 0;
        DigitalLowIn = (ioMode & 1) > 0;
        DigitalHighOut = (ioMode & 2) == 0;
        DigitalHighIn = (ioMode & 2) > 0;

        var digital = await _vm167.ReadAllDigital(Device);
        Digital1 = (digital & 1) > 0;
        Digital2 = (digital & 2) > 0;
        Digital3 = (digital & 4) > 0;
        Digital4 = (digital & 8) > 0;
        Digital5 = (digital & 16) > 0;
        Digital6 = (digital & 32) > 0;
        Digital7 = (digital & 64) > 0;
        Digital8 = (digital & 128) > 0;

        var counter = await _vm167.ReadCounter(Device);
        Counter = counter;

        int[] analog = new int[Vm167.NumAnalogIn];
        await _vm167.ReadAllAnalog(Device, analog);
        AnalogIn1 = analog[0];
        AnalogIn2 = analog[1];
        AnalogIn3 = analog[2];
        AnalogIn4 = analog[3];
        AnalogIn5 = analog[4];

        int[] pwm = new int[Vm167.NumPwmOut];
        await _vm167.ReadBackPWMOut(Device, pwm);
        PwmOut1 = pwm[0];
        PwmOut2 = pwm[1];

        var timestamp = DateTime.Now - _startTime;
        if (_restartScope)
        {
            _startTime = DateTime.Now;
            timestamp = TimeSpan.Zero;
            ScopeModel.ResetAllAxes();
        }
        else if (_resetScope)
        {
            ScopeModel.ResetAllAxes();
        }

        // Update ScopeModel
        for (int i = 0; i < ScopeModel.Series.Count; i++)
        {
            var serie = (LineSeries)ScopeModel.Series[i];
            var points = serie.Points;
            if (_restartScope)
            {
                points.Clear();
            }

            var value = GetType().GetProperty(serie.Title)?.GetValue(this);
            if (value == null)
            {
                throw new ApplicationException($"Property {serie.Title} not found");
            }
            else
            {
                points.Add(new DataPoint(TimeSpanAxis.ToDouble(timestamp), Convert.ToInt32(value)));
            }
        }

        ScopeModel.InvalidatePlot(true);
        _restartScope = false;
        _resetScope = false;
        _pending = false;
    }
}