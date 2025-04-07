using Microsoft.AspNetCore.SignalR.Client;
using SignalRCommonContracts.HubContracts;
using SignalRCommonContracts.Models;

namespace SignalRGroupClient;

class Program
{
    public static async Task Main()
    {
        Console.Write("Введите ваш MachineId: ");
        string? machineId = Console.ReadLine();

        if (string.IsNullOrEmpty(machineId))
        {
            throw new ArgumentNullException(nameof(machineId));
        }

        var connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5000/hub",
                options => { options.Headers.Add("machineId", machineId); })
            .WithAutomaticReconnect()
            .Build();

        // Подписываемся на вызов метода клиента "ReceiveUserIds".
        connection.On<ReceiveUserIdsDto>(nameof(IClientSample.ReceiveUserIds), request =>
        {
            IServerRpcSample serverRpcSample = new ServerRpcClientCall(connection);
            IClientSample clientSample = new ClientSampleImpl(serverRpcSample, machineId);
            clientSample.ReceiveUserIds(request);
        });

        // Подписываемся на вызов метода клиента "ReceiveUserIds".
        connection.On<List<string>, DateTime>(nameof(IClientSample.ReceiveUserIdsSingleClient), async userIds =>
        {
            IServerRpcSample serverRpcSample = new ServerRpcClientCall(connection);
            IClientSample clientSample = new ClientSampleImpl(serverRpcSample, machineId);
            var result = await clientSample.ReceiveUserIdsSingleClient(userIds);
            return result;
        });

        // Запускаем соединение.
        try
        {
            await connection.StartAsync();
            Console.WriteLine("Соединение установлено!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка подключения: {ex.Message}");
        }

        // Консоль остаётся открытой, чтобы получать сообщения.
        Console.WriteLine("Нажмите Enter для завершения...");
        Console.ReadLine();
    }
}