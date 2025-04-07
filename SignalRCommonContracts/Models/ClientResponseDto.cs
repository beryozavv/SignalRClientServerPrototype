namespace SignalRCommonContracts.Models;

public record ClientResponseDto(Guid RequestId, DateTime Timestamp, string ClientId, string Message);