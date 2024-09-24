using Vm167Box.ViewModels;

namespace Vm167Box.Views;

public partial class RegulatorPage : ContentPage
{
    private RegulatorViewModel _vm;

    public RegulatorPage(RegulatorViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = vm;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _vm.Save();
    }
}
