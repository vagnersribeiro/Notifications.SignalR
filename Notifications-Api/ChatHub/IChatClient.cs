namespace Notifications_Api.ChatHub
{
    public interface IChatClient
    {
        Task UserConnected(string username);
        Task ReceiveMessage(string message);
        Task UsersUpdated(ICollection<User> users);
        Task ReceivePrivateMessage(string messageFrom, string message);
        Task ReceivePublicMessage(User user, string message);
        Task AssociateConnectionToUser(string email, string name);
    }
}
