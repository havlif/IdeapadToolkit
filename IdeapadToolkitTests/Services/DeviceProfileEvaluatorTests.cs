using IdeapadToolkit.Core.Models;
using IdeapadToolkit.Core.Services;

namespace IdeapadToolkitTests.Services;

[TestClass]
public class DeviceProfileEvaluatorTests
{
    [TestMethod]
    public void S340_81NB_IsSupportedWithoutFlipToBoot()
    {
        var profile = DeviceProfileEvaluator.Create("LENOVO", "IdeaPad S340-14API", "81NB");
        Assert.IsTrue(profile.IsSupported);
        Assert.IsTrue(profile.Capabilities.SupportsPowerProfile);
        Assert.IsFalse(profile.Capabilities.SupportsFlipToBoot);
    }

    [TestMethod]
    public void UnknownDeviceHasNoCapabilities()
    {
        var profile = DeviceProfileEvaluator.Create("Other", "Laptop", "");
        Assert.IsFalse(profile.IsSupported);
        Assert.IsFalse(profile.Capabilities.SupportsPowerProfile);
    }
}
