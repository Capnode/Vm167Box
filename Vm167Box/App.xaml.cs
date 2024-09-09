using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Globalization;
using Vm167Box.Resources;
using Vm167Box.Services;
using Vm167Box.Views;

namespace Vm167Box;

public partial class App : Application
{
    private const double MinPageWidth = 400;

    private readonly ILogger<App> _logger;
    private readonly IVm167Service _vm167Service;
    private readonly ISettingsService _settingsService;

    public App(ILogger<App> logger, IVm167Service vm167Service, ISettingsService settingsService)
    {
        _logger = logger;
        _vm167Service = vm167Service;
        _settingsService = settingsService;

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

        UserAppTheme = _settingsService.AppTheme;
        MainPage = new AppShell();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        Window window = base.CreateWindow(activationState);
        window.Created += OnCreated;
        window.Destroying += OnDestroying;
        window.PropertyChanged += WindowChanged;

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

        SetShellFlyout(window.Width);
        return window;
    }

    private void WindowChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is not Window window) return;

        if (e.PropertyName == nameof(Window.X))
        {
            Preferences.Set("WindowX", window.X);
        }

        if (e.PropertyName == nameof(Window.Y))
        {
            Preferences.Set("WindowY", window.Y);
        }

        if (e.PropertyName == nameof(Window.Width))
        {
            Preferences.Set("WindowWidth", window.Width);

            // Make sure the flyout is visible when the window is resized
            var currentWidth = ((Window)sender).Width;
            SetShellFlyout(currentWidth);
        }

        if (e.PropertyName == nameof(Window.Height))
        {
            Preferences.Set("WindowHeight", window.Height);
        }
    }

    private static void SetShellFlyout(double currentWidth)
    {
        if (currentWidth > MinPageWidth + Shell.Current.FlyoutWidth)
        {
            Shell.Current.FlyoutBehavior = FlyoutBehavior.Locked;
            Shell.Current.FlyoutIsPresented = true;
        }
        else
        {
            Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;
            Shell.Current.FlyoutIsPresented = false;
        }
    }

    private void OnCreated(object? sender, EventArgs e)
    {
    }

    private void OnDestroying(object? sender, EventArgs e)
    {
        _vm167Service.CloseDevice();
    }
}
