using COINNP.Entities.Enums;
using COINNP.Entities.Messages.Attributes;

namespace COINNP.Entities.Messages;

[LastMessage]
public record Cancel(
    string DossierId,
    string? Note = null
) : Message(COINMessageCode.Cancel, DossierId);