using COINNP.Entities.Enums;
using COINNP.Entities.Messages.Attributes;

namespace COINNP.Entities.Messages;

[SingleMessage]
public record EnumProfileActivation(
    string DossierId,
    string CurrentNetworkOperator,
    TypeOfNumber TypeOfNumber,
    string Scope,
    string ProfileId,
    TimeSpan Ttl,
    string DnsClass,
    string RecType,
    int? Order = null,
    int? Preference = null,
    string? Flags = null,
    string? EnumService = null,
    string? Regexp = null,
    string? UserTag = null,
    string? Domain = null,
    string? SpCode = null,
    string? ProcessType = null,
    string? Gateway = null,
    string? Service = null,
    string? DomainTag = null,
    string? Replacement = null
) : Message(COINMessageCode.EnumProfileActivation, DossierId);