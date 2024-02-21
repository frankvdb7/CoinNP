namespace COINNP.Entities.Common;

public interface INetworkOperatorAndServiceProvider
{
    public string NetworkOperator { get; }
    public string? ServiceProvider { get; }
}
