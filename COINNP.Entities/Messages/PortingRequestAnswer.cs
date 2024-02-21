using COINNP.Entities.Enums;
using COINNP.Entities.Messages.Attributes;
using COINNP.Entities.SequenceItems;

namespace COINNP.Entities.Messages;

[FollowupMessages(typeof(Cancel), typeof(PortingPerformed))]
public record PortingRequestAnswer(
    string DossierId,
    bool? Blocking,
    IEnumerable<PortingRequestAnswerItem> Items
) : Message(COINMessageCode.PortingRequestAnswer, DossierId);