using Microsoft.AspNetCore.SignalR.Client;
using SignalRCommonContracts.HubContracts;
using SignalRCommonContracts.Models;

namespace SignalRGroupClient;

public class ServerRpcClientCall : IServerRpcSample
{
    private readonly HubConnection _hubConnection;

    public ServerRpcClientCall(HubConnection hubConnection)
    {
        _hubConnection = hubConnection;
    }

    public async Task<int> ReceiveResponseFromClient(ClientResponseDto responseDto)
    {
        using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2)))
        {
            try
            {
                // Передаем токен отмены в метод InvokeAsync
                var result =
                    await _hubConnection.InvokeAsync<int>(nameof(ReceiveResponseFromClient), responseDto, cts.Token);
                Console.WriteLine("Sent response to server by requestId = {0}", responseDto.RequestId);
                return result;
            }
            catch (OperationCanceledException ex)
            {
                //await _hubConnection.StopAsync();
                Console.WriteLine("Operation cancelled {0}", ex);
                return -1;
            }
        }
    }
}