using Vm167Demo.ViewModels;

namespace Vm167Demo.Views;

public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        viewModel.Title = "Main Page";
        Unloaded += (object? sender, EventArgs e) =>
        {
            var page = sender as ContentPage;
            var vm = page?.BindingContext as IDisposable;
            vm?.Dispose();
        };
    }
}
