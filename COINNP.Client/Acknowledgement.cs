namespace COINNP.Client;

/// <summary>
/// Defines the available acknowledgement options
/// </summary>
public enum Acknowledgement
{
    /// <summary>Acknowledge message as received/handled; message will be removed from queue.</summary>
    ACK,
    /// <summary>Do NOT Acknowledge message as received/handled; message will be kept in queue.</summary>
    NACK
}
