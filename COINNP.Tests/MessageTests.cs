using COINNP.Client.Mapping;
using COINNP.Entities;
using COINNP.Entities.Common;
using COINNP.Entities.Enums;
using COINNP.Entities.Messages;
using COINNP.Entities.Messages.Attributes;
using COINNP.Entities.SequenceItems;
using System.Reflection;

namespace COINNP.Tests;

[TestClass]
public class MessageTests
{
    private static readonly DateTimeOffset _testdate = new(2023, 9, 13, 12, 38, 33, TimeSpan.FromHours(2));
    private static readonly Header _testheader = new(new Receiver("ANO", "ASP"), new Sender("BNO", "BSP"), _testdate);

    private static readonly string _testdossierid = "FOO-BAR-12345";
    private static readonly string _testrecepientno = "RNO";
    private static readonly string _testdonorno = "DNO";
    private static readonly string _testdonorsp = "DSP";
    private static readonly string _testplatformprovider = "PP";
    private static readonly string _testoptanr = "123ABC456DEF";
    private static readonly string _testpop = "POP";

    private static readonly NumberSerie[] _testnumbers = new[] { "0101234567", "0612345678", "0201234560-69" }.Select(CreateNumberSerie).ToArray();
    private static readonly NumberSerie[] _testservicenumbers = new[] { "08000001", "09000001", "09060100-99" }.Select(CreateNumberSerie).ToArray();
    private static readonly TariffInfo _testtariff = new(1.23m, 4.56m, Currency.Euro, TariffType.PerMinuteNoSetupCost, VAT.High);

    private static readonly MessageEnvelope _activationservicenumber = new(_testheader, new ActivationServiceNumber(_testdossierid, _testplatformprovider, _testdate, null, _testservicenumbers.Select(n => new ActivationServiceNumberItem(n, _testtariff, _testpop))));
    private static readonly MessageEnvelope _cancel = new(_testheader, new Cancel(_testdossierid, null));
    private static readonly MessageEnvelope _deactivation = new(_testheader, new Deactivation(_testdossierid, _testdonorno, _testrecepientno, _testnumbers.Select(n => new DeactivationItem(n))));
    private static readonly MessageEnvelope _deactivationservicenumber = new(_testheader, new DeactivationServiceNumber(_testdossierid, _testplatformprovider, _testdate, _testservicenumbers.Select(n => new DeactivationServiceNumberItem(n, _testpop))));
    private static readonly MessageEnvelope _portingperformed = new(_testheader, new PortingPerformed(_testdossierid, _testrecepientno, _testdonorno, null, null, _testnumbers.Select(n => new PortingPerformedItem(n))));
    private static readonly MessageEnvelope _portingrequest = new(_testheader, new PortingRequest(_testdossierid, _testrecepientno, null, null, null, null, null, null, _testnumbers.Select(n => new PortingRequestItem(n))));
    private static readonly MessageEnvelope _portingrequestanswer = new(_testheader, new PortingRequestAnswer(_testdossierid, null, _testnumbers.Select(n => new PortingRequestAnswerItem(n))));
    private static readonly MessageEnvelope _portingrequestanswerdelayed = new(_testheader, new PortingRequestAnswerDelayed(_testdossierid, _testdonorno, _testdonorsp));
    private static readonly MessageEnvelope _rangeactivation = new(_testheader, new RangeActivation(_testdossierid, _testdonorno, _testoptanr, _testdate, _testnumbers.Select(n => new RangeItem(n))));
    private static readonly MessageEnvelope _rangedeactivation = new(_testheader, new RangeDeactivation(_testdossierid, _testdonorno, _testoptanr, _testdate, _testnumbers.Select(n => new RangeItem(n))));
    private static readonly MessageEnvelope _tariffchangeservicenumber = new(_testheader, new TariffChangeServiceNumber(_testdossierid, _testplatformprovider, _testdate, _testservicenumbers.Select(n => new TariffChangeServiceNumberItem(n, _testtariff))));

    private static NumberSerie CreateNumberSerie(string value)
    {
        var d = value.Split('-', 2);
        return d.Length > 1
            ? new NumberSerie(d[0], d[0][..^d[1].Length] + d[1])
            : new NumberSerie(value, value);
    }


    [TestMethod]
    public void CanFollowUp_Returns_Correct_Results()
    {
        Assert.IsFalse(_portingrequest.CanFollowUp(_portingrequest));
        Assert.IsTrue(_portingrequestanswerdelayed.CanFollowUp(_portingrequest));
        Assert.IsTrue(_portingrequestanswer.CanFollowUp(_portingrequest));
        Assert.IsFalse(_cancel.CanFollowUp(_portingrequest));
        Assert.IsFalse(_portingperformed.CanFollowUp(_portingrequest));

        Assert.IsFalse(_portingrequest.CanFollowUp(_portingrequestanswerdelayed));
        Assert.IsFalse(_portingrequestanswerdelayed.CanFollowUp(_portingrequestanswerdelayed));
        Assert.IsTrue(_portingrequestanswer.CanFollowUp(_portingrequestanswerdelayed));
        Assert.IsFalse(_cancel.CanFollowUp(_portingrequestanswerdelayed));
        Assert.IsFalse(_portingperformed.CanFollowUp(_portingrequestanswerdelayed));

        Assert.IsFalse(_portingrequest.CanFollowUp(_portingrequestanswer));
        Assert.IsFalse(_portingrequestanswerdelayed.CanFollowUp(_portingrequestanswer));
        Assert.IsFalse(_portingrequestanswer.CanFollowUp(_portingrequestanswer));
        Assert.IsTrue(_cancel.CanFollowUp(_portingrequestanswer));
        Assert.IsTrue(_portingperformed.CanFollowUp(_portingrequestanswer));

        Assert.IsFalse(_portingrequest.CanFollowUp(_cancel));
        Assert.IsFalse(_portingrequestanswerdelayed.CanFollowUp(_cancel));
        Assert.IsFalse(_portingrequestanswer.CanFollowUp(_cancel));
        Assert.IsFalse(_cancel.CanFollowUp(_cancel));
        Assert.IsFalse(_portingperformed.CanFollowUp(_cancel));

        Assert.IsFalse(_portingrequest.CanFollowUp(_portingperformed));
        Assert.IsFalse(_portingrequestanswerdelayed.CanFollowUp(_portingperformed));
        Assert.IsFalse(_portingrequestanswer.CanFollowUp(_portingperformed));
        Assert.IsFalse(_cancel.CanFollowUp(_portingperformed));
        Assert.IsFalse(_portingperformed.CanFollowUp(_portingperformed));

        // Check a message type that has NO FollowupMessages attribute
        // First make sure ActivationServiceNumber didn't get a followup attribute (it currently doesn't)
        Assert.IsNull(_activationservicenumber.GetType().GetCustomAttributes<FollowupMessagesAttribute>().FirstOrDefault());
        // Then check CanFollowUp() results
        Assert.IsFalse(_activationservicenumber.CanFollowUp(_portingrequest));
        Assert.IsFalse(_portingrequest.CanFollowUp(_activationservicenumber));

        // Check a message with a different dossierid
        Assert.IsFalse(_portingrequestanswer.CanFollowUp(_portingrequest with { Body = _portingrequestanswer.Body with { DossierId = "987" } }));
    }


    [TestMethod]
    public void IsSingleMessage_Returns_Correct_Results()
    {
        Assert.IsFalse(_portingrequestanswer.IsSingleMessage());
        Assert.IsTrue(_activationservicenumber.IsSingleMessage());
    }

    [TestMethod]
    public void IsFirstMessage_Returns_Correct_Results()
    {
        Assert.IsTrue(_portingrequest.IsFirstMessage());
        Assert.IsTrue(_activationservicenumber.IsFirstMessage());
        Assert.IsFalse(_portingrequestanswer.IsFirstMessage());
    }

    [TestMethod]
    public void IsLastMessage_Returns_Correct_Results()
    {
        Assert.IsTrue(_portingperformed.IsLastMessage());
        Assert.IsTrue(_activationservicenumber.IsLastMessage());
        Assert.IsFalse(_portingrequestanswer.IsLastMessage());
    }

    [TestMethod]
    public void Message_RoundTrip_JSON_IsCorrect()
        => Assert.AreEqual(_cancel, COINJsonParser.FromCOINJson(_cancel.ToCOINJson()));

    [TestMethod]
    public void Message_RoundTrip_Test()
    {
        Assert.IsInstanceOfType(COINJsonParser.FromCOINJson(_activationservicenumber.ToCOINJson()).Body, typeof(ActivationServiceNumber));
        Assert.IsInstanceOfType(COINJsonParser.FromCOINJson(_cancel.ToCOINJson()).Body, typeof(Cancel));
        Assert.IsInstanceOfType(COINJsonParser.FromCOINJson(_deactivation.ToCOINJson()).Body, typeof(Deactivation));
        Assert.IsInstanceOfType(COINJsonParser.FromCOINJson(_deactivationservicenumber.ToCOINJson()).Body, typeof(DeactivationServiceNumber));
        Assert.IsInstanceOfType(COINJsonParser.FromCOINJson(_portingperformed.ToCOINJson()).Body, typeof(PortingPerformed));
        Assert.IsInstanceOfType(COINJsonParser.FromCOINJson(_portingrequest.ToCOINJson()).Body, typeof(PortingRequest));
        Assert.IsInstanceOfType(COINJsonParser.FromCOINJson(_portingrequestanswer.ToCOINJson()).Body, typeof(PortingRequestAnswer));
        Assert.IsInstanceOfType(COINJsonParser.FromCOINJson(_portingrequestanswerdelayed.ToCOINJson()).Body, typeof(PortingRequestAnswerDelayed));
        Assert.IsInstanceOfType(COINJsonParser.FromCOINJson(_rangeactivation.ToCOINJson()).Body, typeof(RangeActivation));
        Assert.IsInstanceOfType(COINJsonParser.FromCOINJson(_rangedeactivation.ToCOINJson()).Body, typeof(RangeDeactivation));
        Assert.IsInstanceOfType(COINJsonParser.FromCOINJson(_tariffchangeservicenumber.ToCOINJson()).Body, typeof(TariffChangeServiceNumber));
    }
}