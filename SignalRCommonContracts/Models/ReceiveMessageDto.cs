namespace SignalRCommonContracts.Models;

public record ReceiveMessageDto(string Message, Guid RequestId);