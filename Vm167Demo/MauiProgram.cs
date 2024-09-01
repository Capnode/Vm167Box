﻿using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using Vm167Box;
using Vm167Demo.ViewModels;
using Vm167Demo.Views;
using SkiaSharp.Views.Maui.Controls.Hosting;
using OxyPlot.Maui.Skia;

namespace Vm167Demo;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseSkiaSharp()
            .UseOxyPlotSkia()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("fa-regular-400.ttf", "FontAwesomeRegular");
                fonts.AddFont("fa-solid-900.ttf", "FontAwesomeSolid");
                fonts.AddFont("fa-brands-400.ttf", "FontAwesomeBrands");
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Services
        builder.Services.AddSingleton<IVm167, Vm167>();

        // Pages
        builder.Services.AddSingleton<PanelPage>();
        builder.Services.AddSingleton<ScopePage>();

        // ViewModels
        builder.Services.AddSingleton<MainViewModel>();

#if DEBUG
        builder.Logging.AddDebug();
        builder.Logging.SetMinimumLevel(LogLevel.Trace);
        builder.Logging.AddFilter("Vm167Box", LogLevel.Debug);
#endif

        return builder.Build();
    }
}