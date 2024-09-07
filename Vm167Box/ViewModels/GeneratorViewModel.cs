using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using OxyPlot;
using OxyPlot.Series;
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
        private readonly ILogger<GeneratorViewModel> _logger;
        private readonly IVm167Service _vm167Service;
        private readonly SemaphoreSlim _lock = new(1, 1);
        private int _index1 = -1;
        private int _index2 = -1;

        public GeneratorViewModel(ILogger<GeneratorViewModel> logger, IVm167Service vm167Service)
        {
            _logger = logger;
            _vm167Service = vm167Service;
        }

        [ObservableProperty]
        private GeneratorFunction _function1 = GeneratorFunction.Off;

        [ObservableProperty]
        private GeneratorFunction _function2 = GeneratorFunction.Off;

        [ObservableProperty]
        private double _frequency1 = 0.1;

        [ObservableProperty]
        private double _frequency2 = 0.1;

        [ObservableProperty]
        private PlotModel _generator1Model = new();

        [ObservableProperty]
        private PlotModel _generator2Model = new();
        private bool _update1;
        private bool _update2;

        [RelayCommand]
        public async Task Generator1(EventArgs args)
        {
            try
            {
                await _lock.WaitAsync();

                if (args is CheckedChangedEventArgs selected && !selected.Value)
                {
                    _logger.LogTrace(">Generator1({Function}, {Frequency}) Off", Function1, Frequency1);
                    Generator1Model.Series.Clear();
                    _update1 = true;
                }
                else
                {
                    _logger.LogTrace(">Generator1({Function}, {Frequency}) On", Function1, Frequency1);
                    var series = GeneratorViewModel.CreateSeries(Function1, Frequency1);
                    if (series != null)
                    {
                        Generator1Model.Series.Add(series);
                        _update1 = true;
                    }
                }
            }
            finally
            {
                _lock.Release();
            }

            Generator1Model.InvalidatePlot(true);
            _logger.LogTrace("<Generator1()");
        }

        [RelayCommand]
        public async Task Generator2(EventArgs args)
        {
            try
            {
                await _lock.WaitAsync();

                if (args is CheckedChangedEventArgs selected && !selected.Value)
                {
                    _logger.LogTrace(">Generator2({Function}, {Frequency}) Off", Function2, Frequency2);
                    Generator2Model.Series.Clear();
                    _update2 = true;
                }
                else
                {
                    _logger.LogTrace(">Generator2({Function}, {Frequency}) On", Function2, Frequency2);
                    var series = GeneratorViewModel.CreateSeries(Function2, Frequency2);
                    if (series != null)
                    {
                        Generator2Model.Series.Add(series);
                        _update2 = true;
                    }
                }
            }
            finally
            {
                _lock.Release();
            }

            Generator2Model.InvalidatePlot(true);
            _logger.LogTrace("<Generator2()");
        }

        private static FunctionSeries? CreateSeries(GeneratorFunction function, double frequency)
        {
            FunctionSeries series = new();
            switch (function)
            {
                case GeneratorFunction.SineWave:
                    var range = 2 * Math.PI;
                    var step = range * frequency * IVm167Service.Period / 1000;
                    for (double i = 0; i < range; i += step)
                    {
                        var y = 255 * (1 + Math.Sin(i)) / 2;
                        series.Points.Add(new DataPoint(i, y));
                    }
                    break;
            }

            return series;
        }

        private async Task UpdateGenerator()
        {
            try
            {
                await _lock.WaitAsync();
                await UpdateGenerator1();
                await UpdateGenerator2();
            }
            finally
            {
                _lock.Release();
            }
        }

        private async Task UpdateGenerator1()
        {
            if (_update1 || _index1 >= 0)
            {
                int pwm = 0;
                if (Generator1Model.Series.FirstOrDefault() is not FunctionSeries series)
                {
                    _index1 = -1;
                }
                else
                {
                    var points = series.Points;
                    var count = points.Count;
                    if (_index1 < 0 || _index1 >= count)
                    {
                        _index1 = 0;
                    }

                    if (_index1 < count)
                    {
                        var value = points[_index1++].Y;
                        pwm = Convert.ToInt32(value);
                    }
                }

                await _vm167Service.SetPwm(1, pwm);
            }

            _update1 = false;
        }

        private async Task UpdateGenerator2()
        {
            if (_update2 || _index2 >= 0)
            {
                int pwm = 0;
                var series = Generator2Model.Series.FirstOrDefault() as FunctionSeries;
                if (series == null)
                {
                    _index2 = -1;
                }
                else
                {
                    var points = series.Points;
                    var count = points.Count;
                    if (_index2 < 0 || _index2 >= count)
                    {
                        _index2 = 0;
                    }

                    if (_index2 < count)
                    {
                        var value = points[_index2++].Y;
                        pwm = Convert.ToInt32(value);
                    }
                }

                await _vm167Service.SetPwm(2, pwm);
            }

            _update2 = false;
        }
    }
}
