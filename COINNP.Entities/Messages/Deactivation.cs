using COINNP.Entities.Enums;
using COINNP.Entities.Messages.Attributes;
using COINNP.Entities.SequenceItems;

namespace COINNP.Entities.Messages;

[SingleMessage]
public record Deactivation(
    string DossierId,
    string CurrentNetworkOperator,
    string OriginalNetworkOperator,
    IEnumerable<DeactivationItem> Items
) : Message(COINMessageCode.Deactivation, DossierId);