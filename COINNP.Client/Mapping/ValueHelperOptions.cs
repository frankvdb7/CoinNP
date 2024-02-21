using System.Globalization;
using System.Runtime.InteropServices;

namespace COINNP.Client.Mapping;

public record ValueHelperOptions
{
    public static readonly TimeZoneInfo DefaultTimeZone = TimeZoneInfo.FindSystemTimeZoneById(
        RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
            ? "Europe/Amsterdam"
            : "W. Europe Standard Time"
    );
    public static readonly NumberFormatInfo DefaultNumberFormatInfo = new()
    {
        CurrencyDecimalSeparator = ",",
        CurrencyDecimalDigits = 5,
        CurrencySymbol = string.Empty,
        CurrencyGroupSeparator = string.Empty,
        NumberDecimalSeparator = ",",
        NumberDecimalDigits = 0,
        NumberGroupSeparator = string.Empty
    };
    public static readonly DateTimeFormatInfo DefaultDateTimeFormatInfo = new()
    {
        FullDateTimePattern = "yyyyMMddHHmmss"
    };

    public bool IgnoreCase { get; init; } = false;
    public string TrueValue { get; init; } = "Y";
    public string FalseValue { get; init; } = "N";
    public NumberFormatInfo CurrencyFormatInfo { get; init; } = DefaultNumberFormatInfo;
    public NumberFormatInfo IntegerFormatInfo { get; init; } = DefaultNumberFormatInfo;
    public DateTimeFormatInfo DateTimeFormatInfo { get; init; } = DefaultDateTimeFormatInfo;
    public TimeZoneInfo TimeZone { get; init; } = DefaultTimeZone;

    public static readonly ValueHelperOptions Default = new();
}
