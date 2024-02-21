using COINNP.Client.ResourceFiles;
using COINNP.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace COINNP.Client;

/// <summary>A delegate called whenever a <see cref="Message"/> is received (contained in a <see cref="MessageEnvelope"/>.
/// </summary>
/// <param name="messageId">The ID of the message.</param>
/// <param name="messageEnvelope">The received <see cref="Message"/> in a <see cref="MessageEnvelope"/>.</param>
/// <param name="cancellationToken">A cancellation token to cancel operation.</param>
/// <returns>
/// Return <see cref="Acknowledgement.ACK"/> to acknowledge the message, <see cref="Acknowledgement.NACK"/> otherwise.
/// </returns>
public delegate Task<Acknowledgement> OnMessageDelegate(string messageId, MessageEnvelope messageEnvelope, CancellationToken cancellationToken = default);

/// <summary>
/// Provides a simple, basic, messagehandler that accepts an <see cref="OnMessageDelegate"/> to handle messages.
/// Keepalives, exceptions, disconnects and unknown messages are only logged to the given logger (or to the NullLogger
/// when no logger was given)
/// </summary>
/// <remarks>
/// This class provides virtual methods so it's easy to derive from this class and implement just the desired methods.
/// However, it is recommended to implement your own <see cref="INPMessageHandler"/> and use that to pass to the
/// <see cref="NPClient"/>.
/// </remarks>
public class SimpleNPMessageHandler : INPMessageHandler
{
    private readonly ILogger<SimpleNPMessageHandler> _logger;
    private readonly OnMessageDelegate _messagehandler;

    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleNPMessageHandler"/> with the given  <paramref name="messageHandler"/>.
    /// </summary>
    /// <param name="messageHandler">The <see cref="OnMessageDelegate"/> to invoke when a message is received.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the <paramref name="messageHandler"/> is null.
    /// </exception>
    /// <remarks>
    ///     The messagehandler should return <see langword="true"/> when the message is handled and should be
    ///     acknowledged, <see langword="false"/> otherwise.
    /// </remarks>
    public SimpleNPMessageHandler(OnMessageDelegate messageHandler)
        : this(NullLogger<SimpleNPMessageHandler>.Instance, messageHandler) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleNPMessageHandler"/> with the given <paramref name="logger"/>
    /// and <paramref name="messageHandler"/>.
    /// </summary>
    /// <param name="logger">The logger to write messages to.</param>
    /// <param name="messageHandler">The <see cref="OnMessageDelegate"/> to invoke when a message is received.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the <paramref name="logger"/> or <paramref name="messageHandler"/> is null.
    /// </exception>
    /// <remarks>
    ///     The messagehandler should return <see langword="true"/> when the message is handled and should be
    ///     acknowledged, <see langword="false"/> otherwise.
    /// </remarks>
    public SimpleNPMessageHandler(ILogger<SimpleNPMessageHandler> logger, OnMessageDelegate messageHandler)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _messagehandler = messageHandler ?? throw new ArgumentNullException(nameof(messageHandler));
    }

    /// <inheritdoc/>
    public Task<Acknowledgement> OnMessageAsync(string messageId, MessageEnvelope messageEnvelope, CancellationToken cancellationToken = default)
        => _messagehandler(messageId, messageEnvelope, cancellationToken);

    /// <inheritdoc/>
    public virtual Task OnKeepAliveAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogTrace("{keepalivemessage}", Translations.EVT_Keepalive);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public virtual Task OnUnknownMessageAsync(string messageId, string message, CancellationToken cancellationToken = default)
    {
        _logger.LogWarning("{unknownmessage}", string.Format(Translations.EVT_OnUnknownMessage, messageId, message));
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public virtual Task OnExceptionAsync(Exception exception, CancellationToken cancellationToken = default)
    {
        _logger.LogError(exception, "{exceptionmessage}", string.Format(Translations.EVT_OnException, exception.Message));
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public virtual void OnFinalDisconnect(Exception exception)
        => _logger.LogError(exception, "{finaldisconnectmessage}", Translations.EVT_OnFinalDisconnect);
}