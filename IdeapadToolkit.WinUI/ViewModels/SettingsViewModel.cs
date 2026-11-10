using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using IdeapadToolkit.Core.Services;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Windows.Storage;

namespace IdeapadToolkit.WinUI3.ViewModels;

internal class Settings
{
    internal static Settings Instance { get; private set; } = Load();
    private Settings() { }

    private static string _path => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "IdeapadToolkit", "settings.txt");

    public string Language { get; set; } = "en-US";
    public bool KeepInTray { get; set; }

    public void Save()
    {
        File.WriteAllText(_path, $"{Language.ToString(CultureInfo.InvariantCulture)};{KeepInTray.ToString(CultureInfo.InvariantCulture)}");
    }

    private static Settings Load()
    {
        var settings = new Settings();
        try
        {
            string contents = File.ReadAllText(_path);
            var segments = contents.Split(";");
            if (segments.Length > 0)
            {
                settings.Language = segments[0];
            }
            if (segments.Length > 1)
            {
                if (bool.TryParse(segments[1], out var intray))
                {
                    settings.KeepInTray = intray;
                }
                else
                {
                    settings.KeepInTray = true;
                }
            }
        }
        catch
        {
            settings.Language = "en-US";
            settings.KeepInTray = true;
        }
        return settings;
    }
}

public record Locale(string Name, string CultureInfoCode);

internal partial class SettingsViewModel : ObservableObject
{
    private readonly IRunOnStartupService _runOnStartupService;

    public SettingsViewModel(IRunOnStartupService runOnStartupService)
    {
        _runOnStartupService = runOnStartupService;
    }

    public Locale[] SupportedLanguages
    {
        get; init;
    } =
        [new Locale("English", "en-US"),
            new Locale("Čeština", "cs-CZ"),
            new Locale("Français", "fr-FR"),
            new Locale("简体中文", "zh-CN")];

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
                    CloseButtonText = "Ok",
                    XamlRoot = App.MainWindow?.Content.XamlRoot
                }.ShowAsync();
            }
        }
    }

    public Locale SelectedLocale
    {
        get
        {
            var setting = Settings.Instance.Language;
            var locale = SupportedLanguages.FirstOrDefault(x => x.CultureInfoCode == setting);
            return locale ?? new Locale(String.Empty, String.Empty);
        }
        set
        {
            if (String.IsNullOrEmpty(value?.Name))
                return;

            Settings.Instance.Language = value.CultureInfoCode;
            var culture = CultureInfo.GetCultureInfo(value.CultureInfoCode);
            if (culture != null)
                DispatcherQueue.GetForCurrentThread().TryEnqueue(() =>
                {
                    Thread.CurrentThread.CurrentUICulture = culture;
                });
            Settings.Instance.Save();
        }
    }

    public bool KeepInTray
    {
        get => Settings.Instance.KeepInTray;
        set
        {
            Settings.Instance.KeepInTray = value;
            Settings.Instance.Save();
        }
    }
}
