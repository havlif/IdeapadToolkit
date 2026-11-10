namespace IdeapadToolkit.Core.Models;

public sealed record DeviceCapabilities(
    bool SupportsPowerProfile,
    bool SupportsConservationMode,
    bool SupportsNormalChargingMode,
    bool SupportsRapidCharge,
    bool SupportsAlwaysOnUsb,
    bool SupportsAlwaysOnUsbOnBattery,
    bool SupportsThermalProfile,
    bool SupportsManualFanControl,
    bool SupportsFlipToBoot);
