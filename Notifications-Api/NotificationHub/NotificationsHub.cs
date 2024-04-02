using Microsoft.AspNetCore.SignalR;

namespace Notifications_Api.NotificationHub
{
    public class NotificationsHub : Hub<INotificationClient>
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.Client(Context.ConnectionId)
                .ReceiveNotification($"Obrigado por conectar {Context.User?.Identity?.Name}");

            await base.OnConnectedAsync();
        }
    }

    public interface INotificationClient
    {
        Task ReceiveNotification(string message);
    }
}
