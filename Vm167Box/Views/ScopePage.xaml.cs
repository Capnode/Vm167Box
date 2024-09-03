using OxyPlot;
using Vm167Box.ViewModels;

namespace Vm167Box.Views;

public partial class ScopePage : ContentPage
{
    public ScopePage(MainViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;

        if (Application.Current != null)
        {
            Application.Current.RequestedThemeChanged += OnRequestedThemeChanged;
        }
    }

    private void OnRequestedThemeChanged(object? sender, AppThemeChangedEventArgs e)
    {
        var model = _scopeView.Model;
        var foreground = e.RequestedTheme == AppTheme.Light
            ? OxyColor.FromRgb(32, 32, 32)
            : OxyColors.WhiteSmoke;

        if (model.TextColor != OxyColors.Transparent)
        {
            model.TextColor = foreground;
        }

        foreach (var axis in model.Axes)
        {
            if (axis.TicklineColor != OxyColors.Transparent)
            {
                axis.TicklineColor = foreground;
            }

            if (axis.AxislineColor != OxyColors.Transparent)
            {
                axis.AxislineColor = foreground;
            }
        }

        model.InvalidatePlot(false);
    }
}