using COINNP.Client.ResourceFiles;
using COINNP.Entities;
using COINNP.Entities.Enums;
using Microsoft.Extensions.Options;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;

namespace COINNP.Client.Mapping;

public class ValueHelper : IValueHelper
{
    private static readonly IDictionary<ContractState, string> _contractstatestrings =
        Enum.GetValues(typeof(ContractState)).OfType<ContractState>()
        .ToDictionary(
            k => k,
            v => typeof(ContractState).GetMember(v.ToString())[0].GetCustomAttribute<EnumValueAttribute>()?.Value ?? v.ToString()
        );
    private readonly IDictionary<string, ContractState> _contractstatevalues;
    private readonly ValueHelperOptions _options;

    public static readonly ValueHelper Default = new(Options.Create(ValueHelperOptions.Default));

    public ValueHelper(IOptions<ValueHelperOptions> options)
    {
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _contractstatevalues = _contractstatestrings.ToDictionary(kv => kv.Value, kv => kv.Key, _options.IgnoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal);
    }

    public ContractState? ParseContractState(string? value)
        => IsNullOrEmpty(value) ? null : _contractstatevalues.TryGetValue(value, out var result) ? result : throw new InvalidOperationException(string.Format(Translations.ERR_Unknown_Enum_Value, value, typeof(ContractState).Name));
    public string? SerializeContractState(ContractState? value)
        => value == null ? null : _contractstatestrings[value.Value];

    public DateTimeOffset? ParseNullableDateTimeOffset(string? value)
        => IsNullOrEmpty(value)
        ? null
        : ParseDateTimeOffset(value);
    public string? SerializeNullableDateTimeOffset(DateTimeOffset? value)
        => value.HasValue
        ? SerializeDateTimeOffset(value.Value)
        : null;

    public DateTimeOffset ParseDateTimeOffset(string value)
    {
        var date = DateTime.ParseExact(value, _options.DateTimeFormatInfo.FullDateTimePattern, _options.DateTimeFormatInfo);
        return new DateTimeOffset(
            date,
            _options.TimeZone.GetUtcOffset(date)
        );
    }
    public string SerializeDateTimeOffset(DateTimeOffset value)
        => TimeZoneInfo.ConvertTime(value, _options.TimeZone).ToString("F", _options.DateTimeFormatInfo);

    public bool? ParseNullableBool(string? value)
        => IsNullOrEmpty(value)
        ? null
        : ParseBool(value);
    public string? SerializeNullableBool(bool? value)
        => value.HasValue
        ? SerializeBool(value.Value)
        : null;

    public bool ParseBool(string value)
        => string.Equals(value, _options.TrueValue, _options.IgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal)
        || (string.Equals(value, _options.FalseValue, _options.IgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal)
        ? false
        : throw new InvalidOperationException(string.Format(Translations.ERR_Unknown_Enum_Value, value, typeof(bool).Name)));
    public string SerializeBool(bool value)
        => value
        ? _options.TrueValue
        : _options.FalseValue;

    public int? ParseNullableInt(string? value)
        => IsNullOrEmpty(value)
        ? null
        : int.Parse(value, NumberStyles.Integer, _options.IntegerFormatInfo);
    public string? SerializeNullableInt(int? value)
        => value.HasValue
        ? value.Value.ToString("N", _options.IntegerFormatInfo)
        : null;

    public TimeSpan ParseTimeSpan(string value)
        => TimeSpan.FromSeconds(int.Parse(value));
    public string SerializeTimeSpan(TimeSpan value)
        => value.TotalSeconds.ToString("N", _options.IntegerFormatInfo);

    public decimal ParseCurrency(string value)
        => decimal.Parse(value, NumberStyles.Currency, _options.CurrencyFormatInfo);
    public string SerializeCurrency(decimal value)
        => value.ToString("C", _options.CurrencyFormatInfo);

    public Currency ParseCurrencyType(string value)
        => ParseEnum<Currency>(value);

    public string SerializeCurrencyType(Currency value)
        => $"{(int)value}";

    public TypeOfNumber ParseTypeOfNumber(string value)
        => ParseEnum<TypeOfNumber>(value);
    public string SerializeTypeOfNumber(TypeOfNumber value)
        => $"{(int)value}";

    public TariffType ParseTariffType(string value)
        => ParseEnum<TariffType>(value);
    public string SerializeTariffType(TariffType value)
        => $"{(int)value}";

    public VAT ParseVAT(string value)
        => ParseEnum<VAT>(value);
    public string SerializeVAT(VAT value)
        => $"{(int)value}";


    public IEnumerable<TDest> ConvertRepeats<TSource, TDest>(IEnumerable<TSource> repeats, Func<TSource, TDest> factory)
        => repeats == null
        ? throw new ArgumentNullException(nameof(repeats))
        : (IEnumerable<TDest>)repeats.Select(i => factory(i)).ToImmutableArray();

    public List<TDest> ConvertItems<TSource, TDest>(IEnumerable<TSource> items, Func<TSource, TDest> factory)
        => items == null
        ? throw new ArgumentNullException(nameof(items))
        : items.Select(i => factory(i)).ToList();

    private T ParseEnum<T>(string value)
        where T : struct, Enum
    {
        if (!int.TryParse(value, out var r) || !Enum.IsDefined(typeof(T), r))
        {
            throw new InvalidOperationException(string.Format(Translations.ERR_Unknown_Enum_Value, value, typeof(T).Name));
        }

        return (T)Enum.Parse(typeof(T), value, _options.IgnoreCase);
    }

    // NetStandard 2.0 is not nullable-aware for string.IsNullOrEmpty() so that's why this is here
    private static bool IsNullOrEmpty([NotNullWhen(false)] string? data)
        => string.IsNullOrEmpty(data);
}