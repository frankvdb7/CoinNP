using COINNP.Entities;

namespace COINNP.Client;

/// <summary>
/// Defines methods that control consuming message from the COIN NP message stream.
/// </summary>
public interface INPClient
{
    /// <summary>
    /// Asynchronously sends an NP message to COIN.
    /// </summary>
    /// <param name="messageEnvelope">The <see cref="MessageEnvelope"/> containing the <see cref="Message"/> to send.</param>
    /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
    /// <returns>A task that represents the asynchronous send operation.</returns>
    public Task SendMessageAsync(MessageEnvelope messageEnvelope, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously confirms an NP message.
    /// </summary>
    /// <param name="id">The id of the message to confirm.</param>
    /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
    /// <returns>A task that represents the asynchronous confirm operation.</returns>
    public Task ConfirmMessageAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Starts consuming all messages.
    /// </summary>
    /// <param name="offsetPersister">The <see cref="IOffsetPersister"/> to use to persist offsets.</param>
    /// <param name="offset">The offset to start consuming messages from.</param>
    /// <param name="filterMessageTypes"><see cref="MessageType"/>s to filter.</param>
    /// <remarks>
    /// Consume all messages, both confirmed and unconfirmed, from a certain offset. Only use for special cases if
    /// <see cref="StartConsumingUnconfirmed"/> does not meet needs.
    /// </remarks>
    void StartConsumingAll(IOffsetPersister offsetPersister, long offset = -1, IEnumerable<MessageType>? filterMessageTypes = null);

    /// <summary>
    /// Starts consuming unconfirmed messages.
    /// </summary>
    /// <param name="filterMessageTypes"><see cref="MessageType"/>s to filter.</param>
    /// <remarks>Recommended method for consuming messages. On connect or reconnect it will consume all unconfirmed messages.</remarks>
    void StartConsumingUnconfirmed(IEnumerable<MessageType>? filterMessageTypes = null);

    /// <summary>
    /// Starts consuming unconfirmed messages from a given offset using an <see cref="IOffsetPersister"/> to persist the offset.
    /// </summary>
    /// <param name="offsetPersister">The <see cref="IOffsetPersister"/> to use to persist offsets.</param>
    /// <param name="offset">The offset to start consuming messages from.</param>
    /// <param name="filterMessageTypes"><see cref="MessageType"/>s to filter.</param>
    /// <remarks>
    /// Only use this method for receiving unconfirmed messages if you make sure that all messages that are received
    /// through this method will be confirmed otherwise, ideally in the stream opened by <see cref="StartConsumingUnconfirmed"/>.
    /// So this method should only be used for a secondary stream (e.g. backoffice process) that needs to consume unconfirmed
    /// messages for administrative purposes.
    /// </remarks>
    void StartConsumingUnconfirmedWithOffsetPersistence(IOffsetPersister offsetPersister, long offset = -1, IEnumerable<MessageType>? filterMessageTypes = null);

    /// <summary>
    /// Stops consuming messages.
    /// </summary>
    void StopConsuming();
}