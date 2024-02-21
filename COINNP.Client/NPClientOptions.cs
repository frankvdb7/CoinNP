namespace COINNP.Client;

public record NPClientOptions
{
    public required string ConsumerName { get; init; }
    public required string EncryptedHmacSecretFile { get; init; }
    public required string PrivateKeyFile { get; init; }
    public required string? PrivateKeyFilePassword { get; init; }
    public required Uri SSEUri { get; init; }
    public required Uri RESTUri { get; init; }
    public TimeSpan? BackoffPeriod { get; init; } = NPClientConfiguration.DefaultBackoffPeriod;
    public int? NumberOfRetries { get; init; } = NPClientConfiguration.DefaultNumberOfRetries;
};
