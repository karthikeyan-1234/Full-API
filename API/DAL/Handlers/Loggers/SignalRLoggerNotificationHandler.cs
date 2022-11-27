using API.DAL.Commands;
using API.DAL.Notifications;
using API.Infrastructure.Hubs;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace API.DAL.Handlers.Loggers
{
    public class SignalRLoggerNotificationHandler : INotificationHandler<LoggerNotification>
    {
        IHubContext<MessageHub> hub;

        public SignalRLoggerNotificationHandler(IHubContext<MessageHub> hub)
        {
            this.hub = hub;
        }

        public async Task Handle(LoggerNotification notification, CancellationToken cancellationToken)
        {
            await hub.Clients.Group("MyGroup").SendAsync("ReceiveMessage",notification.message);

        }
    }
}
