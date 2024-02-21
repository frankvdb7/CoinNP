using COINNP.Entities.Enums;
using COINNP.Entities.Messages.Attributes;

namespace COINNP.Entities.Messages;

[SingleMessage]
public record EnumProfileDeactivation(
    string DossierId,
    string CurrentNetworkOperator,
    TypeOfNumber TypeOfNumber,
    string ProfileId
) : Message(COINMessageCode.EnumProfileDeactivation, DossierId);