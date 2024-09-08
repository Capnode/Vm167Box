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
}
