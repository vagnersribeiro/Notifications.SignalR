using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace Notifications_Api.ChatHub
{
    public class User(string connectionId, string email, string name, string avatar)
    {
        public string ConnectionId { get; set; } = connectionId;
        public string Email { get; set; } = email;
        public string Name { get; set; } = name;
        public string Avatar { get; set; } = avatar;
    }
    
    public class ChatHub : Hub<IChatClient>
    {
        private static readonly ConcurrentDictionary<string, User> _users = new();

        public override async Task OnConnectedAsync()
        {
            _users.TryAdd(Context.ConnectionId, new User(Context.ConnectionId, string.Empty, string.Empty, string.Empty));

            await Clients.Client(Context.ConnectionId).UserConnected(Context.ConnectionId);
            //await Clients.All.UsersUpdated(_users.Values);
        }

        public async Task AssociateConnectionToUser(string email, string name, string avatar)
        {
            _users.TryGetValue(Context.ConnectionId, out User? user);
            
            if(user != null)
            {
                user.ConnectionId = Context.ConnectionId;
                user.Email = email;
                user.Name = name;
                user.Avatar = avatar;
            }

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

        public async Task SendPublicMessage(string message)
        {
            _users.TryGetValue(Context.ConnectionId, out User? user);
            await Clients.All.ReceivePublicMessage(user, message);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _users.TryRemove(Context.ConnectionId, out _);
            await Clients.All.UsersUpdated(_users.Values);
        }
    }
}
