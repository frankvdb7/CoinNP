namespace COINNP.Entities.SequenceItems;

public record ErrorFoundItem(
    string ErrorCode,
    string Description,
    string? PhoneNumber = null
);