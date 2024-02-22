# COIN Number Portability Entities

This library, available on [NuGet](https://www.nuget.org/packages/COINNP.Client), provides a 'wrapper' for COIN's [Number Portability Library](https://www.nuget.org/packages/Vereniging-COIN.Sdk.NP). This library provides functionality to make life easier in an 'idiomatic C#' way, whereas COIN's library may have a few design choices that do not always align with best practices or latest standards. This library aims to solve that, without breaking COIN's API or asking COIN to change / break their API and/or NP library.

Among others, this library:

* Provides an [`NPClient`](NPClient.cs) which hides a lot of complexity
* Uses [`IOptions<T>`](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options) for configuring the `NPClient` and [`Ilogger<T>`](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging) for logging and easy integration
* Provides interfaces like [`INPMessageHandler`](INPMessageHandler.cs) which greatly simplify handling NP messages and still offering total control and flexibility
* [Async API](https://learn.microsoft.com/en-us/dotnet/csharp/asynchronous-programming/) for sending/receiving NP messages

This library uses the [COINNP.Entities](https://www.nuget.org/packages/COINNP.Entities). Switching from the [Vereniging-COIN.Sdk.NP](https://www.nuget.org/packages/Vereniging-COIN.Sdk.NP) library to this library should be straightforward; the actual object model (concerning the NP messages) is largely the same as COIN's object model.

## Quickstart

Install the NuGet package:

```ps
dotnet add package COINNP.Client
```

To be able to send and receive NP messages you need an NPClient. Let's create one:

```c#'
var npclient = new NPClient(
    Options.Create(
        new NPClientOptions()
        {
            ConsumerName = "API_T_EXAMPLE",
            EncryptedHmacSecretFile = "path/to/shared.key",
            PrivateKeyFile = "path/to/private.key",
            PrivateKeyFilePassword = "T0pS3Cr3t!",
            SSEUri = new Uri("https://test-api.coin.nl/number-portability/v3/dossiers/events"),
            RESTUri = new Uri("https://test-api.coin.nl/number-portability/v3/"),
            BackoffPeriod = TimeSpan.FromSeconds(10),
            NumberOfRetries = 15
        }
    ),
    new MyHandler()
);
```

As you can see, a handler of type `MyHandler` is provided. This handler needs to implement `INPMessageHandler`. In it's simplest form this could look like:

```c#
public class MyHandler : INPMessageHandler
{
    public Task<Acknowledgement> OnMessageAsync(MessageEnvelope messageEnvelope, CancellationToken cancellationToken = default)
    {
        // Do something with messageEnvelope here

        // Return `Ack` to acknowledge the message, false otherwise
        return Task.FromResult(Acknowledgement.ACK);
    }
    public Task OnExceptionAsync(Exception exception) => Task.CompletedTask;
    public void OnFinalDisconnect(Exception exception) { }
    public Task OnKeepAliveAsync() => Task.CompletedTask;
    public Task OnUnknownMessageAsync(string messageId, string message) => Task.CompletedTask;
}
```

Next, the `NPClient` should be started to start listening for messages. You can use any one of the [three](https://gitlab.com/verenigingcoin-public/coin-sdk-dotnet/-/blob/9e65be4bbe4c49d203da22cd985fce1547a0f3e9/number-portability-sdk/src/service/impl/NumberPortabilityMessageConsumer.cs#L31) [provided](https://gitlab.com/verenigingcoin-public/coin-sdk-dotnet/-/blob/9e65be4bbe4c49d203da22cd985fce1547a0f3e9/number-portability-sdk/src/service/impl/NumberPortabilityMessageConsumer.cs#L48) [methods](https://gitlab.com/verenigingcoin-public/coin-sdk-dotnet/-/blob/9e65be4bbe4c49d203da22cd985fce1547a0f3e9/number-portability-sdk/src/service/impl/NumberPortabilityMessageConsumer.cs#L69), which correspond with COIN's [`NumberPortabilityServiceConsumer`](https://gitlab.com/verenigingcoin-public/coin-sdk-dotnet/-/blob/9e65be4bbe4c49d203da22cd985fce1547a0f3e9/number-portability-sdk/src/service/impl/NumberPortabilityMessageConsumer.cs) methods:

```c#
npclient.StartConsumingAll();
// ...or
npclient.StartConsumingUnconfirmed(...);
// ...or
npclient.StartConsumingUnconfirmedWithOffsetPersistence(...);

```

From that point on, the `OnMessageAsync` method on the `MyHandler` will be invoked for every received NP message (after it passes any messagetype filters).

Sending a message is even simpler. For brevity the below example uses a simple `Cancel` message:

```c#
var cancel = new MessageEnvelope(
    new Header(
        new Sender("SNDO", "SSP"),
        new Receiver("RCVO", "RSP"),
        DateTimeOffset.Now
    ),
    new Cancel("FOO-123456789", "Hello world!")
);

await client.SendMessageAsync(cancel);
```

You are now ready to send, receive and handle messages.

## SimpleNPMessageHandler

We provide a `SimpleNPMessageHandler`; a basic class that implements `INPMessageHandler` and does the following:

* Messages are passed to an [`OnMessageDelegate`](../-/blob/d921048f8bdaf4113116ee04b1af667c3154eb39/COINNP.Client/SimpleNPMessageHandler.cs#L15)
* Other events (keepalive, unknownmessage, exception and finaldisconnect) are **not** handled except for being logged to an optional logger (which defaults to a [`NullLoger`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.abstractions.nulllogger), so effectively no logger).

This simplifies implementing a messagehandler greatly:

```c#
var npclient = new NPClient(
    ... // Options left out for brevity
    new SimpleNPMessageHandler(OnMessageAsync)
);

private static Task<Acknowledgement> OnMessageAsync(MessageEnvelope messageEnvelope, CancellationToken cancellationToken = default)
{
    // Do something with messageEnvelope
}
```

This does, however, have it's limitations as the name suggests. One can only handle messages this way. However, the `SimpleNPMessageHandler`'s methods are virtual which means you can pick-and-choose to implement methods by inheriting from [`SimpleMessageHandler`](SimpleMessageHandler.cs):

```c#
public class MyMessageHandler : SimpleNPMessageHandler
{
    public MyMessageHandler()
        : base(OnMessageAsyncImpl) { }

    private static Task<Acknowledgement> OnMessageAsyncImpl(MessageEnvelope messageEnvelope, CancellationToken cancellationToken = default)
    {
        // Do something with messageEnvelope here...
        return Task.FromResult(Acknowledgement.ACK);
    }

    public override Task OnExceptionAsync(Exception exception)
    {
        // Do something with exception here...
        return Task.CompletedTask;
    }

    public override Task OnUnknownMessageAsync(string messageId, string message)
    {
        // Do something with message / messageId here...
        return Task.CompletedTask;
    }
}
```

Using the `SimpleNPMessageHandler` is discouraged, implementing an [`INPMessageHandler`](INPMessageHandler.cs) is encouraged. The `SimpleMessageHandler` is provided mostly for convenience purposes where only a simple delegate (`OnMessageDelegate`) is needed to get quick results.

## IOptions<> and Dependency Injection

We provide [`NPClientOptions`](NPClientOptions.cs) which can be passed to the [`NPClient`](NPClient.cs) constructor which will convert it internally to an [`NPClientConfiguration`](NPClientConfiguration.cs) which will in turn be used by the `NPClient` for it's configuration. This means it's easy to configure the `NPClient`:

```c#
// Read configuration file and create a configurtion provider
var configprovider = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json")
    .Build();

// Set up a DI provider
var serviceprovider = new ServiceCollection()
    .Configure<NPClientOptions>(configprovider.GetRequiredSection("NPClient"))
    .AddSingleton<INPMessageHandler, MyMessageHandler>()  // Register messagehandler
    .AddSingleton<INPClient, NPClient>()                // Register NPClient
    .BuildServiceProvider();

// Create NPClient
var npclient = serviceprovider.GetRequiredService<INPClient>();
```

And in `appsettings.json`:

```json
{
  "NPClient": {
    "ConsumerName": "API_T_EXAMPLE",
    "EncryptedHMacSecretFile": "path/to/shared.key",
    "PrivateKeyFile": "path/to/private.key",
    "PrivateKeyFilePassword": "T0pS3Cr3t",
    "SSEUri": "https://test-api.coin.nl/number-portability/v3/dossiers/events",
    "RESTUri": "https://test-api.coin.nl/number-portability/v3/",
    "BackoffPeriod": "0:00:01",
    "NumberOfRetries": 15
  }
}
```

## NPClientOptions vs NPClientConfiguration

If you want more control over the `NPClient`'s configuration (for example when the keys are not stored in files but elsewhere) you can initialize an `NPClientConfiguration`:

```c#
var config = new NPClientConfiguration(...);
```

This class offers a static [FromNPClientOptions()](../-/blob/d921048f8bdaf4113116ee04b1af667c3154eb39/COINNP.Client/NPClientConfiguration.cs#L27) convenience method that takes `NPClientOptions` but also provides convenience methods to read the required [`RSA`](https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.rsa) and [`HMACSHA256`](https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.hmacsha256) from files. If these are stored elsehwere you can simply provide an `RSA` and `HMACSHA256` to the `NPClientConfiguration`'s constructor.

## IValueHelper and ValueHelper

When converting messages from- and to [COIN NP messages](https://www.nuget.org/packages/Vereniging-COIN.Sdk.NP) some conversion is done. Values like `Y` and `N` are converted into actual `booleans` or the value `3` is converted into `VAT.High` for a [`VAT`](../-/blob/69c86203d93565b8fdd37e3e973e3427d463f84a/COINNPClient/Models/Common/TariffInfo.cs#L11) property etc. etc. and vice versa. During this process an `IValueHelper` is used. This interface provides methods that can affect the conversion.

We provide a [`ValueHelper`](ValueHelper.cs) class which exposes a [`Default`](../-/blob/69c86203d93565b8fdd37e3e973e3427d463f84a/COINNPClient/Models/ValueHelper.cs#L16) instance with default values. An instance of a `ValueHelper` can be instantiated providing a [`ValueHelperOptions`](ValueHelperOptions.cs) object. This class controls the `ValueHelper` and can be used to change the handling of "COIN values" to- and from this library's messages. For example, you could change the default date format (which is `yyyyMMddHHmmss`) to any format you like. However, **be aware that changing these values may result in the COIN NP message being incompatible with COIN's NP process!** We recommend implementing an `IValueHelper` only when the default `ValueHelper` doesn't provide the required conversion(s).

An `IValueHelper` can be provided to an `NPClient`'s constructor which will then use the provided `IValueHelper` internally when converting to- (sending) and from (receiving) COIN NP messages.
