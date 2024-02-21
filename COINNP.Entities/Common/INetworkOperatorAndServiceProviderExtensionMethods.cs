namespace COINNP.Entities.Common;

public static class INetworkOperatorAndServiceProviderExtensionMethods
{
    public static string ToCOINCode(this INetworkOperatorAndServiceProvider value, string separator = "-")
        => string.IsNullOrEmpty(value.ServiceProvider)
            ? value.NetworkOperator
            : $"{value.NetworkOperator}{separator}{value.ServiceProvider}";
}