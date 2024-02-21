using COINNP.Entities.Common;

namespace COINNP.Entities.SequenceItems;

public record PortingRequestAnswerItem(
    NumberSerie NumberSerie,
    string? BlockingCode = null,
    DateTimeOffset? FirstPossibleDate = null,
    string? Note = null,
    string? DonorNetworkOperator = null,
    string? DonorServiceProvider = null
);