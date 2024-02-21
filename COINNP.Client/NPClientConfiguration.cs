using Coin.Sdk.Common.Crypto;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace COINNP.Client;

public record NPClientConfiguration(
    string ConsumerName,
    HMACSHA256 Signer,
    RSA PrivateKey,
    Uri SSEUri,
    Uri RESTUri,
    TimeSpan BackoffPeriod,
    int NumberOfRetries
)
{
    public NPClientConfiguration(
        string consumerName,
        HMACSHA256 signer,
        RSA privateKey,
        Uri sseUri,
        Uri restUri,
        TimeSpan? backoffPeriod = null,
        int? numberOfRetries = null
    ) : this(consumerName, signer, privateKey, sseUri, restUri, backoffPeriod ?? DefaultBackoffPeriod, numberOfRetries ?? DefaultNumberOfRetries) { }

    public static NPClientConfiguration FromNPClientOptions(IOptions<NPClientOptions> options)
        => options?.Value == null
        ? throw new ArgumentNullException(nameof(options))
        : new NPClientConfiguration(
            options.Value.ConsumerName,
            CreateSigner(options.Value.EncryptedHmacSecretFile, options.Value.PrivateKeyFile, options.Value.PrivateKeyFilePassword),
            ReadPrivateKeyFile(options.Value.PrivateKeyFile, options.Value.PrivateKeyFilePassword),
            options.Value.SSEUri,
            options.Value.RESTUri,
            options.Value.BackoffPeriod,
            options.Value.NumberOfRetries
        );

    public static readonly TimeSpan DefaultBackoffPeriod = TimeSpan.FromSeconds(1);
    public static readonly int DefaultNumberOfRetries = 15;

    public static RSA ReadPrivateKeyFile(string privateKeyFile, string? password = null)
        => CtpApiClientUtil.ReadPrivateKeyFile(privateKeyFile, password);

    public static HMACSHA256 CreateSigner(string encryptedHmacSecretFile, string privateKeyFile, string? password = null)
        => CtpApiClientUtil.HmacFromEncryptedBase64EncodedSecretFile(encryptedHmacSecretFile, ReadPrivateKeyFile(privateKeyFile, password));

    public static HMACSHA256 CreateSigner(string encryptedHmacSecretFile, RSA privateKey)
        => CtpApiClientUtil.HmacFromEncryptedBase64EncodedSecretFile(encryptedHmacSecretFile, privateKey);
}