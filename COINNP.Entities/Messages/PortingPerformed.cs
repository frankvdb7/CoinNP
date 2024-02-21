using COINNP.Entities.Enums;
using COINNP.Entities.Messages.Attributes;
using COINNP.Entities.SequenceItems;

namespace COINNP.Entities.Messages;

[LastMessage]
public record PortingPerformed(
    string DossierId,
    string RecipientNetworkOperator,
    string DonorNetworkOperator,
    DateTimeOffset? ActualDateTime,
    bool? BatchPorting,
    IEnumerable<PortingPerformedItem> Items
) : Message(COINMessageCode.PortingPerformed, DossierId);