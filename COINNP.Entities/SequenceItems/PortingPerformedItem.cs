using COINNP.Entities.Common;

namespace COINNP.Entities.SequenceItems;

public record PortingPerformedItem(
    NumberSerie NumberSerie,
    bool? BackPorting = null,
    IEnumerable<EnumProfile>? EnumProfiles = null,
    string? PoP = null
);