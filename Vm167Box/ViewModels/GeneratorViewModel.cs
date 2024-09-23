using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using OxyPlot;
using OxyPlot.Series;
using Vm167Box.Helpers;
using Vm167Box.Models;
using Vm167Box.Services;

namespace Vm167Box.ViewModels
{
    public enum GeneratorFunction {Off, SquareWave, TriangleWave, SineWave };

    public partial class GeneratorViewModel : ObservableObject
    {
        private const double epsilon = 1e-6;
        private static List<double> _periods = new() { 0.1, 0.2, 0.5, 1, 2, 5, 10, 20, 50, 100, 200, 500, 1000, 2000, 5000 };

        private readonly ILogger<GeneratorViewModel> _logger;
        private readonly IVm167Service _vm167Service;
        private readonly ISettingsService _settingsService;
        private readonly SemaphoreSlim _lock = new(1, 1);

        private int _indexPwm1 = -1;
        private int _indexPwm2 = -1;
        private int _indexAnalog6 = -1;

        public GeneratorViewModel(ILogger<GeneratorViewModel> logger, IVm167Service vm167Service, ISettingsService settingsService)
        {
            _logger = logger;
            _vm167Service = vm167Service;
            _settingsService = settingsService;

            _vm167Service.Tick += Loop;
            ModelPwm1.Series.Add(new FunctionSeries());
            ModelPwm2.Series.Add(new FunctionSeries());
            ModelAnalog6.Series.Add(new FunctionSeries());
        }

        [ObservableProperty]
        private bool _isOpen;

        [ObservableProperty]
        private GeneratorFunction _functionPwm1 = GeneratorFunction.Off;

        [ObservableProperty]
        private GeneratorFunction _functionPwm2 = GeneratorFunction.Off;

        [ObservableProperty]
        private GeneratorFunction _functionAnalog6 = GeneratorFunction.Off;

        [ObservableProperty]
        private double _periodPwm1 = 10;

        [ObservableProperty]
        private double _periodPwm2 = 10;

        [ObservableProperty]
        private double _periodAnalog6 = 10;

        [ObservableProperty]
        private int _dutyCyclePwm1 = 50;

        [ObservableProperty]
        private int _dutyCyclePwm2 = 50;

        [ObservableProperty]
        private int _dutyCycleAnalog6 = 50;

        [ObservableProperty]
        private PlotModel _modelPwm1 = new();

        [ObservableProperty]
        private PlotModel _modelPwm2 = new();

        [ObservableProperty]
        private PlotModel _modelAnalog6 = new();

        private bool _updatePwm1;
        private bool _updatePwm2;
        private bool _updateAnalog6;

        public IEnumerable<double> Periods => _periods;

        [RelayCommand]
        public async Task GeneratorPwm1(EventArgs args)
        {
            var Pwm1 = _vm167Service.Pwm1;
            using (await _lock.UseWaitAsync())
            {
                if (args is CheckedChangedEventArgs selected)
                {
                    _logger.LogTrace(">GeneratorPwm1({}, {}, {}, {})", selected.Value, FunctionPwm1, PeriodPwm1, DutyCyclePwm1);
                    if (selected.Value)
                    {
                        var series = CreateSeries(Pwm1, FunctionPwm1, PeriodPwm1, DutyCyclePwm1);
                        ModelPwm1.Series.Add(series);
                        _updatePwm1 = true;
                    }
                    else
                    {
                        ModelPwm1.Series.Clear();
                        _updatePwm1 = true;
                    }
                }
                else
                {
                    _logger.LogTrace(">GeneratorPwm1({}, {}, {})", FunctionPwm1, PeriodPwm1, DutyCyclePwm1);
                    ModelPwm1.Series.Clear();
                    var series = CreateSeries(Pwm1, FunctionPwm1, PeriodPwm1, DutyCyclePwm1);
                    ModelPwm1.Series.Add(series);
                    _updatePwm1 = true;
                }
            }

            ModelPwm1.InvalidatePlot(true);
            _logger.LogTrace("<GeneratorPwm1()");
        }

        [RelayCommand]
        public async Task GeneratorPwm2(EventArgs args)
        {
            var Pwm2 = _vm167Service.Pwm2;
            using (await _lock.UseWaitAsync())
            {
                if (args is CheckedChangedEventArgs selected)
                {
                    _logger.LogTrace(">GeneratorPwm2({}, {}, {}, {})", selected.Value, FunctionPwm2, PeriodPwm2, DutyCyclePwm2);
                    if (selected.Value)
                    {
                        var series = CreateSeries(Pwm2, FunctionPwm2, PeriodPwm2, DutyCyclePwm2);
                        ModelPwm2.Series.Add(series);
                        _updatePwm2 = true;
                    }
                    else
                    {
                        ModelPwm2.Series.Clear();
                        _updatePwm2 = true;
                    }
                }
                else
                {
                    _logger.LogTrace(">GeneratorPwm2({}, {}, {})", FunctionPwm2, PeriodPwm2, DutyCyclePwm2);
                    ModelPwm2.Series.Clear();
                    var series = CreateSeries(Pwm2, FunctionPwm2, PeriodPwm2, DutyCyclePwm2);
                    ModelPwm2.Series.Add(series);
                    _updatePwm2 = true;
                }
            }

            ModelPwm2.InvalidatePlot(true);
            _logger.LogTrace("<GeneratorPwm2()");
        }

        [RelayCommand]
        public async Task GeneratorAnalog6(EventArgs args)
        {
            var Analog6 = _settingsService.Analog6;
            using (await _lock.UseWaitAsync())
            {
                if (args is CheckedChangedEventArgs selected)
                {
                    _logger.LogTrace(">GeneratorAnalog6({}, {}, {}, {})", selected.Value, FunctionAnalog6, PeriodAnalog6, DutyCycleAnalog6);
                    if (selected.Value)
                    {
                        var series = CreateSeries(Analog6, FunctionAnalog6, PeriodAnalog6, DutyCycleAnalog6);
                        ModelAnalog6.Series.Add(series);
                        _updateAnalog6 = true;
                    }
                    else
                    {
                        ModelAnalog6.Series.Clear();
                        _updateAnalog6 = true;
                    }
                }
                else
                {
                    _logger.LogTrace(">GeneratorAnalog6({}, {}, {})", FunctionAnalog6, PeriodAnalog6, DutyCycleAnalog6);
                    ModelAnalog6.Series.Clear();
                    var series = CreateSeries(Analog6, FunctionAnalog6, PeriodAnalog6, DutyCycleAnalog6);
                    ModelAnalog6.Series.Add(series);
                    _updateAnalog6 = true;
                }
            }

            ModelAnalog6.InvalidatePlot(true);
            _logger.LogTrace("<GeneratorAnalog6()");
        }

        private static FunctionSeries CreateSeries(AnalogChannel channel, GeneratorFunction function, double period, int dutyCycle)
        {
            switch (function)
            {
                case GeneratorFunction.SquareWave:
                    return SquareSeries(channel, period, dutyCycle);
                case GeneratorFunction.TriangleWave:
                    return TriangleSeries(channel, period, dutyCycle);
                case GeneratorFunction.SineWave:
                    return SineSeries(channel, period, dutyCycle);
                default:
                    return new FunctionSeries();
            }
        }

        private static FunctionSeries SquareSeries(AnalogChannel channel, double period, int dutyCycle)
        {
            FunctionSeries series = new();
            var step = IVm167Service.Period / 1000d;
            for (var i = 0d; i <= period + epsilon; i += step)
            {
                var x = Math.Round(i, 7);
                var t = Signal.Duty(x, period, dutyCycle);
                var y = t < period / 2 ? channel.MaxSignal : 0;
                channel.Signal = y;
                series.Points.Add(new DataPoint(x, channel.Value));
            }

            return series;
        }

        private static FunctionSeries TriangleSeries(AnalogChannel channel, double period, int dutyCycle)
        {
            FunctionSeries series = new();
            var step = IVm167Service.Period / 1000d;
            var half = period / 2;
            for (var i = 0d; i <= period + epsilon; i += step)
            {
                var x = Math.Round(i, 7);
                var t = Signal.Duty(x, period, dutyCycle);
                var y = t < half ? channel.MaxSignal * t / half : channel.MaxSignal * (1 - (t - half) / half);
                channel.Signal = Convert.ToInt32(y);
                series.Points.Add(new DataPoint(x, channel.Value));
            }

            return series;
        }

        private static FunctionSeries SineSeries(AnalogChannel channel, double period, int dutyCycle)
        {
            FunctionSeries series = new();
            var range = 2 * Math.PI;
            var step = range * IVm167Service.Period / (1000 * period);
            for (var i = 0d; i <= range + epsilon; i += step)
            {
                var x = Math.Round(i * period / range, 7);
                var t = Signal.Duty(i, range, dutyCycle);
                var y = channel.MaxSignal * (1 + Math.Sin(t)) / 2;
                channel.Signal = Convert.ToInt32(y);
                series.Points.Add(new DataPoint(x, channel.Value));
            }

            return series;
        }

        private async Task Loop()
        {
            IsOpen = true;
            using (await _lock.UseWaitAsync())
            {
                UpdatePwm1();
                UpdatePwm2();
                UpdateAnalog6();
            }
        }

        private void UpdatePwm1()
        {
            if (!_updatePwm1 && _indexPwm1 < 0) return;

            double value = 0;
            if (ModelPwm1.Series.FirstOrDefault() is FunctionSeries series)
            {
                var points = series.Points;
                var count = points.Count;
                if (count == 0)
                {
                    _indexPwm1 = -1;
                }
                else if (_updatePwm1 || _indexPwm1 >= count - 1)
                {
                    _indexPwm1 = 0;
                }

                if (0 <= _indexPwm1 && _indexPwm1 < count)
                {
                    value = points[_indexPwm1++].Y;
                }
            }
            else
            {
                _indexPwm1 = -1;
            }

            _vm167Service.Pwm1.Value =  value;
            _updatePwm1 = false;
        }

        private void UpdatePwm2()
        {
            if (!_updatePwm2 && _indexPwm2 < 0) return;

            double value = 0;
            if (ModelPwm2.Series.FirstOrDefault() is FunctionSeries series)
            {
                var points = series.Points;
                var count = points.Count;
                if (count == 0)
                {
                    _indexPwm2 = -1;
                }
                else if (_updatePwm2 || _indexPwm2 >= count - 1)
                {
                    _indexPwm2 = 0;
                }

                if (0 <= _indexPwm2 && _indexPwm2 < count)
                {
                    value = points[_indexPwm2++].Y;
                }
            }
            else
            {
                _indexPwm2 = -1;
            }

            _vm167Service.Pwm2.Value = value;
            _updatePwm2 = false;
        }

        private void UpdateAnalog6()
        {
            if (!_updateAnalog6 && _indexAnalog6 < 0) return;

            double value = 0;
            if (ModelAnalog6.Series.FirstOrDefault() is FunctionSeries series)
            {
                var points = series.Points;
                var count = points.Count;
                if (count == 0)
                {
                    _indexAnalog6 = -1;
                }
                else if (_updateAnalog6 || _indexAnalog6 >= count - 1)
                {
                    _indexAnalog6 = 0;
                }

                if (0 <= _indexAnalog6 && _indexAnalog6 < count)
                {
                    value = points[_indexAnalog6++].Y;
                }
            }
            else
            {
                _indexAnalog6 = -1;
            }

            _settingsService.Analog6.Value = value;
            _updateAnalog6 = false;
        }
    }
}
