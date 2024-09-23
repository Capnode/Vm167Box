using Vm167Box.ViewModels;

namespace Vm167Box.Views;

public partial class RegulatorPage : ContentPage
{
    public RegulatorPage(RegulatorViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
