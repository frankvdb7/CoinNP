using COINNP.Entities.Common;

namespace COINNP.Tests;

[TestClass]
public class INetworkOperatorAndServiceProviderExtensionMethodsTests
{
    [TestMethod]
    public void ToCOINCode_Returns_Correct_COINCode()
    {
        Assert.AreEqual("TEST-FOO", new Sender("TEST", "FOO").ToCOINCode());
        Assert.AreEqual("TEST", new Sender("TEST", null).ToCOINCode());
    }

    [TestMethod]
    public void ToCOINCode_Uses_Given_Separator()
    {
        Assert.AreEqual("TEST:FOO", new Receiver("TEST", "FOO").ToCOINCode(":"));
        Assert.AreEqual("TEST", new Receiver("TEST", null).ToCOINCode(":"));
    }
}