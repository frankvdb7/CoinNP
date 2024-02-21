using COINNP.Entities.Enums;

namespace COINNP.Entities.Common;

public record TariffInfo(
    decimal Peak,
    decimal OffPeak,
    Currency Currency,
    TariffType TariffType,
    VAT VAT
);