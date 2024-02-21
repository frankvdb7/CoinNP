using COINNP.Client.ResourceFiles;
using COINNP.Entities;
using COINNP.Entities.Common;
using COINNP.Entities.Messages;
using C = Coin.Sdk.NP.Messages.V3;

namespace COINNP.Client.Mapping;

internal static class ToCOINExtensionMethods
{
    internal static C.IMessageEnvelope<C.INpMessageContent> ToCOINMessageEnvelope(this MessageEnvelope messageEnvelope, IValueHelper valueHelper)
        => new C.MessageEnvelope<C.INpMessageContent>()
        {
            Message = messageEnvelope.ToCOINBody(valueHelper)
        };

    internal static C.INpMessage<C.INpMessageContent> ToCOINBody(this MessageEnvelope messageEnvelope, IValueHelper valueHelper)
    {
        var header = messageEnvelope.Header.ToCOIN(valueHelper);
        return messageEnvelope.Body switch
        {
            ActivationServiceNumber asn => new C.ActivationServiceNumberMessage { Header = header, Body = asn.ToCOINBody(valueHelper) },
            Cancel c => new C.CancelMessage { Header = header, Body = c.ToCOINBody(valueHelper) },
            Deactivation d => new C.DeactivationMessage { Header = header, Body = d.ToCOINBody(valueHelper) },
            DeactivationServiceNumber dsn => new C.DeactivationServiceNumberMessage { Header = header, Body = dsn.ToCOINBody(valueHelper) },
            EnumActivationNumber ean => new C.EnumActivationNumberMessage { Header = header, Body = ean.ToCOINBody(valueHelper) },
            EnumActivationOperator eao => new C.EnumActivationOperatorMessage { Header = header, Body = eao.ToCOINBody(valueHelper) },
            EnumActivationRange ear => new C.EnumActivationRangeMessage { Header = header, Body = ear.ToCOINBody(valueHelper) },
            EnumDeactivationNumber edn => new C.EnumDeactivationNumberMessage { Header = header, Body = edn.ToCOINBody(valueHelper) },
            EnumDeactivationOperator edo => new C.EnumDeactivationOperatorMessage { Header = header, Body = edo.ToCOINBody(valueHelper) },
            EnumDeactivationRange edr => new C.EnumDeactivationRangeMessage { Header = header, Body = edr.ToCOINBody(valueHelper) },
            EnumProfileActivation eda => new C.EnumProfileActivationMessage { Header = header, Body = eda.ToCOINBody(valueHelper) },
            EnumProfileDeactivation epd => new C.EnumProfileDeactivationMessage { Header = header, Body = epd.ToCOINBody(valueHelper) },
            PortingRequest pr => new C.PortingRequestMessage { Header = header, Body = pr.ToCOINBody(valueHelper) },
            PortingRequestAnswer pra => new C.PortingRequestAnswerMessage { Header = header, Body = pra.ToCOINBody(valueHelper) },
            PortingPerformed pp => new C.PortingPerformedMessage { Header = header, Body = pp.ToCOINBody(valueHelper) },
            PortingRequestAnswerDelayed prad => new C.PortingRequestAnswerDelayedMessage { Header = header, Body = prad.ToCOINBody(valueHelper) },
            RangeActivation ra => new C.RangeActivationMessage { Header = header, Body = ra.ToCOINBody(valueHelper) },
            RangeDeactivation rd => new C.RangeDeactivationMessage { Header = header, Body = rd.ToCOINBody(valueHelper) },
            TariffChangeServiceNumber tcsn => new C.TariffChangeServiceNumberMessage { Header = header, Body = tcsn.ToCOINBody(valueHelper) },
            ErrorFound ef => new C.ErrorFoundMessage { Header = header, Body = ef.ToCOINBody(valueHelper) },
            _ => throw new NotImplementedException(string.Format(Translations.ERR_Messagetype_Not_Implemented, messageEnvelope.Body.GetType().Name))
        };
    }

    internal static C.Header ToCOIN(this Header header, IValueHelper valueHelper)
        => new()
        {
            Sender = header.Sender.ToCOINSender(valueHelper),
            Receiver = header.Receiver.ToCOINReceiver(valueHelper),
            Timestamp = valueHelper.SerializeDateTimeOffset(header.DateTimeOffset)
        };

    internal static C.INpMessageBody<C.INpMessageContent> ToCOIN(this Message message, IValueHelper valueHelper)
        => message switch
        {
            ActivationServiceNumber asn => asn.ToCOINBody(valueHelper),
            Cancel c => c.ToCOINBody(valueHelper),
            Deactivation d => d.ToCOINBody(valueHelper),
            DeactivationServiceNumber dsn => dsn.ToCOINBody(valueHelper),
            EnumActivationNumber ean => ean.ToCOINBody(valueHelper),
            EnumActivationOperator eao => eao.ToCOINBody(valueHelper),
            EnumActivationRange ear => ear.ToCOINBody(valueHelper),
            EnumDeactivationNumber edn => edn.ToCOINBody(valueHelper),
            EnumDeactivationOperator edo => edo.ToCOINBody(valueHelper),
            EnumDeactivationRange edr => edr.ToCOINBody(valueHelper),
            EnumProfileActivation epa => epa.ToCOINBody(valueHelper),
            EnumProfileDeactivation epd => epd.ToCOINBody(valueHelper),
            PortingRequest pr => pr.ToCOINBody(valueHelper),
            PortingRequestAnswer pra => pra.ToCOINBody(valueHelper),
            PortingPerformed pp => pp.ToCOINBody(valueHelper),
            PortingRequestAnswerDelayed prad => prad.ToCOINBody(valueHelper),
            RangeActivation ra => ra.ToCOINBody(valueHelper),
            RangeDeactivation rd => rd.ToCOINBody(valueHelper),
            TariffChangeServiceNumber tcsn => tcsn.ToCOINBody(valueHelper),
            ErrorFound ef => ef.ToCOINBody(valueHelper),
            _ => throw new NotImplementedException(string.Format(Translations.ERR_Messagetype_Not_Implemented, message.GetType().Name))
        };

    internal static C.CustomerInfo? ToCOIN(this CustomerInfo? customerInfo, IValueHelper valueHelper)
        => customerInfo == null ? null : new()
        {
            Companyname = customerInfo.CompanyName,
            CustomerId = customerInfo.CustomerId,
            HouseNr = customerInfo.HouseNr,
            HouseNrExt = customerInfo.HouseNrExt,
            Lastname = customerInfo.LastName,
            Postcode = customerInfo.Postcode,
        };

    internal static C.Sender ToCOINSender(this Sender sender, IValueHelper valueHelper)
        => new()
        {
            NetworkOperator = sender.NetworkOperator,
            ServiceProvider = sender.ServiceProvider
        };

    internal static C.Receiver ToCOINReceiver(this Receiver recipient, IValueHelper valueHelper)
        => new()
        {
            NetworkOperator = recipient.NetworkOperator,
            ServiceProvider = recipient.ServiceProvider
        };

    internal static C.NumberSeries ToCOIN(this NumberSerie numberSerie, IValueHelper valueHelper)
        => new()
        {
            Start = numberSerie.Start,
            End = numberSerie.End
        };

    internal static C.TariffInfo ToCOIN(this TariffInfo tariffInfo, IValueHelper valueHelper)
        => new()
        {
            Currency = valueHelper.SerializeCurrencyType(tariffInfo.Currency),
            Peak = valueHelper.SerializeCurrency(tariffInfo.Peak),
            OffPeak = valueHelper.SerializeCurrency(tariffInfo.OffPeak),
            Type = valueHelper.SerializeTariffType(tariffInfo.TariffType),
            Vat = valueHelper.SerializeVAT(tariffInfo.VAT)
        };

    internal static C.ActivationServiceNumberBody ToCOINBody(this ActivationServiceNumber activationServiceNumber, IValueHelper valueHelper)
        => new() { Content = activationServiceNumber.ToCOIN(valueHelper) };

    internal static C.ActivationServiceNumber ToCOIN(this ActivationServiceNumber activationServiceNumber, IValueHelper valueHelper)
        => new()
        {
            DossierId = activationServiceNumber.DossierId,
            Note = activationServiceNumber.Note,
            PlannedDatetime = valueHelper.SerializeDateTimeOffset(activationServiceNumber.PlannedDateTime),
            PlatformProvider = activationServiceNumber.PlatformProvider,
            Repeats = activationServiceNumber.Items.ToCOINRepeats(valueHelper)
        };

    internal static C.CancelBody ToCOINBody(this Cancel cancel, IValueHelper valueHelper)
        => new() { Content = cancel.ToCOIN(valueHelper) };

    internal static C.Cancel ToCOIN(this Cancel cancel, IValueHelper valueHelper)
        => new()
        {
            DossierId = cancel.DossierId,
            Note = cancel.Note,
        };

    internal static C.DeactivationBody ToCOINBody(this Deactivation deactivation, IValueHelper valueHelper)
        => new() { Content = deactivation.ToCOIN(valueHelper) };

    internal static C.Deactivation ToCOIN(this Deactivation deactivation, IValueHelper valueHelper)
        => new()
        {
            DossierId = deactivation.DossierId,
            CurrentNetworkOperator = deactivation.CurrentNetworkOperator,
            OriginalNetworkOperator = deactivation.OriginalNetworkOperator,
            Repeats = deactivation.Items.ToCOINRepeats(valueHelper)
        };

    internal static C.DeactivationServiceNumberBody ToCOINBody(this DeactivationServiceNumber deactivationServiceNumber, IValueHelper valueHelper)
        => new() { Content = deactivationServiceNumber.ToCOIN(valueHelper) };

    internal static C.DeactivationServiceNumber ToCOIN(this DeactivationServiceNumber deactivationServiceNumber, IValueHelper valueHelper)
        => new()
        {
            DossierId = deactivationServiceNumber.DossierId,
            PlannedDatetime = valueHelper.SerializeDateTimeOffset(deactivationServiceNumber.PlannedDateTime),
            PlatformProvider = deactivationServiceNumber.PlatformProvider,
            Repeats = deactivationServiceNumber.Items.ToCOINRepeats(valueHelper)
        };

    internal static C.EnumActivationNumberBody ToCOINBody(this EnumActivationNumber enumActivationNumber, IValueHelper valueHelper)
        => new() { Content = enumActivationNumber.ToCOIN(valueHelper) };

    internal static C.EnumActivationNumber ToCOIN(this EnumActivationNumber enumActivationNumber, IValueHelper valueHelper)
        => new()
        {
            DossierId = enumActivationNumber.DossierId,
            CurrentNetworkOperator = enumActivationNumber.CurrentNetworkOperator,
            TypeOfNumber = valueHelper.SerializeTypeOfNumber(enumActivationNumber.TypeOfNumber),
            Repeats = enumActivationNumber.Items.ToCOINRepeats(valueHelper)
        };

    internal static C.EnumActivationOperatorBody ToCOINBody(this EnumActivationOperator enumActivationOperator, IValueHelper valueHelper)
        => new() { Content = ToCOIN(enumActivationOperator, valueHelper) };

    internal static C.EnumActivationOperator ToCOIN(EnumActivationOperator enumActivationOperator, IValueHelper valueHelper)
        => new()
        {
            DossierId = enumActivationOperator.DossierId,
            CurrentNetworkOperator = enumActivationOperator.CurrentNetworkOperator,
            TypeOfNumber = valueHelper.SerializeTypeOfNumber(enumActivationOperator.TypeOfNumber),
            Repeats = enumActivationOperator.Items.ToCOINRepeats(valueHelper)
        };


    internal static C.EnumActivationRangeBody ToCOINBody(this EnumActivationRange enumActivationRange, IValueHelper valueHelper)
        => new() { Content = enumActivationRange.ToCOIN(valueHelper) };

    internal static C.EnumActivationRange ToCOIN(this EnumActivationRange enumActivationRange, IValueHelper valueHelper)
        => new()
        {
            DossierId = enumActivationRange.DossierId,
            CurrentNetworkOperator = enumActivationRange.CurrentNetworkOperator,
            TypeOfNumber = valueHelper.SerializeTypeOfNumber(enumActivationRange.TypeOfNumber),
            Repeats = enumActivationRange.Items.ToCOINRepeats(valueHelper)
        };

    internal static C.EnumDeactivationNumberBody ToCOINBody(this EnumDeactivationNumber enumDeactivationNumber, IValueHelper valueHelper)
        => new() { Content = enumDeactivationNumber.ToCOIN(valueHelper) };

    internal static C.EnumDeactivationNumber ToCOIN(this EnumDeactivationNumber enumDeactivationNumber, IValueHelper valueHelper)
        => new()
        {
            DossierId = enumDeactivationNumber.DossierId,
            CurrentNetworkOperator = enumDeactivationNumber.CurrentNetworkOperator,
            TypeOfNumber = valueHelper.SerializeTypeOfNumber(enumDeactivationNumber.TypeOfNumber),
            Repeats = enumDeactivationNumber.Items.ToCOINRepeats(valueHelper)
        };

    internal static C.EnumDeactivationOperatorBody ToCOINBody(this EnumDeactivationOperator enumDeactivationOperator, IValueHelper valueHelper)
        => new() { Content = enumDeactivationOperator.ToCOIN(valueHelper) };

    internal static C.EnumDeactivationOperator ToCOIN(this EnumDeactivationOperator enumDeactivationOperator, IValueHelper valueHelper)
        => new()
        {
            DossierId = enumDeactivationOperator.DossierId,
            CurrentNetworkOperator = enumDeactivationOperator.CurrentNetworkOperator,
            TypeOfNumber = valueHelper.SerializeTypeOfNumber(enumDeactivationOperator.TypeOfNumber),
            Repeats = enumDeactivationOperator.Items.ToCOINRepeats(valueHelper)
        };

    internal static C.EnumDeactivationRangeBody ToCOINBody(this EnumDeactivationRange enumDeactivationRange, IValueHelper valueHelper)
        => new() { Content = enumDeactivationRange.ToCOIN(valueHelper) };

    internal static C.EnumDeactivationRange ToCOIN(this EnumDeactivationRange enumDeactivationRange, IValueHelper valueHelper)
        => new()
        {
            DossierId = enumDeactivationRange.DossierId,
            CurrentNetworkOperator = enumDeactivationRange.CurrentNetworkOperator,
            TypeOfNumber = valueHelper.SerializeTypeOfNumber(enumDeactivationRange.TypeOfNumber),
            Repeats = enumDeactivationRange.Items.ToCOINRepeats(valueHelper)
        };

    internal static C.EnumProfileActivationBody ToCOINBody(this EnumProfileActivation enumProfileActivation, IValueHelper valueHelper)
        => new() { Content = enumProfileActivation.ToCOIN(valueHelper) };

    internal static C.EnumProfileActivation ToCOIN(this EnumProfileActivation enumProfileActivation, IValueHelper valueHelper)
        => new()
        {
            DossierId = enumProfileActivation.DossierId,
            CurrentNetworkOperator = enumProfileActivation.CurrentNetworkOperator,
            TypeOfNumber = valueHelper.SerializeTypeOfNumber(enumProfileActivation.TypeOfNumber),
            Scope = enumProfileActivation.Scope,
            ProfileId = enumProfileActivation.ProfileId,
            Ttl = valueHelper.SerializeTimeSpan(enumProfileActivation.Ttl),
            DnsClass = enumProfileActivation.DnsClass,
            RecType = enumProfileActivation.RecType,
            Order = valueHelper.SerializeNullableInt(enumProfileActivation.Order),
            Preference = valueHelper.SerializeNullableInt(enumProfileActivation.Preference),
            Flags = enumProfileActivation.Flags,
            EnumService = enumProfileActivation.EnumService,
            Regexp = enumProfileActivation.Regexp,
            UserTag = enumProfileActivation.UserTag,
            Domain = enumProfileActivation.Domain,
            SpCode = enumProfileActivation.SpCode,
            ProcessType = enumProfileActivation.ProcessType,
            Gateway = enumProfileActivation.Gateway,
            Service = enumProfileActivation.Service,
            DomainTag = enumProfileActivation.DomainTag,
            Replacement = enumProfileActivation.Replacement
        };

    internal static C.EnumProfileDeactivationBody ToCOINBody(this EnumProfileDeactivation enumProfileDeactivation, IValueHelper valueHelper)
        => new() { Content = enumProfileDeactivation.ToCOIN(valueHelper) };

    internal static C.EnumProfileDeactivation ToCOIN(this EnumProfileDeactivation enumProfileDeactivation, IValueHelper valueHelper)
        => new()
        {
            DossierId = enumProfileDeactivation.DossierId,
            CurrentNetworkOperator = enumProfileDeactivation.CurrentNetworkOperator,
            TypeOfNumber = valueHelper.SerializeTypeOfNumber(enumProfileDeactivation.TypeOfNumber),
            ProfileId = enumProfileDeactivation.ProfileId,
        };

    internal static C.ErrorFoundBody ToCOINBody(this ErrorFound errorFound, IValueHelper valueHelper)
        => new() { Content = errorFound.ToCOIN(valueHelper) };

    internal static C.ErrorFound ToCOIN(this ErrorFound errorFound, IValueHelper valueHelper)
        => new()
        {
            DossierId = errorFound.DossierId,
            Repeats = errorFound.Errors.ToCOINRepeats(valueHelper)
        };

    internal static C.PortingPerformedBody ToCOINBody(this PortingPerformed portingPerformed, IValueHelper valueHelper)
        => new() { Content = ToCOIN(portingPerformed, valueHelper) };

    internal static C.PortingPerformed ToCOIN(PortingPerformed portingPerformed, IValueHelper valueHelper)
        => new()
        {
            DossierId = portingPerformed.DossierId,
            ActualDatetime = valueHelper.SerializeNullableDateTimeOffset(portingPerformed.ActualDateTime),
            BatchPorting = valueHelper.SerializeNullableBool(portingPerformed.BatchPorting.HasValue && portingPerformed.BatchPorting.Value ? true : null),
            DonorNetworkOperator = portingPerformed.DonorNetworkOperator,
            RecipientNetworkOperator = portingPerformed.RecipientNetworkOperator,
            Repeats = portingPerformed.Items.ToCOINRepeats(valueHelper)
        };
    internal static C.PortingRequestBody ToCOINBody(this PortingRequest portingRequest, IValueHelper valueHelper)
        => new() { Content = portingRequest.ToCOIN(valueHelper) };

    internal static C.PortingRequest ToCOIN(this PortingRequest portingRequest, IValueHelper valueHelper)
        => new()
        {
            DossierId = portingRequest.DossierId,
            Contract = valueHelper.SerializeContractState(portingRequest.Contract),
            CustomerInfo = portingRequest.CustomerInfo.ToCOIN(valueHelper),
            DonorNetworkOperator = portingRequest.DonorNetworkOperator,
            DonorServiceProvider = portingRequest.DonorServiceProvider,
            Note = portingRequest.Note,
            RecipientNetworkOperator = portingRequest.RecipientNetworkOperator,
            RecipientServiceProvider = portingRequest.RecipientServiceProvider,
            Repeats = portingRequest.Items.ToCOINRepeats(valueHelper)
        };

    internal static C.PortingRequestAnswerBody ToCOINBody(this PortingRequestAnswer portingRequestAnswer, IValueHelper valueHelper)
        => new() { Content = portingRequestAnswer.ToCOIN(valueHelper) };

    internal static C.PortingRequestAnswer ToCOIN(this PortingRequestAnswer portingRequestAnswer, IValueHelper valueHelper)
        => new()
        {
            DossierId = portingRequestAnswer.DossierId,
            Blocking = valueHelper.SerializeNullableBool(portingRequestAnswer.Blocking),
            Repeats = portingRequestAnswer.Items.ToCOINRepeats(valueHelper)
        };

    internal static C.PortingRequestAnswerDelayedBody ToCOINBody(this PortingRequestAnswerDelayed portingRequestAnswerDelayed, IValueHelper valueHelper)
        => new() { Content = portingRequestAnswerDelayed.ToCOIN(valueHelper) };

    internal static C.PortingRequestAnswerDelayed ToCOIN(this PortingRequestAnswerDelayed portingRequestAnswerDelayed, IValueHelper valueHelper)
        => new()
        {
            DossierId = portingRequestAnswerDelayed.DossierId,
            AnswerDueDatetime = valueHelper.SerializeNullableDateTimeOffset(portingRequestAnswerDelayed.AnswerDueDateTime),
            DonorNetworkOperator = portingRequestAnswerDelayed.DonorNetworkOperator,
            DonorServiceProvider = portingRequestAnswerDelayed.DonorServiceProvider,
            ReasonCode = portingRequestAnswerDelayed.ReasonCode
        };

    internal static C.RangeActivationBody ToCOINBody(this RangeActivation rangeActivation, IValueHelper valueHelper)
        => new() { Content = rangeActivation.ToCOIN(valueHelper) };

    internal static C.RangeActivation ToCOIN(this RangeActivation rangeActivation, IValueHelper valueHelper)
        => new()
        {
            DossierId = rangeActivation.DossierId,
            CurrentNetworkOperator = rangeActivation.CurrentNetworkOperator,
            OptaNr = rangeActivation.OptaNr,
            PlannedDatetime = valueHelper.SerializeDateTimeOffset(rangeActivation.PlannedDateTime),
            Repeats = rangeActivation.Items.ToCOINRepeats(valueHelper)
        };

    internal static C.RangeDeactivationBody ToCOINBody(this RangeDeactivation rangeDeactivation, IValueHelper valueHelper)
        => new() { Content = rangeDeactivation.ToCOIN(valueHelper) };

    internal static C.RangeDeactivation ToCOIN(this RangeDeactivation rangeDeactivation, IValueHelper valueHelper)
        => new()
        {
            DossierId = rangeDeactivation.DossierId,
            CurrentNetworkOperator = rangeDeactivation.CurrentNetworkOperator,
            OptaNr = rangeDeactivation.OptaNr,
            PlannedDatetime = valueHelper.SerializeDateTimeOffset(rangeDeactivation.PlannedDateTime),
            Repeats = rangeDeactivation.Items.ToCOINRepeats(valueHelper)
        };

    internal static C.TariffChangeServiceNumberBody ToCOINBody(this TariffChangeServiceNumber tariffChangeServiceNumber, IValueHelper valueHelper)
        => new() { Content = tariffChangeServiceNumber.ToCOIN(valueHelper) };

    internal static C.TariffChangeServiceNumber ToCOIN(this TariffChangeServiceNumber tariffChangeServiceNumber, IValueHelper valueHelper)
        => new()
        {
            DossierId = tariffChangeServiceNumber.DossierId,
            PlannedDatetime = valueHelper.SerializeDateTimeOffset(tariffChangeServiceNumber.PlannedDateTime),
            PlatformProvider = tariffChangeServiceNumber.PlatformProvider,
            Repeats = tariffChangeServiceNumber.Items.ToCOINRepeats(valueHelper)
        };
}