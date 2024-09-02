using Vm167Box.ViewModels;

namespace Vm167Box.Views;

public partial class ScopePage : ContentPage
{
    public ScopePage(MainViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}