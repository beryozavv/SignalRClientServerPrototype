using SignalRHub;

var builder = WebApplication.CreateBuilder(args);

// Регистрируем SignalR в DI-контейнере.
builder.Services.AddSignalR();
builder.Services.AddTransient<IHubContextWrapper, HubContextWrapper>();

var app = builder.Build();

// Маршрутизация для SignalR-хаба.
app.MapHub<MyServerHub>("/hub");

// Эндпоинт для трансляции списка идентификаторов указанным клиентам.
app.MapGet("/broadcast", async (IHubContextWrapper hubContextWrapper, string machineIds, string userIds) =>
{
    var machineIdsList = machineIds.Split(',', StringSplitOptions.RemoveEmptyEntries).Distinct().ToHashSet();
    var userIdsList = userIds.Split(',', StringSplitOptions.RemoveEmptyEntries).Distinct();

    var connectionIds = MyServerHub.ConnectedMachines.Where(c =>
        machineIdsList.Contains(c.Key)).Select(m => m.Value);

    if (connectionIds.Any())
    {
        await hubContextWrapper.SendToClients(connectionIds, userIdsList);
        return Results.Ok("Список идентификаторов успешно отправлен указанным клиентам");
    }
    else
    {
        return Results.NotFound("Ни один из указанных клиентов не найден");
    }
});

// Эндпоинт для трансляции списка идентификаторов только одному указанному клиенту
app.MapGet("/single", async (IHubContextWrapper hubContextWrapper, string machineId, string userIds) =>
{
    var userIdsList = userIds.Split(',', StringSplitOptions.RemoveEmptyEntries).Distinct();

    var connectionId = MyServerHub.ConnectedMachines.SingleOrDefault(c => c.Key == machineId);

    if (connectionId.Value != null)
    {
        var dateTime = await hubContextWrapper.SendToSingleClient(connectionId.Value, userIdsList);
        return Results.Ok(
            $"Список идентификаторов успешно отправлен указанному клиенту {machineId} и обработан клиентом в {dateTime}");
    }
    else
    {
        return Results.NotFound("Ни один из указанных клиентов не найден");
    }
});

app.MapGet("/multipleConnections", async (IHubContextWrapper hubContext, string message) =>
{
    await hubContext.SendToAllClients(message);
    return Results.Ok("Список идентификаторов успешно отправлен всем клиентам");
});
app.Run();