using Microsoft.AspNetCore.SignalR;
using SignalRCommonContracts.HubContracts;
using SignalRCommonContracts.Models;

namespace SignalRHub;

public class HubContextWrapper : IHubContextWrapper
{
    private readonly IHubContext<MyServerHub, IClientSample> _hubContext;

    public HubContextWrapper(IHubContext<MyServerHub, IClientSample> hubContext)
    {
        _hubContext = hubContext;
    }
    
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

    public async Task SendToAllClients(string message)
    {
        var requestId = Guid.NewGuid();
        await _hubContext.Clients.All.ReceiveMessage(new ReceiveMessageDto(message, requestId));
        Console.WriteLine("Command {0} sent to clients", requestId);
    }
}