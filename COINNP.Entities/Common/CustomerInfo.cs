namespace COINNP.Entities.Common;

public record CustomerInfo(
    string? LastName = null,
    string? CompanyName = null,
    string? HouseNr = null,
    string? HouseNrExt = null,
    string? Postcode = null,
    string? CustomerId = null
);