using API.DAL.Commands;
using API.DAL.Notifications;
using API.Services;
using MediatR;

namespace API.DAL.Handlers.Loggers
{
    public class SeriLoggerNotificationHandler : INotificationHandler<LoggerNotification>
    {
        ILogger<string> logger;
        public SeriLoggerNotificationHandler(ILogger<string> logger)
        {
            this.logger = logger;
        }

        public async Task Handle(LoggerNotification notification, CancellationToken cancellationToken)
        {
            logger.LogInformation(notification.message);

        }
    }
}
