using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Vm167Lib;

namespace Vm167Box.ViewModels
{
    public partial class GeneratorViewModel : ObservableObject
    {
        private readonly ILogger<GeneratorViewModel> _logger;
        private readonly IVm167 _vm167;
        //private Timer? _timer;

        public GeneratorViewModel(ILogger<GeneratorViewModel> logger, IVm167 vm167)
        {
            _logger = logger;
            _vm167 = vm167;
        }

        [ObservableProperty]
        private int _pwmOut1;

        [ObservableProperty]
        private int _pwmOut2;

        [ObservableProperty]
        private int _pwmFreq;

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

            await _vm167.SetPWM(Vm167.Device0, int.Parse(channel), value, Math.Min(PwmFreq + 1, 3));
            _logger.LogTrace("<PwmOut({channel})", channel);
        }
    }
}
