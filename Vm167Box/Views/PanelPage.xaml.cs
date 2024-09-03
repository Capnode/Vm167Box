using Vm167Box.ViewModels;

namespace Vm167Box.Views;

public partial class PanelPage : ContentPage
{
    public PanelPage(PanelViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
