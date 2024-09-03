using Microsoft.Extensions.Logging;
using System.Globalization;
using Vm167Box.Resources;
using Vm167Box.ViewModels;
using Vm167Box.Views;

namespace Vm167Box;

public partial class App : Application
{
    private const double MinPageWidth = 400;

    private readonly ILogger<App> _logger;
    private readonly MainViewModel _vm;

    public App(ILogger<App> logger, MainViewModel viewModel)
    {
        _logger = logger;
        _vm = viewModel;

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

        UserAppTheme = PlatformAppTheme;
        MainPage = new AppShell();
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

        SetShellFlyout(window.Width);
        return window;
    }

    private void SizeChanged(object? sender, EventArgs e)
    {
        if (sender is not Window window) return;
        Preferences.Set("WindowX", window.X);
        Preferences.Set("WindowY", window.Y);
        Preferences.Set("WindowWidth", window.Width);
        Preferences.Set("WindowHeight", window.Height);

        // Make sure the flyout is visible when the window is resized
        var currentWidth = ((Window)sender).Width;
        SetShellFlyout(currentWidth);
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
        _vm.Dispose();
    }
}
