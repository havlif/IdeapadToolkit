namespace IdeapadToolkit.Core.Models;

public sealed record DeviceProfile(
    string Manufacturer,
    string Model,
    string MachineType,
    bool IsSupported,
    DeviceCapabilities Capabilities)
{
    public static DeviceProfile Unknown(string manufacturer = "", string model = "", string machineType = "") =>
        new(manufacturer, model, machineType, false, new(false, false, false, false, false, false, false, false, false));
}
