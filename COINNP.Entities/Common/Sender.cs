namespace COINNP.Entities.Common;

public record Sender(string NetworkOperator, string? ServiceProvider = null) : INetworkOperatorAndServiceProvider;