using COINNP.Entities.Enums;
using COINNP.Entities.Messages.Attributes;
using COINNP.Entities.SequenceItems;

namespace COINNP.Entities.Messages;

[SingleMessage]
public record RangeDeactivation(
    string DossierId,
    string CurrentNetworkOperator,
    string OptaNr,
    DateTimeOffset PlannedDateTime,
    IEnumerable<RangeItem> Items
) : Message(COINMessageCode.RangeDeactivation, DossierId);