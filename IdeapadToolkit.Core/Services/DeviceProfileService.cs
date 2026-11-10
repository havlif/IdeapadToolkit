using IdeapadToolkit.Core.Models;
using Microsoft.Win32;
using System.Runtime.Versioning;

namespace IdeapadToolkit.Core.Services;

public sealed class DeviceProfileService : IDeviceProfileService
{
    [SupportedOSPlatform("windows")]
    public DeviceProfile GetCurrentProfile()
    {
        var manufacturer = Read("SystemManufacturer");
        var model = Read("SystemProductName");
        var machineType = Read("SystemSKU");
        return DeviceProfileEvaluator.Create(manufacturer, model, machineType);
    }

    [SupportedOSPlatform("windows")]
    private static string Read(string name) =>
        Registry.GetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\SystemInformation", name, "")?.ToString() ?? "";
}

public static class DeviceProfileEvaluator
{
    public static DeviceProfile Create(string? manufacturer, string? model, string? machineType)
    {
        manufacturer ??= "";
        model ??= "";
        machineType ??= "";
        var supported = manufacturer.Contains("Lenovo", StringComparison.OrdinalIgnoreCase)
            && (model.Contains("S340-14API", StringComparison.OrdinalIgnoreCase)
                || machineType.Contains("81NB", StringComparison.OrdinalIgnoreCase));
        if (!supported) return DeviceProfile.Unknown(manufacturer, model, machineType);

        return new DeviceProfile(manufacturer, model, machineType, true,
            new(true, true, true, true, true, true, false, false, false));
    }
}
