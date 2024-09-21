using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using System.Runtime.InteropServices;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Vm167Box.WinUI
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : MauiWinUIApplication
    {
        private const uint EsContinous = 0x80000000;
        private const uint EsSystemRequired = 0x00000001;
        private const uint EsDisplayRequired = 0x00000002;

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern uint SetThreadExecutionState([In] uint esFlags);

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
            DispatcherQueue.GetForCurrentThread().ShutdownCompleted += OnShutdownCompleted;
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            base.OnLaunched(args);

            // Prevent going to sleep mode
            _ = SetThreadExecutionState(EsContinous | EsSystemRequired);
        }

        private void OnShutdownCompleted(DispatcherQueue sender, object args)
        {
            // Enable sleep mode
            _ = SetThreadExecutionState(EsContinous);
        }
    }
}
