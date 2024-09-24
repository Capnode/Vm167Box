using CommunityToolkit.Mvvm.ComponentModel;
using Vm167Box.Models;

namespace Vm167Box.ViewModels
{
    public partial class SafetyViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool _isSafe;

        [ObservableProperty]
        private AnalogChannel? _channel;
    }
}
