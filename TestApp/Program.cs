using COINNP.Client;
using COINNP.Entities;
using COINNP.Entities.Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace TestApp;

internal class Program
{
    private static readonly JsonSerializerOptions _serializeroptions = new() { WriteIndented = true };

    private static void Main(string[] args)
    {
        var configprovider = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        var serviceprovider = new ServiceCollection()
            .Configure<NPClientOptions>(configprovider.GetRequiredSection("NPClient"))
            .AddSingleton<OnMessageDelegate>(OnMessageAsync)
            .AddSingleton<INPMessageHandler, SimpleNPMessageHandler>()
            .AddSingleton<INPClient, NPClient>()
            .BuildServiceProvider();

        var client = serviceprovider.GetRequiredService<INPClient>();

        client.StartConsumingUnconfirmed();
        Console.WriteLine("Consuming messages. Press any key to quit.");

        Console.ReadKey();

        client.StopConsuming();
        Console.WriteLine("Done.");
    }

    private static Task<Acknowledgement> OnMessageAsync(string messageId, MessageEnvelope messageEnvelope, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("Received message '{0}':\n{1}\n----", messageId, JsonSerializer.Serialize(messageEnvelope, _serializeroptions));

        return messageEnvelope.Body switch
        {
            PortingRequest r => DummyPortingRequestHandler(r),
            //Cancel c => // Do something
            // ...
            _ => Task.FromResult(Acknowledgement.NACK) // Don't acknowledge unhandled messages
        };
    }

    private static Task<Acknowledgement> DummyPortingRequestHandler(PortingRequest portingRequest)
    {
        Console.WriteLine("Received porting request {0}", portingRequest.DossierId);
        return Task.FromResult(Acknowledgement.NACK);  // Return 'Ack' to acknowledge the message
    }
}