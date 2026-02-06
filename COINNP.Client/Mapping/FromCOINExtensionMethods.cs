using COINNP.Client.ResourceFiles;
using COINNP.Entities;
using COINNP.Entities.Common;
using COINNP.Entities.Messages;
using COINNP.Entities.SequenceItems;
using C = Coin.Sdk.NP.Messages.V3;

namespace COINNP.Client.Mapping;

internal static class FromCOINExtensionMethods
{
    internal static SendMessageResponse FromCOIN(this C.MessageResponse messageResponse)
        => messageResponse switch
        {
            C.ErrorResponse errorResponse => new(
                errorResponse.TransactionId,
                errorResponse.Errors?
                    .Select(error => new SendMessageError(error.Code, error.Message))
                    .ToArray()
            ),
            _ => new(messageResponse.TransactionId)
        };

    internal static MessageEnvelope FromCOIN<T>(this C.IMessageEnvelope<T> messageEnvelope, IValueHelper valueHelper)
         where T : C.INpMessageContent
         => new(
             messageEnvelope.Message.Header.FromCOIN(valueHelper),
             messageEnvelope.Message.Body.FromCOIN(valueHelper)
         );

    internal static MessageEnvelope FromCOIN<T>(this C.INpMessage<T> message, IValueHelper valueHelper)
        where T : C.INpMessageContent
        => new(
            message.Header.FromCOIN(valueHelper),
            message.Body.FromCOIN(valueHelper)
        );

    internal static Header FromCOIN(this C.Header header, IValueHelper valueHelper)
        => new(
             header.Receiver.FromCOIN(valueHelper),
             header.Sender.FromCOIN(valueHelper),
             valueHelper.ParseDateTimeOffset(header.Timestamp)
        );

    internal static Message FromCOIN<T>(this C.INpMessageBody<T> body, IValueHelper valueHelper)
          where T : C.INpMessageContent
          => body.Content switch
          {
              C.ActivationServiceNumber asn => asn.FromCOIN(valueHelper),
              C.Cancel c => c.FromCOIN(valueHelper),
              C.Deactivation d => d.FromCOIN(valueHelper),
              C.DeactivationServiceNumber dsn => dsn.FromCOIN(valueHelper),
              C.EnumActivationNumber ean => ean.FromCOIN(valueHelper),
              C.EnumActivationOperator eao => eao.FromCOIN(valueHelper),
              C.EnumActivationRange ear => ear.FromCOIN(valueHelper),
              C.EnumDeactivationNumber edn => edn.FromCOIN(valueHelper),
              C.EnumDeactivationOperator edo => edo.FromCOIN(valueHelper),
              C.EnumDeactivationRange edr => edr.FromCOIN(valueHelper),
              C.EnumProfileActivation epa => epa.FromCOIN(valueHelper),
              C.EnumProfileDeactivation epd => epd.FromCOIN(valueHelper),
              C.PortingRequest pr => pr.FromCOIN(valueHelper),
              C.PortingRequestAnswer pra => pra.FromCOIN(valueHelper),
              C.PortingPerformed pp => pp.FromCOIN(valueHelper),
              C.PortingRequestAnswerDelayed prad => prad.FromCOIN(valueHelper),
              C.RangeActivation ra => ra.FromCOIN(valueHelper),
              C.RangeDeactivation rd => rd.FromCOIN(valueHelper),
              C.TariffChangeServiceNumber tcsn => tcsn.FromCOIN(valueHelper),
              C.ErrorFound ef => ef.FromCOIN(valueHelper),
              _ => throw new NotImplementedException(string.Format(Translations.ERR_Messagetype_Not_Implemented, body.Content.GetType().Name))
          };

    internal static CustomerInfo? FromCOIN(this C.CustomerInfo? customerInfo, IValueHelper valueHelper)
        => customerInfo == null ? null : new(
            customerInfo.Lastname,
            customerInfo.Companyname,
            customerInfo.HouseNr,
            customerInfo.HouseNrExt,
            customerInfo.Postcode,
            customerInfo.CustomerId
        );

    internal static Sender FromCOIN(this C.Sender sender, IValueHelper valueHelper)
        => new(
              sender.NetworkOperator,
              sender.ServiceProvider
        );

    internal static Receiver FromCOIN(this C.Receiver receiver, IValueHelper valueHelper)
        => new(
              receiver.NetworkOperator,
              receiver.ServiceProvider
        );

    internal static NumberSerie FromCOIN(this C.NumberSeries numberSeries, IValueHelper valueHelper)
        => new(
            numberSeries.Start,
            numberSeries.End
        );

    internal static TariffInfo FromCOIN(this C.TariffInfo tariffInfo, IValueHelper valueHelper)
        => new(
            valueHelper.ParseCurrency(tariffInfo.Peak),
            valueHelper.ParseCurrency(tariffInfo.OffPeak),
            valueHelper.ParseCurrencyType(tariffInfo.Currency),
            valueHelper.ParseTariffType(tariffInfo.Type),
            valueHelper.ParseVAT(tariffInfo.Vat)
        );

    internal static ActivationServiceNumber FromCOIN(this C.ActivationServiceNumber activationServiceNumber, IValueHelper valueHelper)
        => new(
            activationServiceNumber.DossierId,
            activationServiceNumber.PlatformProvider,
            valueHelper.ParseDateTimeOffset(activationServiceNumber.PlannedDatetime),
            activationServiceNumber.Note,
            valueHelper.ConvertRepeats(activationServiceNumber.Repeats, i => i.FromCOINRepeats(valueHelper))
        );

    internal static Cancel FromCOIN(this C.Cancel cancel, IValueHelper valueHelper)
        => new(
            cancel.DossierId,
            cancel.Note
        );

    internal static Deactivation FromCOIN(this C.Deactivation deactivation, IValueHelper valueHelper)
        => new(
            deactivation.DossierId,
            deactivation.CurrentNetworkOperator,
            deactivation.OriginalNetworkOperator,
            valueHelper.ConvertRepeats(deactivation.Repeats, i => i.FromCOINRepeats(valueHelper))
        );

    internal static DeactivationServiceNumber FromCOIN(this C.DeactivationServiceNumber deactivationServiceNumber, IValueHelper valueHelper)
        => new(
            deactivationServiceNumber.DossierId,
            deactivationServiceNumber.PlatformProvider,
            valueHelper.ParseDateTimeOffset(deactivationServiceNumber.PlannedDatetime),
            valueHelper.ConvertRepeats(deactivationServiceNumber.Repeats, i => i.FromCOINRepeats(valueHelper))
        );

    internal static EnumActivationNumber FromCOIN(this C.EnumActivationNumber enumActivationNumber, IValueHelper valueHelper)
        => new(
            enumActivationNumber.DossierId,
            enumActivationNumber.CurrentNetworkOperator,
            valueHelper.ParseTypeOfNumber(enumActivationNumber.TypeOfNumber),
            valueHelper.ConvertRepeats(enumActivationNumber.Repeats, i => i.FromCOINRepeats(valueHelper))
        );

    internal static EnumActivationOperator FromCOIN(this C.EnumActivationOperator enumActivationOperator, IValueHelper valueHelper)
        => new(
            enumActivationOperator.DossierId,
            enumActivationOperator.CurrentNetworkOperator,
            valueHelper.ParseTypeOfNumber(enumActivationOperator.TypeOfNumber),
            valueHelper.ConvertRepeats(enumActivationOperator.Repeats, i => i.FromCOINRepeats(valueHelper))
        );

    internal static EnumActivationRange FromCOIN(this C.EnumActivationRange enumActivationRange, IValueHelper valueHelper)
        => new(
            enumActivationRange.DossierId,
            enumActivationRange.CurrentNetworkOperator,
            valueHelper.ParseTypeOfNumber(enumActivationRange.TypeOfNumber),
            valueHelper.ConvertRepeats(enumActivationRange.Repeats, i => i.FromCOINRepeats(valueHelper))
        );

    internal static EnumDeactivationNumber FromCOIN(this C.EnumDeactivationNumber enumDeactivationNumber, IValueHelper valueHelper)
        => new(
            enumDeactivationNumber.DossierId,
            enumDeactivationNumber.CurrentNetworkOperator,
            valueHelper.ParseTypeOfNumber(enumDeactivationNumber.TypeOfNumber),
            valueHelper.ConvertRepeats(enumDeactivationNumber.Repeats, i => i.FromCOINRepeats(valueHelper))
        );

    internal static EnumDeactivationOperator FromCOIN(this C.EnumDeactivationOperator enumDeactivationOperator, IValueHelper valueHelper)
        => new(
            enumDeactivationOperator.DossierId,
            enumDeactivationOperator.CurrentNetworkOperator,
            valueHelper.ParseTypeOfNumber(enumDeactivationOperator.TypeOfNumber),
            valueHelper.ConvertRepeats(enumDeactivationOperator.Repeats, i => i.FromCOINRepeats(valueHelper))
        );

    internal static EnumDeactivationRange FromCOIN(this C.EnumDeactivationRange enumDeactivationRange, IValueHelper valueHelper)
        => new(
            enumDeactivationRange.DossierId,
            enumDeactivationRange.CurrentNetworkOperator,
            valueHelper.ParseTypeOfNumber(enumDeactivationRange.TypeOfNumber),
            valueHelper.ConvertRepeats(enumDeactivationRange.Repeats, i => i.FromCOINRepeats(valueHelper))
        );

    internal static EnumProfileActivation FromCOIN(this C.EnumProfileActivation enumProfileActivation, IValueHelper valueHelper)
        => new(
            enumProfileActivation.DossierId,
            enumProfileActivation.CurrentNetworkOperator,
            valueHelper.ParseTypeOfNumber(enumProfileActivation.TypeOfNumber),
            enumProfileActivation.Scope,
            enumProfileActivation.ProfileId,
            valueHelper.ParseTimeSpan(enumProfileActivation.Ttl),
            enumProfileActivation.DnsClass,
            enumProfileActivation.RecType,
            valueHelper.ParseNullableInt(enumProfileActivation.Order),
            valueHelper.ParseNullableInt(enumProfileActivation.Preference),
            enumProfileActivation.Flags,
            enumProfileActivation.EnumService,
            enumProfileActivation.Regexp,
            enumProfileActivation.UserTag,
            enumProfileActivation.Domain,
            enumProfileActivation.SpCode,
            enumProfileActivation.ProcessType,
            enumProfileActivation.Gateway,
            enumProfileActivation.Service,
            enumProfileActivation.DomainTag,
            enumProfileActivation.Replacement
        );

    internal static EnumProfileDeactivation FromCOIN(this C.EnumProfileDeactivation enumProfiledeactivation, IValueHelper valueHelper)
        => new(
            enumProfiledeactivation.DossierId,
            enumProfiledeactivation.CurrentNetworkOperator,
            valueHelper.ParseTypeOfNumber(enumProfiledeactivation.TypeOfNumber),
            enumProfiledeactivation.ProfileId
        );

    internal static ErrorFound FromCOIN(this C.ErrorFound errorFound, IValueHelper valueHelper)
        => new(
            errorFound.DossierId,
            valueHelper.ConvertRepeats(errorFound.Repeats, i => i.FromCOINRepeats(valueHelper))
        );

    internal static PortingPerformed FromCOIN(this C.PortingPerformed portingPerformed, IValueHelper valueHelper)
        => new(
            portingPerformed.DossierId,
            portingPerformed.RecipientNetworkOperator,
            portingPerformed.DonorNetworkOperator,
            valueHelper.ParseNullableDateTimeOffset(portingPerformed.ActualDatetime),
            valueHelper.ParseNullableBool(portingPerformed.BatchPorting),
            valueHelper.ConvertRepeats(portingPerformed.Repeats, i => i.FromCOINRepeats(valueHelper))
        );

    internal static PortingRequest FromCOIN(this C.PortingRequest portingRequest, IValueHelper valueHelper)
        => new(
            portingRequest.DossierId,
            portingRequest.RecipientNetworkOperator,
            portingRequest.RecipientServiceProvider,
            portingRequest.DonorNetworkOperator,
            portingRequest.DonorServiceProvider,
            portingRequest.CustomerInfo.FromCOIN(valueHelper),
            valueHelper.ParseContractState(portingRequest.Contract),
            portingRequest.Note,
            valueHelper.ConvertRepeats(portingRequest.Repeats, i => i.FromCOINRepeats(valueHelper))
        );

    internal static PortingRequestAnswer FromCOIN(this C.PortingRequestAnswer portingRequestAnswer, IValueHelper valueHelper)
        => new(
             portingRequestAnswer.DossierId,
             valueHelper.ParseNullableBool(portingRequestAnswer.Blocking),
             valueHelper.ConvertRepeats(portingRequestAnswer.Repeats, i => i.FromCOINRepeats(valueHelper))
        );

    internal static PortingRequestAnswerDelayed FromCOIN(this C.PortingRequestAnswerDelayed portingRequestAnswerDelayed, IValueHelper valueHelper)
        => new(
            portingRequestAnswerDelayed.DossierId,
            portingRequestAnswerDelayed.DonorNetworkOperator,
            portingRequestAnswerDelayed.DonorServiceProvider,
            valueHelper.ParseNullableDateTimeOffset(portingRequestAnswerDelayed.AnswerDueDatetime),
            portingRequestAnswerDelayed.ReasonCode
        );

    internal static RangeActivation FromCOIN(this C.RangeActivation rangeActivation, IValueHelper valueHelper)
        => new(
            rangeActivation.DossierId,
            rangeActivation.CurrentNetworkOperator,
            rangeActivation.OptaNr,
            valueHelper.ParseDateTimeOffset(rangeActivation.PlannedDatetime),
            valueHelper.ConvertRepeats<C.RangeRepeats, RangeItem>(rangeActivation.Repeats, i => i.FromCOINRepeats(valueHelper))
        );

    internal static RangeDeactivation FromCOIN(this C.RangeDeactivation rangeDeactivation, IValueHelper valueHelper)
        => new(
            rangeDeactivation.DossierId,
            rangeDeactivation.CurrentNetworkOperator,
            rangeDeactivation.OptaNr,
            valueHelper.ParseDateTimeOffset(rangeDeactivation.PlannedDatetime),
            valueHelper.ConvertRepeats<C.RangeRepeats, RangeItem>(rangeDeactivation.Repeats, i => i.FromCOINRepeats(valueHelper))
        );

    internal static TariffChangeServiceNumber FromCOIN(this C.TariffChangeServiceNumber tariffChangeServiceNumber, IValueHelper valueHelper)
        => new(
            tariffChangeServiceNumber.DossierId,
            tariffChangeServiceNumber.PlatformProvider,
            valueHelper.ParseDateTimeOffset(tariffChangeServiceNumber.PlannedDatetime),
            valueHelper.ConvertRepeats(tariffChangeServiceNumber.Repeats, i => i.FromCOINRepeats(valueHelper))
        );
}
