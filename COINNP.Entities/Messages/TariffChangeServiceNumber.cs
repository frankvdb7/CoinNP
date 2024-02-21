using COINNP.Entities.Enums;
using COINNP.Entities.Messages.Attributes;
using COINNP.Entities.SequenceItems;
namespace COINNP.Entities.Messages;

[SingleMessage]
public record TariffChangeServiceNumber(
    string DossierId,
    string PlatformProvider,
    DateTimeOffset PlannedDateTime,
    IEnumerable<TariffChangeServiceNumberItem> Items
) : Message(COINMessageCode.TariffChangeServiceNumber, DossierId);