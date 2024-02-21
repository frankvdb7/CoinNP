using COINNP.Entities.Enums;
using COINNP.Entities.Messages;
using System.Text.Json.Serialization;

namespace COINNP.Entities;

[JsonDerivedType(typeof(ActivationServiceNumber), typeDiscriminator: nameof(ActivationServiceNumber))]
[JsonDerivedType(typeof(Cancel), typeDiscriminator: nameof(Cancel))]
[JsonDerivedType(typeof(Deactivation), typeDiscriminator: nameof(Deactivation))]
[JsonDerivedType(typeof(DeactivationServiceNumber), typeDiscriminator: nameof(DeactivationServiceNumber))]
[JsonDerivedType(typeof(EnumActivationNumber), typeDiscriminator: nameof(EnumActivationNumber))]
[JsonDerivedType(typeof(EnumActivationOperator), typeDiscriminator: nameof(EnumActivationOperator))]
[JsonDerivedType(typeof(EnumActivationRange), typeDiscriminator: nameof(EnumActivationRange))]
[JsonDerivedType(typeof(EnumDeactivationNumber), typeDiscriminator: nameof(EnumDeactivationNumber))]
[JsonDerivedType(typeof(EnumDeactivationOperator), typeDiscriminator: nameof(EnumDeactivationOperator))]
[JsonDerivedType(typeof(EnumDeactivationRange), typeDiscriminator: nameof(EnumDeactivationRange))]
[JsonDerivedType(typeof(EnumProfileActivation), typeDiscriminator: nameof(EnumProfileActivation))]
[JsonDerivedType(typeof(EnumProfileDeactivation), typeDiscriminator: nameof(EnumProfileDeactivation))]
[JsonDerivedType(typeof(PortingRequest), typeDiscriminator: nameof(PortingRequest))]
[JsonDerivedType(typeof(PortingRequestAnswer), typeDiscriminator: nameof(PortingRequestAnswer))]
[JsonDerivedType(typeof(PortingPerformed), typeDiscriminator: nameof(PortingPerformed))]
[JsonDerivedType(typeof(PortingRequestAnswerDelayed), typeDiscriminator: nameof(PortingRequestAnswerDelayed))]
[JsonDerivedType(typeof(RangeActivation), typeDiscriminator: nameof(RangeActivation))]
[JsonDerivedType(typeof(RangeDeactivation), typeDiscriminator: nameof(RangeDeactivation))]
[JsonDerivedType(typeof(TariffChangeServiceNumber), typeDiscriminator: nameof(TariffChangeServiceNumber))]
[JsonDerivedType(typeof(ErrorFound), typeDiscriminator: nameof(ErrorFound))]
public abstract record Message : IMessage
{
    public int MessageCode { get; private set; }
    public string DossierId { get; init; }

    public Message(int messageCode, string dossierId)
    {
        MessageCode = messageCode;
        DossierId = dossierId;
    }

    public Message(COINMessageCode messageCode, string dossierId)
        : this((int)messageCode, dossierId) { }
}