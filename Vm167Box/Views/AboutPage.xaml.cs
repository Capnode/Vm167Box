using Vm167Box.ViewModels;

namespace Vm167Box.Views;

public partial class AboutPage : ContentPage
{
    public AboutPage(AboutViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}