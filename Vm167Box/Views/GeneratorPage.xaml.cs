using OxyPlot;
using Vm167Box.ViewModels;

namespace Vm167Box.Views;

public partial class GeneratorPage : ContentPage
{
    public GeneratorPage(GeneratorViewModel viewModel)
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
        var foreground = e.RequestedTheme == AppTheme.Light
            ? OxyColor.FromRgb(32, 32, 32)
            : OxyColors.WhiteSmoke;

        SetForeground(_plotPwm1.Model, foreground);
        SetForeground(_plotPwm2.Model, foreground);
        SetForeground(_plotAnalog6.Model, foreground);
    }

    private static void SetForeground(PlotModel model, OxyColor foreground)
    {
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