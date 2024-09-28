using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Series;
using Vm167Box.Helpers;
using Vm167Box.Models;
using Vm167Box.Services;

namespace Vm167Box.ViewModels;

public partial class RegulatorViewModel : ObservableObject
{
    private const double Period = IVm167Service.Period / 1000.0;
    private const int MaxPoints = 1000;

    private readonly ILogger<RegulatorViewModel> _logger;
    private readonly IVm167Service _vm167Service;
    private readonly ISettingsService _settingsService;
    private readonly ObservableRangeCollection<AnalogChannel> _inputs;
    private readonly ObservableRangeCollection<AnalogChannel> _outputs;
    private readonly ObservableRangeCollection<SafetyViewModel> _safety;
    private double _previousError;
    private double _integral;
    private bool _running;
    private bool _stopping;
    private bool _abort;
    private DateTime _startTime;
    private bool _restartScope = true;
    private bool _resetScope;
    private int _plotInterval = 1;
    private TimeSpan _lastPlot;
    private int _maxPoints = MaxPoints;

    public RegulatorViewModel(ILogger<RegulatorViewModel> logger, IVm167Service vm167Service, ISettingsService settingsService)
    {
        _logger = logger;
        _vm167Service = vm167Service;
        _settingsService = settingsService;
        _inputs = new ObservableRangeCollection<AnalogChannel>
        {
            _settingsService.Analog1,
            _settingsService.Analog2,
            _settingsService.Analog3,
            _settingsService.Analog4,
            _settingsService.Analog5,
            _settingsService.Analog6
        };

        _outputs = new ObservableRangeCollection<AnalogChannel>
        {
            _settingsService.Pwm1,
            _settingsService.Pwm2
        };

        _safety = new ObservableRangeCollection<SafetyViewModel>
        {
            new SafetyViewModel { Channel = _settingsService.Analog1 },
            new SafetyViewModel { Channel = _settingsService.Analog2 },
            new SafetyViewModel { Channel = _settingsService.Analog3 },
            new SafetyViewModel { Channel = _settingsService.Analog4 },
            new SafetyViewModel { Channel = _settingsService.Analog5 },
            new SafetyViewModel { Channel = _settingsService.Analog6 },
            new SafetyViewModel { Channel = _settingsService.Pwm1 },
            new SafetyViewModel { Channel = _settingsService.Pwm2 }
        };

        _referenceSignal = _inputs[0];
        _feedbackSignal = _inputs[0];
        _controlSignal = _outputs[0];

        ScopeModel = new PlotModel();
        ScopeModel.Legends.Add(new Legend { LegendPosition = LegendPosition.TopRight, LegendPlacement = LegendPlacement.Inside });

        OnUpdateSettings();
        SetStates();

        OnConnected().ConfigureAwait(false);
        _vm167Service.Connected += OnConnected;
        _vm167Service.Tick += OnTick;
        _settingsService.Update += OnUpdateSettings;
    }

    public ObservableRangeCollection<AnalogChannel> Inputs => _inputs;

    public ObservableRangeCollection<AnalogChannel> Outputs => _outputs;

    public ObservableRangeCollection<SafetyViewModel> Safety => _safety;

    [ObservableProperty]
    private bool _isOpen;

    [ObservableProperty]
    private bool _isSafetyStop;

    [ObservableProperty]
    private PlotModel _scopeModel;

    private AnalogChannel _referenceSignal;
    public AnalogChannel ReferenceSignal
    {
        get => _referenceSignal;
        set
        {
            if (SetProperty(ref _referenceSignal, value))
            {
                _settingsService.ReferenceIndex = _inputs.IndexOf(value);
            }
        }
    }

    private AnalogChannel _feedbackSignal;
    public AnalogChannel FeedbackSignal
    {
        get => _feedbackSignal;
        set
        {
            if (SetProperty(ref _feedbackSignal, value))
            {
                _settingsService.FeedbackIndex = _inputs.IndexOf(value);
            }
        }
    }

    private AnalogChannel _controlSignal;
    public AnalogChannel ControlSignal
    {
        get => _controlSignal;
        set
        {
            if (SetProperty(ref _controlSignal, value))
            {
                _settingsService.ControlIndex = _outputs.IndexOf(value);
            }
        }
    }

    private double _regulatorKp = 1;
    public double RegulatorKp
    {
        get => _regulatorKp;
        set
        {
            if (SetProperty(ref _regulatorKp, value))
            {
                _settingsService.RegulatorKp = value;
            }
        }
    }

    private double _regulatorKi = 0;
    public double RegulatorKi
    {
        get => _regulatorKi;
        set
        {
            if (SetProperty(ref _regulatorKi, value))
            {
                _settingsService.RegulatorKi = value;
            }
        }
    }

    private double _regulatorKd = 0;
    public double RegulatorKd
    {
        get => _regulatorKd;
        set
        {
            if (SetProperty(ref _regulatorKd, value))
            {
                _settingsService.RegulatorKd = value;
            }
        }
    }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(StartCommand))]
    private bool _canStart;

    [RelayCommand(CanExecute = nameof(CanStart))]
    public void Start()
    {
        _previousError = 0;
        _integral = 0;
        _abort = false;
        _stopping = false;
        _running = true;
        _restartScope = true;
        SetStates();
    }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(StopCommand))]
    private bool _canStop;

    [RelayCommand(CanExecute = nameof(CanStop))]
    private void Stop()
    {
        _stopping = true;
        _abort = false;
        SetStates();
    }

    [RelayCommand]
    public void ResetScope()
    {
        _resetScope = true;
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
    public void SafetyChanged(CheckedChangedEventArgs args)
    {
        string safety = string.Join("", _safety.Select(s => s.IsSafe ? '1' : '0'));
        _settingsService.Safety = safety;
    }

    private Task OnConnected()
    {
        IsOpen = _vm167Service.IsConnected;
        return Task.CompletedTask;
    }

    private Task OnTick()
    {
        if (!_running) return Task.CompletedTask;

        if (ReferenceSignal == null || FeedbackSignal == null || ControlSignal == null)
        {
            return Task.CompletedTask;
        }

        foreach (var safety in _safety)
        {
            if (!safety.IsSafe) continue;
            var channel = safety.Channel;
            if (channel == null) continue;
            if (channel.Value > channel.MaxValue)
            {
                _abort = true;
                ControlSignal.Value = 0;
                _logger.LogWarning("Safety: {} value {} > {}", channel.Name, channel.Value, channel.MaxValue);
                Threading.RunOnMainThread(SetStates);
                break;
            }
        }

        if (_stopping)
        {
            _running = false;
            _stopping = false;
            ControlSignal.Value = 0;
            Threading.RunOnMainThread(SetStates);
            return Task.CompletedTask;
        }

        // PID regulator logic here
        // Calculate the error
        double error = ReferenceSignal.Value - FeedbackSignal.Value;

        // Integral term
        _integral += error * Period;

        // Derivative term
        double derivative = (error - _previousError) / Period;

        // Calculate the control signal
        if (!_abort)
        {
            ControlSignal.Value = (RegulatorKp * error) + (RegulatorKi * _integral) + (RegulatorKd * derivative);
        }

        // Save the current error for the next loop
        _previousError = error;

        // Update scope
        var timestamp = DateTime.Now - _startTime;
        if (_restartScope)
        {
            _startTime = DateTime.Now;
            timestamp = TimeSpan.Zero;
            InitializeScope();
        }
        else if (_resetScope)
        {
            ScopeModel.ResetAllAxes();
        }

        // Update points
        if ((_plotInterval == 1) || (timestamp.Seconds != _lastPlot.Seconds))
        {
            var i = 0;
            AddPoint(i++, timestamp, ReferenceSignal.Value);
            AddPoint(i++, timestamp, FeedbackSignal.Value);
            AddPoint(i++, timestamp, ControlSignal.Value);
            ScopeModel.InvalidatePlot(true);
            _lastPlot = timestamp;
        }

        _restartScope = false;
        _resetScope = false;

        return Task.CompletedTask;
    }

    private void OnUpdateSettings()
    {
        Inputs.Refresh();
        Outputs.Refresh();

        var referenceIndex = _settingsService.ReferenceIndex;
        referenceIndex = referenceIndex >= 0 && referenceIndex < _inputs.Count ? referenceIndex : 0;
        ReferenceSignal = _inputs[referenceIndex];
        var feedbackIndex = _settingsService.FeedbackIndex;
        feedbackIndex = feedbackIndex >= 0 && feedbackIndex < _inputs.Count ? feedbackIndex : 0;
        FeedbackSignal = _inputs[feedbackIndex];
        var controlIndex = _settingsService.ControlIndex;
        controlIndex = controlIndex >= 0 && controlIndex < _outputs.Count ? controlIndex : 0;
        ControlSignal = _outputs[controlIndex];
        InitializeScope();

        RegulatorKp = _settingsService.RegulatorKp;
        RegulatorKi = _settingsService.RegulatorKi;
        RegulatorKd = _settingsService.RegulatorKd;

        var safety = _settingsService.Safety;
        var len = Math.Min(safety.Length, _safety.Count);
        for (int i = 0; i < len; i++)
        {
            _safety[i].IsSafe = safety[i] == '1';
        }
    }

    private void SetStates()
    {
        CanStart = !_stopping && !_running;
        CanStop = _running;
        IsSafetyStop = _abort;
    }

    private void InitializeScope()
    {
        ScopeModel.ResetAllAxes();
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

        AddSerie(ReferenceSignal.Name, ReferenceSignal.Unit, Resources.AppResources.AnalogIn, ReferenceSignal.MinValue, ReferenceSignal.MaxValue);
        AddSerie(FeedbackSignal.Name, FeedbackSignal.Unit, Resources.AppResources.AnalogIn, FeedbackSignal.MinValue, FeedbackSignal.MaxValue);
        AddSerie(ControlSignal.Name, ControlSignal.Unit, Resources.AppResources.AnalogIn, ControlSignal.MinValue, ControlSignal.MaxValue);

        ScopeModel.DefaultColors = OxyPalette.Interpolate(
            ScopeModel.Series.Count,
            OxyColors.Green,
            OxyColors.Blue,
            OxyColors.Red)
            .Colors;

        ScopeModel.InvalidatePlot(true);
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
