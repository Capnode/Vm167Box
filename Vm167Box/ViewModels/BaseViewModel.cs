using CommunityToolkit.Mvvm.ComponentModel;

namespace Vm167Box.ViewModels;

public partial class BaseViewModel() : ObservableObject
{
    [ObservableProperty]
    private string _title = string.Empty;
}
