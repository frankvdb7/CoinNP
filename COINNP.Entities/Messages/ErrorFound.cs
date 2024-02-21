using COINNP.Entities.Enums;
using COINNP.Entities.Messages.Attributes;
using COINNP.Entities.SequenceItems;

namespace COINNP.Entities.Messages;

[SingleMessage]
public record ErrorFound(
    string DossierId,
    IEnumerable<ErrorFoundItem> Errors
) : Message(COINMessageCode.ErrorFound, DossierId);