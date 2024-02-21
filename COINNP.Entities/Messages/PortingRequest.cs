using COINNP.Entities.Common;
using COINNP.Entities.Enums;
using COINNP.Entities.Messages.Attributes;
using COINNP.Entities.SequenceItems;

namespace COINNP.Entities.Messages;

[FirstMessage]
[FollowupMessages(typeof(PortingRequestAnswerDelayed), typeof(PortingRequestAnswer))]
public record PortingRequest(
    string DossierId,
    string RecipientNetworkOperator,
    string? RecipientServiceProvider,
    string? DonorNetworkOperator,
    string? DonorServiceProvider,
    CustomerInfo? CustomerInfo,
    ContractState? Contract,
    string? Note,
    IEnumerable<PortingRequestItem> Items
) : Message(COINMessageCode.PortingRequest, DossierId);