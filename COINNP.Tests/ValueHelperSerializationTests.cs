using COINNP.Client.Mapping;
using COINNP.Entities.Enums;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace COINNP.Client.Tests;

[TestClass]
public class ValueHelperSerializationTests
{
    [TestMethod]
    public void SerializeBool()
    {
        var target = new ValueHelper(Options.Create(ValueHelperOptions.Default));

        Assert.AreEqual("Y", target.SerializeBool(true));
        Assert.AreEqual("N", target.SerializeBool(false));
    }

    [TestMethod]
    public void SerializeBool_WithCustomValues()
    {
        var target = new ValueHelper(Options.Create(ValueHelperOptions.Default with { TrueValue = "Oui", FalseValue = "Non" }));

        Assert.AreEqual("Oui", target.SerializeBool(true));
        Assert.AreEqual("Non", target.SerializeBool(false));
    }

    [TestMethod]
    public void SerializeNullableBool()
    {
        var target = new ValueHelper(Options.Create(ValueHelperOptions.Default));

        Assert.AreEqual("Y", target.SerializeNullableBool(true));
        Assert.AreEqual("N", target.SerializeNullableBool(false));
        Assert.AreEqual(null, target.SerializeNullableBool(null));
    }

    [TestMethod]
    public void SerializeContractState()
    {
        var target = new ValueHelper(Options.Create(ValueHelperOptions.Default));

        Assert.AreEqual("CONTINUATION", target.SerializeContractState(ContractState.Continuation));
        Assert.AreEqual("EARLY_TERMINATION", target.SerializeContractState(ContractState.EarlyTermination));
        Assert.AreEqual(null, target.SerializeContractState(null));
    }

    [TestMethod]
    public void SerializeCurrency()
    {
        var target = new ValueHelper(Options.Create(ValueHelperOptions.Default));

        Assert.AreEqual("12,34568", target.SerializeCurrency(12.3456798m));
    }

    [TestMethod]
    public void SerializeCurrency_WithCustomFormat()
    {
        var target = new ValueHelper(Options.Create(ValueHelperOptions.Default with { CurrencyFormatInfo = new NumberFormatInfo { CurrencyDecimalSeparator = ".", CurrencyDecimalDigits = 3, CurrencySymbol = "ƒ" } }));

        Assert.AreEqual("ƒ12.346", target.SerializeCurrency(12.3456798m));
    }

    [TestMethod]
    public void SerializeCurrencyType()
    {
        var target = new ValueHelper(Options.Create(ValueHelperOptions.Default));

        Assert.AreEqual("1", target.SerializeCurrencyType(Currency.Euro));
    }

    [TestMethod]
    public void SerializeDateTimeOffset()
    {
        var target = new ValueHelper(Options.Create(ValueHelperOptions.Default));

        Assert.AreEqual("20230424161957", target.SerializeDateTimeOffset(new DateTimeOffset(2023, 4, 24, 16, 19, 57, 123, TimeSpan.FromHours(2)))); //NL timezone
        Assert.AreEqual("20230424161957", target.SerializeDateTimeOffset(new DateTimeOffset(2023, 4, 24, 6, 19, 57, 123, TimeSpan.FromHours(-8)))); //Alaska timezone
    }

    [TestMethod]
    public void SerializeNullableDateTimeOffset()
    {
        var target = new ValueHelper(Options.Create(ValueHelperOptions.Default));

        Assert.AreEqual("20230424161957", target.SerializeNullableDateTimeOffset(new DateTimeOffset(2023, 4, 24, 16, 19, 57, 123, TimeSpan.FromHours(2)))); //NL timezone
        Assert.AreEqual("20230424161957", target.SerializeNullableDateTimeOffset(new DateTimeOffset(2023, 4, 24, 6, 19, 57, 123, TimeSpan.FromHours(-8)))); //Alaska timezone
        Assert.IsNull(target.SerializeNullableDateTimeOffset(null));
    }

    [TestMethod]
    public void SerializeDateTimeOffset_WithCustomFormat()
    {
        var target = new ValueHelper(Options.Create(ValueHelperOptions.Default with { DateTimeFormatInfo = new DateTimeFormatInfo { FullDateTimePattern = "yyyy-MM-dd HH:mm:ss.fff" } }));

        Assert.AreEqual("2023-04-24 16:19:57.123", target.SerializeDateTimeOffset(new DateTimeOffset(2023, 4, 24, 16, 19, 57, 123, TimeSpan.FromHours(2)))); //NL timezone
        Assert.AreEqual("2023-04-24 16:19:57.123", target.SerializeDateTimeOffset(new DateTimeOffset(2023, 4, 24, 6, 19, 57, 123, TimeSpan.FromHours(-8)))); //Alaska timezone
    }

    [TestMethod]
    public void SerializeNullableInt()
    {
        var target = new ValueHelper(Options.Create(ValueHelperOptions.Default));

        Assert.AreEqual("123456789", target.SerializeNullableInt(123456789));
        Assert.IsNull(target.SerializeNullableInt(null));
    }

    [TestMethod]
    public void SerializeTariffType()
    {
        var target = new ValueHelper(Options.Create(ValueHelperOptions.Default));

        Assert.AreEqual("0", target.SerializeTariffType(TariffType.Minutes));
        Assert.AreEqual("1", target.SerializeTariffType(TariffType.Call));
        Assert.AreEqual("2", target.SerializeTariffType(TariffType.Miaco067));
        Assert.AreEqual("3", target.SerializeTariffType(TariffType.Friaco067));
        Assert.AreEqual("4", target.SerializeTariffType(TariffType.PerMinuteNoSetupCost));
        Assert.AreEqual("5", target.SerializeTariffType(TariffType.PerMinuteWithSetupCost));
    }

    [TestMethod]
    public void SerializeTimeSpan()
    {
        var target = new ValueHelper(Options.Create(ValueHelperOptions.Default));

        Assert.AreEqual("123", target.SerializeTimeSpan(TimeSpan.FromMilliseconds(123456)));
    }

    [TestMethod]
    public void SerializeTypeOfNumber()
    {
        var target = new ValueHelper(Options.Create(ValueHelperOptions.Default));

        Assert.AreEqual("0", target.SerializeTypeOfNumber(TypeOfNumber.Fixed));
        Assert.AreEqual("1", target.SerializeTypeOfNumber(TypeOfNumber.Mobile));
        Assert.AreEqual("2", target.SerializeTypeOfNumber(TypeOfNumber.Service));
        Assert.AreEqual("3", target.SerializeTypeOfNumber(TypeOfNumber.M2M));
    }

    [TestMethod]
    public void SerializeVAT()
    {
        var target = new ValueHelper(Options.Create(ValueHelperOptions.Default));

        Assert.AreEqual("0", target.SerializeVAT(VAT.EuropeanTaxExemption));
        Assert.AreEqual("1", target.SerializeVAT(VAT.FreeTaxExemption));
        Assert.AreEqual("2", target.SerializeVAT(VAT.Low));
        Assert.AreEqual("3", target.SerializeVAT(VAT.High));
    }
}