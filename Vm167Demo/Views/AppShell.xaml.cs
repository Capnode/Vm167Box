namespace Vm167Demo.Views;

/// <summary>
/// Note: For icons see: https://fontawesome.com/
/// </summary>

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
