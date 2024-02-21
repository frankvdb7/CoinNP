using COINNP.Entities.Common;

namespace COINNP.Entities.SequenceItems;

public record EnumNumberItem(
    NumberSerie NumberSerie,
    IEnumerable<EnumProfile> EnumProfiles
);