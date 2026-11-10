global using System;
using IdeapadToolkit.Core.Services;
using IdeapadToolkit.WinUI3.Helpers;
using IdeapadToolkit.WinUI3.Localization;
using IdeapadToolkit.WinUI3.Services;
using IdeapadToolkit.WinUI3.ViewModels;
using IdeapadToolkit.WinUI3.Views;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Pure.DI;
using Serilog;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using WinUIEx;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace IdeapadToolkit.WinUI3;
/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        this.InitializeComponent();
    }

    private void ConfigureServices()
    {
        DI.Setup("Composition", CompositionKind.Public)
            .RootBind<ILogger>().As(Lifetime.Singleton).To(_ =>
            {
                return new LoggerConfiguration()
                    .MinimumLevel.Error()
                    .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7)
                    .CreateLogger();
            })
            .RootBind<ILenovoPowerSettingsService>().As(Lifetime.Singleton).To<LenovoPowerSettingsService>()
            .RootBind<IDeviceProfileService>().As(Lifetime.Singleton).To<DeviceProfileService>()
            .Bind().As(Lifetime.Singleton).To<TrayIconView>().Root<TrayIconView>()
            .Bind<IRunOnStartupService>().As(Lifetime.Singleton).To<RunOnStartupService>()
            .Bind().As(Lifetime.Singleton).To<MainPageViewModel>().Root<MainPageViewModel>()
            .Bind().To<SettingsViewModel>().Root<SettingsViewModel>()
            .Bind().To<MainPage>().Root<MainPage>()
            .RootBind<INavigationService>().As(Lifetime.Singleton).To<NavigationService>()
            .Bind().As(Lifetime.Transient).To<MainWindow>()
            .Root<MainWindow>();
    }

    /// <summary>
    /// Invoked when the application is launched.
    /// </summary>
    /// <param name="args">Details about the launch request and process.</param>
    /// 

    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        ConfigureServices();
        Composition = new Composition();

        var arguments = Environment.GetCommandLineArgs();

        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

        bool exists = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(Environment.ProcessPath)).Length > 1;
        TrySetCulture();
        if (exists && (arguments?.Any(x => x.Contains("ignoreRunning")) != true))
        {
            Win32.MessageBox(IntPtr.Zero, Strings.ALREADY_RUNNING, "", Win32.MB_OK | Win32.MB_ICONASTERISK);
            Environment.Exit(1);
            return;
        }
        else
        {
            base.DispatcherShutdownMode = DispatcherShutdownMode.OnExplicitShutdown;
        }

        if (!File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PowerBattery.dll")))
        {
            Win32.MessageBox(IntPtr.Zero, Strings.DLL_MISSING_ERROR, "", Win32.MB_OK | Win32.MB_ICONASTERISK);
            Environment.Exit(1);
            return;
        }
        if (arguments?.Any(x => x.Contains("nogui")) != true)
        {
            ShowMainWindow(null, EventArgs.Empty);
        }

        var iconView = Composition.Resolve<TrayIconView>();
        iconView.MakeVisible();
        iconView.TrayIconClicked += ShowMainWindow;
    }

    private void CurrentDomain_UnhandledException(object sender, System.UnhandledExceptionEventArgs e)
    {
        Composition.Resolve<ILogger>().Fatal((Exception)e.ExceptionObject, "Unhandled exception");
    }

    private void ShowMainWindow(object? sender, EventArgs args)
    {
        if (MainWindow == null)
        {
            TrySetCulture();
            var window = Composition.Resolve<MainWindow>();
            MainWindow = window;
            window.Activate();
        }
        else
        {
            MainWindow.Activate();
        }
    }

    private void TrySetCulture()
    {
        try
        {
            var culture = Settings.Instance.Language;
            if (!String.IsNullOrWhiteSpace(culture))
            {
                var cultureInfo = CultureInfo.GetCultureInfo(culture);
                if (cultureInfo != null)
                {
                    DispatcherQueue.GetForCurrentThread().TryEnqueue(() =>
                    {
                        Thread.CurrentThread.CurrentUICulture = cultureInfo;
                    });
                }
            }
        }
        catch (Exception ex)
        {
            Composition.Resolve<ILogger>().Warning(ex, "Unable to apply saved culture");
        }
    }

    private void MenuItemExit_Click(object sender, RoutedEventArgs e)
    {
        Environment.Exit(0);
    }

    internal static MainWindow? MainWindow = null!;

    internal static Composition Composition = null!;
}

