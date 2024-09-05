using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Vm167Box.Services;

namespace Vm167Box.ViewModels
{
    public partial class GeneratorViewModel : ObservableObject
    {
        private readonly ILogger<GeneratorViewModel> _logger;
        private readonly IVm167Service _vm167Service;
        //private Timer? _timer;

        public GeneratorViewModel(ILogger<GeneratorViewModel> logger, IVm167Service vm167Service)
        {
            _logger = logger;
            _vm167Service = vm167Service;
        }

        [ObservableProperty]
        private Function _function1 = Function.Off;

        [ObservableProperty]
        private Function _function2 = Function.Off;

        [ObservableProperty]
        private double _frequency1 = 10;

        [ObservableProperty]
        private double _frequency2 = 10;

        [RelayCommand]
        public async Task Generator1(EventArgs args)
        {
            if (args is CheckedChangedEventArgs selected && !selected.Value) return;
            _logger.LogTrace(">Generator1({Function}, {Frequency})",Function1, Frequency1);
             await _vm167Service.Generator(1, Function1, Frequency1);
            _logger.LogTrace("<Generator1()");
        }

        [RelayCommand]
        public async Task Generator2(EventArgs args)
        {
            if (args is CheckedChangedEventArgs selected && !selected.Value) return;
            _logger.LogTrace(">Generator2({Function}, {Frequency})", Function2, Frequency2);
            await _vm167Service.Generator(2, Function2, Frequency2);
            _logger.LogTrace("<Generator2()");
        }
    }
}
