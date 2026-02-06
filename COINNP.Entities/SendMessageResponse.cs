namespace COINNP.Entities;

/// <summary>
/// Represents a single send-message error returned by COIN.
/// </summary>
/// <param name="Code">The COIN error code.</param>
/// <param name="Message">The COIN error message.</param>
public record SendMessageError(string Code, string Message);

/// <summary>
/// Represents the response returned after sending a message to COIN.
/// </summary>
/// <param name="TransactionId">The transaction id assigned by COIN.</param>
/// <param name="Errors">Optional COIN errors returned for the request.</param>
public record SendMessageResponse(string TransactionId, IReadOnlyList<SendMessageError>? Errors = null)
{
    /// <summary>
    /// Indicates whether the response contains one or more errors.
    /// </summary>
    public bool HasErrors => Errors is { Count: > 0 };
}
