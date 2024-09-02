using CommunityToolkit.Mvvm.ComponentModel;
using System.Reflection;

namespace Vm167Box.ViewModels;

public partial class AboutViewModel : ObservableObject
{
    public string Title => AppInfo.Name;
    public string Version => AppInfo.VersionString;
    public string? Copyright => Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyCopyrightAttribute>()?.Copyright;
    public string? Author => Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyCompanyAttribute>()?.Company;
    public string Message => Resources.AppResources.AboutMessage;

    public bool IsLightTheme
    {
        get => Application.Current?.RequestedTheme is AppTheme.Light;
        set
        {
            if (Application.Current is null) return;
            Application.Current.UserAppTheme = value ? AppTheme.Light : AppTheme.Dark;
            OnPropertyChanged();
        }
    }
}
