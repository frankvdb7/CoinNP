using COINNP.Client.Adapters;
using COINNP.Client.Mapping;
using COINNP.Client.ResourceFiles;
using COINNP.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using CC = Coin.Sdk.Common.Client;
using CI = Coin.Sdk.NP.Service.Impl;
using CM = Coin.Sdk.NP.Messages.V3;
using CS = Coin.Sdk.NP.Service;

namespace COINNP.Client;

/// <summary>
/// COIN NumberPortability client
/// </summary>
/// <remarks>
/// Provides a simple(r) NP client API than the COIN-provided client whilst keeping the same functionality. This client
/// is entirely a wrapper around the COIN-provided classes.
/// </remarks>
public class NPClient : IDisposable, INPClient
{
    private readonly ILogger<NPClient> _logger;
    private readonly CC.SseConsumer _sseconsumer;
    private readonly CI.NumberPortabilityService _numberportabilityservice;
    private readonly CI.NumberPortabilityMessageConsumer _numberportabilitymessageconsumer;
    private readonly INPMessageHandler _messagehandler;
    private readonly NPClientConfiguration _configuration;
    private readonly object _lock = new();
    private readonly IValueHelper _valuehelper;

    private CS.INumberPortabilityMessageListener? _numberportabilitymessagelistener = null;
    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of an <see cref="NPClient"/>.
    /// </summary>
    /// <param name="options">Options to initialize the <see cref="NPClient"/>.</param>
    /// <param name="messageHandler">The <see cref="INPMessageHandler"/> to use when handling messages.</param>
    /// <param name="valueHelper">
    /// The <see cref="IValueHelper"/> to use when creating COIN messages from messages. Uses <see cref="ValueHelper.Default"/>
    /// when no valuehelper is specified.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="logger"/>, <paramref name="options"/> or <paramref name="messageHandler"/> are null.
    /// </exception>
    public NPClient(IOptions<NPClientOptions> options, INPMessageHandler messageHandler, IValueHelper? valueHelper = null)
        : this(NullLogger<NPClient>.Instance, NPClientConfiguration.FromNPClientOptions(options), messageHandler, valueHelper) { }

    /// <summary>
    /// Initializes a new instance of an <see cref="NPClient"/>.
    /// </summary>
    /// <param name="logger">The logger to log to.</param>
    /// <param name="options">Options to initialize the <see cref="NPClient"/>.</param>
    /// <param name="messageHandler">The <see cref="INPMessageHandler"/> to use when handling messages.</param>
    /// <param name="valueHelper">
    /// The <see cref="IValueHelper"/> to use when creating COIN messages from messages. Uses <see cref="ValueHelper.Default"/>
    /// when no valuehelper is specified.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="logger"/>, <paramref name="options"/> or <paramref name="messageHandler"/> are null.
    /// </exception>
    public NPClient(ILogger<NPClient> logger, IOptions<NPClientOptions> options, INPMessageHandler messageHandler, IValueHelper? valueHelper = null)
        : this(logger, NPClientConfiguration.FromNPClientOptions(options), messageHandler, valueHelper) { }

    /// <summary>
    /// Initializes a new instance of an <see cref="NPClient"/>.
    /// </summary>
    /// <param name="configuration">Configuration for the <see cref="NPClient"/>.</param>
    /// <param name="messageHandler">The <see cref="INPMessageHandler"/> to use when handling messages.</param>
    /// <param name="valueHelper">
    /// The <see cref="IValueHelper"/> to use when creating COIN messages from messages. Uses <see cref="ValueHelper.Default"/>
    /// when no valuehelper is specified.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="logger"/>, <paramref name="options"/> or <paramref name="messageHandler"/> are null.
    /// </exception>
    public NPClient(NPClientConfiguration configuration, INPMessageHandler messageHandler, IValueHelper? valueHelper = null)
        : this(NullLogger<NPClient>.Instance, configuration, messageHandler, valueHelper) { }

    /// <summary>
    /// Initializes a new instance of an <see cref="NPClient"/>.
    /// </summary>
    /// <param name="logger">The logger to log to.</param>
    /// <param name="configuration">Configuration for the <see cref="NPClient"/>.</param>
    /// <param name="messageHandler">The <see cref="INPMessageHandler"/> to use when handling messages.</param>
    /// <param name="valueHelper">
    /// The <see cref="IValueHelper"/> to use when creating COIN messages from messages. Uses <see cref="ValueHelper.Default"/>
    /// when no valuehelper is specified.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="logger"/>, <paramref name="options"/> or <paramref name="messageHandler"/> are null.
    /// </exception>
    public NPClient(ILogger<NPClient> logger, NPClientConfiguration configuration, INPMessageHandler messageHandler, IValueHelper? valueHelper = null)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _messagehandler = messageHandler ?? throw new ArgumentNullException(nameof(messageHandler));
        _valuehelper = valueHelper ?? ValueHelper.Default;

        _sseconsumer = new CC.SseConsumer(
            _logger,
            _configuration.ConsumerName,
            _configuration.SSEUri,
            _configuration.PrivateKey,
            _configuration.Signer,
            (int)_configuration.BackoffPeriod.TotalSeconds,
            _configuration.NumberOfRetries
        );
        _numberportabilitymessageconsumer = new CI.NumberPortabilityMessageConsumer(
            _sseconsumer,
            _logger
        );
        _numberportabilityservice = new CI.NumberPortabilityService(
            _configuration.RESTUri,
            _configuration.ConsumerName,
            _configuration.Signer,
            _configuration.PrivateKey
        );
    }

    /// <inheritdoc/>
    public async Task<SendMessageResponse> SendMessageAsync(MessageEnvelope messageEnvelope, CancellationToken cancellationToken = default)
    {
        var response = await _numberportabilityservice
            .SendMessageAsync(messageEnvelope.ToCOINMessageEnvelope(_valuehelper), cancellationToken)
            .ConfigureAwait(false);

        return response.FromCOIN();
    }

    /// <inheritdoc/>
    public Task ConfirmMessageAsync(string id, CancellationToken cancellationToken = default)
        => _numberportabilityservice.SendConfirmationAsync(id, cancellationToken);

    /// <inheritdoc/>
    public void StopConsuming()
    {
        lock (_lock)
        {
            _numberportabilitymessageconsumer.StopConsuming();
            _numberportabilitymessagelistener = null;
        }
    }

    /// <inheritdoc/>
    public void StartConsumingUnconfirmed(IEnumerable<MessageType>? filterMessageTypes = null)
    {
        lock (_lock)
        {
            _numberportabilitymessagelistener = CreateMessageListener();
            _numberportabilitymessageconsumer.StartConsumingUnconfirmed(
                _numberportabilitymessagelistener,
                OnFinalDisconnect,
                MapMessageTypes(filterMessageTypes).ToArray()
            );
        }
    }

    /// <inheritdoc/>
    public void StartConsumingUnconfirmedWithOffsetPersistence(IOffsetPersister offsetPersister, long offset = -1, IEnumerable<MessageType>? filterMessageTypes = null)
    {
        lock (_lock)
        {
            _numberportabilitymessagelistener = CreateMessageListener();
            _numberportabilitymessageconsumer.StartConsumingUnconfirmedWithOffsetPersistence(
                _numberportabilitymessagelistener,
                new OffsetPersistorAdapter(offsetPersister),
                offset,
                OnFinalDisconnect,
                MapMessageTypes(filterMessageTypes).ToArray()
            );
        }
    }

    /// <inheritdoc/>
    public void StartConsumingAll(IOffsetPersister offsetPersister, long offset = -1, IEnumerable<MessageType>? filterMessageTypes = null)
    {
        lock (_lock)
        {
            _numberportabilitymessagelistener = CreateMessageListener();
            _numberportabilitymessageconsumer.StartConsumingAll(
                _numberportabilitymessagelistener,
                new OffsetPersistorAdapter(offsetPersister),
                offset,
                OnFinalDisconnect,
                MapMessageTypes(filterMessageTypes).ToArray()
            );
        }
    }

    private void OnFinalDisconnect(Exception exception)
    {
        lock (_lock)
        {
            try
            {
                // Invoke user-defined OnFinalDisconnect
                _messagehandler.OnFinalDisconnect(exception);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{message}", Translations.ERR_Invoking_OnFinalDisconnect);
            }
            // Do our own cleanup
            _numberportabilitymessagelistener = null;
        }
    }

    // Maps our MessageType enum to COIN's messagetype enum
    private static IEnumerable<CM.MessageType> MapMessageTypes(IEnumerable<MessageType>? messageTypes)
        => messageTypes == null
        ? Enumerable.Empty<CM.MessageType>()
        : messageTypes.Select(mt => (CM.MessageType)(int)mt);


    // Ensures there's currently no NP messagelistener in use and returns a new NP messagelistener
    private CS.INumberPortabilityMessageListener CreateMessageListener()
    {
        // Ensure we have no ("current") message listener
        if (_numberportabilitymessagelistener != null)
        {
            throw new InvalidOperationException(Translations.ERR_Listener_Already_In_Use);
        }
        // Create and return a new messagelistener
        return new NPMessageListenerAdapter(_numberportabilityservice, _messagehandler, _valuehelper);
    }

    #region IDisposable
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                try { _sseconsumer?.Dispose(); } catch (ObjectDisposedException) { }
                _numberportabilityservice?.Dispose();
            }
            _disposed = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
    #endregion
}
