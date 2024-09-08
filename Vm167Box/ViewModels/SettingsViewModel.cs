using CommunityToolkit.Mvvm.ComponentModel;
using Vm167Box.Services;

namespace Vm167Box.ViewModels
{
    public class SettingsViewModel : ObservableObject
    {
        private readonly ISettingsService _settingsService;

        public SettingsViewModel(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public bool IsLightTheme
        {
            get => Application.Current?.RequestedTheme is AppTheme.Light;
            set
            {
                var appTheme = value ? AppTheme.Light : AppTheme.Dark;
                _settingsService.AppTheme = appTheme;
                if (Application.Current is null) return;
                Application.Current.UserAppTheme = appTheme;
                OnPropertyChanged();
            }
        }
    }
}
