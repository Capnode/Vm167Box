using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Vm167Box.Services;
using Vm167Lib;

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
        private int _pwmOut1;

        [ObservableProperty]
        private int _pwmOut2;

        [RelayCommand]
        public async Task PwmOut(string channel)
        {
            _logger.LogTrace(">PwmOut({channel})", channel);
            var value = channel switch
            {
                "1" => PwmOut1,
                "2" => PwmOut2,
                _ => 0
            };

            var pwmFrequency = _vm167Service.PWMFrequency;
            await _vm167Service.SetPWM(int.Parse(channel), value, pwmFrequency);
            _logger.LogTrace("<PwmOut({channel})", channel);
        }
    }
}
