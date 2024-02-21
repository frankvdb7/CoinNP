using COINNP.Entities;

namespace COINNP.Client;

/// <summary>
/// Defines methods that handle events invoked by the <see cref="NPClient"/> for message processing.
/// </summary>
public interface INPMessageHandler
{
    /// <summary>
    /// Invoked for every received message.
    /// </summary>
    /// <param name="messageId">The ID of the message.</param>
    /// <param name="messageEnvelope">The received message.</param>
    /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
    /// <returns>
    /// A task that represents the asynchronous message received operation; when <see cref="Acknowledgement.ACK"/>,
    /// the message is handled and should be removed from the queue; use <see cref="Acknowledgement.NACK"/> otherwise.
    /// </returns>
    Task<Acknowledgement> OnMessageAsync(string messageId, MessageEnvelope messageEnvelope, CancellationToken cancellationToken = default);

    /// <summary>
    /// Invoked when a keepalive is received.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
    /// <returns>A task representing the keepalive received event.</returns>
    Task OnKeepAliveAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Invoked whenever an unknown message(type) is received.
    /// </summary>
    /// <param name="messageId">The ID of the message.</param>
    /// <param name="message">The raw content of the message.</param>
    /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
    /// <returns>A task representing the unknown message received event.</returns>
    Task OnUnknownMessageAsync(string messageId, string message, CancellationToken cancellationToken = default);

    /// <summary>
    /// Invoked whenever an exception occurs.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
    /// <returns>A task representing the exception event.</returns>
    Task OnExceptionAsync(Exception exception, CancellationToken cancellationToken = default);

    /// <summary>
    /// Invoked when the connection is disconnected.
    /// </summary>
    /// <param name="exception">The exception that is the cause of the disconnect.</param>
    void OnFinalDisconnect(Exception exception);
}