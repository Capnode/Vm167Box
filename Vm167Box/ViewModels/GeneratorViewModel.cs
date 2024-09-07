using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using OxyPlot;
using OxyPlot.Series;
using Vm167Box.Helpers;
using Vm167Box.Services;

namespace Vm167Box.ViewModels
{
    public enum GeneratorFunction
    {
        Off = 0,
        SquareWave = 1,
        TriangleWave = 2,
        SawtoothWave = 3,
        SineWave = 4,
    }

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
            _vm167Service.Tick += UpdateGenerator;
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
        private PlotModel _generator1Model = new();

        [ObservableProperty]
        private PlotModel _generator2Model = new();
        private bool _update1;
        private bool _update2;

        public IEnumerable<double> Periods => _periods;

        [RelayCommand]
        public async Task Generator1(EventArgs args)
        {
            using (await _lock.UseWaitAsync())
            {
                if (args is CheckedChangedEventArgs selected)
                {
                    _logger.LogTrace(">Generator1({}, {}, {})", selected.Value, Function1, Period1);
                    if (selected.Value)
                    {
                        var series = CreateSeries(Function1, Period1);
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
                    _logger.LogTrace(">Generator1({}, {})", Function1, Period1);
                    Generator1Model.Series.Clear();
                    var series = CreateSeries(Function1, Period1);
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
            using (await _lock.UseWaitAsync())
            {
                if (args is CheckedChangedEventArgs selected)
                {
                    _logger.LogTrace(">Generator2({}, {}, {})", selected.Value, Function2, Period2);
                    if (selected.Value)
                    {
                        var series = CreateSeries(Function2, Period2);
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
                    _logger.LogTrace(">Generator2({}, {})", Function2, Period2);
                    Generator2Model.Series.Clear();
                    var series = CreateSeries(Function2, Period2);
                    Generator2Model.Series.Add(series);
                    _update2 = true;
                }
            }

            Generator2Model.InvalidatePlot(true);
            _logger.LogTrace("<Generator2()");
        }

        private static FunctionSeries CreateSeries(GeneratorFunction function, double period)
        {
            switch (function)
            {
                case GeneratorFunction.SineWave:
                    return SineSeries(period);
                case GeneratorFunction.SquareWave:
                    return SquareSeries(period, 0.5);
                default:
                    return new FunctionSeries();
            }
        }

        private static FunctionSeries SineSeries(double period)
        {
            FunctionSeries series = new();
            var range = 2 * Math.PI;
            var step = range * IVm167Service.Period / (1000 * period);
            for (double i = 0; i <= range + epsilon; i += step)
            {
                var x = Math.Round(i * period / range, 7);
                var y = 255 * (1 + Math.Sin(i)) / 2;
                series.Points.Add(new DataPoint(x, y));
            }

            return series;
        }

        private static FunctionSeries SquareSeries(double period, double dutyCycle)
        {
            FunctionSeries series = new();
            var step = IVm167Service.Period / 1000d;
            var duty = dutyCycle * period;
            for (double i = 0; i <= period + epsilon; i += step)
            {
                var x = Math.Round(i, 7);
                var y = x < duty ? 255 : 0;
                series.Points.Add(new DataPoint(x, y));
            }

            return series;
        }

        private async Task UpdateGenerator()
        {
            IsOpen = true;
            using (await _lock.UseWaitAsync())
            {
                await UpdateGenerator1();
                await UpdateGenerator2();
            }
        }

        private async Task UpdateGenerator1()
        {
            if (!_update1 && _index1 < 0) return;

            int pwm = 0;
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
                    pwm = Convert.ToInt32(value);
                }
            }
            else
            {
                _index1 = -1;
            }

            await _vm167Service.SetPwm(1, pwm);
            _update1 = false;
        }

        private async Task UpdateGenerator2()
        {
            if (!_update2 && _index2 < 0) return;

            int pwm = 0;
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
                    pwm = Convert.ToInt32(value);
                }
            }
            else
            {
                _index2 = -1;
            }

            await _vm167Service.SetPwm(2, pwm);
            _update2 = false;
        }
    }
}
