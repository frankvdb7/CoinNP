using COINNP.Entities.Enums;

namespace COINNP.Client.Mapping;

public interface IValueHelper
{
    ContractState? ParseContractState(string? value);
    string? SerializeContractState(ContractState? value);

    Currency ParseCurrencyType(string value);
    string SerializeCurrencyType(Currency value);

    DateTimeOffset ParseDateTimeOffset(string value);
    string SerializeDateTimeOffset(DateTimeOffset value);

    DateTimeOffset? ParseNullableDateTimeOffset(string? value);
    string? SerializeNullableDateTimeOffset(DateTimeOffset? value);

    decimal ParseCurrency(string value);
    string SerializeCurrency(decimal value);

    bool? ParseNullableBool(string? value);
    string? SerializeNullableBool(bool? value);

    int? ParseNullableInt(string value);
    string? SerializeNullableInt(int? value);

    TimeSpan ParseTimeSpan(string value);
    string SerializeTimeSpan(TimeSpan value);

    bool ParseBool(string value);
    string SerializeBool(bool value);

    TypeOfNumber ParseTypeOfNumber(string value);
    string SerializeTypeOfNumber(TypeOfNumber value);

    TariffType ParseTariffType(string value);
    string SerializeTariffType(TariffType value);

    VAT ParseVAT(string value);
    string SerializeVAT(VAT value);

    IEnumerable<TDest> ConvertRepeats<TSource, TDest>(IEnumerable<TSource> repeats, Func<TSource, TDest> factory);
    List<TDest> ConvertItems<TSource, TDest>(IEnumerable<TSource> items, Func<TSource, TDest> factory);
}