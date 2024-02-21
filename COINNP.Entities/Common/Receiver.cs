namespace COINNP.Entities.Common;

public record Receiver(string NetworkOperator, string? ServiceProvider = null) : INetworkOperatorAndServiceProvider;