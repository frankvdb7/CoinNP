using COINNP.Client.Mapping;
using COINNP.Entities.Enums;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace COINNP.Client.Tests;

[TestClass]
public class ValueHelperParseTests
{
    [TestMethod]
    public void ParseBool_CaseSensitive()
    {
        var target = new ValueHelper(Options.Create(ValueHelperOptions.Default with { IgnoreCase = false }));

        Assert.AreEqual(true, target.ParseBool("Y"));
        Assert.AreEqual(false, target.ParseBool("N"));
    }

    [TestMethod]
    public void ParseBool_CaseInsensitive()
    {
        var target = new ValueHelper(Options.Create(ValueHelperOptions.Default with { IgnoreCase = true }));

        Assert.AreEqual(true, target.ParseBool("Y"));
        Assert.AreEqual(false, target.ParseBool("N"));

        Assert.AreEqual(true, target.ParseBool("y"));
        Assert.AreEqual(false, target.ParseBool("n"));
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ParseBool_ThrowsOnUnknownValue()
    {
        var target = new ValueHelper(Options.Create(ValueHelperOptions.Default with { IgnoreCase = false }));

        target.ParseBool("x");
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ParseBool_ThrowsOnInvalidCase()
    {
        var target = new ValueHelper(Options.Create(ValueHelperOptions.Default with { IgnoreCase = false }));

        target.ParseBool("y");
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ParseBool_ThrowsOnEmpty()
    {
        var target = new ValueHelper(Options.Create(ValueHelperOptions.Default with { IgnoreCase = false }));

        target.ParseBool(string.Empty);
    }

    [TestMethod]
    public void ParseNullableBool_CaseSensitive()
    {
        var target = new ValueHelper(Options.Create(ValueHelperOptions.Default with { IgnoreCase = false }));

        Assert.AreEqual(true, target.ParseNullableBool("Y"));
        Assert.AreEqual(false, target.ParseNullableBool("N"));

        Assert.IsNull(target.ParseNullableBool(string.Empty));
        Assert.IsNull(target.ParseNullableBool(null));
    }

    [TestMethod]
    public void ParseNullableBool_CaseInsensitive()
    {
        var target = new ValueHelper(Options.Create(ValueHelperOptions.Default with { IgnoreCase = true }));

        Assert.AreEqual(true, target.ParseNullableBool("Y"));
        Assert.AreEqual(false, target.ParseNullableBool("N"));

        Assert.AreEqual(true, target.ParseNullableBool("y"));
        Assert.AreEqual(false, target.ParseNullableBool("n"));

        Assert.IsNull(target.ParseNullableBool(string.Empty));
        Assert.IsNull(target.ParseNullableBool(null));
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ParseNullableBoolThrowsOnUnknownValue()
    {
        var target = new ValueHelper(Options.Create(ValueHelperOptions.Default with { IgnoreCase = false }));

        target.ParseBool("x");
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ParseNullableBoolThrowsOnInvalidCase()
    {
        var target = new ValueHelper(Options.Create(ValueHelperOptions.Default with { IgnoreCase = false }));

        target.ParseBool("y");
    }

    [TestMethod]
    public void ParseBool_CustomValues()
    {
        var target = new ValueHelper(Options.Create(ValueHelperOptions.Default with { TrueValue = "Oui", FalseValue = "Non", IgnoreCase = true }));

        Assert.AreEqual(true, target.ParseBool("Oui"));
        Assert.AreEqual(false, target.ParseBool("Non"));
    }


    [TestMethod]
    public void ParseContractState_CaseSensitive()
    {
        var target = new ValueHelper(Options.Create(ValueHelperOptions.Default with { IgnoreCase = false }));

        Assert.AreEqual(ContractState.EarlyTermination, target.ParseContractState("EARLY_TERMINATION"));
        Assert.AreEqual(ContractState.Continuation, target.ParseContractState("CONTINUATION"));

        Assert.IsNull(target.ParseContractState(string.Empty));
        Assert.IsNull(target.ParseContractState(null));
    }


    [TestMethod]
    public void ParseContractState_CaseInsensitive()
    {
        var target = new ValueHelper(Options.Create(ValueHelperOptions.Default with { IgnoreCase = true }));

        Assert.AreEqual(ContractState.EarlyTermination, target.ParseContractState("EARLY_TERMINATION"));
        Assert.AreEqual(ContractState.EarlyTermination, target.ParseContractState("early_termination"));
        Assert.AreEqual(ContractState.Continuation, target.ParseContractState("CONTINUATION"));
        Assert.AreEqual(ContractState.Continuation, target.ParseContractState("continuation"));

        Assert.IsNull(target.ParseContractState(string.Empty));
        Assert.IsNull(target.ParseContractState(null));
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ParseContractState_ThrowsOnUnknownValue()
    {
        var target = new ValueHelper(Options.Create(ValueHelperOptions.Default with { IgnoreCase = false }));

        target.ParseContractState("x");
    }

    [TestMethod]
    public void ParseCurrency()
    {
        var target = new ValueHelper(Options.Create(ValueHelperOptions.Default));
        Assert.AreEqual(12.3456m, target.ParseCurrency("12,3456"));
    }

    [TestMethod]
    [ExpectedException(typeof(FormatException))]
    public void ParseCurrency_ThrowsOnInvalidDecimalSeparator()
    {
        var target = new ValueHelper(Options.Create(ValueHelperOptions.Default));
        target.ParseCurrency("12.3456");
    }

    [TestMethod]
    [ExpectedException(typeof(FormatException))]
    public void ParseCurrency_ThrowsOnInvalidValue()
    {
        var target = new ValueHelper(Options.Create(ValueHelperOptions.Default));
        Assert.AreEqual(12.3456m, target.ParseCurrency("x"));
    }

    [TestMethod]
    public void ParseCurrency_WithCustomFormat()
    {
        var target = new ValueHelper(Options.Create(ValueHelperOptions.Default with { CurrencyFormatInfo = new NumberFormatInfo { CurrencyDecimalSeparator = "G" } }));
        Assert.AreEqual(12.3456m, target.ParseCurrency("12G3456"));
    }

    [TestMethod]
    public void ParseCurrencyType()
    {
        var target = new ValueHelper(Options.Create(ValueHelperOptions.Default));
        Assert.AreEqual(Currency.Euro, target.ParseCurrencyType("1"));
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ParseCurrencyType_ThrowsOnInvalidValue()
    {
        var target = new ValueHelper(Options.Create(ValueHelperOptions.Default));
        target.ParseCurrencyType("x");
    }

    [TestMethod]
    public void ParseDateTimeOffset()
    {
        var target = new ValueHelper(Options.Create(ValueHelperOptions.Default));
        Assert.AreEqual(new DateTimeOffset(2023, 4, 24, 16, 19, 57, TimeSpan.FromHours(2)), target.ParseDateTimeOffset("20230424161957"));
    }

    [TestMethod]
    [ExpectedException(typeof(FormatException))]
    public void ParseDateTimeOffset_ThrowsOnInvalidValue()
    {
        var target = new ValueHelper(Options.Create(ValueHelperOptions.Default));
        target.ParseDateTimeOffset("x");
    }

    [TestMethod]
    public void ParseNullableDateTimeOffset()
    {
        var target = new ValueHelper(Options.Create(ValueHelperOptions.Default));
        Assert.AreEqual(new DateTimeOffset(2023, 4, 24, 16, 19, 57, TimeSpan.FromHours(2)), target.ParseNullableDateTimeOffset("20230424161957"));
        Assert.IsNull(target.ParseNullableDateTimeOffset(string.Empty));
        Assert.IsNull(target.ParseNullableDateTimeOffset(null));
    }

    [TestMethod]
    [ExpectedException(typeof(FormatException))]
    public void ParseNullableDateTimeOffset_ThrowsOnInvalidValue()
    {
        var target = new ValueHelper(Options.Create(ValueHelperOptions.Default));
        target.ParseNullableDateTimeOffset("x");
    }

    [TestMethod]
    public void ParseDateTimeOffset_WithCustomFormat()
    {
        var target = new ValueHelper(Options.Create(ValueHelperOptions.Default with { DateTimeFormatInfo = new DateTimeFormatInfo { FullDateTimePattern = "yyyy-MM-dd HH:mm:ss.fff" } }));
        Assert.AreEqual(new DateTimeOffset(2023, 4, 24, 16, 19, 57, 123, TimeSpan.FromHours(2)), target.ParseDateTimeOffset("2023-04-24 16:19:57.123"));
    }

    [TestMethod]
    public void ParseNullableInt()
    {
        var target = new ValueHelper(Options.Create(ValueHelperOptions.Default));
        Assert.AreEqual(123, target.ParseNullableInt("123"));
        Assert.IsNull(target.ParseNullableInt(string.Empty));
        Assert.IsNull(target.ParseNullableInt(null));
    }

    [TestMethod]
    [ExpectedException(typeof(FormatException))]
    public void ParseNullableInt_ThrowsOnInvalidValue()
    {
        var target = new ValueHelper(Options.Create(ValueHelperOptions.Default));
        target.ParseNullableInt("x");
    }

    [TestMethod]
    public void ParseTariffType()
    {
        var target = new ValueHelper(Options.Create(ValueHelperOptions.Default));
        Assert.AreEqual(TariffType.Minutes, target.ParseTariffType("0"));
        Assert.AreEqual(TariffType.Call, target.ParseTariffType("1"));
        Assert.AreEqual(TariffType.Miaco067, target.ParseTariffType("2"));
        Assert.AreEqual(TariffType.Friaco067, target.ParseTariffType("3"));
        Assert.AreEqual(TariffType.PerMinuteNoSetupCost, target.ParseTariffType("4"));
        Assert.AreEqual(TariffType.PerMinuteWithSetupCost, target.ParseTariffType("5"));
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ParseTariffType_ThrowsOnInvalidValue()
    {
        var target = new ValueHelper(Options.Create(ValueHelperOptions.Default));
        target.ParseCurrencyType("x");
    }

    [TestMethod]
    public void ParseTimespan()
    {
        var target = new ValueHelper(Options.Create(ValueHelperOptions.Default));
        Assert.AreEqual(TimeSpan.FromSeconds(123), target.ParseTimeSpan("123"));
    }

    [TestMethod]
    [ExpectedException(typeof(FormatException))]
    public void ParseTimespan_ThrowsOnInvalidValue()
    {
        var target = new ValueHelper(Options.Create(ValueHelperOptions.Default));
        target.ParseTimeSpan("x");
    }

    [TestMethod]
    public void ParseTypeOfNumber()
    {
        var target = new ValueHelper(Options.Create(ValueHelperOptions.Default));
        Assert.AreEqual(TypeOfNumber.Fixed, target.ParseTypeOfNumber("0"));
        Assert.AreEqual(TypeOfNumber.Mobile, target.ParseTypeOfNumber("1"));
        Assert.AreEqual(TypeOfNumber.Service, target.ParseTypeOfNumber("2"));
        Assert.AreEqual(TypeOfNumber.M2M, target.ParseTypeOfNumber("3"));
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ParseTypeOfNumber_ThrowsOnInvalidValue()
    {
        var target = new ValueHelper(Options.Create(ValueHelperOptions.Default));
        target.ParseTypeOfNumber("x");
    }

    [TestMethod]
    public void ParseVAT()
    {
        var target = new ValueHelper(Options.Create(ValueHelperOptions.Default));
        Assert.AreEqual(VAT.EuropeanTaxExemption, target.ParseVAT("0"));
        Assert.AreEqual(VAT.FreeTaxExemption, target.ParseVAT("1"));
        Assert.AreEqual(VAT.Low, target.ParseVAT("2"));
        Assert.AreEqual(VAT.High, target.ParseVAT("3"));
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ParseVAT_ThrowsOnInvalidValue()
    {
        var target = new ValueHelper(Options.Create(ValueHelperOptions.Default));
        target.ParseVAT("x");
    }
}