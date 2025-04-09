using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using SignalRCommonContracts.HubContracts;
using SignalRCommonContracts.Models;

namespace SignalRHub;

public class MyServerHub : Hub<IClientSample>, IServerRpcSample
{
    public static ConcurrentDictionary<string, string> ConnectedMachines { get; } = new();

    public async Task<int> ReceiveResponseFromClient(ClientResponseDto responseDto)
    {
        // токен отмены, который сигнализирует об обрыве соединения с клиентом
        var cancellationToken = Context.ConnectionAborted;

        // долгая операция
        await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);

        Console.WriteLine("Received response from client: {0}", responseDto);
        return 58;
    }

    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        string? machineId = httpContext?.Request.Headers["machineId"];

        if (!string.IsNullOrEmpty(machineId))
        {
            ConnectedMachines[machineId] = Context.ConnectionId;
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var item = ConnectedMachines.FirstOrDefault(kvp => kvp.Value == Context.ConnectionId);
        if (!string.IsNullOrEmpty(item.Key))
        {
            ConnectedMachines.TryRemove(item.Key, out _);
        }

        await base.OnDisconnectedAsync(exception);
    }
}