using Microsoft.AspNetCore.SignalR.Client;
using SignalRCommonContracts.HubContracts;
using SignalRCommonContracts.Models;
using SignalRGroupClient;

namespace SignalRMultipleConnections;

class Program
{
    static async Task Main(string[] args)
    {
        var sharedHandler = new HttpClientHandler();

        var connections = new List<HubConnection>();

        for (int i = 0; i < 3_000; i++)
        {
            int connectionNumber = i;
            var connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/hub", options =>
                {
                    // Переиспользуем общий HttpMessageHandler
                    options.HttpMessageHandlerFactory = _ => sharedHandler;
                })
                .Build();

            connection.On<ReceiveMessageDto>(nameof(IClientSample.ReceiveMessage), request =>
            {
                IServerRpcSample serverRpcSample = new ServerRpcClientCall(connection);
                IClientSample clientSample = new ClientSampleImpl(serverRpcSample, $"Mcm-{connectionNumber}");
                clientSample.ReceiveMessage(request);
            });

            connections.Add(connection);
        }

        var startTasks = connections.Select(c => c.StartAsync());
        await Task.WhenAll(startTasks);
        Console.WriteLine("Все 1000 подключений успешно установлены.");

        Console.WriteLine("Нажмите ENTER для остановки всех подключений...");
        Console.ReadLine();

        var stopTasks = connections.Select(c => c.StopAsync());
        await Task.WhenAll(stopTasks);
        Console.WriteLine("Все подключения остановлены.");
    }
}