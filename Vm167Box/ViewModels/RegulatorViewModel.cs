using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Vm167Box.Services;

namespace Vm167Box.ViewModels
{
    public partial class RegulatorViewModel : ObservableObject
    {
        private readonly ILogger<RegulatorViewModel> _logger;
        private readonly IVm167Service _vm167Service;
        private readonly ISettingsService _settingsService;

        public RegulatorViewModel(ILogger<RegulatorViewModel> logger, IVm167Service vm167Service, ISettingsService settingsService)
        {
            _logger = logger;
            _vm167Service = vm167Service;
            _settingsService = settingsService;

            _vm167Service.Tick += Loop;
        }

        [ObservableProperty]
        private bool _isOpen;

        [RelayCommand]
        public async Task Start()
        {
        }

        [RelayCommand]
        public async Task Stop()
        {
        }

        private async Task Loop()
        {
            IsOpen = true;
        }
    }
}
