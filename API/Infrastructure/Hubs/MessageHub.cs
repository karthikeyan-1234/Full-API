using Microsoft.AspNetCore.SignalR;

namespace API.Infrastructure.Hubs
{
    public class MessageHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        //Acts as server to get data from one method in a client to another method (clientHandlerMethod) in client
        public Task SendMessageToGroup(string group, string clientHandlerMethod, string message)
        {
            return Clients.Group(group).SendAsync(clientHandlerMethod, message);
        }

        public Task JoinGroup(string group)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, group);
        }

    }
}
