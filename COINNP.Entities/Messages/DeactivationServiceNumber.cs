using COINNP.Entities.Enums;
using COINNP.Entities.Messages.Attributes;
using COINNP.Entities.SequenceItems;

namespace COINNP.Entities.Messages;

[SingleMessage]
public record DeactivationServiceNumber(
    string DossierId,
    string PlatformProvider,
    DateTimeOffset PlannedDateTime,
    IEnumerable<DeactivationServiceNumberItem> Items
) : Message(COINMessageCode.DeactivationServiceNumber, DossierId);