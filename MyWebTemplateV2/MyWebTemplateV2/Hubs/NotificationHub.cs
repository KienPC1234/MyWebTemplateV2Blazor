using Microsoft.AspNetCore.SignalR;

namespace MyWebTemplateV2.Hubs;

public class NotificationHub : Hub
{
    public async Task SendNotification(string message)
    {
        await Clients.All.SendAsync("ReceiveNotification", message);
    }

    public async Task NotifySubmissionUpdated()
    {
        await Clients.All.SendAsync("SubmissionUpdated");
    }
}
