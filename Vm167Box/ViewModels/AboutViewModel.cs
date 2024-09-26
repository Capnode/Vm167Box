using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Vm167Box.Services;

namespace Vm167Box.ViewModels;

public partial class AboutViewModel : ObservableObject
{
    private readonly ILogger<AboutViewModel> _logger;
    private readonly IVm167Service _vm167Service;

    public AboutViewModel(ILogger<AboutViewModel> logger, IVm167Service vm167service)
    {
        _logger = logger;
        _vm167Service = vm167service;

        OnConnected().ConfigureAwait(false);
        _vm167Service.Connected += OnConnected;
    }

    public string Title => AppInfo.Name;
    public string Version => AppInfo.VersionString;
    public string? Copyright => Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyCopyrightAttribute>()?.Copyright;
    public string? Author => Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyCompanyAttribute>()?.Company;
    public string Message => Resources.AppResources.AboutMessage;

    [ObservableProperty]
    private bool _isOpen;

    [ObservableProperty]
    private Version? _firmwareVersion;

    [ObservableProperty]
    private Version? _dllVersion;

    private async Task OnConnected()
    {
        IsOpen = _vm167Service.IsConnected;
        if (!IsOpen) return;

        _logger.LogTrace(">OnConnected()");
        var version = await _vm167Service.VersionFirmware();
        var FirmwareVersion = new Version(version >> 24, (version >> 16) & 0xFF, (version >> 8) & 0xFF, version & 0xFF);
        version = _vm167Service.VersionDLL();
        DllVersion = new Version(version >> 24, (version >> 16) & 0xFF, (version >> 8) & 0xFF, version & 0xFF);
        _logger.LogTrace("<OnConnected()");
    }
}
