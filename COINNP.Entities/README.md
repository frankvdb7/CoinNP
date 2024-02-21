# COIN Number Portability Entities

This library, available on [NuGet](https://www.nuget.org/packages/COINNP.Entities), provides the COIN Number Porting (NP) messages as [records](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/tutorials/records). These messages are a close representation of the messages defined by [Vereniging-COIN.Sdk.NP](https://www.nuget.org/packages/Vereniging-COIN.Sdk.NP). This library provides functionality to make life easier in an 'idiomatic C#' way, whereas COIN's library may have a few design choices that do not always align with best practices or latest standards. This library aims to solve that, without breaking COIN's API or asking COIN to change / break their API and/or NP library.

Among others, this library:

* Uses [records](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/tutorials/records) (immutable) for models (i.e. NP messages and the related objects)
* Uses correct types (like [TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan?view=net-8.0) for [Ttl](https://gitlab.com/verenigingcoin-public/coin-sdk-dotnet/-/blob/9e65be4bbe4c49d203da22cd985fce1547a0f3e9/number-portability-sdk/src/messages/v3/EnumProfileActivation.cs#L49), [DateTimeOffset](https://learn.microsoft.com/en-us/dotnet/api/system.datetimeoffset?view=net-8.0) for [datetime](https://gitlab.com/verenigingcoin-public/coin-sdk-dotnet/-/blob/9e65be4bbe4c49d203da22cd985fce1547a0f3e9/number-portability-sdk/src/messages/v3/PortingPerformed.cs#L42) [values](https://gitlab.com/verenigingcoin-public/coin-sdk-dotnet/-/blob/9e65be4bbe4c49d203da22cd985fce1547a0f3e9/number-portability-sdk/src/messages/v3/RangeActivation.cs#L42), `bool`s for [boolean](https://gitlab.com/verenigingcoin-public/coin-sdk-dotnet/-/blob/9e65be4bbe4c49d203da22cd985fce1547a0f3e9/number-portability-sdk/src/messages/v3/PortingRequestAnswer.cs#L34) [values](https://gitlab.com/verenigingcoin-public/coin-sdk-dotnet/-/blob/9e65be4bbe4c49d203da22cd985fce1547a0f3e9/number-portability-sdk/src/messages/v3/PortingPerformed.cs#L46), `decimal`s for [monetary values](https://gitlab.com/verenigingcoin-public/coin-sdk-dotnet/-/blob/9e65be4bbe4c49d203da22cd985fce1547a0f3e9/number-portability-sdk/src/messages/v3/TariffChangeServiceNumber.cs#L67), actual enums for [enum](https://gitlab.com/verenigingcoin-public/coin-sdk-dotnet/-/blob/9e65be4bbe4c49d203da22cd985fce1547a0f3e9/number-portability-sdk/src/messages/v3/PortingRequest.cs#L54) [values](https://gitlab.com/verenigingcoin-public/coin-sdk-dotnet/-/blob/9e65be4bbe4c49d203da22cd985fce1547a0f3e9/number-portability-sdk/src/messages/v3/EnumActivationNumber.cs#L38) [like](https://gitlab.com/verenigingcoin-public/coin-sdk-dotnet/-/blob/9e65be4bbe4c49d203da22cd985fce1547a0f3e9/number-portability-sdk/src/messages/v3/TariffChangeServiceNumber.cs#L79) [these](https://gitlab.com/verenigingcoin-public/coin-sdk-dotnet/-/blob/9e65be4bbe4c49d203da22cd985fce1547a0f3e9/number-portability-sdk/src/messages/v3/TariffChangeServiceNumber.cs#L83) etc.)
* Hides [`Content`](https://gitlab.com/verenigingcoin-public/coin-sdk-dotnet/-/blob/9e65be4bbe4c49d203da22cd985fce1547a0f3e9/number-portability-sdk/src/messages/v3/PortingRequest.cs#L23), [`Seq`](https://gitlab.com/verenigingcoin-public/coin-sdk-dotnet/-/blob/9e65be4bbe4c49d203da22cd985fce1547a0f3e9/number-portability-sdk/src/messages/v3/PortingRequest.cs#L96) and [`Repeats`](https://gitlab.com/verenigingcoin-public/coin-sdk-dotnet/-/blob/9e65be4bbe4c49d203da22cd985fce1547a0f3e9/number-portability-sdk/src/messages/v3/PortingRequest.cs#L62) etc. and makes enumerable items on an object "first-class citizens" implemented as `IEnumerable`s, for example:
    ```c#
    public record RangeActivation(
        string DossierId,
        string CurrentNetworkOperator,
        string OptaNr,
        DateTimeOffset PlannedDateTime,
        IEnumerable<RangeActivationItem> Items
    ) : Message(209, DossierId) { }

    public record RangeActivationItem(
        NumberSerie NumberSerie,
        string? PoP
    );
    ```
    Which simplifies use.
* Is [nullable aware](https://learn.microsoft.com/en-us/dotnet/csharp/nullable-references)
* Provides NP messages that are actually serializable to/from JSON without additional work; *It-just-works*™ because they're plain [POCO's](https://stackoverflow.com/a/250006/215042) (with the exception of [`Message`](Message.cs) which contains [type discriminators](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/polymorphism?pivots=dotnet-7-0#polymorphic-type-discriminators) because of it's polymorphic nature).

This library is used by [COINNP.Client](https://www.nuget.org/packages/COINNP.Client).