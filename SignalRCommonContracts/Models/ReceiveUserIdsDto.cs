namespace SignalRCommonContracts.Models;

public record ReceiveUserIdsDto(IEnumerable<string> UserIds, Guid RequestId);