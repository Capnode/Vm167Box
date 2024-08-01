using Microsoft.Extensions.Logging;
using System.Globalization;
using Vm167Demo.Resources;
using Vm167Demo.Views;

namespace Vm167Demo;

public partial class App : Application
{
    private readonly ILogger<App> _logger;

    public App(ILogger<App> logger)
    {
        _logger = logger;
        InitializeComponent();

        MauiExceptions.UnhandledException += async (sender, args) =>
        {
            var ex = args.ExceptionObject as Exception;
            logger.LogError(ex, "Unhandled Exception:");
            if (MainPage is AppShell mainPage && ex != null)
            {
                await mainPage.ShowMessage(AppResources.UnhandledException, ex.Message);
            }
        };

        // Create working directory
        var wd = FileSystem.Current.AppDataDirectory;
        _logger.LogDebug("Working directory: {}", wd);
        if (!Directory.Exists(wd))
        {
            Directory.CreateDirectory(wd);
        }

        Directory.SetCurrentDirectory(wd);

        _logger.LogDebug("CurrentCulture: {}", CultureInfo.CurrentCulture);
        _logger.LogDebug("CurrentUICulture: {}", CultureInfo.CurrentUICulture);

        if (Current != null)
        {
            Current.UserAppTheme = Current.RequestedTheme;
        }

        MainPage = new AppShell();
        UserAppTheme = PlatformAppTheme;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        Window window = base.CreateWindow(activationState);
        window.Created += OnCreated;
        window.Destroying += OnDestroying;
        window.SizeChanged += SizeChanged;

        if (Preferences.ContainsKey("WindowX"))
        {
            window.X = Preferences.Get("WindowX", 0d);
        }

        if (Preferences.ContainsKey("WindowY"))
        {
            window.Y = Preferences.Get("WindowY", 0d);
        }

        if (Preferences.ContainsKey("WindowWidth"))
        {
            window.Width = Preferences.Get("WindowWidth", 0d);
        }

        if (Preferences.ContainsKey("WindowHeight"))
        {
            window.Height = Preferences.Get("WindowHeight", 0d);
        }

        return window;
    }

    private void SizeChanged(object? sender, EventArgs e)
    {
        if (sender is not Window window) return;
        Preferences.Set("WindowX", window.X);
        Preferences.Set("WindowY", window.Y);
        Preferences.Set("WindowWidth", window.Width);
        Preferences.Set("WindowHeight", window.Height);
    }

    private void OnCreated(object? sender, EventArgs e)
    {
    }

    private void OnDestroying(object? sender, EventArgs e)
    {
    }
}
