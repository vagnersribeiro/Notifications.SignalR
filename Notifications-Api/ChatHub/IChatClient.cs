namespace Notifications_Api.ChatHub
{
    public interface IChatClient
    {
        Task UserConnected(string username);
        Task ReceiveMessage(string message);
        Task UsersUpdated(ICollection<string> users);
        Task ReceivePrivateMessage(string messageFrom, string message);
        Task ReceivePublicMessage(string messageFrom, string message);
    }
}
