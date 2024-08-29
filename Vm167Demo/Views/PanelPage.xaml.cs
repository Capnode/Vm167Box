using Vm167Demo.ViewModels;

namespace Vm167Demo.Views;

public partial class PanelPage : ContentPage
{
    public PanelPage(MainViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;

        Unloaded += (object? sender, EventArgs e) =>
        {
            var page = sender as ContentPage;
            var vm = page?.BindingContext as IDisposable;
            vm?.Dispose();
        };
    }
}
