using COINNP.Entities.Enums;
using COINNP.Entities.Messages.Attributes;
using COINNP.Entities.SequenceItems;

namespace COINNP.Entities.Messages;

[SingleMessage]
public record EnumActivationRange(
    string DossierId,
    string CurrentNetworkOperator,
    TypeOfNumber TypeOfNumber,
    IEnumerable<EnumNumberItem> Items
) : Message(COINMessageCode.EnumActivationRange, DossierId);