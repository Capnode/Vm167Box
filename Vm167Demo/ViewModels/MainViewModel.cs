using CommunityToolkit.Mvvm.Input;
using Vm167Box;

namespace Vm167Demo.ViewModels;

public partial class MainViewModel(IVm167 vm167)
    : BaseViewModel
{
    [RelayCommand]
    public async Task OpenDevices()
    {
        await vm167.OpenDevices();
    }

    [RelayCommand]
    public async Task CloseDevices()
    {
        await vm167.CloseDevices();
    }
}