using Microsoft.AspNetCore.SignalR;

namespace SignalRHub;

public class TestHubWithCancellation : Hub
{
    async Task TestSendWithCancellation()
    {
        using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1)))
        {
            try
            {
                await Clients.All.SendAsync("method", cancellationToken: cts.Token);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Операция была отменена по истечению времени или из-за обрыва связи.");
            }
        }
    }

    async Task TestInvokeWithCancellation()
    {
        using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1)))
        {
            try
            {
                await Clients.Client("clientId").InvokeAsync<int>("method", cancellationToken: cts.Token);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Операция была отменена по истечению времени или из-за обрыва связи.");
            }
        }
    }
}