using CommunityToolkit.Mvvm.ComponentModel;
using IdeapadToolkit.Core.Services;
using ModernWpf.Controls;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;

namespace IdeapadToolkit.ViewModels
{
    public record Locale(string Name, string CultureInfoCode);

    [INotifyPropertyChanged]
    public partial class SettingsViewModel
    {
        private readonly IRunOnStartupService _runOnStartupService;

        public SettingsViewModel(IRunOnStartupService runOnStartupService)
        {
            _runOnStartupService = runOnStartupService;
        }

        public Locale[] SupportedLanguages { get; init; } =
            [new Locale("English", "en-US"),
            new Locale("Čeština", "cs-CZ"),
            new Locale("Français", "fr-FR"),
            new Locale("简体中文", "zh-CN"),
            new Locale("Türkçe", "tr-TR")];

        public bool IsRunOnStartupEnabled
        {
            get => _runOnStartupService.IsRunOnStartupEnabled();
            set
            {
                try
                {
                    _runOnStartupService.ToggleRunOnStartup();
                }
                catch (Exception ex)
                {
                    var dialog = new ContentDialog
                    {
                        Title = "Error",
                        Content = ex.Message,
                        CloseButtonText = "Ok"
                    }.ShowAsync();
                }
            }
        }

        public Locale SelectedLocale
        {
            get
            {
                var setting = Settings.Default.Language;
                var locale = SupportedLanguages.FirstOrDefault(x => x.CultureInfoCode == setting);
                return locale ?? new Locale(String.Empty, String.Empty);
            }
            set
            {
                if (String.IsNullOrEmpty(value?.Name))
                    return;

                Settings.Default.Language = value.CultureInfoCode;
                var culture = CultureInfo.GetCultureInfo(value.CultureInfoCode);
                if (culture != null)
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Thread.CurrentThread.CurrentUICulture = culture;
                    });
                Settings.Default.Save();
            }
        }

        public bool KeepInTray
        {
            get => Settings.Default.KeepInTray;
            set
            {
                Settings.Default.KeepInTray = value;
                Settings.Default.Save();
            }
        }
    }
}
