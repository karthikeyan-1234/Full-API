using MediatR;

namespace API.DAL.Notifications
{
    public class LoggerNotification : INotification
    {
        public string message;

        public LoggerNotification(string message)
        {
            this.message = message;
        }
    }
}
