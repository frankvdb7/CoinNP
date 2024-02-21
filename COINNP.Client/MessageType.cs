using C = Coin.Sdk.NP.Messages.V3;

namespace COINNP.Client;

/// <summary>
/// Defines the available (and supported) messagetypes.
/// </summary>
public enum MessageType
{
    ActivationServiceNumberV3 = C.MessageType.ActivationServiceNumberV3,
    CancelV3 = C.MessageType.CancelV3,
    DeactivationV3 = C.MessageType.DeactivationV3,
    DeactivationServiceNumberV3 = C.MessageType.DeactivationServiceNumberV3,
    EnumActivationNumberV3 = C.MessageType.EnumActivationNumberV3,
    EnumActivationOperatorV3 = C.MessageType.EnumActivationOperatorV3,
    EnumActivationRangeV3 = C.MessageType.EnumActivationRangeV3,
    EnumDeactivationNumberV3 = C.MessageType.EnumDeactivationNumberV3,
    EnumDeactivationOperatorV3 = C.MessageType.EnumDeactivationOperatorV3,
    EnumDeactivationRangeV3 = C.MessageType.EnumDeactivationRangeV3,
    EnumProfileActivationV3 = C.MessageType.EnumProfileActivationV3,
    EnumProfileDeactivationV3 = C.MessageType.EnumProfileDeactivationV3,
    ErrorFoundV3 = C.MessageType.ErrorFoundV3,
    PortingRequestV3 = C.MessageType.PortingRequestV3,
    PortingRequestAnswerV3 = C.MessageType.PortingRequestAnswerV3,
    PortingPerformedV3 = C.MessageType.PortingPerformedV3,
    PortingRequestAnswerDelayedV3 = C.MessageType.PortingRequestAnswerDelayedV3,
    RangeActivationV3 = C.MessageType.RangeActivationV3,
    RangeDeactivationV3 = C.MessageType.RangeDeactivationV3,
    TariffChangeServiceNumberV3 = C.MessageType.TariffChangeServiceNumberV3
}
