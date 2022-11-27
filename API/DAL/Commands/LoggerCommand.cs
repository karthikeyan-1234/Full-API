using MediatR;

namespace API.DAL.Commands
{
    public class LoggerCommand : IRequest<string>
    {
        public string message { get; set; }

        public LoggerCommand(string message)
        {
            this.message = message;
        }
    }
}
