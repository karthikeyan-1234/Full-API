namespace API.Services
{
    public class SessionService : ISessionService
    {
        IHttpContextAccessor accessor;
        string? user;

        public SessionService(IHttpContextAccessor accessor)
        {
            this.accessor = accessor;
            var session = accessor?.HttpContext?.Session;
            user = session?.GetString("user");
        }

        public SessionService() { }

        public string? GetString(string key)
        {
            var session = accessor?.HttpContext?.Session;
            return session?.GetString(key);
        }
    }
}
