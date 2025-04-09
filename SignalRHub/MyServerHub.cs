using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using SignalRCommonContracts.HubContracts;
using SignalRCommonContracts.Models;

namespace SignalRHub;

public class MyServerHub : Hub<IClientSample>, IServerRpcSample
{
    private readonly IHubContext<MyServerHub, IClientSample> _hubContext;

    public MyServerHub(IHubContext<MyServerHub, IClientSample> hubContext)
    {
        _hubContext = hubContext;
    }

    public static ConcurrentDictionary<string, string> ConnectedMachines { get; } = new();

    public async Task SendToClients(IEnumerable<string> connectionIds, IEnumerable<string> userIds)
    {
        var requestId = Guid.NewGuid();
        await _hubContext.Clients.Clients(connectionIds).ReceiveUserIds(new ReceiveUserIdsDto(userIds, requestId));
        Console.WriteLine("Command {0} sent to clients", requestId);
    }

    public async Task<DateTime> SendToSingleClient(string connectionId, IEnumerable<string> userIds)
    {
        var result = await _hubContext.Clients.Client(connectionId).ReceiveUserIdsSingleClient(userIds);
        Console.WriteLine("Command sent to client with result: {0}", result);
        return result;
    }
    
    public async Task SendToAllClients(IEnumerable<string> userIds)
    {
        var requestId = Guid.NewGuid();
        await _hubContext.Clients.All.ReceiveUserIds(new ReceiveUserIdsDto(userIds, requestId));
        Console.WriteLine("Command {0} sent to clients", requestId);
    }

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