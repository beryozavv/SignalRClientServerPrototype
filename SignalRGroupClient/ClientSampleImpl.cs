using SignalRCommonContracts.HubContracts;
using SignalRCommonContracts.Models;

namespace SignalRGroupClient;

public class ClientSampleImpl : IClientSample
{
    private readonly IServerRpcSample _serverRpcSample;
    private readonly string _machineId;

    public ClientSampleImpl(IServerRpcSample serverRpcSample, string machineId)
    {
        _serverRpcSample = serverRpcSample;
        _machineId = machineId;
    }

    public async Task ReceiveUserIds(ReceiveUserIdsDto dto)
    {
        Console.WriteLine("По запросу {0} получены следующие идентификаторы пользователей:", dto.RequestId);
        foreach (var id in dto.UserIds)
        {
            Console.WriteLine($"UserId = {id};");
        }

        var clientResponseDto = new ClientResponseDto(dto.RequestId, DateTime.Now, _machineId, "Success");
        await _serverRpcSample.ReceiveResponseFromClient(clientResponseDto);
    }
    
    public async Task ReceiveMessage(ReceiveMessageDto dto)
    {
        Console.WriteLine("По запросу {0} получено сообщение {1}", dto.RequestId, dto.Message);

        var clientResponseDto = new ClientResponseDto(dto.RequestId, DateTime.Now, _machineId, "Success - "+dto.Message);
        await _serverRpcSample.ReceiveResponseFromClient(clientResponseDto);
    }

    /// <summary>
    /// Пример метода, который возвращает результат
    /// </summary>
    /// <param name="userIds"></param>
    /// <returns></returns>
    /// <remarks>Разрешено вызывать только для одного клиента</remarks>
    public Task<DateTime> ReceiveUserIdsSingleClient(IEnumerable<string> userIds)
    {
        Console.WriteLine("Получены следующие идентификаторы пользователей:");
        foreach (var id in userIds)
        {
            Console.WriteLine($"UserId = {id};");
        }

        return Task.FromResult(DateTime.Now);
    }
}