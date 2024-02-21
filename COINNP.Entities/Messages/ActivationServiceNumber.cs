using COINNP.Entities.Enums;
using COINNP.Entities.Messages.Attributes;
using COINNP.Entities.SequenceItems;

namespace COINNP.Entities.Messages;

[SingleMessage]
public record ActivationServiceNumber(
    string DossierId,
    string PlatformProvider,
    DateTimeOffset PlannedDateTime,
    string? Note,
    IEnumerable<ActivationServiceNumberItem> Items
) : Message(COINMessageCode.ActivationServiceNumer, DossierId);