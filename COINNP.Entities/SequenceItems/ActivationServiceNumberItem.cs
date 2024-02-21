using COINNP.Entities.Common;

namespace COINNP.Entities.SequenceItems;

public record ActivationServiceNumberItem(
    NumberSerie NumberSerie,
    TariffInfo TariffInfo,
    string PoP
);