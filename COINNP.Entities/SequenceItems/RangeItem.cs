using COINNP.Entities.Common;

namespace COINNP.Entities.SequenceItems;

public record RangeItem(
    NumberSerie NumberSerie,
    string? PoP = null
);