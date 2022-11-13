namespace API.Services
{
    public class SessionService : ISessionService
    {
        IHttpContextAccessor accessor;

        public SessionService(IHttpContextAccessor accessor)
        {
            this.accessor = accessor;
            var session = accessor?.HttpContext?.Session;
        }

        public SessionService() { }

        public string? GetString(string key)
        {
            var session = accessor?.HttpContext?.Session;
            return session?.GetString(key);
        }
    }
}
