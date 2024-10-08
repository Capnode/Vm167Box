﻿using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using Vm167Box.ViewModels;
using Vm167Box.Views;
using SkiaSharp.Views.Maui.Controls.Hosting;
using OxyPlot.Maui.Skia;
using Vm167Lib;
using Vm167Box.Services;
using Vm167Box.Services.Internal;
using Vm167Box.Converters;

namespace Vm167Box;

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
        builder.Services.AddSingleton<IVm167Service, Vm167Service>();
        builder.Services.AddSingleton<ISettingsService, SettingsService>();

        // Pages
        builder.Services.AddSingleton<PanelPage>();
        builder.Services.AddSingleton<ScopePage>();
        builder.Services.AddSingleton<GeneratorPage>();
        builder.Services.AddSingleton<RegulatorPage>();
        builder.Services.AddSingleton<SettingsPage>();
        builder.Services.AddSingleton<AboutPage>();

        // ViewModels
        builder.Services.AddSingleton<PanelViewModel>();
        builder.Services.AddSingleton<GeneratorViewModel>();
        builder.Services.AddSingleton<RegulatorViewModel>();
        builder.Services.AddSingleton<SettingsViewModel>();
        builder.Services.AddSingleton<AboutViewModel>();

        // Converters
        builder.Services.AddSingleton<AnalogChannelToUnitIntervalConverter>();

#if DEBUG
        builder.Logging.AddDebug();
        builder.Logging.SetMinimumLevel(LogLevel.Trace);
        builder.Logging.AddFilter(nameof(Vm167Box), LogLevel.Debug);
        builder.Logging.AddFilter(nameof(Vm167Lib), LogLevel.Debug);
#endif

        return builder.Build();
    }
}