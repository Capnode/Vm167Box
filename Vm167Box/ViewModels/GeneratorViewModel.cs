using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using OxyPlot;
using OxyPlot.Series;
using Vm167Box.Helpers;
using Vm167Box.Models;
using Vm167Box.Services;
using Vm167Lib;

namespace Vm167Box.ViewModels
{
    public enum GeneratorFunction {Off, SquareWave, TriangleWave, SineWave };

    public partial class GeneratorViewModel : ObservableObject
    {
        private const double epsilon = 1e-6;
        private static List<double> _periods = new () { 60, 30, 10, 5, 2, 1, 0.5, 0.2, 0.1 };

        private readonly ILogger<GeneratorViewModel> _logger;
        private readonly IVm167Service _vm167Service;
        private readonly SemaphoreSlim _lock = new(1, 1);
        private int _index1 = -1;
        private int _index2 = -1;

        public GeneratorViewModel(ILogger<GeneratorViewModel> logger, IVm167Service vm167Service)
        {
            _logger = logger;
            _vm167Service = vm167Service;
            _vm167Service.Tick += Loop;
            Generator1Model.Series.Add(new FunctionSeries());
            Generator2Model.Series.Add(new FunctionSeries());
        }

        [ObservableProperty]
        private bool _isOpen;

        [ObservableProperty]
        private GeneratorFunction _function1 = GeneratorFunction.Off;

        [ObservableProperty]
        private GeneratorFunction _function2 = GeneratorFunction.Off;

        [ObservableProperty]
        private double _period1 = 10;

        [ObservableProperty]
        private double _period2 = 10;

        [ObservableProperty]
        private int _dutyCycle1 = 50;

        [ObservableProperty]
        private int _dutyCycle2 = 50;

        [ObservableProperty]
        private PlotModel _generator1Model = new();

        [ObservableProperty]
        private PlotModel _generator2Model = new();
        private bool _update1;
        private bool _update2;

        public IEnumerable<double> Periods => _periods;

        [RelayCommand]
        public async Task Generator1(EventArgs args)
        {
            var Pwm1 = _vm167Service.Pwm1;
            using (await _lock.UseWaitAsync())
            {
                if (args is CheckedChangedEventArgs selected)
                {
                    _logger.LogTrace(">Generator1({}, {}, {}, {})", selected.Value, Function1, Period1, DutyCycle1);
                    if (selected.Value)
                    {
                        var series = CreateSeries(Pwm1, Function1, Period1, DutyCycle1);
                        Generator1Model.Series.Add(series);
                        _update1 = true;
                    }
                    else
                    {
                        Generator1Model.Series.Clear();
                        _update1 = true;
                    }
                }
                else
                {
                    _logger.LogTrace(">Generator1({}, {}, {})", Function1, Period1, DutyCycle1);
                    Generator1Model.Series.Clear();
                    var series = CreateSeries(Pwm1, Function1, Period1, DutyCycle1);
                    Generator1Model.Series.Add(series);
                    _update1 = true;
                }
            }

            Generator1Model.InvalidatePlot(true);
            _logger.LogTrace("<Generator1()");
        }

        [RelayCommand]
        public async Task Generator2(EventArgs args)
        {
            var Pwm2 = _vm167Service.Pwm2;
            using (await _lock.UseWaitAsync())
            {
                if (args is CheckedChangedEventArgs selected)
                {
                    _logger.LogTrace(">Generator2({}, {}, {}, {})", selected.Value, Function2, Period2, DutyCycle2);
                    if (selected.Value)
                    {
                        var series = CreateSeries(Pwm2, Function2, Period2, DutyCycle2);
                        Generator2Model.Series.Add(series);
                        _update2 = true;
                    }
                    else
                    {
                        Generator2Model.Series.Clear();
                        _update2 = true;
                    }
                }
                else
                {
                    _logger.LogTrace(">Generator2({}, {}, {})", Function2, Period2, DutyCycle2);
                    Generator2Model.Series.Clear();
                    var series = CreateSeries(Pwm2, Function2, Period2, DutyCycle2);
                    Generator2Model.Series.Add(series);
                    _update2 = true;
                }
            }

            Generator2Model.InvalidatePlot(true);
            _logger.LogTrace("<Generator2()");
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
                var y = t < period / 2 ? IVm167.PwmMax : 0;
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
                var y = t < half ? IVm167.PwmMax * t / half : IVm167.PwmMax * (1 - (t - half) / half);
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
                var y = IVm167.PwmMax * (1 + Math.Sin(t)) / 2;
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
                UpdateGenerator1();
                UpdateGenerator2();
            }
        }

        private void UpdateGenerator1()
        {
            if (!_update1 && _index1 < 0) return;

            double pwm = 0;
            if (Generator1Model.Series.FirstOrDefault() is FunctionSeries series)
            {
                var points = series.Points;
                var count = points.Count;
                if (count == 0)
                {
                    _index1 = -1;
                }
                else if (_update1 || _index1 >= count - 1)
                {
                    _index1 = 0;
                }

                if (0 <= _index1 && _index1 < count)
                {
                    var value = points[_index1++].Y;
                    pwm = value;
                }
            }
            else
            {
                _index1 = -1;
            }

            _vm167Service.Pwm1.Value =  pwm;
            _update1 = false;
        }

        private void UpdateGenerator2()
        {
            if (!_update2 && _index2 < 0) return;

            double pwm = 0;
            if (Generator2Model.Series.FirstOrDefault() is FunctionSeries series)
            {
                var points = series.Points;
                var count = points.Count;
                if (count == 0)
                {
                    _index2 = -1;
                }
                else if (_update2 || _index2 >= count - 1)
                {
                    _index2 = 0;
                }

                if (0 <= _index2 && _index2 < count)
                {
                    var value = points[_index2++].Y;
                    pwm = value;
                }
            }
            else
            {
                _index2 = -1;
            }

            _vm167Service.Pwm2.Value = pwm;
            _update2 = false;
        }
    }
}
