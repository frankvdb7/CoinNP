using COINNP.Entities.Common;

namespace COINNP.Entities.SequenceItems;

public record DeactivationServiceNumberItem(
    NumberSerie NumberSerie,
    string PoP
);