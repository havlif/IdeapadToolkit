using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IdeapadToolkit.Core.Models;
using IdeapadToolkit.Core.Services;
using Serilog;
using System.Threading.Tasks;

namespace IdeapadToolkit.WinUI3.ViewModels;
internal partial class MainPageViewModel : ObservableObject
{
    private readonly ILenovoPowerSettingsService _lenovoPowerSettingsService;
    private readonly DeviceProfile _deviceProfile;
    private ILogger _logger;

    internal MainPageViewModel()
    {
        _lenovoPowerSettingsService = App.Composition.Resolve<ILenovoPowerSettingsService>();
        _deviceProfile = App.Composition.Resolve<IDeviceProfileService>().GetCurrentProfile();
        _logger = App.Composition.Resolve<ILogger>();
    }

    public bool IsSupportedDevice => _deviceProfile.IsSupported;
    public bool SupportsPowerProfile => _deviceProfile.Capabilities.SupportsPowerProfile;
    public bool SupportsChargingMode => _deviceProfile.Capabilities.SupportsNormalChargingMode;
    public bool SupportsAlwaysOnUsb => _deviceProfile.Capabilities.SupportsAlwaysOnUsb;
    public bool SupportsAlwaysOnUsbOnBattery => _deviceProfile.Capabilities.SupportsAlwaysOnUsbOnBattery;
    public string ErrorMessage { get; private set; } = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsEfficientChecked), nameof(IsIntelligentCoolingChecked), nameof(IsExtremePerformanceChecked), nameof(IconSource))]
    private PowerPlan _plan;
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsConservationModeEnabled), nameof(IsNormalModeEnabled), nameof(IsRapidModeEnabled), nameof(IconSource))]
    private ChargingMode _mode;
    public void Refresh()
    {
        try
        {
            Plan = _lenovoPowerSettingsService.GetPowerPlan();
            Mode = _lenovoPowerSettingsService.GetChargingMode();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            OnPropertyChanged(nameof(ErrorMessage));
            _logger.Error(ex, "Exception while fetching current settings");
        }
    }

    #region PowerPlanProperties
    public bool IsEfficientChecked
    {
        get => _plan == PowerPlan.EfficiencyMode;
        set
        {
        }
    }

    public bool IsIntelligentCoolingChecked
    {
        get => _plan == PowerPlan.IntelligentCooling;
        set
        {
        }
    }

    public bool IsExtremePerformanceChecked
    {
        get => _plan == PowerPlan.ExtremePerformance;
        set
        {
        }
    }
    #endregion

    #region OtherProperties
    public bool IsAlwaysOnUsbCombinedEnabled
    {
        get => IsAlwaysOnUsbEnabled && IsAlwaysOnUsbBatteryEnabled;
        set
        {
            try
            {
                if (value)
                {
                    _lenovoPowerSettingsService.SetAlwaysOnUsb(true);
                    _lenovoPowerSettingsService.SetAlwaysOnUsbBattery(true);
                }
                else
                {
                    _lenovoPowerSettingsService.SetAlwaysOnUsbBattery(false);
                    _lenovoPowerSettingsService.SetAlwaysOnUsb(false);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error while setting combined Always On USB setting");
            }

            OnPropertyChanged(nameof(IsAlwaysOnUsbCombinedEnabled));
        }
    }

    public bool IsAlwaysOnUsbEnabled
    {
        get
        {
            try
            {
                return _lenovoPowerSettingsService.IsAlwaysOnUsbEnabled();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error while getting Always on usb setting");
                return false;
            }
        }
        set
        {
            try
            {
                _lenovoPowerSettingsService.SetAlwaysOnUsb(value);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error while setting Always on usb setting");
            }
            OnPropertyChanged(nameof(IsAlwaysOnUsbEnabled));
        }
    }

    public bool IsAlwaysOnUsbBatteryEnabled
    {
        get
        {
            try
            {
                return _lenovoPowerSettingsService.IsAlwaysOnUsbBatteryEnabled();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error while getting Always on usb battery setting");
                return false;
            }
        }
        set
        {
            try
            {
                _lenovoPowerSettingsService.SetAlwaysOnUsbBattery(value);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error while setting Always on usb battery setting");
            }
        }
    }
    #endregion

    #region ChargingModeProperties
    public bool IsConservationModeEnabled
    {
        get => _mode == ChargingMode.Conservation;
        set
        {
        }
    }

    public bool IsNormalModeEnabled
    {
        get => _mode == ChargingMode.Normal;
        set
        {
        }
    }

    public bool IsRapidModeEnabled
    {
        get => _mode == ChargingMode.Rapid;
        set
        {
        }
    }

    #endregion

    public string IconSource
    {
        get
        {
            string path = (_plan, _mode) switch
            {
                (PowerPlan.IntelligentCooling, ChargingMode.Normal) => "ms-appx:///Resources/intelligent_normal.ico",
                (PowerPlan.IntelligentCooling, ChargingMode.Conservation) => "ms-appx:///Resources/intelligent_conservation.ico",
                (PowerPlan.IntelligentCooling, ChargingMode.Rapid) => "ms-appx:///Resources/intelligent_quick.ico",
                (PowerPlan.EfficiencyMode, ChargingMode.Normal) => "ms-appx:///Resources/saving_normal.ico",
                (PowerPlan.EfficiencyMode, ChargingMode.Conservation) => "ms-appx:///Resources/saving_conservation.ico",
                (PowerPlan.EfficiencyMode, ChargingMode.Rapid) => "ms-appx:///Resources/saving_quick.ico",
                (PowerPlan.ExtremePerformance, ChargingMode.Normal) => "ms-appx:///Resources/performance_normal.ico",
                (PowerPlan.ExtremePerformance, ChargingMode.Conservation) => "ms-appx:///Resources/performance_conservation.ico",
                (PowerPlan.ExtremePerformance, ChargingMode.Rapid) => "ms-appx:///Resources/performance_quick.ico",
                _ => "ms-appx:///Resources/intelligent_normal.ico"
            };
            return path;
        }
    }
    [RelayCommand]
    private void SetChargingMode(int? mode)
    {
        if (mode == null) return;
        try
        {
            _lenovoPowerSettingsService.SetChargingMode((ChargingMode)mode);
            Refresh();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            OnPropertyChanged(nameof(ErrorMessage));
            _logger.Error(ex, "Error while setting charging mode");
        }
    }

    [RelayCommand]
    private void SetPlan(int? plan)
    {
        if (plan == null) return;
        try
        {
            _lenovoPowerSettingsService.SetPowerPlan((PowerPlan)plan);
            Refresh();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            OnPropertyChanged(nameof(ErrorMessage));
            _logger.Error(ex, "Exception while setting power plan");
        }
    }
}

