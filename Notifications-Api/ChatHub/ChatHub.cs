using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace Notifications_Api.ChatHub
{
    public class ChatHub : Hub<IChatClient>
    {
        private static readonly ConcurrentDictionary<string, string> _users = new();

        public override async Task OnConnectedAsync()
        {
            _users.TryAdd(Context.ConnectionId, Context.ConnectionId);

            await Clients.Client(Context.ConnectionId).UserConnected(Context.ConnectionId);
            await Clients.All.UsersUpdated(_users.Values);
        }

        public async Task SendMessage(string message)
        {
            await Clients.All.ReceiveMessage($"{message}");
        }

        public async Task SendPrivateMessage(string usernameFrom, string usernameTo, string message)
        {
            await Clients.Client(usernameTo).ReceivePrivateMessage(usernameFrom, message);
        }

        public async Task SendPublicMessage(string usernameFrom, string message)
        {
            await Clients.Others.ReceivePublicMessage(usernameFrom, message);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _users.TryRemove(Context.ConnectionId, out _);
            await Clients.All.UsersUpdated(_users.Values);
        }
    }
}
