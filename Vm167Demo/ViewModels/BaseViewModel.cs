using CommunityToolkit.Mvvm.ComponentModel;

namespace Vm167Demo.ViewModels
{
    public partial class BaseViewModel()
        : ObservableObject
    {
        [ObservableProperty]
        private string _title = string.Empty;
    }
}
