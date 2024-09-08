using Microsoft.Extensions.Logging;

namespace Vm167Box.Services.Internal;

internal class SettingsService : ISettingsService
{
    private readonly ILogger<SettingsService> _logger;
    private readonly IPreferences _settings = Preferences.Default;

    public SettingsService(ILogger<SettingsService> logger)
    {
        _logger = logger;
    }

    public AppTheme AppTheme
    {
        get => _settings.Get(nameof(AppTheme), Application.Current?.PlatformAppTheme ?? AppTheme.Unspecified);
        set => _settings.Set(nameof(AppTheme), (int)value);
    }
}
