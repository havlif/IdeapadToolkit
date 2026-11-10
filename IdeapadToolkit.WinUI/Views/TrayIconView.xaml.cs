using CommunityToolkit.Mvvm.Input;
using IdeapadToolkit.Core.Models;
using IdeapadToolkit.WinUI3.Localization;
using IdeapadToolkit.WinUI3.ViewModels;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;

namespace IdeapadToolkit.WinUI3.Views;

public sealed partial class TrayIconView : Page
{
    public TrayIconView()
    {
        InitializeComponent();
        ViewModel = App.Composition.Resolve<MainPageViewModel>();
    }

    internal MainPageViewModel ViewModel
    {
        get;
    }

    public event EventHandler? TrayIconClicked;

    [RelayCommand]
    private void OnTrayIconClicked()
    {
        TrayIconClicked?.Invoke(this, EventArgs.Empty);
    }

    public void MakeVisible()
    {
        TrayIcon.ForceCreate();
        TrayIcon.Visibility = Visibility.Visible;
        TrayIcon.LeftClickCommand = TrayIconClickedCommand;
        ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        RefreshBindings();
    }

    private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        TrayIcon.IconSource = new BitmapImage(new Uri(ViewModel.IconSource));
    }

    private void MenuItemExit_Click(object sender, RoutedEventArgs e)
    {
        Environment.Exit(0);
    }

    private void RefreshBindings()
    {
        ViewModel.Refresh();

        RadioConservation.Text = Strings.CONSERVATION;
        RadioConservation.Command = ViewModel.SetChargingModeCommand;
        RadioConservation.CommandParameter = (int)ChargingMode.Conservation;
        RadioConservation.Background = ViewModel.IsConservationModeEnabled ? new SolidColorBrush(Colors.DarkGreen) : new SolidColorBrush(Colors.Transparent);

        RadioNormal.Text = Strings.NORMAL;
        RadioNormal.Command = ViewModel.SetChargingModeCommand;
        RadioNormal.CommandParameter = (int)ChargingMode.Normal;
        RadioNormal.Background = ViewModel.IsNormalModeEnabled ? new SolidColorBrush(Colors.DarkBlue) : new SolidColorBrush(Colors.Transparent);

        RadioRapid.Text = Strings.RAPID;
        RadioRapid.Command = ViewModel.SetChargingModeCommand;
        RadioRapid.CommandParameter = (int)ChargingMode.Rapid;
        RadioRapid.Background = ViewModel.IsRapidModeEnabled ? new SolidColorBrush(Colors.DarkRed) : new SolidColorBrush(Colors.Transparent);

        RadioBatterySaving.Text = Strings.BATTERY_SAVING;
        RadioBatterySaving.Command = ViewModel.SetPlanCommand;
        RadioBatterySaving.CommandParameter = (int)PowerPlan.EfficiencyMode;
        RadioBatterySaving.Background = ViewModel.IsEfficientChecked ? new SolidColorBrush(Colors.DarkGreen) : new SolidColorBrush(Colors.Transparent);

        RadioIntelligent.Text = Strings.INTELLIGENT_COOLING;
        RadioIntelligent.Command = ViewModel.SetPlanCommand;
        RadioIntelligent.CommandParameter = (int)PowerPlan.IntelligentCooling;
        RadioIntelligent.Background = ViewModel.IsIntelligentCoolingChecked ? new SolidColorBrush(Colors.DarkBlue) : new SolidColorBrush(Colors.Transparent);

        RadioExtreme.Text = Strings.EXTREME_PERFORMANCE;
        RadioExtreme.Command = ViewModel.SetPlanCommand;
        RadioExtreme.CommandParameter = (int)PowerPlan.ExtremePerformance;
        RadioExtreme.Background = ViewModel.IsExtremePerformanceChecked ? new SolidColorBrush(Colors.DarkRed) : new SolidColorBrush(Colors.Transparent);
    }

    private void RadioConservation_Loaded(object sender, RoutedEventArgs e)
    {
        RefreshBindings();
    }
}
