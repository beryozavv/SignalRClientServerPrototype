namespace SignalRHub;

public interface IHubContextWrapper
{
    Task SendToClients(IEnumerable<string> connectionIds, IEnumerable<string> userIds);
    Task<DateTime> SendToSingleClient(string connectionId, IEnumerable<string> userIds);
    Task SendToAllClients(string message);
}