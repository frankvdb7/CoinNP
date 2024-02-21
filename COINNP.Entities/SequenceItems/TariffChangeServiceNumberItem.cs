using COINNP.Entities.Common;

namespace COINNP.Entities.SequenceItems;

public record TariffChangeServiceNumberItem(
    NumberSerie NumberSerie,
    TariffInfo TariffInfoNew
);