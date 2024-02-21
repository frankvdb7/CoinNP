using COINNP.Entities.Enums;
using COINNP.Entities.Messages.Attributes;

namespace COINNP.Entities.Messages;

[FollowupMessages(typeof(PortingRequestAnswer))]
public record PortingRequestAnswerDelayed(
    string DossierId,
    string DonorNetworkOperator,
    string? DonorServiceProvider = null,
    DateTimeOffset? AnswerDueDateTime = null,
    string? ReasonCode = null
) : Message(COINMessageCode.PortingRequestAnswerDelayed, DossierId);