namespace Vm167Demo.Views;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
    }

    internal async Task ShowMessage(string title, string message)
    {
        await DisplayAlert(title, message, "OK");
    }
}
