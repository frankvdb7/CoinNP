namespace COINNP.Client;

/// <summary>
/// Provides a mechanism for persisting message offsets in the COIN queue.
/// </summary>
public interface IOffsetPersister
{
    /// <summary>
    /// Retrieves the offset.
    /// </summary>
    /// <returns>Returns the offset.</returns>
    long GetOffset();

    /// <summary>
    /// Stores the offset.
    /// </summary>
    /// <param name="offset">The offset value to store.</param>
    void SetOffset(long offset);
}
