using IdeapadToolkit.Services;
using IdeapadToolkit.ViewModels;
using IdeapadToolkit.Views;
using SimpleInjector;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Serilog;
using System.Windows;
using Serilog.Core;
using System.Globalization;
using System.Threading;
using IdeapadToolkit.Localization;
using IdeapadToolkit.Core.Services;

namespace IdeapadToolkit
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Container _container;
        private ILogger _logger;

        public void ConfigureServices(Container container)
        {
            // For later use
            var logLevel = new LoggingLevelSwitch();
            logLevel.MinimumLevel = Serilog.Events.LogEventLevel.Error;
            var loggerConfig = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(logLevel)
                .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7, levelSwitch: logLevel);
            var logger = _logger = loggerConfig.CreateLogger();

            container.RegisterInstance(logger);
            container.RegisterInstance(logLevel);

            container.RegisterSingleton<INavigationService, NavigationService>();
            container.RegisterSingleton<ILenovoPowerSettingsService, LenovoPowerSettingsService>();
            container.RegisterSingleton<IRunOnStartupService, RunOnStartupService>();
            container.RegisterSingleton<IAdministratorPermissionService, AdministratorPermissionService>();
            container.RegisterSingleton<TrayIconView>();

            container.Register<MainWindow>();

            container.Register<MainPage>();
            container.Register<SettingsPage>();

            container.Register<SettingsViewModel>();
            container.RegisterSingleton<LenovoSettingsViewModel>();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Container container = _container = new Container();
            ConfigureServices(container);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            bool exists = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(Environment.ProcessPath)).Length > 1;
            TrySetCulture();
            if (exists && !e.Args.Contains("ignoreRunning"))
            {
                MessageBox.Show(Strings.ALREADY_RUNNING, "", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                Application.Current.Shutdown();
                return;
            }
            else
            {
                base.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            }

            if (!File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PowerBattery.dll")))
            {
                MessageBox.Show(Strings.DLL_MISSING_ERROR, "", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                Application.Current.Shutdown();
            }
            if (!e.Args.Contains("nogui"))
            {
                ShowMainWindow(null, null);
            }

            var iconview = _container.GetInstance<TrayIconView>();
            iconview.MakeVisible();
            iconview.TrayIconClicked += ShowMainWindow;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _logger.Fatal((Exception)e.ExceptionObject, "Unhandled exception");
        }

        public void ShowMainWindow(object? sender, EventArgs? e)
        {
            if (App.Current.MainWindow == null)
            {
                TrySetCulture();
                App.Current.MainWindow = _container.GetInstance<MainWindow>();
            }
            App.Current.MainWindow.Show();
        }

        private void TrySetCulture()
        {
            try
            {
                var culture = Settings.Default.Language;
                if (!String.IsNullOrWhiteSpace(culture))
                {
                    var cultureInfo = CultureInfo.GetCultureInfo(culture);
                    if (cultureInfo != null)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            Thread.CurrentThread.CurrentUICulture = cultureInfo;
                        });
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}

