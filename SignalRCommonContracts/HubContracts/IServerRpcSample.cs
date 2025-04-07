using SignalRCommonContracts.Models;

namespace SignalRCommonContracts.HubContracts;

public interface IServerRpcSample
{
    Task<int> ReceiveResponseFromClient(ClientResponseDto responseDto);
}