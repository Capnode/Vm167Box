using Vm167Demo.ViewModels;

namespace Vm167Demo.Views;

public partial class PanelPage : ContentPage
{
    public PanelPage(MainViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
