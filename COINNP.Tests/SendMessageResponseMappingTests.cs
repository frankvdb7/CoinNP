using COINNP.Client.Mapping;
using C = Coin.Sdk.NP.Messages.V3;

namespace COINNP.Tests;

[TestClass]
public class SendMessageResponseMappingTests
{
    [TestMethod]
    public void FromCOIN_MessageResponse_Maps_TransactionId_Without_Errors()
    {
        var response = new C.MessageResponse
        {
            TransactionId = "tx-123"
        };

        var result = response.FromCOIN();

        Assert.AreEqual("tx-123", result.TransactionId);
        Assert.IsFalse(result.HasErrors);
        Assert.IsNull(result.Errors);
    }

    [TestMethod]
    public void FromCOIN_ErrorResponse_Maps_TransactionId_And_Errors()
    {
        var response = new C.ErrorResponse
        {
            TransactionId = "tx-456",
            Errors = new()
            {
                new C.ErrorContent { Code = "E001", Message = "First error" },
                new C.ErrorContent { Code = "E002", Message = "Second error" }
            }
        };

        var result = response.FromCOIN();

        Assert.AreEqual("tx-456", result.TransactionId);
        Assert.IsTrue(result.HasErrors);
        Assert.IsNotNull(result.Errors);
        Assert.AreEqual(2, result.Errors.Count);
        Assert.AreEqual("E001", result.Errors[0].Code);
        Assert.AreEqual("First error", result.Errors[0].Message);
        Assert.AreEqual("E002", result.Errors[1].Code);
        Assert.AreEqual("Second error", result.Errors[1].Message);
    }

    [TestMethod]
    public void FromCOIN_ErrorResponse_With_Null_Errors_Maps_Without_Errors()
    {
        var response = new C.ErrorResponse
        {
            TransactionId = "tx-789",
            Errors = null
        };

        var result = response.FromCOIN();

        Assert.AreEqual("tx-789", result.TransactionId);
        Assert.IsFalse(result.HasErrors);
        Assert.IsNull(result.Errors);
    }
}
