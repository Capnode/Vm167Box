using Vm167Box.ViewModels;

namespace Vm167Box.Views;

public partial class GeneratorPage : ContentPage
{
	public GeneratorPage(GeneratorViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}