using IdeapadToolkit.WinUI3.Services;
using IdeapadToolkit.WinUI3.ViewModels;
using IdeapadToolkit.WinUI3.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using WinUIEx;

namespace IdeapadToolkit.WinUI3;
public sealed partial class MainWindow : WinUIEx.WindowEx
{
    private readonly INavigationService _navigationService;
    internal MainWindow(INavigationService navigationService)
    {
        this.InitializeComponent();
        this.ExtendsContentIntoTitleBar = true;
        NavigationView.SelectedItem = NavigationView.MenuItems[0];
        _navigationService = navigationService;
        _navigationService.NavFrame = RootFrame;
        _navigationService.Navigate<MainPage>(null, new EntranceNavigationTransitionInfo());
    }

    private void NavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        if (args?.IsSettingsInvoked == true)
        {
            _navigationService.Navigate<SettingsPage>(null, args.RecommendedNavigationTransitionInfo);
        }

        var page = args?.InvokedItemContainer?.Tag as string;
        if (page != null && args?.RecommendedNavigationTransitionInfo is { } transition)
        {
            switch (page)
            {
                case "Home":
                    _navigationService.Navigate<MainPage>(null, transition);
                    break;
            }
        }
    }

    private void Window_Closed(object sender, WindowEventArgs args)
    {
        if (!Settings.Instance.KeepInTray)
        {
            Environment.Exit(0);
        }
        else
        {
            App.MainWindow = null;
        }
    }
}
