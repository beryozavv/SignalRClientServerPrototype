using SignalRCommonContracts.Models;

namespace SignalRCommonContracts.HubContracts;

public interface IClientSample
{
    Task ReceiveUserIds(ReceiveUserIdsDto dto);
    Task ReceiveMessage(ReceiveMessageDto dto);
    Task<DateTime> ReceiveUserIdsSingleClient(IEnumerable<string> userIds);
}