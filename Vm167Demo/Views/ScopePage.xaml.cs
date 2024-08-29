using Vm167Demo.ViewModels;

namespace Vm167Demo.Views;

public partial class ScopePage : ContentPage
{
    public ScopePage(MainViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}