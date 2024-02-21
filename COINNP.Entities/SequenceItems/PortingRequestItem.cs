using COINNP.Entities.Common;

namespace COINNP.Entities.SequenceItems;

public record PortingRequestItem(
    NumberSerie NumberSerie,
    IEnumerable<EnumProfile>? EnumProfiles = null
);