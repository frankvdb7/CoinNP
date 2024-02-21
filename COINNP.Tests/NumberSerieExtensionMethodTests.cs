using COINNP.Entities.Common;

namespace COINNP.Tests;

[TestClass]
public class NumberSerieExtensionMethodTests
{
    [TestMethod]
    public void NumberSerie_ToFriendlyString_Returns_Range()
    {
        var target = new NumberSerie("01012345670", "01012345679"); // From and To are different
        Assert.AreEqual("01012345670-01012345679", target.ToFriendlyString());
    }
    [TestMethod]
    public void NumberSerie_ToFriendlyString_Returns_SingleNumber()
    {
        var target = new NumberSerie("01012345670", "01012345670"); // From and To are same
        Assert.AreEqual("01012345670", target.ToFriendlyString());
    }
}
