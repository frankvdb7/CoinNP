using COINNP.Entities.Common;

namespace COINNP.Entities;

public record Header(
    Receiver Receiver,
    Sender Sender,
    DateTimeOffset DateTimeOffset
);