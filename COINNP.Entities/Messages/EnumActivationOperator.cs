using COINNP.Entities.Enums;
using COINNP.Entities.Messages.Attributes;
using COINNP.Entities.SequenceItems;

namespace COINNP.Entities.Messages;

[SingleMessage]
public record EnumActivationOperator(
    string DossierId,
    string CurrentNetworkOperator,
    TypeOfNumber TypeOfNumber,
    IEnumerable<EnumOperatorItem> Items
) : Message(COINMessageCode.EnumActivationOperator, DossierId);